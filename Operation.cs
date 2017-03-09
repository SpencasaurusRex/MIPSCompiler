namespace MIPSCompiler
{
    public class Operation
    {
        public char character;
        public int priority;
        public string MIPS;

        public Operation(char character, int priority, string MIPS)
        {
            this.character = character;
            this.priority = priority;
            this.MIPS = MIPS;
        }
    }
}