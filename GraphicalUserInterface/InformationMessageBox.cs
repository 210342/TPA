﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Library.Logic.ViewModel;

namespace GraphicalUserInterface
{
    public class InformationMessageBox : IInformationMessageTarget
    {
        public void SendMessage(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
