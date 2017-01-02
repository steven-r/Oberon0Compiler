﻿using System;

namespace Oberon0.Compiler.Tests
{
    internal class CompilerError
    {
        public int Line { get; set; }

        public int Column { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }
    }
}