using iNKORE.UI.WPF.Modern.Controls;
using iNKORE.UI.WPF.Modern.Gallery.DataModel;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using System.Diagnostics;
using System.Windows.Controls;
using MessageBox = iNKORE.UI.WPF.Modern.Controls.MessageBox;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Bouncycastleconnector;
using System.IO;
using iText.StyledXmlParser.Jsoup.Nodes;
using iText.Kernel.Geom;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using System.Windows.Shapes;
using System.Printing;
using System.Windows.Xps;


namespace iNKORE.UI.WPF.Modern.Gallery
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>

    public sealed partial class DatosRecargasPage : ItemsPageBase
    {

        private string proveedor { get; set; }

        public DatosRecargasPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var queryText = e.Parameter()?.ToString().ToLower();

            proveedor = queryText;

            Items = ControlInfoDataSource.Instance.Groups.SelectMany(g => g.Items).OrderBy(i => i.Title).ToList();
            DataContext = Items;
        }

        private void elegirNumero(object sender, RoutedEventArgs e){
            var numero = ((Button)sender).Content.ToString();

            switch (numero)
            {
                case "Eliminar":
                    if(numeroCelular.Text.Length > 0)
                        numeroCelular.Text = numeroCelular.Text.Substring(0, numeroCelular.Text.Length - 1);
                    break;

                default:
                    numeroCelular.Text += numero;
                    break;
            }

        }

        private async void guardarRecarga(object sender, RoutedEventArgs clickEvent){
            string numeCelular = numeroCelular.Text.Trim();
            string monto = montoRecarga.Text.Trim();

            if(numeCelular.Length > 0){
                if(monto.Length > 0){
                    using (HttpClient client = new HttpClient())
                    {
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "zYZOcixz3621T26TC1vSWtRTT5FeKPZkf5CZgP92SY6lYI126A");
                        try
                        {
                            string url = "http://api.paycloud.com/api-paycloud/public/v1/enviarPaquetico";
                            var data = new { to = numeCelular, monto, salesperson_id = "1", provider = proveedor}; // session
                            var json = JsonConvert.SerializeObject(data);
                            var content = new StringContent(json, Encoding.UTF8, "application/json");

                            HttpResponseMessage response = await client.PostAsync(url, content);
                            response.EnsureSuccessStatusCode();
                            string responseBody = await response.Content.ReadAsStringAsync();
                            
                            var result = JObject.Parse(responseBody);

                            // Check the status and show a notification
                            if ((bool)result["status"])
                            {
                                // Show notification with message
                                MessageBox.Show((string)result["message"], "Notificación");
                                crearFactura((int)result["ID"],numeCelular, monto);
                            }
                            else
                            {
                                // Show notification error
                                MessageBox.Show("Error: " + (string)result["message"], "Notificación");
                            }
                            // Aquí puedes hacer algo con la respuesta
                        }
                        catch (HttpRequestException e)
                        {
                            MessageBox.Show("Error del lado de la API" + e.ToString(), "Notificación");
                        }
                    }
                }else{
                    MessageBox.Show("Debe ingresar un monto para la recarga");
                }
            }else{
                MessageBox.Show("Debe ingresar un número de celular");
            }
        }

        private void crearFactura(int id, string numeCelular, string monto){
            String dest = "../../../invoices/" + id + ".pdf";
            FileInfo file = new FileInfo(dest);
            file.Directory.Create();
            var writer = new PdfWriter(dest);
            var pdf = new PdfDocument(writer);
            var document = new iText.Layout.Document(pdf, PageSize.A5);

            //Agregar header
            document.Add(new iText.Layout.Element.Image(iText.IO.Image.ImageDataFactory.Create("../../../images/lotenal.png")).SetWidth(110).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER));
            document.Add(new iText.Layout.Element.Paragraph("Paquetico Paycloud").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold().SetFontSize(15));

            document.Add(new iText.Layout.Element.Paragraph("Vendedor: Jorge Luis").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            document.Add(new iText.Layout.Element.Paragraph("Número Factura: "+ id).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            document.Add(new iText.Layout.Element.Paragraph("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy") + " " + DateTime.Now.ToString("hh:mm:ss tt") ).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            document.Add(new iText.Layout.Element.Paragraph("Descripción: " + "Recarga de saldo").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            document.Add(new iText.Layout.Element.Paragraph("Numero celular: " + numeCelular).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            document.Add(new iText.Layout.Element.Paragraph("Saldo: " + monto).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

            //Agregar footer
            //Agregar footer bold, al final de la hoja
            document.Add(new iText.Layout.Element.Paragraph("Gracias por su compra en Paycloud").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold());

            document.Close();

            var basePath = Directory.GetCurrentDirectory().Split(new string[] { "\\bin" }, StringSplitOptions.None)[0];

            var document_to_print = basePath + "/invoices/" + id + ".pdf";

            PrintPDF(document_to_print);

            // Limpiar campos
            numeroCelular.Text = "";
            montoRecarga.Text = "";
        }

        private void PrintPDF(string pdfFilePath)
        {
            if (File.Exists(pdfFilePath))
            {
                string edgePath = GetEdgePath();
                if (!string.IsNullOrEmpty(edgePath))
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = edgePath,
                        Arguments = $"file:///{pdfFilePath}",
                    };

                    Process.Start(startInfo);
                }
                else
                {
                    MessageBox.Show("Microsoft Edge no ha sido encontrado en el sistema.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show($"Archivo PDF no encontrado: {pdfFilePath}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetEdgePath()
        {
            string edgePath = string.Empty;
            using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\msedge.exe", false))
            {
                if (key != null)
                {
                    edgePath = key.GetValue(null) as string;
                }
            }
            return edgePath;
        }


    }
}
