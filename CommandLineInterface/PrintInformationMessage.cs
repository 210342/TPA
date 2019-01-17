using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Logic.ViewModel;

namespace CommandLineInterface
{
    internal class PrintInformationMessage : IInformationMessageTarget
    {
        public void SendMessage(string title, string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(title.ToUpper());
            Console.ResetColor();
            Console.WriteLine(message);
        }
    }
}
