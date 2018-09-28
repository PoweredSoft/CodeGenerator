using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoweredSoft.CodeGenerator.Constants;
using PoweredSoft.CodeGenerator.Core;
using PoweredSoft.CodeGenerator.Extensions;

namespace PoweredSoft.CodeGenerator
{
    public class MethodBuilder : IMultiLineGeneratable, IHasName, IHasGeneratableChildren
    {
        private string _name;
        private bool _isVirtual;
        private bool _isStatic;
        private string _returnType;
        private bool _isPartial;
        private bool _isAbstract;
        private AccessModifiers _accessModifier;

        public List<ParameterBuilder> Parameters { get; } = new List<ParameterBuilder>();
        public List<IGeneratable> Children { get; } = new List<IGeneratable>();

        public MethodBuilder Name(string name)
        {
            _name = name;
            return this;
        }

        public MethodBuilder Virtual(bool isVirtual)
        {
            _isVirtual = isVirtual;
            return this;
        }

        public MethodBuilder IsStatic(bool isStatic)
        {
            _isStatic = isStatic;
            return this;
        }

        public MethodBuilder ReturnType(string type)
        {
            _returnType = type;
            return this;
        }

        public MethodBuilder Partial(bool partial)
        {
            _isPartial = partial;
            return this;
        }

        public MethodBuilder Abstract(bool isAbstract)
        {
            _isAbstract = isAbstract;
            return this;
        }

        public MethodBuilder AccessModifier(AccessModifiers accessModifier)
        {
            _accessModifier = accessModifier;
            return this;
        }

        public MethodBuilder Parameter(Action<ParameterBuilder> configurator)
        {
            var param = ParameterBuilder.Create();
            Parameters.Add(param);
            configurator(param);
            return this;
        }

        public MethodBuilder Add(Func<IMultiLineGeneratable> child)
        {
            var result = child();
            Children.Add(result);
            return this;
        }

        public MethodBuilder Add(IMultiLineGeneratable child)
        {
            Children.Add(child);
            return this;
        }

        protected string GenerateSignature()
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

            signature += $" {_returnType} {_name}";

            signature += "(";
            signature += string.Join(", ", Parameters.Select(t => t.GenerateInline()));
            signature += ")";

            if (_isPartial || _isAbstract)
                signature += ";";

            return signature;
        }

        public List<string> GenerateLines()
        {
            var ret = new List<string>();

            // signature.
            var signature = GenerateSignature();
            ret.Add(signature);

            if (!_isAbstract && !_isPartial)
            {
                ret.Add("{");
                ret.AddRange(Children.IdentChildren());
                ret.Add("}");
            }

            return ret;
        }

        public string GetName() => _name;
    }
}
