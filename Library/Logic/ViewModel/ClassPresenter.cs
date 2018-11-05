using Library.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace Library.Logic.ViewModel
{
    public class ClassPresenter : INotifyPropertyChanged
    {
        #region Fields
        private IRepresentation classSelected;

        private IRepresentation ClassToDisplay;
        #endregion

        #region Properties
        public ICommand ShowCurrentClass { get; }
        public ICommand ReloadAssemblyCommand { get; }

        public IRepresentation ClassSelected
        {
            get
            {
                return classSelected;
            }
            set
            {
                classSelected = value;
                OnPropertyChanged("ClassSelected");
                //Messenger.Default.Send(new SelectedChangedMessage(currentlySelected)); TODO MESSENGER PATTERN
            }
        }

        public string LoadedAssembly { get; set; }
        public IRepresentation LoadedAssemblyRepresentation { get; private set; }

        public ObservableCollection<IRepresentation> ClassesList { get; }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public ClassPresenter()
        {
            ShowCurrentClass = new RelayCommand(ChangeClassToDisplay, () => ClassSelected != null);
            ClassesList = new ObservableCollection<IRepresentation>() { null };
            ReloadAssemblyCommand = new RelayCommand(RelodAssembly);
        }

        private void LoadAssembly()
        {
            Reflector reflector = new Reflector(LoadedAssembly);
            this.LoadedAssemblyRepresentation = reflector.AssemblyModel;
        }

        private void RelodAssembly()
        {
            LoadAssembly();
            this.ClassesList.Clear();
            if(this.LoadedAssemblyRepresentation != null)
                this.ClassesList.Add(this.LoadedAssemblyRepresentation);
        }

        public void ChangeClassToDisplay()
        {
            ClassToDisplay = ClassSelected;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
