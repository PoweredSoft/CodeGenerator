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
    public class ClassBuilder : IGeneratable
    {
        protected ClassModel Model { get; set; } = new ClassModel();

        public static ClassBuilder Create()
        {
            return new ClassBuilder();;
        }

        public ClassBuilder Name(string name)
        {
            Model.Name = name;
            return this;
        }

        public ClassBuilder Partial(bool isPartial)
        {
            Model.IsPartial = isPartial;
            return this;
        }

        public ClassBuilder AccessModifier(AccessModifiers accessModifier)
        {
            Model.AccessModifier = accessModifier;
            return this;
        }

        public ClassBuilder Static(bool isStatic)
        {
            Model.IsStatic = isStatic;
            return this;
        }

        public ClassBuilder Field(Action<FieldBuilder> configurator)
        {
            var child = FieldBuilder.Create();
            Model.Children.Add(child);
            configurator(child);
            return this;
        }

        public ClassBuilder Property(Action<PropertyBuilder> configurator)
        {
            var child = PropertyBuilder.Create();
            Model.Children.Add(child);
            configurator(child);
            return this;
        }

        private string GenerateClassSignature()
        {
            var ret = Model.AccessModifier.Generate();

            if (Model.IsStatic)
                ret += " static";
            
            if (Model.IsPartial)
                ret += " partial";

            ret += $" class {Model.Name}";

            // TODO inheritance
            return ret;
        }

        public List<string> GenerateLines()
        {
            var ret = new List<string>();
            ret.Add(GenerateClassSignature());
            ret.Add("{");
            ret.AddRange(Model.Children.IdentChildren());
            ret.Add("}");
            return ret;
        }


        public ClassBuilder Method(Action<MethodBuilder> action)
        {
            var method = new MethodBuilder();
            Model.Children.Add(method);
            action(method);
            return this;
        }
    }
}
