using System;
using System.Reflection;
using Library.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibraryTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AssemblyLoadTest()
        {
            string path = System.IO.Path.GetDirectoryName(Assembly.GetCallingAssembly().Location) + @"\test.dll";
            LoadDllTestsClass.LoadAssembly(path);
            LoadDllTestsClass.MemberTypes();
            int keyOne = LoadDllTestsClass.DictSize();
            LoadDllTestsClass.MemberTypes();
            int keyTwo = LoadDllTestsClass.DictSize();
            Console.WriteLine(keyOne + "    " + keyTwo);
            Assert.AreEqual(keyOne, keyTwo);
        }
    }
}
