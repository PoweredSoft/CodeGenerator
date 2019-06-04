using System;
using System.Collections.Generic;
using System.Linq;
using PoweredSoft.CodeGenerator.Constants;
using PoweredSoft.CodeGenerator.Core;

namespace PoweredSoft.CodeGenerator
{
    public abstract class ClassMemberBuilder<TBuilder> : IMultiLineGeneratable, IHasName, IHasMeta
        where TBuilder : class, new()
    {
        protected string _type;
        protected string _comment;
        protected string _name;
        protected AccessModifiers _accessModifier = AccessModifiers.Public;
        protected string _defaultValue;
        protected bool _isStatic;
        protected object _meta;

        public List<AttributeBuilder> Attributes { get; protected set; } = new List<AttributeBuilder>();

        protected virtual IEnumerable<string> GetAttributesLines()
        {
            return Attributes.Select(t => t.GenerateInline());
        }

        public TBuilder Attribute(Action<AttributeBuilder> action)
        {
            var attribute = new AttributeBuilder();
            Attributes.Add(attribute);
            action(attribute);
            return this as TBuilder;
        }

        public static TBuilder Create() => new TBuilder();

        public string GetTypeName() => _type;

        public TBuilder Type(string typeName)
        {
            _type = typeName;
            return this as TBuilder;
        }

        public TBuilder Comment(string comment)
        {
            _comment = comment;
            return this as TBuilder;
        }

        public TBuilder Name(string name)
        {
            _name = name;
            return this as TBuilder;
        }

        public TBuilder AccessModifier(AccessModifiers access)
        {
            _accessModifier = access;
            return this as TBuilder;
        }

        public TBuilder DefaultValue(string defaultValue)
        {
            _defaultValue = defaultValue;
            return this as TBuilder;
        }

        public TBuilder Static(bool isStatic)
        {
            _isStatic = isStatic;
            return this as TBuilder;
        }

        public TBuilder Meta(object meta)
        {
            _meta = meta;
            return this as TBuilder;
        }

        public abstract List<string> GenerateLines();

        public string GetName() => _name;
        public object GetMeta() => _meta;
    }
}