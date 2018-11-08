using System.Reflection;
// <copyright file="AssemblyMetadataTest.cs" company="Microsoft">Copyright © Microsoft 2018</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TPA.Reflection.Model;

namespace TPA.Reflection.Model.Tests
{
    [TestClass]
    [PexClass(typeof(AssemblyMetadata))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class AssemblyMetadataTest
    {
    }
}
