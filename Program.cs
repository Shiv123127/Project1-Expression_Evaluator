using System;
using System.Collections.Generic;

namespace Project1_Expression_Evaluator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tokeniser tokeniser = new Tokeniser();
            InfixToPostfixConverter converter = new InfixToPostfixConverter();
            PostfixEvaluator evaluator = new PostfixEvaluator();

            Dictionary<string, int> variables = new Dictionary<string, int>();

            Console.Write("enter an infix expression: ");
            string input = Console.ReadLine();

            // tokenise
            List<string> tokens = tokeniser.Tokenise(input);

            Console.WriteLine("\ntokens:");
            foreach (string t in tokens)
            {
                Console.Write(t + " ");
            }
            Console.WriteLine();

            // convert to postfix
            List<string> postfix = converter.Convert(tokens);

            Console.WriteLine("\npostfix:");
            foreach (string p in postfix)
            {
                Console.Write(p + " ");
            }
            Console.WriteLine();

            // ask for variable values (skip operators and booleans)
            foreach (string token in postfix)
            {
                string lower = token.ToLower();

                bool isOperator =
                    lower == "+" || lower == "-" || lower == "*" || lower == "/" ||
                    lower == "^" ||
                    lower == "<" || lower == ">" || lower == "<=" || lower == ">=" ||
                    lower == "==" ||
                    lower == "and" || lower == "or" || lower == "not";

                bool isBooleanLiteral =
                    lower == "true" || lower == "false";

                if (!isOperator && !isBooleanLiteral && char.IsLetter(token[0]))
                {
                    if (!variables.ContainsKey(token))
                    {
                        Console.Write($"enter value for {token}: ");
                        string inputValue = Console.ReadLine();

                        if (int.TryParse(inputValue, out int val))
                        {
                            variables[token] = val;
                        }
                        else if (inputValue.ToLower() == "true")
                        {
                            variables[token] = 1;
                        }
                        else if (inputValue.ToLower() == "false")
                        {
                            variables[token] = 0;
                        }
                        else
                        {
                            Console.WriteLine("error: invalid value entered.");
                            return;
                        }
                    }
                }
            }

            // evaluate postfix
            int result = evaluator.Evaluate(postfix, variables);

            // decide how to print result
            bool isBooleanExpression = false;

            foreach (string token in postfix)
            {
                string lower = token.ToLower();
                if (lower == "and" || lower == "or" || lower == "not" ||
                    lower == "<" || lower == ">" || lower == "<=" ||
                    lower == ">=" || lower == "==")
                {
                    isBooleanExpression = true;
                    break;
                }
            }

            if (isBooleanExpression)
            {
                Console.WriteLine("\nresult: " + (result != 0));
            }
            else
            {
                Console.WriteLine("\nresult: " + result);
            }

            Console.WriteLine("\npress enter to exit...");
            Console.ReadLine();
        }
    }
}
