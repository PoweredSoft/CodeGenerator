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
        public ClassModel Model { get; protected set; } = new ClassModel();

        public IEnumerable<PropertyBuilder> Properties => Model.Children.Where(t => t is PropertyBuilder).Cast<PropertyBuilder>();
        public IEnumerable<FieldBuilder> Fields => Model.Children.Where(t => t is FieldBuilder).Cast<FieldBuilder>();
        public IEnumerable<MethodBuilder> Methods => Model.Children.Where(t => t is MethodBuilder).Cast<MethodBuilder>();

        public bool HasMemberWithName(string name) => Model.Children.Any(t => (t as IHasName)?.Name == name);
        public bool PropertyExists(string name) => Properties.Any(t => t.Model.Name == name);
        public bool FieldExists(string name) => Fields.Any(t => t.Model.Name == name);
        public PropertyBuilder GetProperty(string name) => Properties.FirstOrDefault(t => t.Model.Name == name);
        public FieldBuilder GetField(string name) => Fields.FirstOrDefault(t => t.Model.Name == name);


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

        public ClassBuilder Property(string name, Action<PropertyBuilder> action)
        {
            var property = GetProperty(name);
            if (property == null)
                throw new Exception($"Could not find property with name: {name}");

            action(property);
            return this;
        }

        public ClassBuilder Field(string name, Action<FieldBuilder> action)
        {
            var field = GetField(name);
            if (field == null)
                throw new Exception($"Could not find field with name: {name}");

            action(field);
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
