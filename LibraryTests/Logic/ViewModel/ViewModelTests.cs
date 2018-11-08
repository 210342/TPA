using System;
using System.Diagnostics.CodeAnalysis;
using Library.Data.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VM = Library.Logic.ViewModel;

namespace LibraryTests.Logic.ViewModel
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ViewModelTests
    {
        [TestMethod]
        public void ReloadAssemblyChangesAssembly()
        {
            VM.ViewModel vm = new VM.ViewModel();
            vm.LoadedAssembly = typeof(VM.ViewModel).Assembly.Location;
            IMetadata loadedAssemblyRepr = vm.LoadedAssemblyRepresentation;
            vm.ReloadAssemblyCommand.Execute(null);
            IMetadata reloadedAssemblyRepr = vm.LoadedAssemblyRepresentation;
            Assert.AreNotEqual(loadedAssemblyRepr, reloadedAssemblyRepr);
        }
        [TestMethod]
        public void ReloadAsseblyListPopulated()
        {
            VM.ViewModel vm = new VM.ViewModel();
            vm.LoadedAssembly = typeof(VM.ViewModel).Assembly.Location;
            vm.ReloadAssemblyCommand.Execute(null);
            Assert.AreNotEqual(0, vm.ObjectsList.Count);
        }
    }
}
