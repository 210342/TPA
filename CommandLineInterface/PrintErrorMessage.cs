using System;
using Library.Logic.ViewModel;

namespace CommandLineInterface
{
    public class PrintErrorMessage : IErrorMessageBox
    {
        public void ShowMessage(string title, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(title.ToUpper());
            Console.ResetColor();
            Console.WriteLine(message);
        }

        public void CloseApp()
        {
            Console.WriteLine("Fatal error occured, application will now exit.");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}