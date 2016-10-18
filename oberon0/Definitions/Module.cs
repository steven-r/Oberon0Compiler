namespace Oberon0.Compiler.Definitions
{
    public class Module
    {
        public string Name { get; set; }
        public Block Block { get; set; }

        public Module()
        {
            Block = new Block();
            Block.Types.Add(new SimpleTypeDefinition(BaseType.IntType, "INTEGER"));
            Block.Types.Add(new SimpleTypeDefinition(BaseType.StringType, "STRING"));
            Block.Types.Add(new SimpleTypeDefinition(BaseType.DecimalType, "REAL"));
        }
    }
}