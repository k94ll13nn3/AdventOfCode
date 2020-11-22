using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace AdventOfCode.Generator
{
    [Generator]
    public class ProgramGenerator : ISourceGenerator
    {
        private const string AttributeText = @"using System;

namespace AdventOfCode.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CoupledDayAttribute : Attribute
    {
        public CoupledDayAttribute(string firstAnswer, string secondAnswer)
        {
            FirstAnswer = firstAnswer;
            SecondAnswer = secondAnswer;
        }

        public string FirstAnswer { get; set; }

        public string SecondAnswer { get; set; }
    }
}
";

        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource("CoupledDayAttribute", SourceText.From(AttributeText, Encoding.UTF8));

            if (context.SyntaxReceiver is not SyntaxReceiver receiver)
            {
                return;
            }

            CSharpParseOptions? options = (context.Compilation as CSharpCompilation)?.SyntaxTrees[0].Options as CSharpParseOptions;
            Compilation compilation = context.Compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(AttributeText, Encoding.UTF8), options));

            INamedTypeSymbol? attributeSymbol = compilation.GetTypeByMetadataName("AdventOfCode.Attributes.CoupledDayAttribute");

            StringBuilder sourceBuilder = new(@"using System;
using System.Collections.Generic;
using AdventOfCode.Days;

namespace AdventOfCode
{
    internal static partial class Program
    {
        private static Day GetDay(string dayNumber)
        {
            return dayNumber switch
            {
");
            foreach (ClassDeclarationSyntax candidateClass in receiver.CandidateClasses)
            {
                SemanticModel model = compilation.GetSemanticModel(candidateClass.SyntaxTree);
                INamedTypeSymbol? symbol = model.GetDeclaredSymbol(candidateClass);

                if (symbol is not null && symbol.BaseType?.ToString() == "AdventOfCode.Days.Day")
                {
                    sourceBuilder.AppendLine($@"                ""{candidateClass.Identifier.ToString().Substring(3)}"" => new {candidateClass.Identifier}(),");
                }
            }

            sourceBuilder.Append(@"                _ => throw new InvalidOperationException(),
            };
        }");

            sourceBuilder.Append(@"

        private static List<(Day coupledDay, string firstAnswer, string secondAnswer)> GetCoupledDays()
        {
            return new()
            {
");

            foreach (ClassDeclarationSyntax candidateClass in receiver.CandidateClasses)
            {
                SemanticModel model = compilation.GetSemanticModel(candidateClass.SyntaxTree);
                INamedTypeSymbol? symbol = model.GetDeclaredSymbol(candidateClass);

                AttributeData? attributeData = symbol?.GetAttributes().FirstOrDefault(ad => ad.AttributeClass?.Equals(attributeSymbol, SymbolEqualityComparer.Default) == true);
                if (attributeData is not null)
                {
                    string firstAnswer = attributeData.ConstructorArguments[0].Value?.ToString() ?? "";
                    string secondAnswer = attributeData.ConstructorArguments[1].Value?.ToString() ?? "";
                    sourceBuilder.AppendLine($@"                (new {candidateClass.Identifier}(), ""{firstAnswer}"", ""{secondAnswer}""),");
                }
            }

            sourceBuilder.Append(@"            };
        }
    }
}");

            context.AddSource("Program.Generated.cs", sourceBuilder.ToString());
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // Register a syntax receiver that will be created for each generation pass.
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }
    }

    /// <summary>
    /// Created on demand before each generation pass
    /// </summary>
    internal class SyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> CandidateClasses { get; } = new List<ClassDeclarationSyntax>();

        /// <summary>
        /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation.
        /// </summary>
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax)
            {
                CandidateClasses.Add(classDeclarationSyntax);
            }
        }
    }
}
