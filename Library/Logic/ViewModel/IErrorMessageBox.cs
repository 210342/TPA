﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Logic.ViewModel
{
    public interface IErrorMessageBox
    {
        void ShowMessage(string title, string message);
        void CloseApp();
    }
}
