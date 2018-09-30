using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using PoweredSoft.CodeGenerator.Constants;
using PoweredSoft.CodeGenerator.Core;

namespace PoweredSoft.CodeGenerator.Extensions
{
    public static class GenerateExtensions
    {
        public static string Generate(this AccessModifiers accessModifier)
        {
            if (accessModifier == AccessModifiers.Public)
                return "public";
            if (accessModifier == AccessModifiers.Internal)
                return "internal";
            if (accessModifier == AccessModifiers.Private)
                return "private";
            if (accessModifier == AccessModifiers.Protected)
                return "protected";
            if (accessModifier == AccessModifiers.Omit)
                return "";

            throw new NotSupportedException("Unknown access modifier specified");
        }

        public static TBuilderChild FindByMeta<TBuilderChild>(this IHasGeneratableChildren parent, object meta)
            where TBuilderChild : class, IHasMeta
        {
            var ret = parent
                .Children
                .Where(child =>
                {
                    var metaChild = child as TBuilderChild;
                    if (metaChild == null)
                        return false;

                    return metaChild.GetMeta()?.Equals(meta) == true;
                })
                .FirstOrDefault();

            return (TBuilderChild)ret;
        }

        public static string GetOutputType(this Type type)
        {
            var compiler = new CSharpCodeProvider();
            var codeTypeReference = new CodeTypeReference(type);
            return compiler.GetTypeOutput(codeTypeReference);
        }

        public static List<string> IdentLines(this IEnumerable<string> lines, int identCount = 1)
        {
            var prepend = "";
            for (var i = 0; i < identCount; i++)
                prepend += "    ";


            var ret = lines.Select(t => $"{prepend}{t}").ToList();
            return ret;
        }

        public static List<string> IdentGeneratable(this IGeneratable multiLineGeneratable, int identCount = 1)
        {
            var lines = multiLineGeneratable.ToLines();
            var ret = lines.IdentLines(identCount);
            return ret;
        }

        public static List<string> ToLines(this IGeneratable that)
        {
            if (that is IMultiLineGeneratable)
                return (that as IMultiLineGeneratable).GenerateLines();
            if (that is ISingleLineGeneratable)
                return new List<string> { (that as ISingleLineGeneratable).GenerateLine() };

            throw new NotSupportedException();
        }

        public static List<string> IdentChildren(this IEnumerable<IGeneratable> children, int identCount = 1)
        {
            var childrenLines = children.SelectMany(t => t.ToLines());
            var ret = childrenLines.IdentLines(identCount);
            return ret;
        }

        public static bool IsNamed(this IHasName that, string name)
        {
            return that.GetName() == name;
        }
    }
}
