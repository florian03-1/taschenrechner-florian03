using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionParser
{
    public class Token
    {
        public char Operator { get; set; }
        public double Value { get; set; }
        public TokenType Type { get; set; }
        public TokenFunction Function { get; set; }

        public int GetRank()
        {
            if (Type == TokenType.Operator)
            {
                switch (Operator)
                {
                    case '!':
                        return 95;
                    case '^':
                        return 90;

                    case '*':
                    case '/':
                        return 70;

                    case '+':
                    case '-':
                        return 40;


                    default:
                        return 10;
                }
            }
            else if (Type == TokenType.Function) return 99;
            else return 0;
        }

        public override string ToString()
        {
            if (Type == TokenType.Constant) return Value.ToString();
            else if (Type == TokenType.Function) return Function.ToString();
            else return Operator.ToString();
        }
    }


    public enum TokenType
    {
        Operator,
        Constant,
        Function,
        Bracket
    }

    public enum TokenFunction
    {
        Sin,
        Cos,
        Sqrt
    }
}
