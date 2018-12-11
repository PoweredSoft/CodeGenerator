using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoweredSoft.CodeGenerator.Constants;
using PoweredSoft.CodeGenerator.Core;
using PoweredSoft.CodeGenerator.Extensions;

namespace PoweredSoft.CodeGenerator
{
    public class InterfaceBuilder : IMultiLineGeneratable, IHasName,
        IHasGeneratableChildren, IHasMeta,
        IShouldGenerateBuilder<InterfaceBuilder>
    {
        private string _name;
        private bool _isPartial;
        private AccessModifiers _accessModifier;
        private object _meta;
        private bool _generate;

        public List<IInlineGeneratable> Inheritances { get; } = new List<IInlineGeneratable>();
        public List<IGeneratable> Children { get; } = new List<IGeneratable>();
        public IEnumerable<PropertyBuilder> Properties => Children.Where(t => t is PropertyBuilder).Cast<PropertyBuilder>();
        public IEnumerable<FieldBuilder> Fields => Children.Where(t => t is FieldBuilder).Cast<FieldBuilder>();
        public IEnumerable<MethodBuilder> Methods => Children.Where(t => t is MethodBuilder).Cast<MethodBuilder>();

        public bool HasMemberWithName(string name) => Children.Any(t => t is IHasName && (t as IHasName).GetName() == name);

        public bool PropertyExists(string name) => Properties.Any(t => t.IsNamed(name));
        public bool FieldExists(string name) => Fields.Any(t => t.IsNamed(name));
        public PropertyBuilder GetProperty(string name) => Properties.FirstOrDefault(t => t.IsNamed(name));
        public FieldBuilder GetField(string name) => Fields.FirstOrDefault(t => t.IsNamed(name));

        public string GetUniqueMemberName(string name)
        {
            if (!HasMemberWithName(name))
                return name;

            for (var i = 1; i <= 1000; i++)
            {
                var temp = $"{name}{i}";
                if (!HasMemberWithName(temp))
                    return temp;
            }

            throw new Exception("Tried 1000 unique names all are taken.");
        }

        public static InterfaceBuilder Create()
        {
            return new InterfaceBuilder();;
        }

        public InterfaceBuilder Name(string name)
        {
            _name = name;
            return this;
        }

        public InterfaceBuilder Partial(bool isPartial)
        {
            _isPartial = isPartial;
            return this;
        }

        public InterfaceBuilder AccessModifier(AccessModifiers accessModifier)
        {
            _accessModifier = accessModifier;
            return this;
        }

        public InterfaceBuilder Field(Action<FieldBuilder> configurator)
        {
            var child = FieldBuilder.Create();
            Children.Add(child);
            configurator(child);
            return this;
        }

        public InterfaceBuilder Property(Action<PropertyBuilder> configurator)
        {
            var child = PropertyBuilder.Create();
            Children.Add(child);
            configurator(child);
            return this;
        }

        public InterfaceBuilder Property(string name, bool createIfNotExist, Action<PropertyBuilder> action)
        {
            var property = GetProperty(name);
            if (property == null && !createIfNotExist)
                throw new Exception($"Could not find property with name: {name}");

            if (property == null)
            {
                return Property(t =>
                {
                    t.Name(name);
                    action(t);
                });
            }

            action(property);
            return this;
        }

        public InterfaceBuilder Field(string name, bool createIfNotExist, Action<FieldBuilder> action)
        {
            var field = GetField(name);
            if (field == null && !createIfNotExist)
                throw new Exception($"Could not find field with name: {name}");

            if (field == null)
            {
                return Field(t =>
                {
                    t.Name(name);
                    action(t);
                });
            }

            action(field);
            return this;
        }

        private string GenerateInterfaceSignature()
        {
            var ret = _accessModifier.Generate();

            if (_isPartial)
                ret += " partial";

            ret += $" interface {_name}";

            if (Inheritances.Any())
                ret += " : " + string.Join(", ", Inheritances.Select(inheritance => inheritance.GenerateInline()));
            
            return ret;
        }

        public List<string> GenerateLines()
        {
            var ret = new List<string>();
            ret.Add(GenerateInterfaceSignature());
            ret.Add("{");
            var orderedChildren = OrderChildren(Children);
            ret.AddRange(orderedChildren.IdentChildren());
            ret.Add("}");
            return ret;
        }

        public IEnumerable<IGeneratable> OrderChildren(List<IGeneratable> children)
        {
            return children.OrderBy(c =>
            {
                if (c is FieldBuilder)
                    return 1;
                if (c is PropertyBuilder)
                    return 2;
                if (c is ConstructorBuilder)
                    return 3;
                if (c is MethodBuilder)
                    return 4;
                return 5;
            });
        }


        public InterfaceBuilder Method(Action<MethodBuilder> action)
        {
            var method = new MethodBuilder();
            Children.Add(method);
            action(method);
            return this;
        }

        public InterfaceBuilder Meta(object meta)
        {
            _meta = meta;
            return this;
        }

        public string GetName()
        {
            return _name;
        }


        public object GetMeta()
        {
            return _meta;
        }

        public InterfaceBuilder Inherits(string raw)
        {
            Inheritances.Add(RawInlineBuilder.Create(raw));
            return this;
        }

        public InterfaceBuilder Generate(bool generate)
        {
            this._generate = generate;
            return this;
        }

        public bool ShouldGenerate() => _generate;
    }
}
