#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrontendTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil.Tests/FrontendTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Generator.Msil.Tests.Libraries
{
    using System;
    using System.IO;

    using NUnit.Framework;

    using Oberon0.Msil;

    [TestFixture]
    public class FrontendTests
    {
        [Test]
        public void TestEmptyArgsRun()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                Console.SetError(sw);

                Program.Main(new string[0]);

                Assert.That(sw.ToString(), Contains.Substring("A required value not bound to option name is missing."));
            }
        }

        [Test]
        public void TestFileNotFound()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                Console.SetError(sw);

                Program.Main(new[] { "dummy-file.ob0" });

                Assert.That(sw.ToString(), Contains.Substring("Cannot find dummy-file.ob0"));
            }
        }
    }
}