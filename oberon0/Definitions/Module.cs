namespace Oberon0.Compiler.Definitions
{
    public partial class Module
    {
        public string Name { get; set; }
        public Block Block { get; set; }

        public Module()
        {
            Block = new Block();

            DeclareStandardTypes();
            DeclareStandardConsts();
            DeclareStandardFunctions();
        }
    }
}