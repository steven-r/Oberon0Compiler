using System;
using System.Collections.Generic;
using Oberon0.Compiler;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Generator.Msil
{
    public partial class CodeGenerator
    {
        private static readonly Dictionary<TokenType, Func<CodeGenerator, Block, BinaryExpression, BinaryExpression>> OperatorMapping = 
            new Dictionary<TokenType, Func<CodeGenerator, Block, BinaryExpression, BinaryExpression>>
            {
                {TokenType.Add, HandleSimpleOperation},
                {TokenType.Sub, HandleSimpleOperation},
                {TokenType.Mul, HandleSimpleOperation},
                {TokenType.Div, HandleSimpleOperation},
                {TokenType.Mod, HandleSimpleOperation},
                {TokenType.Unary, HandleSimpleOperation},
                {TokenType.Equals, HandleRelOperation},
                {TokenType.NotEquals, HandleRelOperation},
                {TokenType.GT, HandleRelOperation},
                {TokenType.GE, HandleRelOperation},
                {TokenType.LT, HandleRelOperation},
                {TokenType.LE, HandleRelOperation},
                {TokenType.Not, HandleRelOperation},
            };

        private static readonly Dictionary<TokenType, string> SimpleInstructionMapping = 
            new Dictionary<TokenType, string>
            {
                {TokenType.Add, "add" },
                {TokenType.Div, "div" },
                {TokenType.Mul, "mul" },
                {TokenType.Sub, "sub" },
                {TokenType.Mod, "rem" },
            };

        private static BinaryExpression HandleSimpleOperation(CodeGenerator generator, Block block, BinaryExpression bin)
        {
            generator.ExpressionCompiler(block, bin.LeftHandSide);
            if (!bin.IsUnary)
                generator.ExpressionCompiler(block, bin.RightHandSide);
            generator.Code.WriteLine($"\t{SimpleInstructionMapping[bin.Operator]}");
            return bin;
        }

        private static BinaryExpression HandleRelOperation(CodeGenerator generator, Block block, BinaryExpression bin)
        {
            generator.ExpressionCompiler(block, bin.LeftHandSide);
            if (!bin.IsUnary)
                generator.ExpressionCompiler(block, bin.RightHandSide);
            return bin;
        }

        /// <summary>
        /// Compile the expression and return the register number where the result is stored
        /// </summary>
        /// <param name="block">The block.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>The register</returns>
        public Expression ExpressionCompiler(Block block, Expression expression)
        {
            var eInfo = new ExpressionGeneratorInfo();
            expression.GeneratorInfo = eInfo;
            var v = expression as VariableReferenceExpression;
            if (v != null)
                return HandleVariableReferenceExpression(v);
            var s = expression as StringExpression;
            if (s != null)
            {
                Code.WriteLine($"\tldstr \"{s.Value}\"");
                return null;
            }
            var bin = expression as BinaryExpression;
            if (bin != null)
            {
                Code.EmitComment(bin.ToString());
                return OperatorMapping[bin.Operator](this, block, bin);
            }
            var cons = expression as ConstantExpression;
            if (cons != null)
            {
                LoadConstantExpression(cons, null);
                return null;
            }
            var fc = expression as FunctionCallExpression;
            if (fc != null)
            {
                Code.EmitComment(fc.FunctionDeclaration.ToString());
                int i = 0;
                foreach (ProcedureParameter parameter in fc.FunctionDeclaration.Parameters)
                {
                    if (parameter.IsVar)
                    {
                        VariableReferenceExpression reference = (VariableReferenceExpression)fc.Parameters[i];
                        Code.Load(reference);
                    }
                    else
                    {
                        ExpressionCompiler(block, fc.Parameters[i]);
                    }
                    i++;
                }
                Code.Call(fc.FunctionDeclaration);
                return fc;
            }
            return null;
        }

        private void LoadConstantExpression(ConstantExpression cons, ConstDeclaration declaration)
        {
            if (declaration != null)
                Code.LoadConstRef(declaration);
            else
                Code.PushConst(cons.Value);
        }

        private Expression HandleVariableReferenceExpression(VariableReferenceExpression v)
        {
            if (v.IsConst)
            {
                LoadConstantExpression(((ConstDeclaration) v.Declaration).Value, (ConstDeclaration)v.Declaration);
                return null;
            }
            Code.Load(v);
            return null;
        }
    }
}
