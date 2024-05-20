﻿using iNKORE.UI.WPF.Modern.Controls;
using iNKORE.UI.WPF.Modern.Controls.Helpers;
using iNKORE.UI.WPF.Modern.Controls.Primitives;
using iNKORE.UI.WPF.Modern.Helpers.Styles;
using iNKORE.UI.WPF.Modern.Gallery.SamplePages;
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

namespace iNKORE.UI.WPF.Modern.Gallery.ControlPages
{
    /// <summary>
    /// SystemBackdropsPage.xaml 的交互逻辑
    /// </summary>
    public partial class SystemBackdropsPage : Page
    {
        public SystemBackdropsPage()
        {
            InitializeComponent();
        }

        private void createMicaWindow_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new SampleSystemBackdropsWindow();
            newWindow.Show();
        }

        private void createAcrylicWindow_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new SampleSystemBackdropsWindow();
            WindowHelper.SetSystemBackdropType(newWindow,BackdropType.Acrylic);
            newWindow.Show();
        }
    }
}
