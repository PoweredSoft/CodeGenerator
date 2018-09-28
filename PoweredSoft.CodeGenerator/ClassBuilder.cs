using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoweredSoft.CodeGenerator.Constants;
using PoweredSoft.CodeGenerator.Core;
using PoweredSoft.CodeGenerator.Extensions;

namespace PoweredSoft.CodeGenerator
{
    public class ClassBuilder : IMultiLineGeneratable, IHasName, IHasGeneratableChildren, IHasMeta
    {
        private string _name;
        private bool _isPartial;
        private AccessModifiers _accessModifier;
        private bool _isStatic;
        private object _meta;

        public List<IGeneratable> Children { get; } = new List<IGeneratable>();
        public IEnumerable<PropertyBuilder> Properties => Children.Where(t => t is PropertyBuilder).Cast<PropertyBuilder>();
        public IEnumerable<FieldBuilder> Fields => Children.Where(t => t is FieldBuilder).Cast<FieldBuilder>();
        public IEnumerable<MethodBuilder> Methods => Children.Where(t => t is MethodBuilder).Cast<MethodBuilder>();

        public bool HasMemberWithName(string name) => Children.Any(t => t is IHasName && (t as IHasName).GetName() == name);

        public bool PropertyExists(string name) => Properties.Any(t => t.IsNamed(name));
        public bool FieldExists(string name) => Fields.Any(t => t.IsNamed(name));
        public PropertyBuilder GetProperty(string name) => Properties.FirstOrDefault(t => t.IsNamed(name));
        public FieldBuilder GetField(string name) => Fields.FirstOrDefault(t => t.IsNamed(name));


        public static ClassBuilder Create()
        {
            return new ClassBuilder();;
        }

        public ClassBuilder Name(string name)
        {
            _name = name;
            return this;
        }

        public ClassBuilder Partial(bool isPartial)
        {
            _isPartial = isPartial;
            return this;
        }

        public ClassBuilder AccessModifier(AccessModifiers accessModifier)
        {
            _accessModifier = accessModifier;
            return this;
        }

        public ClassBuilder Static(bool isStatic)
        {
            _isStatic = isStatic;
            return this;
        }

        public ClassBuilder Field(Action<FieldBuilder> configurator)
        {
            var child = FieldBuilder.Create();
            Children.Add(child);
            configurator(child);
            return this;
        }

        public ClassBuilder Property(Action<PropertyBuilder> configurator)
        {
            var child = PropertyBuilder.Create();
            Children.Add(child);
            configurator(child);
            return this;
        }

        public ClassBuilder Property(string name, bool createIfNotExist, Action<PropertyBuilder> action)
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

        public ClassBuilder Field(string name, bool createIfNotExist, Action<FieldBuilder> action)
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

        private string GenerateClassSignature()
        {
            var ret = _accessModifier.Generate();

            if (_isStatic)
                ret += " static";
            
            if (_isPartial)
                ret += " partial";

            ret += $" class {_name}";

            // TODO inheritance
            return ret;
        }

        public List<string> GenerateLines()
        {
            var ret = new List<string>();
            ret.Add(GenerateClassSignature());
            ret.Add("{");
            ret.AddRange(Children.IdentChildren());
            ret.Add("}");
            return ret;
        }


        public ClassBuilder Method(Action<MethodBuilder> action)
        {
            var method = new MethodBuilder();
            Children.Add(method);
            action(method);
            return this;
        }

        public ClassBuilder Meta(object meta)
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
    }
}
