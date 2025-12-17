using System;
using System.Collections.Generic;
using System.Text;

namespace Project1_Expression_Evaluator
{
    public class Tokeniser
    {
        // splits the input expression into tokens
        public List<string> Tokenise(string input)
        {
            List<string> tokens = new List<string>();
            StringBuilder current = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                // ignore spaces but finish current token first
                if (char.IsWhiteSpace(c))
                {
                    addCurrentToken(tokens, current);
                    continue;
                }

                // handle letters (variables or keywords like and/or/not)
                if (char.IsLetter(c))
                {
                    current.Append(c);
                    continue;
                }

                // handle digits
                if (char.IsDigit(c))
                {
                    current.Append(c);
                    continue;
                }

                // if we reach here, it must be an operator or bracket
                addCurrentToken(tokens, current);

                // check for two character operators like <= >= ==
                if (i + 1 < input.Length)
                {
                    string twoCharOp = "" + c + input[i + 1];
                    if (twoCharOp == "<=" || twoCharOp == ">=" || twoCharOp == "==")
                    {
                        tokens.Add(twoCharOp);
                        i++; // skip next character
                        continue;
                    }
                }

                // single character operator or parenthesis
                tokens.Add(c.ToString());
            }

            // add any remaining token
            addCurrentToken(tokens, current);

            return tokens;
        }

        // helper method to add the current token if it exists
        private void addCurrentToken(List<string> tokens, StringBuilder current)
        {
            if (current.Length > 0)
            {
                tokens.Add(current.ToString());
                current.Clear();
            }
        }
    }
}
