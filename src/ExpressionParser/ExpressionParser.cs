using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExpressionParser
{
    public class ExpressionParser
    {
        public string Expression { get; set; }
        public double Result { get; set; }

        public List<Token> InfixTokens { get; set; }
        public List<Token> PostfixTokens { get; set; }

        public AngleMode AngleMode { get; set; }

        public ExpressionParser(string expression, AngleMode angleMode)
        {
            AngleMode = angleMode;
            Expression = expression;
            InfixTokens = Tokenize(expression);
            PostfixTokens = InfixToPostfix(InfixTokens);
            Result = Calculate(PostfixTokens);

        }

        public ExpressionParser()
        {
            InfixTokens = new List<Token>();
            PostfixTokens = new List<Token>();
            Result = 0;
            Expression = "";
        }


        //Statische Methoden



        //private Methoden
        private List<Token> Tokenize(string expression)
        {
            expression = expression.Replace("e", Math.E.ToString());
            expression = expression.Replace("π", Math.PI.ToString().Replace(".", ","));


            List<Token> tokens = new();
            Regex regTokens = new Regex(@"(?<operator>[+\-*/^])|(?<bracket>[()])|(?<constant>[\d\,]+)|(?<function>sin|cos|sqrt)");

            foreach (Match match in regTokens.Matches(expression))
            {
                if (match.Groups["constant"].Success) tokens.Add(new() { Type = TokenType.Constant, Value = double.Parse(match.Value) });
                if (match.Groups["operator"].Success)
                {
                    char op = match.Value[0];

                    //Wenn zwei Operatoren aufeinander stoßen, und das aktuelle Token ein "-" ist, dann wird dies durch ein eindeutiges ! ersetzt - außer bei einer schließenden Klammer
                    Token lastToken = tokens.Last();
                    if (op == '-' && (lastToken.Type == TokenType.Operator || lastToken.Type == TokenType.Function) && lastToken.Operator != ')') op = '!';

                    tokens.Add(new() { Type = TokenType.Operator, Operator = op });
                }
                if (match.Groups["function"].Success)
                {
                    Token t = new() { Type = TokenType.Function, Operator = '#' };
                    if (match.Value == "sin") t.Function = TokenFunction.Sin;
                    else if (match.Value == "cos") t.Function = TokenFunction.Cos;
                    else if (match.Value == "sqrt") t.Function = TokenFunction.Sqrt;
                    tokens.Add(t);
                }
                if (match.Groups["bracket"].Success) tokens.Add(new() { Type = TokenType.Bracket, Operator = match.Value[0] });

            }

            return tokens;
        }
        private List<Token> InfixToPostfix(List<Token> tokens)
        {
            //siehe: https://de.wikipedia.org/wiki/Shunting-yard-Algorithmus
            //siehe auch: https://de.wikipedia.org/wiki/Umgekehrte_polnische_Notation

            List<Token> result = new();

            //Zwischenspeicher für Operatoren
            Stack<Token> operatorStack = new();

            try
            {
                foreach (var token in tokens)
                {
                    if (token.Type == TokenType.Constant)
                    {
                        result.Add(token);
                    }

                    else if (token.Type == TokenType.Function)
                    {
                        operatorStack.Push(token);
                    }

                    else if (token.Type == TokenType.Operator)  //operator = +-*/^
                    {
                        while (operatorStack.Count > 0 && operatorStack.Peek().Type == TokenType.Operator && operatorStack.Peek().GetRank() > token.GetRank())
                        {
                            result.Add(operatorStack.Pop());
                        }
                        operatorStack.Push(token);
                    }

                    else if (token.Type == TokenType.Bracket)
                    {
                        if (token.Operator == '(')
                        {
                            operatorStack.Push(token);
                        }

                        else if (token.Operator == ')')
                        {
                            if (operatorStack.Count == 0) throw new Exception("Eine öffnende Klammer fehlt.");
                            while (operatorStack.Peek().Operator != '(') result.Add(operatorStack.Pop());

                            operatorStack.Pop(); //öffnende Klammer, die jetzt oben steht entfernen

                            if (operatorStack.Count > 0)
                            {
                                if (operatorStack.Peek().Type == TokenType.Function) result.Add(operatorStack.Pop());
                            }

                        }
                    }

                }

                result.AddRange(operatorStack);  //Den restlichen Stack in die Ausgabe übertragen
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler bei der Eingabe!");
            }

            

            return result;
        }
        private double Calculate(List<Token> postfixTokens)
        {

            Dictionary<char, Action<Stack<double>, Token>> operators = new();

            operators.Add('\0', (stack, token) => stack.Push(token.Value));
            operators.Add('#', (stack, token) => 
            {
                double value = stack.Pop();

                if (token.Function == TokenFunction.Sqrt) stack.Push(Math.Sqrt(value));

                if (AngleMode == AngleMode.Deg) value = value * (Math.PI / 180);

                if (token.Function == TokenFunction.Sin) stack.Push(Math.Sin(value));
                else if (token.Function == TokenFunction.Cos) stack.Push(Math.Cos(value));
                
            });

            operators.Add('!', (stack, token) => stack.Push(-stack.Pop()));
            operators.Add('^', (stack, token) => 
            {
                var right = stack.Pop();
                stack.Push(Math.Pow(stack.Pop(), right));
            });

            operators.Add('+', (stack, token) => stack.Push(stack.Pop() + stack.Pop()));
            operators.Add('-', (stack, token) => stack.Push(- stack.Pop() + stack.Pop()));  //Reihenfolge bei Subtraktion beachten. Das oberste Zeichen wird abgezogen, deshalb minus!!
            operators.Add('*', (stack, token) => stack.Push(stack.Pop() * stack.Pop()));
            operators.Add('/', (stack, token) => stack.Push( 1 / stack.Pop() * stack.Pop()));  //Reihenfolge beachten! Das oberste Zeichen ist der Teiler, deshalb wird mit dem KEhrbruch multipliziert.




            //Berechnung

            var resultStack = new Stack<double>();
            foreach (var token in postfixTokens)
            {
                operators[token.Operator](resultStack, token);
            }


            double res =  resultStack.Pop();
            return Math.Round(res, 5);
        }
    }

    public enum AngleMode
    {
        Deg,
        Rad
    }
}
