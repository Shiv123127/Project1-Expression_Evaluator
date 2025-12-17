using System;
using System.Collections.Generic;

namespace Project1_Expression_Evaluator
{
    public class InfixToPostfixConverter
    {
        // converts a list of infix tokens into postfix form
        public List<string> Convert(List<string> tokens)
        {
            List<string> output = new List<string>();
            Stack<string> operators = new Stack<string>();

            foreach (string token in tokens)
            {
                // operands go straight to output
                if (IsOperand(token))
                {
                    output.Add(token);
                }
                // opening bracket
                else if (token == "(")
                {
                    operators.Push(token);
                }
                // closing bracket
                else if (token == ")")
                {
                    while (operators.Count > 0 && operators.Peek() != "(")
                    {
                        output.Add(operators.Pop());
                    }

                    if (operators.Count == 0)
                        throw new Exception("mismatched parentheses");

                    operators.Pop(); // remove "("
                }
                // operator
                else
                {
                    while (operators.Count > 0 &&
                           operators.Peek() != "(" &&
                           Precedence(operators.Peek()) >= Precedence(token))
                    {
                        output.Add(operators.Pop());
                    }
                    operators.Push(token);
                }
            }

            // pop remaining operators
            while (operators.Count > 0)
            {
                if (operators.Peek() == "(")
                    throw new Exception("mismatched parentheses");

                output.Add(operators.Pop());
            }

            return output;
        }

        // checks if token is an operand (number or variable)
        private bool IsOperand(string token)
        {
            // number literal
            int num;
            if (int.TryParse(token, out num))
                return true;

            // variable name (letters) but not boolean keywords
            string lower = token.ToLower();
            if (token.Length > 0 && char.IsLetter(token[0]) &&
                lower != "and" && lower != "or" && lower != "not")
            {
                return true;
            }

            return false;
        }

        // returns operator precedence
        private int Precedence(string op)
        {
            switch (op.ToLower())
            {
                case "not":
                    return 4;
                case "*":
                case "/":
                    return 3;
                case "+":
                case "-":
                    return 2;
                case "<":
                case ">":
                case "<=":
                case ">=":
                case "==":
                    return 1;
                case "and":
                    return 0;
                case "or":
                    return -1;
                default:
                    return -2;
            }
        }
    }
}
