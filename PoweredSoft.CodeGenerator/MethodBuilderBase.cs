using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoweredSoft.CodeGenerator.Constants;
using PoweredSoft.CodeGenerator.Core;
using PoweredSoft.CodeGenerator.Extensions;

namespace PoweredSoft.CodeGenerator
{
    public abstract class MethodBuilderBase<TBuilder> : IMultiLineGeneratable, IHasName, IHasGeneratableChildren
        where TBuilder : class, new()
    {
        private string _name;
        private bool _isVirtual;
        private bool _isStatic;
        private string _returnType;
        private bool _isPartial;
        private bool _isAbstract;
        private AccessModifiers _accessModifier;
        private bool _override;
        private bool _isBodyLess;


        public static TBuilder Create() => new TBuilder();

        public List<ParameterBuilder> Parameters { get; } = new List<ParameterBuilder>();
        public List<IGeneratable> Children { get; } = new List<IGeneratable>();

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

        public virtual TBuilder Name(string name)
        {
            _name = name;
            return this as TBuilder;
        }

        public virtual TBuilder BodyLess(bool isBodyLess)
        {
            _isBodyLess = isBodyLess;
            return this as TBuilder;
        }

        public TBuilder Virtual(bool isVirtual)
        {
            _isVirtual = isVirtual;
            return this as TBuilder;
        }

        public TBuilder Override(bool o)
        {
            _override = o;
            return this as TBuilder;
        }

        public TBuilder IsStatic(bool isStatic)
        {
            _isStatic = isStatic;
            return this as TBuilder;
        }

        public TBuilder ReturnType(string type)
        {
            _returnType = type;
            return this as TBuilder;
        }

        public TBuilder Partial(bool partial)
        {
            _isPartial = partial;
            return this as TBuilder;
        }

        public TBuilder Abstract(bool isAbstract)
        {
            _isAbstract = isAbstract;
            return this as TBuilder;
        }

        public TBuilder AccessModifier(AccessModifiers accessModifier)
        {
            _accessModifier = accessModifier;
            return this as TBuilder;
        }

        public TBuilder Parameter(Action<ParameterBuilder> configurator)
        {
            var param = ParameterBuilder.Create();
            Parameters.Add(param);
            configurator(param);
            return this as TBuilder;
        }

        public TBuilder Add(Func<IGeneratable> child)
        {
            var result = child();
            Children.Add(result);
            return this as TBuilder;
        }

        public TBuilder AddComment(string comment)
        {
            Children.Add(SingleLineCommentBuilder.Create(comment));
            return this as TBuilder;
        }

        public TBuilder Add(IGeneratable child)
        {
            Children.Add(child);
            return this as TBuilder;
        }

        public void AddEmptyLine()
        {
            Children.Add(RawLineBuilder.Create("").NoEndOfLine());
        }

        public TBuilder RawLine(string raw) => Add(RawLineBuilder.Create(raw));

        public bool HasBody()
        {
            return !this._isAbstract && !this._isPartial && !this._isBodyLess;
        }

        protected virtual string GenerateSignature()
        {
            var signature = $"{_accessModifier.Generate()}";

            if (_isStatic)
                signature += " static";

            if (_isAbstract)
                signature += "abstract";

            if (_isVirtual)
                signature += " virtual";

            if (_isPartial)
                signature += " partial";

            if (_override)
                signature += " override";

            if (!string.IsNullOrWhiteSpace(_returnType))
                signature += $" {_returnType}";

            signature += $" {_name}";

            signature += "(";
            signature += string.Join(", ", Parameters.Select(t => t.GenerateInline()));
            signature += ")";

            if (!HasBody())
                signature += ";";

            return signature.TrimStart();
        }

        public List<string> GenerateLines()
        {
            var ret = new List<string>();

            if (Attributes?.Any() == true)
                ret.AddRange(GetAttributesLines());

            // signature.
            var signature = GenerateSignature();
            ret.Add(signature);

            if (HasBody())
            {
                ret.Add("{");
                ret.AddRange(Children.IdentChildren());
                ret.Add("}");
            }

            return ret;
        }

        public virtual string GetName() => _name;
    }
}
