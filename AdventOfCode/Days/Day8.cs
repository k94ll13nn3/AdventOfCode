// stylecop.header

using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    internal class Day8 : Day
    {
        public override object ProcessFirst()
        {
            return this.Lines.Sum(c => c.Length - (Regex.Unescape(c).Length - 2));
        }

        public override object ProcessSecond()
        {
            return this.Lines.Sum(c => ToLiteral(c).Length - c.Length);
        }

        // http://stackoverflow.com/a/324812
        private static string ToLiteral(string input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }
    }
}