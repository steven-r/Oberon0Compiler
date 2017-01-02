using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Oberon0.Compiler;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Statements;
using Oberon0.Generator.Msil.PredefinedFunctions;

namespace Oberon0.Generator.Msil
{
    /// <summary>
    /// The code generator to generate RISC code
    /// </summary>
    public partial class CodeGenerator
    {
        /// <summary>
        /// The default namespace use for standard functions like <c>WriteInt</c>, ...
        /// </summary>
        public const string DefaultNamespace = "$DEFAULT";

        private static readonly Dictionary<int, string> RelRevJumpMapping =
            new Dictionary<int, string>
            {
                {OberonGrammarLexer.EQUAL, "bne.un"},
                {OberonGrammarLexer.LT, "bge.un"},
                {OberonGrammarLexer.LE, "bgt.un"},
                {OberonGrammarLexer.GT, "ble.un"},
                {OberonGrammarLexer.GE, "blt.un"},
                {OberonGrammarLexer.NOTEQUAL, "beq.un"},
                {OberonGrammarLexer.NOT, "brfalse"},
            };

        private readonly Module _module;


        /// <summary>
        /// Initializes a new instance of the <see cref="CodeGenerator"/> class.
        /// </summary>
        /// <param name="module">The parsed module.</param>
        public CodeGenerator(Module module)
        {
            OutputBuffer = new StringBuilder();
            _module = module;
            Code = new Code(OutputBuffer);
        }

        private StringBuilder OutputBuffer { get; }

        /// <summary>
        /// The generated code
        /// </summary>
        /// <value>The code.</value>
        public Code Code { get; }

        public void Generate()
        {
            StandardFunctionRepository.Initialize(_module);
            Code.Reference("mscorlib");
            Code.Reference("Oberon0.System");
            Code.StartAssembly(_module.Name);
            Code.EmitComment("Code compiled for module " + _module.Name);
            Code.StartModule(_module.Name);
            Code.StartClass($"Oberon0.{_module.Name}");
            ProcessMainBlock(_module.Block);
            Code.EndMethod();
            Code.EndClass();
        }

        private void ProcessMainBlock(Block block)
        {
            ProcessDeclarations(block, true);
            foreach (var functionDeclaration in block.Procedures)
            {
                if (functionDeclaration.IsInternal)
                    continue;
                Code.StartMethod(functionDeclaration);
                ProcessDeclarations(functionDeclaration.Block);
                ProcessStatements(functionDeclaration.Block);
                Code.EndMethod();
            }
            Code.StartMainMethod();
            ProcessStatements(block);
        }

        private void ProcessStatements(Block block)
        {
            InitComplexData(block);
            foreach (var statement in block.Statements)
            {
                var assignment = statement as AssignmentStatement;
                if (assignment != null)
                {
                    StartStoreVar(block, assignment.Variable, assignment.Selector);
                    ExpressionCompiler(block, assignment.Expression);
                    StoreVar(block, assignment.Variable, assignment.Selector);
                }
                var ifStmt = statement as IfStatement;
                if (ifStmt != null)
                    GenerateIfStatement(ifStmt, block);
                var whileStmt = statement as WhileStatement;
                if (whileStmt != null)
                    GenerateWhileStatement(whileStmt, block);
                var repeatStmt = statement as RepeatStatement;
                if (repeatStmt != null)
                    GenerateRepeatStatement(repeatStmt, block);
                var callStmt = statement as ProcedureCallStatement;
                if (callStmt != null)
                    CallProcedure(callStmt, block);
            }
        }

        private void GenerateIfStatement(IfStatement stmt, Block block)
        {
            string endLabel = Code.GetLabel();
            Code.EmitComment("IF");
            for (int i = 0; i < stmt.Conditions.Count; i++)
            {
                string nextLabel = Code.GetLabel();
                BinaryExpression bin = (BinaryExpression)ExpressionCompiler(block, stmt.Conditions[i]);
                Code.Branch(RelRevJumpMapping[bin.Operator], nextLabel);
                ProcessStatements(stmt.ThenParts[i]);
                Code.Branch("br", endLabel);
                Code.EmitLabel(nextLabel);
            }
            if (stmt.ElsePart != null)
                ProcessStatements(stmt.ElsePart);
            Code.EmitLabel(endLabel);
        }

        private void GenerateWhileStatement(WhileStatement stmt, Block block)
        {
            string endLabel = Code.GetLabel();
            Code.EmitComment("WHILE");
            string label = Code.EmitLabel();
            BinaryExpression bin = (BinaryExpression)ExpressionCompiler(block, stmt.Condition);
            Code.Branch(RelRevJumpMapping[bin.Operator], endLabel);
            ProcessStatements(stmt.Block);
            Code.Branch("br", label);
            Code.EmitLabel(endLabel);
        }

        private void GenerateRepeatStatement(RepeatStatement stmt, Block block)
        {
            Code.EmitComment("REPEAT");
            string label = Code.EmitLabel();
            ProcessStatements(stmt.Block);
            BinaryExpression bin = (BinaryExpression)ExpressionCompiler(block, stmt.Condition);
            Code.Branch(RelRevJumpMapping[bin.Operator], label);
        }


        private void CallProcedure(ProcedureCallStatement call, Block block)
        {
            Code.EmitComment("Call " + call.FunctionDeclaration.Name);
            int i = 0;
            if (call.FunctionDeclaration.IsInternal)
            {
                CallInternalFunction(call, block);
            }
            else
            {
                foreach (ProcedureParameter parameter in call.FunctionDeclaration.Block.Declarations.OfType<ProcedureParameter>())
                {
                    if (parameter.IsVar)
                    {
                        VariableReferenceExpression reference = (VariableReferenceExpression)call.Parameters[i];
                        Load(block, reference.Declaration, reference.Selector);
                    }
                    else
                    {
                        ExpressionCompiler(block, call.Parameters[i]);
                    }
                    i++;
                }
                Code.Call(call.FunctionDeclaration);
            }
        }

        private void CallInternalFunction(ProcedureCallStatement call, Block block)
        {
            var function = StandardFunctionRepository.Get(call.FunctionDeclaration);
            function.Instance.Generate(function, this, call.FunctionDeclaration, call.Parameters, block);
        }

        public void DumpCode(TextWriter writer)
        {
            writer.Write(OutputBuffer.ToString());
        }
    }
}
