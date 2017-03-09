using System;
using System.Diagnostics;

namespace MIPSCompiler
{
    class Program
    {
        static int index = 0;

        // List of valid operators
        static Operation[] operations = {
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

            // Assume well formed
            // Remove all whitespace
            line = line.Replace(" ", string.Empty);

            // Convert to expression tree

            Node parent = CreateNode(line);

            // Turn expression tree into MIPS
            Console.WriteLine(parent.ToMIPS());
        }

        static Node CreateNode(String line)
        {
            // Scan for the highest,rightest operation
            char[] charLine = line.ToCharArray();
            int highestPriority = 0;
            Operation highestPriorityOperation = null;
            int operationCharIndex = -1;
            for (int charIndex = charLine.Length - 1; charIndex >= 0; charIndex--)
            {
                for (int operationIndex = 0; operationIndex < operations.Length; operationIndex++)
                {
                    // If it's an operation check its priority
                    if (charLine[charIndex] == operations[operationIndex].character &&
                        operations[operationIndex].priority > highestPriority)
                    {
                        highestPriorityOperation = operations[operationIndex];
                        highestPriority = highestPriorityOperation.priority;
                        operationCharIndex = charIndex;
                    }
                }
            }

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
            // If it's simple, it can inherit the ID of it's non-leaf child for simplification purposes
            if (parent.IsSimple())
            {
                if (parent.left.IsLeaf())
                {
                    parent.id = parent.right.id;
                }
                else
                {
                    parent.id = parent.left.id;
                }
            }
            else
            {
                parent.id = index++;
            }

            return parent;
        }
    }
}