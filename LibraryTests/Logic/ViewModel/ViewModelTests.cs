using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Library.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelContract;
using VM = Library.Logic.ViewModel;

namespace LibraryTests.Logic.ViewModel.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ViewModelTests
    {
        private readonly VM.ViewModel _vm = new VM.ViewModel(false);

        [TestMethod]
        public void ReloadAssemblyChangesAssembly()
        {
            VM.ViewModel vm = new VM.ViewModel(false);
            IMetadata loadedAssemblyRepr = vm.LoadedAssemblyRepresentation;
            vm.LoadedAssembly = GetType().Assembly.Location;
            vm.ReloadAssemblyCommand.Execute(null);
            IMetadata reloadedAssemblyRepr = vm.LoadedAssemblyRepresentation;
            Assert.AreNotEqual(loadedAssemblyRepr, reloadedAssemblyRepr);
        }

        [TestMethod]
        public void ReloadAsseblyListPopulated()
        {
            VM.ViewModel vm = new VM.ViewModel(false)
            {
                LoadedAssembly = typeof(VM.ViewModel).Assembly.Location
            };
            vm.ReloadAssemblyCommand.Execute(null);
            Assert.AreNotEqual(0, vm.ObjectsList.Count);
        }

        [TestMethod]
        public void ObjectToDisplayChanges()
        {
            VM.ViewModel vm = new VM.ViewModel(false)
            {
                ObjectSelected = new VM.TypeItem(new TypeMetadata(typeof(Type)))
            };
            vm.ShowCurrentObject.Execute(null);
            Assert.AreEqual(vm.ObjectSelected, vm.ObjectToDisplay);
        }

        [TestMethod]
        public void OpenFileWorks()
        {
            VM.ViewModel vm = new VM.ViewModel(false)
            {
                OpenFileSourceProvider = new TestClass()
            };
            vm.OpenFileCommand.Execute(null);
            Assert.IsNotNull(vm.LoadedAssembly);
        }

        [TestMethod]
        public void ImportTracerTest()
        {
            VM.ViewModel vm = new VM.ViewModel(true);
            vm.EndInit();
            Assert.IsNotNull(vm.Tracer);
        }

        private class TestClass : VM.ISourceProvider
        {
            public bool GetAccess()
            {
                return true;
            }

            public string GetPath()
            {
                return Assembly.GetAssembly(GetType()).Location;
            }
        }
    }
}