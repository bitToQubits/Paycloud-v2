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

namespace iNKORE.UI.WPF.Modern.Gallery.ControlPages
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>

    public partial class RecargasPage : ItemsPageBase
    // public sealed partial class RecargasPage : ItemsPageBase
    {
        public RecargasPage()
        {
            InitializeComponent();
            // this.DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Items = ControlInfoDataSource.Instance.Groups.SelectMany(g => g.Items).OrderBy(i => i.Title).ToList();
            DataContext = Items;

            // Populate the ObservableCollection with sample data
            ObservableCollection<CustomDataObject> Lotos = new ObservableCollection<CustomDataObject>()
            {
                new CustomDataObject() { Title = "Viva", ImageLocation = "/Assets/proveedores/viva.png" },
                new CustomDataObject() { Title = "Altice", ImageLocation = "/Assets/proveedores/altice_blanco.png" },
                new CustomDataObject() { Title = "Claro", ImageLocation = "/Assets/proveedores/claro_2.png" },
            };

            // Set the ItemsSource of the GridView
            BasicGridView.ItemsSource = Lotos;
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
