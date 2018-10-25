﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data
{
    public interface IRepresantation
    {
        string Name { get; }
        string FullName { get; }
        IEnumerable<string> Print();
    }
}
