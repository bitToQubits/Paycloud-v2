﻿using iNKORE.UI.WPF.Modern.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace iNKORE.UI.WPF.Modern.Gallery.SamplePages
{
    /// <summary>
    /// SampleCompactSizingPage.xaml 的交互逻辑
    /// </summary>
    public partial class SampleCompactSizingPage : Page
    {
        public TextBox FirstName => firstName;
        public TextBox LastName => lastName;
        public PasswordBox Password => password;
        public PasswordBox ConfirmPassword => confirmPassword;
        public DatePicker ChosenDate => chosenDate;

        public SampleCompactSizingPage()
        {
            InitializeComponent();
        }

        public void CopyState(SampleStandardSizingPage page)
        {
            FirstName.Text = page.FirstName.Text;
            LastName.Text = page.LastName.Text;
            Password.Password = page.Password.Password;
            ConfirmPassword.Password = page.ConfirmPassword.Password;
            ChosenDate.SelectedDate = page.ChosenDate.SelectedDate;
        }
    }
}
