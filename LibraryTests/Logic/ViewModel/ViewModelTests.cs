using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VM = Library.Logic.ViewModel;
using Library.Model;
using Library.Logic.ViewModel;
using Library.Logic.ViewModel;



namespace LibraryTests.Logic.ViewModel.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ViewModelTests
    {
        class TestClass : ISourceProvider
        {
            public bool GetAccess()
            {
                return true;
            }

            public string GetFilePath()
            {
                return System.Reflection.Assembly.GetAssembly(this.GetType()).Location;
            }
        }

        VM.ViewModel vm = new VM.ViewModel(false);
        [TestMethod]
        public void ReloadAssemblyChangesAssembly()
        {
            VM.ViewModel vm = new VM.ViewModel(false);
            IMetadata loadedAssemblyRepr = vm.LoadedAssemblyRepresentation;
            vm.LoadedAssembly = this.GetType().Assembly.Location;
            vm.ReloadAssemblyCommand.Execute(null);
            IMetadata reloadedAssemblyRepr = vm.LoadedAssemblyRepresentation;
            Assert.AreNotEqual(loadedAssemblyRepr, reloadedAssemblyRepr);
        }

        [TestMethod]
        public void ReloadAsseblyListPopulated()
        {
            VM.ViewModel vm = new VM.ViewModel(false);
            vm.LoadedAssembly = typeof(VM.ViewModel).Assembly.Location;
            vm.ReloadAssemblyCommand.Execute(null);
            Assert.AreNotEqual(0, vm.ObjectsList.Count);
        }

        [TestMethod]
        public void ObjectToDisplayChanges()
        {
            VM.ViewModel vm = new VM.ViewModel(false);
            vm.ObjectSelected = new TypeItem(new TypeMetadata(typeof(Type)));
            vm.ShowCurrentObject.Execute(null);
            Assert.AreEqual(vm.ObjectSelected, vm.ObjectToDisplay);
        }

        [TestMethod]
        public void OpenFileWorks()
        {
            VM.ViewModel vm = new VM.ViewModel(false);
            vm.OpenFileSourceProvider = new TestClass();
            vm.OpenFileCommand.Execute(null);
            Assert.IsNotNull(vm.LoadedAssembly);
        }
    }
}
