using System;
using System.IO;
using System.Linq;
using Library.Logic.ViewModel;

namespace CommandLineInterface
{
    public class CommandLineInterface
    {
        private ClassPresenter dataContext;

        public void Start(string dllPath)
        {
            try
            {
                dataContext = new ClassPresenter();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File with given path doesn't exist \n {0}", e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("Couldn't find a directory \n{0}", e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Insufficient access rights to read the file \n{0}", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown problem occured. \n{0}", e.Message);
            }
            Menu();
        }

        private void Menu()
        {
            string selection = "";
            do
            {
                for(int i = 0; i < dataContext.ObjectsList.Count(); ++i) // print children
                {
                    Console.WriteLine($"INDEX: {i}");
                    Console.WriteLine(dataContext.ObjectsList.ElementAt(i).GetType().ToString().Split('.').Last().Replace("Representation", ""));
                    Console.WriteLine(dataContext.ObjectsList.ElementAt(i).FullName);
                    Console.WriteLine();
                }
                Console.WriteLine(dataContext.ClassSelected.ToString()); // print detailed info
                bool isIncorrectInput = true; // flag used to control application's flow
                do
                {
                    try
                    {
                        Console.WriteLine("___________________________________________________");
                        Console.Write("Your selection (type \"quit\" to leave application): ");
                        selection = Console.ReadLine();
                        int index = int.Parse(selection); // try to read chosen index
                        dataContext.ClassSelected = dataContext.ObjectsList.ElementAt(index); // get an item under input index
                        dataContext.InteractWithTreeItem(dataContext.ClassSelected); // interact with that item
                        isIncorrectInput = false; // get out of the loop
                    }
                    catch(FormatException)
                    {
                        Console.WriteLine("Incorrect option \nPossible options: \n-> indexes written above objects \n-> parent \n-> quit");
                        dataContext.ClassSelected = dataContext.PreviousSelection; // retrieve previous selection

                    }
                    catch(ArgumentOutOfRangeException)
                    {
                        Console.WriteLine("Incorrect option \nUndefined index");
                        dataContext.ClassSelected = dataContext.PreviousSelection; // retrieve previous selection
                    }
                    catch(ArgumentNullException)
                    {
                        Console.WriteLine("This object doesn't have a parent");
                        dataContext.ClassSelected = dataContext.PreviousSelection; // retrieve previous selection
                    }
                }
                while (isIncorrectInput && !Quit(selection));
            }
            while(!Quit(selection));
        }

        private bool Quit(string input)
        {
            return input.ToLower() == "quit" || input.ToLower() == "q";
        }
    }
}
