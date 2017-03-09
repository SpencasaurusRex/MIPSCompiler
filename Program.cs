using System;
using System.Diagnostics;

namespace MIPSCompiler
{
    class Program
    {
        static int index = 0;

        // List of valid operators
        static Operation[] allOperations = {
            new Operation('=', 3, "Not supported"),
            new Operation('+', 2, "add"), new Operation('-', 2, "sub"),
            new Operation('*', 1, "mul"), new Operation('/', 1, "div") };

        static void Main(string[] args)
        {
            string line = string.Empty;
            for (int i = 0; i < args.Length; i++)
            {
                line += args[i];
            }

            // Remove all whitespace
            line = line.Replace(" ", string.Empty);

            // Convert to expression tree
            Node parent = CreateNode(line);

            // Turn expression tree into MIPS
            Console.WriteLine(parent.ToMIPS());
        }

        static Node CreateNode(String line)
        {
            // If we received a line surrounded in parentheses, and none inside we can safely remove them
            if (line.StartsWith("(") && line.EndsWith(")"))
            {
                String inner = line.Substring(1, line.Length - 2);
                if (!inner.Contains("(") && !inner.Contains(")"))
                {
                    line = inner;
                }
            }

            // Assume well formed (TODO: verify this)

            // Scan for the highest,rightest operation
            char[] charLine = line.ToCharArray();
            int highestPriority = 0;
            Operation highestPriorityOperation = null;
            int operationCharIndex = -1;
            int parenthesesDepth = 0;

            for (int charIndex = charLine.Length - 1; charIndex >= 0; charIndex--)
            {
                for (int operationIndex = 0; parenthesesDepth == 0 && operationIndex < allOperations.Length; operationIndex++)
                {
                    // If it's an operation check its priority
                    if (charLine[charIndex] == allOperations[operationIndex].character &&
                        allOperations[operationIndex].priority > highestPriority)
                    {
                        highestPriorityOperation = allOperations[operationIndex];
                        highestPriority = highestPriorityOperation.priority;
                        operationCharIndex = charIndex;
                    }
                }

                // As we are reversing through the string, right parentheses increases depth and vice versa
                if (charLine[charIndex] == ')')
                {
                    parenthesesDepth++;
                }
                else if (charLine[charIndex] == '(')
                {
                    parenthesesDepth--;
                }
            }

            // TODO: verify parenthesesDepth now at 0

            // Return a node with just the line if there are no operators found
            if (operationCharIndex == -1)
            {
                return new Node(line);
            }

            // Split based line on operation and recursively create a node for both sides
            String left = line.Substring(0, operationCharIndex);
            Node leftNode = CreateNode(left);

            String right = line.Substring(operationCharIndex + 1);
            Node rightNode = CreateNode(right);

            Node parent = new Node(charLine[operationCharIndex].ToString());
            parent.operation = highestPriorityOperation;
            parent.left = leftNode;
            parent.right = rightNode;

            if (parent.left.IsBranch() || parent.right.IsBranch())
            {
                // TODO: Replace this with duplication tracking 
                // (don't steal an id if it's duplicated elsewhere)
                parent.id = 0;
            }
            else
            {
                // TODO: Replace this with register tracking
                // (Reuse ids if they are freed, when parent is evaluated?)
                parent.id = index++;
            }
            return parent;
        }
    }
}