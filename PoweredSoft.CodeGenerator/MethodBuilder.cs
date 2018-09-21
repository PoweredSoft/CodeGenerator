using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoweredSoft.CodeGenerator.Constants;
using PoweredSoft.CodeGenerator.Core;
using PoweredSoft.CodeGenerator.Extensions;
using PoweredSoft.CodeGenerator.Models;

namespace PoweredSoft.CodeGenerator
{
    public class MethodBuilder : IGeneratable
    {
        public MethodModel Model { get; set; } = new MethodModel();

        public MethodBuilder Name(string name)
        {
            Model.Name = name;
            return this;
        }

        public MethodBuilder Virtual(bool isVirtual)
        {
            Model.IsVirtual = isVirtual;
            return this;
        }

        public MethodBuilder IsStatic(bool isStatic)
        {
            Model.IsStatic = isStatic;
            return this;
        }

        public MethodBuilder ReturnType(string type)
        {
            Model.ReturnType = type;
            return this;
        }

        public MethodBuilder Partial(bool partial)
        {
            Model.IsPartial = partial;
            return this;
        }

        public MethodBuilder Abstract(bool isAbstract)
        {
            Model.IsAbstract = isAbstract;
            return this;
        }

        public MethodBuilder AccessModifier(AccessModifiers accessModifier)
        {
            Model.AccessModifier = accessModifier;
            return this;
        }

        public MethodBuilder Parameter(Action<ParameterBuilder> configurator)
        {
            var param = ParameterBuilder.Create();
            Model.Parameters.Add(param.Model);
            configurator(param);
            return this;
        }

        public MethodBuilder Add(Func<IGeneratable> child)
        {
            var result = child();
            Model.Children.Add(result);
            return this;
        }

        public MethodBuilder Add(IGeneratable child)
        {
            Model.Children.Add(child);
            return this;
        }

        protected string GenerateSignature()
        {
            var signature = $"{Model.AccessModifier.Generate()}";

            if (Model.IsStatic)
                signature += " static";

            if (Model.IsAbstract)
                signature += "abstract";

            if (Model.IsVirtual)
                signature += " virtual";

            if (Model.IsPartial)
                signature += " partial";

            signature += $" {Model.ReturnType} {Model.Name}";

            signature += "(";
            signature += string.Join(", ", Model.Parameters.Select(t => t.Generate()));
            signature += ")";

            if (Model.IsPartial || Model.IsAbstract)
                signature += ";";

            return signature;
        }

        public List<string> GenerateLines()
        {
            var ret = new List<string>();

            // signature.
            var signature = GenerateSignature();
            ret.Add(signature);

            if (!Model.IsAbstract && !Model.IsPartial)
            {
                ret.Add("{");
                ret.AddRange(Model.Children.IdentChildren());
                ret.Add("}");
            }

            return ret;
        }
    }
}
