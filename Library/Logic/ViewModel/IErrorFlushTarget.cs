﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Logic.ViewModel
{
    public interface IErrorFlushTarget
    {
        void SendMessage(string title, string message);
        void CloseApp();
    }
}