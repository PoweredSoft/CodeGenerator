using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using PoweredSoft.CodeGenerator.Constants;
using PoweredSoft.CodeGenerator.Core;
using PoweredSoft.CodeGenerator.Models;

namespace PoweredSoft.CodeGenerator.Extensions
{
    public static class GenerateExtensions
    {
        public static string Generate(this ParameterModel parameterModel)
        {
            var ret = $"{parameterModel.Type} {parameterModel.Name}";
            if (!string.IsNullOrWhiteSpace(parameterModel.DefaultValue))
                ret += $" = {parameterModel.DefaultValue}";
            return ret;
        }

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

        public static TBuilderChild FindByMeta<TBuilderChild>(this object parent, object meta)
            where TBuilderChild : class 
        {
            var propertyModel = parent.GetType().GetProperty("Model");
            if (propertyModel == null)
                return null;

            var model = propertyModel.GetValue(parent) as IHasGeneratableChildren;
            if (model == null)
                return null;

            var ret = model
                .Children
                .Where(t => t is TBuilderChild)
                .FirstOrDefault(t =>
                {
                    var modelProp = t.GetType().GetProperties().FirstOrDefault(t2 => t2.Name == "Model");
                    if (modelProp == null)
                        return false;

                    var modelInstance = modelProp.GetValue(t);
                    if (modelInstance == null)
                        return false;

                    if (!(modelInstance is IHasMeta))
                        return false;

                    return (modelInstance as IHasMeta).Meta == meta;
                });

            return (TBuilderChild)ret;
        }

        public static string GetOutputType(this Type type)
        {
            var compiler = new CSharpCodeProvider();
            var codeTypeReference = new CodeTypeReference(type);
            return compiler.GetTypeOutput(codeTypeReference);
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

        public static List<string> IdentChildren(this IEnumerable<IGeneratable> children, int identCount = 1)
        {
            var childrenLines = children.SelectMany(t => t.GenerateLines()).ToList();
            var ret = childrenLines.IdentLines(identCount);
            return ret;
        }
    }
}
