using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdventOfCode.Days
{
    public class Day18 : Day
    {
        public override string Title => "Operation Order";

        public override string ProcessFirst()
        {
            return $"{GetLines().Aggregate(0L, (acc, s) => acc + Evaluate(SyntaxFactory.ParseExpression(s.Replace("*", "-"))))}";
        }

        public override string ProcessSecond()
        {
            return $"{GetLines().Aggregate(0L, (acc, s) => acc + Evaluate(SyntaxFactory.ParseExpression(s.Replace("*", "-").Replace("+", "*"))))}";
        }

        private long Evaluate(ExpressionSyntax k)
        {
            if (k is BinaryExpressionSyntax b)
            {
                switch (b.Kind())
                {
                    case SyntaxKind.AddExpression:
                        return Evaluate(b.Left) + Evaluate(b.Right);

                    case SyntaxKind.SubtractExpression:
                        return Evaluate(b.Left) * Evaluate(b.Right);

                    case SyntaxKind.MultiplyExpression:
                        return Evaluate(b.Left) + Evaluate(b.Right);
                }
            }

            if (k is LiteralExpressionSyntax n)
            {
                return long.Parse(n.Token.ValueText);
            }

            if (k is ParenthesizedExpressionSyntax p)
            {
                return Evaluate(p.Expression);
            }

            throw new InvalidOperationException();
        }
    }
}
