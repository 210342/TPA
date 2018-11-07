﻿using Library.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using System.Diagnostics;
using Tracing;

namespace Library.Logic.ViewModel
{
    public class ClassPresenter : INotifyPropertyChanged
    {
        #region Fields
        private IRepresentation objectSelected;

        private IRepresentation ObjectToDisplay;
        #endregion

        #region Properties
        public ICommand ShowCurrentObject { get; }
        public ICommand ReloadAssemblyCommand { get; }


        public ObservableCollection<IRepresentation> ObjectsList { get; }
        public IRepresentation ObjectSelected
        {
            get
            {
                return objectSelected;
            }
            set
            {
                PreviousSelection = objectSelected;
                objectSelected = value;
                OnPropertyChanged();
            }
        }
        public IRepresentation PreviousSelection { get; private set; }
        private string _loadedAssembly;
        public string LoadedAssembly { get
            {
                return _loadedAssembly;
            }
            set
            {
                _loadedAssembly = value;
                OnPropertyChanged();
            }
        }
        public IRepresentation LoadedAssemblyRepresentation { get; private set; }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public ClassPresenter()
        {
            /*
            Trace.Listeners.Add(new DbTraceListener(
                System.IO.Path.GetDirectoryName(
                    Assembly.GetAssembly(typeof(DbTraceListener)).Location) + @"\connConfig.xml"));
                    */
            ShowCurrentObject = new RelayCommand(ChangeClassToDisplay, () => ObjectSelected != null);
            ObjectsList = new ObservableCollection<IRepresentation>() { null };
            ReloadAssemblyCommand = new RelayCommand(ReloadAssembly, () => !string.IsNullOrEmpty(LoadedAssembly));
        }

        private void LoadAssembly()
        {
            Reflector reflector = new Reflector(LoadedAssembly);
            LoadedAssemblyRepresentation = reflector.AssemblyModel;
        }

        private void ReloadAssembly()
        {
            LoadAssembly();
            ObjectsList.Clear();
            if(LoadedAssemblyRepresentation != null)
            {
                ObjectsList.Add(LoadedAssemblyRepresentation);
                ObjectSelected = LoadedAssemblyRepresentation;
            }
        }
        public void ChangeClassToDisplay()
        {
            ObjectToDisplay = ObjectSelected;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void InteractWithTreeItem(int index)
        {
            if (index < 0 || index >= ObjectsList.Count())
            {
                throw new IndexOutOfRangeException(nameof(index));
            }
            else 
            {
                IRepresentation item = ObjectsList.ElementAt(index);
                if (item.Children.Count() != 0)
                {
                    List<IRepresentation> laterItems = ObjectsList.Skip(index + 1)
                        .Take(ObjectsList.Count() - index - 1).ToList();
                    if (laterItems.Contains(item.Children.First()))
                    {
                        CloseTreeItem(item, index, laterItems);
                    }
                    else
                    {
                        OpenTreeItem(item, index);
                    }
                }
            }
        }

        private void OpenTreeItem(IRepresentation item, int index)
        {
            foreach (IRepresentation kid in item.Children)
            {
                ObjectsList.Insert(++index, kid);
            }
        }

        private void CloseTreeItem(IRepresentation item, int index, List<IRepresentation> laterItems)
        {
            foreach (IRepresentation kid in item.Children)
            {
                if (laterItems.Contains(kid))
                {
                    int kidIndex = laterItems.IndexOf(kid);
                    CloseTreeItem(kid, index + 1, laterItems);
                    ObjectsList.RemoveAt(index + 1); // one after the parent
                }
            }
        }
    }
}
