using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oberon0.Compiler.Expressions.Operations
{
    using JetBrains.Annotations;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions.Constant;
    using Oberon0.Compiler.Expressions.Operations.Internal;
    using Oberon0.Compiler.Types;

    [ArithmeticOperation(OberonGrammarLexer.OR, BaseTypes.Bool, BaseTypes.Bool, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.AND, BaseTypes.Bool, BaseTypes.Bool, BaseTypes.Bool)]
    [UsedImplicitly]
    internal class OpRelOp2 : BinaryOperation
    {
        protected override Expression BinaryOperate(BinaryExpression bin, Block block, IArithmeticOpMetadata operationParameters)
        {
            if (bin.LeftHandSide.IsConst && bin.RightHandSide.IsConst)
            {
                var left = (ConstantExpression)bin.LeftHandSide;
                var right = (ConstantExpression)bin.RightHandSide;
                bool res = false;
                switch (operationParameters.Operation)
                {
                    case OberonGrammarLexer.AND:
                        res = left.ToBool() && right.ToBool();
                        break;
                    case OberonGrammarLexer.OR:
                        res = left.ToBool() || right.ToBool();
                        break;
                }

                return new ConstantBoolExpression(res);
            }

            return bin; // expression remains the same
        }
    }
}
