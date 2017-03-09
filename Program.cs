using System;
using System.Diagnostics;
using System.Collections.Generic;

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

        static int Depth(char character)
        {
            if (character == '(')
            {
                return 1;
            }
            else if (character == ')')
            {
                return -1;
            }
            else return 0;
        }

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
            char[] charLine = line.ToCharArray();
            int parenthesesDepth = 0;

            // If we received a line surrounded in parentheses, and they match, we can safely remove them
            while (line.StartsWith("(") && line.EndsWith(")"))
            {
                bool done = false;
                for (int charIndex = 0; charIndex < line.Length; charIndex++)
                {
                    parenthesesDepth += Depth(charLine[charIndex]);
                    // Because there is a depth of zero in the middle of an expression 
                    // we know that we can't safely remove parentheses
                    if (parenthesesDepth == 0 && charIndex < line.Length - 1)
                    {
                        done = true; // Break out of nested loop
                    }
                }
                if (done)
                {
                    break;
                }
                line = line.Substring(1, line.Length - 2);
                charLine = line.ToCharArray();
            }
            // Scan for the highest,rightest operation, while also indexing

            // TODO: Verify well formed

            // Token can be an identifier or an operator
            List<int> tokenIndices = new List<int>();
            int highestPriority = 0;
            Operation highestPriorityOperation = null;
            int operationCharIndex = -1;
            parenthesesDepth = 0;

            for (int charIndex = 0; charIndex < charLine.Length; charIndex++)
            {
                for (int operationIndex = 0; parenthesesDepth == 0 && operationIndex < allOperations.Length; operationIndex++)
                {
                    // If it's an operation check its priority
                    if (charLine[charIndex] == allOperations[operationIndex].character)
                    {
                        if (allOperations[operationIndex].priority > highestPriority)
                        {
                            highestPriorityOperation = allOperations[operationIndex];
                            highestPriority = highestPriorityOperation.priority;
                            operationCharIndex = charIndex;
                        }
                    }
                }
                parenthesesDepth += Depth(charLine[charIndex]);
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