using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoweredSoft.CodeGenerator.Constants;
using PoweredSoft.CodeGenerator.Core;

namespace PoweredSoft.CodeGenerator
{
    public class ConstructorBuilder : MethodBuilderBase<ConstructorBuilder>
    {
        private ClassBuilder _class;

        public List<IInlineGeneratable> BaseParameters { get; } = new List<IInlineGeneratable>();

        public override ConstructorBuilder Name(string name)
        {
            throw new InvalidOperationException("Constructor you should set class instead");
        }

        public ConstructorBuilder Class(ClassBuilder parentClass)
        {
            _class = parentClass;
            base.Name(_class.GetName());
            return this;
        }

        public ConstructorBuilder BaseParameter(string raw)
        {
            BaseParameters.Add(RawInlineBuilder.Create(raw));
            return this;
        }

        protected override string GenerateSignature()
        {
            var ret = base.GenerateSignature();

            if (BaseParameters.Any())
                ret += " : base(" + string.Join(", ", BaseParameters.Select(t2 => t2.GenerateInline())) + ")";

            return ret;
        }
    }
}
