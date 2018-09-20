using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            throw new NotSupportedException("Unknown access modifier specified");
        }

        public static List<string> IdentLines(this List<string> lines, int identCount = 1)
        {
            var prepend = "";
            for (var i = 0; i < identCount; i++)
                prepend += "    ";


            var ret = lines.Select(t => $"{prepend}{t}").ToList();
            return ret;
        }

        public static List<string> IdentGeneratable(this IGeneratable generatable, int identCount = 1)
        {
            var lines = generatable.GenerateLines();
            var ret = lines.IdentLines(identCount);
            return ret;
        }

        public static List<string> IdentChildren(this List<IGeneratable> children, int identCount = 1)
        {
            var childrenLines = children.SelectMany(t => t.GenerateLines()).ToList();
            var ret = childrenLines.IdentLines(identCount);
            return ret;
        }
    }
}
