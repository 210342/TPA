using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Library.Logic.ViewModel;

namespace CommandLineInterface
{
    public class CommandLineInterface
    {
        private readonly ViewModel dataContext = new ViewModel();
        private const int startIndex = 0;
        private const string tab = "   ";
        private int maxIndex;
        private TreeViewItem root;
        private int selectionIndex; // used to iterate through items

        public void Start(string dllPath)
        {
            dataContext.OpenFileSourceProvider = new TextFileSourceProvider(dllPath); //source provider
            dataContext.SaveFileSourceProvider = new TextFileSourceProvider(dllPath); //source provider
            dataContext.ErrorMessageProvider = new PrintErrorMessage(); // 'error message box'
            dataContext.EndInit();
            try
            {
                dataContext.OpenFileCommand.Execute(null);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File with given path doesn't exist \n {0}", e.Message);
                Console.WriteLine("Application will now exit");
                Console.Read();
                return;
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("Couldn't find a directory \n{0}", e.Message);
                Console.WriteLine("Application will now exit");
                Console.Read();
                return;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Insufficient access rights to read the file \n{0}", e.Message);
                Console.WriteLine("Application will now exit");
                Console.Read();
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown problem occured. \n{0}", e.Message);
                Console.WriteLine("Application will now exit");
                Console.Read();
                return;
            }

            root = dataContext.ObjectsList.First();
            dataContext.ObjectsList.CollectionChanged += ObservableCollectionChangedEventHandler;
            Menu();
        }

        private void Menu()
        {
            string selection = "";
            do
            {
                if (root != null)
                {
                    maxIndex = 0; // reset index before each printout
                    Print(root, startIndex);
                    Console.WriteLine();
                    Console.WriteLine(dataContext.ObjectSelected.ToString()); // print detailed info
                    bool isIncorrectInput = false; // flag used to control application's flow
                    do
                    {
                        Console.WriteLine("___________________________________________________");
                        Console.Write("Your selection (type \"quit\" to leave application): ");
                        selection = Console.ReadLine();
                        try
                        {
                            if (!Quit(selection))
                            {
                                if (IsInputLoad(selection))
                                {
                                    Console.WriteLine("Please provide path to a file where model is saved:");
                                    string path = Console.ReadLine();
                                    dataContext.OpenFileSourceProvider = new TextFileSourceProvider(path);

                                    dataContext.LoadModel.Execute(null);
                                    try
                                    {
                                        root = dataContext.ObjectsList.First();
                                    }
                                    catch (InvalidOperationException)
                                    {
                                        root = null;
                                    }
                                }
                                else if (IsInputSave(selection))
                                {
                                    Console.WriteLine("Please provide path to a file where the model should be saved:");
                                    string path = Console.ReadLine();
                                    dataContext.SaveFileSourceProvider = new TextFileSourceProvider(path);
                                    dataContext.SaveModel.Execute(null);
                                }
                                else
                                {
                                    int index = int.Parse(selection); // try to read chosen index
                                    selectionIndex = 0; // reset index before selection
                                    dataContext.ObjectSelected =
                                        SelectItem(root, index); // get an item under input index
                                    if (dataContext.ObjectSelected == null)
                                        throw new IndexOutOfRangeException(nameof(index));
                                    dataContext.ObjectSelected.IsExpanded = !dataContext.ObjectSelected.IsExpanded;
                                    isIncorrectInput = false; // get out of the loop
                                }
                            }
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine(
                                "Incorrect option \nPossible options: \n-> indexes written above objects \n-> quit");
                            dataContext.ObjectSelected = dataContext.PreviousSelection; // retrieve previous selection
                            isIncorrectInput = true; // stay in the loop
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
                            isIncorrectInput = true; // stay in the loop
                        }
                    } while (isIncorrectInput);
                }
                else
                {
                    try
                    {
                        root = dataContext.ObjectsList.First();
                    }
                    catch (InvalidOperationException)
                    {
                        root = null;
                    }
                }
            } while (!Quit(selection));
            dataContext.AppClosing.Execute(null);
        }

        private bool Quit(string input)
        {
            return input.ToLower() == "quit" || input.ToLower() == "q";
        }

        private bool IsInputLoad(string input)
        {
            return input.ToLower() == "load" || input.ToLower() == "l";
        }

        private bool IsInputSave(string input)
        {
            return input.ToLower() == "save" || input.ToLower() == "s";
        }

        private void Print(TreeViewItem item, int depth)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < depth; ++i) sb.Append(tab);
            Console.WriteLine();
            Console.WriteLine($"{sb}INDEX: {maxIndex++}");
            Console.WriteLine($"{sb}{item.FullName}");
            if (item.IsExpanded)
                foreach (TreeViewItem kid in item.Children)
                    Print(kid, depth + 1);
        }

        private TreeViewItem SelectItem(TreeViewItem selected, int index)
        {
            if (selectionIndex++ == index) return selected;

            if (!selected.IsExpanded) return null;

            foreach (TreeViewItem kid in selected.Children)
            {
                TreeViewItem tmp = SelectItem(kid, index);
                if (tmp != null) return tmp;
            }

            return null;
        }

        private void ObservableCollectionChangedEventHandler(object sender, EventArgs a)
        {
            if (dataContext.ObjectsList.Any())
            {
                root = dataContext.ObjectsList.First();
            }
        }
    }
}