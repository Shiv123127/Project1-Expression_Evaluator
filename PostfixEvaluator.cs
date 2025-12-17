using System;
using System.Collections.Generic;

namespace Project1_Expression_Evaluator
{
    public class PostfixEvaluator
    {
        // evaluates a postfix expression
        public int Evaluate(List<string> postfix, Dictionary<string, int> variables)
        {
            Stack<int> stack = new Stack<int>();

            foreach (string token in postfix)
            {
                string lower = token.ToLower();

                // number literal
                int number;
                if (int.TryParse(token, out number))
                {
                    stack.Push(number);
                    continue;
                }

                // variable operand
                if (IsVariable(token))
                {
                    if (!variables.ContainsKey(token))
                        throw new Exception("missing value for: " + token);

                    stack.Push(variables[token]);
                    continue;
                }

                // unary not
                if (lower == "not")
                {
                    if (stack.Count < 1)
                        throw new Exception("invalid expression (not missing operand)");

                    int value = stack.Pop();
                    stack.Push(value == 0 ? 1 : 0);
                    continue;
                }

                // binary operator
                if (stack.Count < 2)
                    throw new Exception("invalid expression (operator missing operands)");

                int right = stack.Pop();
                int left = stack.Pop();
                stack.Push(ApplyOperator(token, left, right));
            }

            if (stack.Count != 1)
                throw new Exception("invalid expression (too many operands)");

            return stack.Pop();
        }

        // checks if token is a variable name (letters) and not and/or/not
        private bool IsVariable(string token)
        {
            if (string.IsNullOrEmpty(token))
                return false;

            string lower = token.ToLower();
            return char.IsLetter(token[0]) &&
                   lower != "and" && lower != "or" && lower != "not";
        }

        // applies operator logic
        private int ApplyOperator(string op, int left, int right)
        {
            switch (op.ToLower())
            {
                case "+":
                    return left + right;
                case "-":
                    return left - right;
                case "*":
                    return left * right;
                case "/":
                    return left / right;

                case "<":
                    return left < right ? 1 : 0;
                case ">":
                    return left > right ? 1 : 0;
                case "<=":
                    return left <= right ? 1 : 0;
                case ">=":
                    return left >= right ? 1 : 0;
                case "==":
                    return left == right ? 1 : 0;

                case "and":
                    return (left != 0 && right != 0) ? 1 : 0;
                case "or":
                    return (left != 0 || right != 0) ? 1 : 0;

                default:
                    throw new Exception("unknown operator: " + op);
            }
        }
    }
}
