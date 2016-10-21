﻿using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Solver;

namespace Oberon0.Compiler.Expressions
{
    public class ConstantIntExpression : ConstantExpression
    {
        public int Value { get; }

        public ConstantIntExpression(int value)
            : base(BaseType.IntType)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString("G");
        }
    }
}