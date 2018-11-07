using System;
using System.IO;
using System.Linq;
using Library.Logic.ViewModel;

namespace CommandLineInterface
{
    public class CommandLineInterface
    {
        private ClassPresenter dataContext = new ClassPresenter();

        public void Start(string dllPath)
        {
            try
            {
                dataContext.LoadedAssembly = dllPath;
                dataContext.ReloadAssemblyCommand.Execute(null);
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
                Console.WriteLine(dataContext.ObjectSelected.ToString()); // print detailed info
                bool isIncorrectInput = false; // flag used to control application's flow
                do
                {
                    try
                    {
                        Console.WriteLine("___________________________________________________");
                        Console.Write("Your selection (type \"quit\" to leave application): ");
                        selection = Console.ReadLine();
                        if(!Quit(selection))
                        {
                            int index = int.Parse(selection); // try to read chosen index
                            dataContext.InteractWithTreeItem(index); // interact with that item
                            isIncorrectInput = false; // get out of the loop
                        }
                    }
                    catch(FormatException)
                    {
                        Console.WriteLine("Incorrect option \nPossible options: \n-> indexes written above objects \n-> quit");
                        dataContext.ObjectSelected = dataContext.PreviousSelection; // retrieve previous selection
                        isIncorrectInput = true;
                    }
                    catch(ArgumentOutOfRangeException)
                    {
                        Console.WriteLine("Incorrect option \nUndefined index");
                        dataContext.ObjectSelected = dataContext.PreviousSelection; // retrieve previous selection
                        isIncorrectInput = true;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Console.WriteLine("Incorrect option \nUndefined index");
                        dataContext.ObjectSelected = dataContext.PreviousSelection; // retrieve previous selection
                        isIncorrectInput = true;
                    }
                    catch (ArgumentNullException)
                    {
                        Console.WriteLine("This object doesn't have a parent");
                        dataContext.ObjectSelected = dataContext.PreviousSelection; // retrieve previous selection
                        isIncorrectInput = true;
                    }
                }
                while (isIncorrectInput );
            }
            while(!Quit(selection));
        }

        private bool Quit(string input)
        {
            return input.ToLower() == "quit" || input.ToLower() == "q";
        }
    }
}
