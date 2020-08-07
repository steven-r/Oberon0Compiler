#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Oberon0.Generator.MsilBin.Tests
{
    /// <summary>
    /// Runner class to execute compiled assemblies in memory
    /// </summary>
    /// <remarks>
    /// When using <b>xcode</b> please ensure that tests are executed sequential. Please use <code>[Colletion("Sequential")]</code>
    /// as a class attribute.
    /// </remarks>
    internal static class Runner
    {
        internal static void Execute(byte[] compiledAssembly, TextWriter output, TextReader input = null, TextWriter error = null, params string[] args)
        {
            var assemblyLoadContextWeakRef = LoadAndExecute(compiledAssembly, output, input, error, args);

            for (var i = 0; i < 8 && assemblyLoadContextWeakRef.IsAlive; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            if (assemblyLoadContextWeakRef.IsAlive)
            {
                Console.WriteLine("Unloading failed!");
            }
            else
            {
                Console.WriteLine("Unloading failed!");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static WeakReference LoadAndExecute(byte[] compiledAssembly, TextWriter output, TextReader input = null, TextWriter error = null, params string[] args)
        {
            var assemblyLoadContext = new SimpleUnloadableAssemblyLoadContext();
            
            using var asm = new MemoryStream(compiledAssembly);

            var assembly = assemblyLoadContext.LoadFromStream(asm);

            var currentIn = Console.In;
            var currentOut = Console.Out;
            var currentError = Console.Error;

            try
            {
                if (error != null) Console.SetError(error);
                if (input != null) Console.SetIn(input);
                if (output != null) Console.SetOut(output);

                var entry = assembly.EntryPoint;

                if (entry == null)
                {
                    throw new InvalidOperationException("No entrypoint found in " + assembly.FullName);
                }

                _ = entry.GetParameters().Length > 0
                    ? entry.Invoke(null, new object[] {args})
                    : entry.Invoke(null, null);

                assemblyLoadContext.Unload();
            }
            finally
            {
                Console.SetIn(currentIn);
                Console.SetOut(currentOut);
                Console.SetError(currentError);
            } 

            return new WeakReference(assemblyLoadContext);
        }
    }
}
