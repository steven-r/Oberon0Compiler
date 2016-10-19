﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Solver;

namespace Oberon0.Compiler.Expressions.Arithmetic
{
    [Export(typeof(ICalculatable))]
    class SubExpression: BinaryExpression, ICalculatable
    {
        public SubExpression()
        {
            Operator = TokenType.Sub;
        }

        public override Expression Calc(Block block)
        {
            if (this.BinaryConstChecker(block) == null)
            {
                return null;
            }
            // 1. Easy as int
            var leftHandSide = (ConstantExpression)LeftHandSide;
            var rightHandSide = (ConstantExpression)RightHandSide;
            if (rightHandSide.BaseType == leftHandSide.BaseType && leftHandSide.BaseType == BaseType.IntType)
            {
                return new ConstantIntExpression(leftHandSide.ToInt32() - rightHandSide.ToInt32());
            }
            // at least one of them is double
            return new ConstantDoubleExpression(leftHandSide.ToDouble() - rightHandSide.ToDouble());
        }
    }
}
