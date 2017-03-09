namespace MIPSCompiler
{
    public class Node
    {
        public Operation operation;
        public string data;
        public Node left;
        public Node right;
        public int id;

        public Node(string data)
        {
            this.data = data;
        }

        public string ToMIPS()
        {
            if (IsLeaf())
            {
                return "";
            }
            string leftAlias = left.IsLeaf() ? left.data : "$t" + left.id;
            string rightAlias = right.IsLeaf() ? right.data : "$t" + right.id;
            string MIPS = operation.MIPS + " $t" + id + ", " + leftAlias + ", " + rightAlias + "\n";
            return left.ToMIPS() + right.ToMIPS() + MIPS;
        }

        public bool IsLeaf()
        {
            return left == null && right == null;
        }

        public bool IsBranch()
        {
            return left != null || right != null;
        }
    }
}