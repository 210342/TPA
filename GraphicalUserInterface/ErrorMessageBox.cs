﻿using Library.Logic.ViewModel;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalUserInterface
{
    public class ErrorMessageBox : IErrorMessageBox
    {
        public void ShowMessage(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Cancel);
        }

        public void CloseApp()
        {
            Application.Current.Shutdown();
        }
    }
}
