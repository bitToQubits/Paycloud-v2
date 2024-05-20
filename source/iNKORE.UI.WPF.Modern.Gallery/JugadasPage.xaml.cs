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

namespace iNKORE.UI.WPF.Modern.Gallery
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>

    public sealed partial class JugadasPage : ItemsPageBase
    {
        public JugadasPage()
        {
            InitializeComponent();
            // this.DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var menuItem = NavigationRootPage.Current.NavigationView.MenuItems.Cast<NavigationViewItem>().ElementAt(2);
            menuItem.IsSelected = true;
            NavigationRootPage.Current.NavigationView.Header = string.Empty;

            Items = ControlInfoDataSource.Instance.Groups.SelectMany(g => g.Items).OrderBy(i => i.Title).ToList();
            DataContext = Items;

            // Populate the ObservableCollection with sample data
            ObservableCollection<CustomDataObject> Lotos = new ObservableCollection<CustomDataObject>()
            {
                new CustomDataObject() { Title = "8:52 p. m. mié, 01 may", ImageLocation = "/Assets/lotos/laprimeralotodom.png" },
                new CustomDataObject() { Title = "00D 09H 16M 22S", ImageLocation = "/Assets/lotos/lasuertedominicana.png" },
                new CustomDataObject() { Title = "00D 09H 16M 08S", ImageLocation = "/Assets/lotos/lotenal.png" },
                new CustomDataObject() { Title = "00D 09H 15M 58S", ImageLocation = "/Assets/lotos/lotopool.png" },
                new CustomDataObject() { Title = "00D 02H 49M 42S", ImageLocation = "/Assets/lotos/quinielaloteka.png" },
                new CustomDataObject() { Title = "8:00 p. m. sáb, 25 may", ImageLocation = "/Assets/lotos/quinielapale.png" },
                new CustomDataObject() { Title = "8:00 p. m. sáb, 25 may", ImageLocation = "/Assets/lotos/sev.png" },
                new CustomDataObject() { Title = "8:00 p. m. sáb, 25 may", ImageLocation = "/Assets/lotos/supermas.png" }
            };

            // Set the ItemsSource of the GridView
            BasicGridView.ItemsSource = Lotos;
        }
    }
}
