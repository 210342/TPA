﻿using Library.Logic.ViewModel;
using Microsoft.Win32;
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

namespace GraphicalUserInterface
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItemHelper.Content = e.NewValue;
        }

        private void FileDialog(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = "dll";
            dialog.Filter = "Dynamically linked library (*dll)|*dll|All Files(*.*)|*.*";
            if(dialog.ShowDialog().Value)
            {
                var viewModel = DataContext as ViewModel;
                viewModel.LoadedAssembly = dialog.FileName;
                viewModel.ReloadAssemblyCommand.Execute(null);
            }
        }
    }
}
