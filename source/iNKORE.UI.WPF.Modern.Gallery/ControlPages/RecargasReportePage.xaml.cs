using iNKORE.UI.WPF.Modern.Controls;
using iNKORE.UI.WPF.Modern.Gallery.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Diagnostics;
using iNKORE.UI.WPF.Modern.Gallery.Helper;
using System.Windows.Controls;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;
using Frame = iNKORE.UI.WPF.Modern.Controls.Frame;
using iNKORE.UI.WPF.Modern.Gallery.Common;
using iNKORE.UI.WPF.Modern.Helpers;
using iNKORE.UI.WPF.Helpers;
using System.Net.Http;
using MessageBox = iNKORE.UI.WPF.Modern.Controls.MessageBox;
using Newtonsoft.Json;
//using System.Collections;
using Newtonsoft.Json.Linq;
using System.IO;

namespace iNKORE.UI.WPF.Modern.Gallery.ControlPages
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>

    public partial class RecargasReportePage : ItemsPageBase
    // public sealed partial class RecargasPage : ItemsPageBase
    {
        public ObservableCollection<paquetico> Paqueticos { get; set; } = new ObservableCollection<paquetico>();

        public RecargasReportePage()
        {
            InitializeComponent();
            DataContext = this;
            fillRecharge();
        }

        public async void fillRecharge()
        {
            using (HttpClient client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "zYZOcixz3621T26TC1vSWtRTT5FeKPZkf5CZgP92SY6lYI126A");
                try
                {
                    string url = "http://api.paycloud.com/api-paycloud/public/v1/obtenerRecargas";

                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JArray result = (JArray) JsonConvert.DeserializeObject(responseBody);

                    foreach (JObject recarga in result)
                    {
                        paquetico Paquetico = new paquetico
                        (
                            (int)recarga?.GetValue("ID"),
                            (string)recarga?.GetValue("amount"),
                            (string)recarga?.GetValue("cellphone"),
                            (string)recarga?.GetValue("provider"),
                            (int)recarga?.GetValue("point_sale"),
                            (int)recarga?.GetValue("salesperson")
                        );


                        Paqueticos.Add(Paquetico);
                    }

                    // this.dataGrid.DataContext = this.Paqueticos;

                }
                catch (HttpRequestException _e)
                {
                    MessageBox.Show("Error del lado de la API" + _e.ToString(), "Notificación");
                }
            }
            
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            //gracias stackoverflow
            var row = ItemsControl.ContainerFromElement((DataGrid)sender,
                                                e.OriginalSource as DependencyObject) as DataGridRow;

            if (row == null) return;

            int factura_id = (int)row.Item.GetProperty("ID");

            string basePath = Directory.GetCurrentDirectory().Split(new string[] { "\\bin" }, StringSplitOptions.None)[0];

            string invoice = basePath + "/invoices/" + factura_id + ".pdf";

            Debug.WriteLine("Invoice: ",invoice);

            if (File.Exists(invoice))
            {
                string edgePath = GetEdgePath();
                if (!string.IsNullOrEmpty(edgePath))
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = edgePath,
                        Arguments = $"file:///{invoice}",
                    };

                    Process.Start(startInfo);
                }
                else
                {
                    MessageBox.Show("Factura generada, pero no se ha encontrado programa para abrirlo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No existe", "Notificación");
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = Paqueticos;
        }

        protected void OnItemGridViewItemClick_2(object sender, RoutedEventArgs e)
        {
            var buttonSender = (Button)sender;
            // this.Frame.Navigate(typeof(ItemPage), _itemId, new DrillInNavigationTransitionInfo());
            Frame.Navigate(typeof(DatosRecargasPage), buttonSender.Uid);
        }

        // public void MiBoton_Click(object sender, MouseButtonEventArgs e)
        // {
        //     Debug.WriteLine("MiBoton_Click");
        // }

        // private void RedirigirPestana()
        // {
        //     Debug.WriteLine("RedirigirPestana");
        // }

    }
}
