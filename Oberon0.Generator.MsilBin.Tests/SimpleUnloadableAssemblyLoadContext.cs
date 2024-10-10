#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Reflection;
using System.Runtime.Loader;

// ReSharper disable IdentifierTypo

namespace Oberon0.Generator.MsilBin.Tests
{
    internal class SimpleUnloadableAssemblyLoadContext() : AssemblyLoadContext(true)
    {
        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null;
        }
    }
}
