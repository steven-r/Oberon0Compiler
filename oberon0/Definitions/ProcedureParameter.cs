namespace Oberon0.Compiler.Definitions
{
    public class ProcedureParameter
    {
        public string Name { get; set; }

        public TypeDefinition Type { get; set; }

        public bool IsVar { get; set; }
        public ProcedureParameter(string name, TypeDefinition type, bool isVar)
        {
            Name = name;
            Type = type;
            IsVar = isVar;
        }
    }
}