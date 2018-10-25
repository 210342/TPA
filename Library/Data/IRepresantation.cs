﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data
{
    interface IRepresantation
    {
        string Name { get; }
        IEnumerable<string> Print();
    }
}
