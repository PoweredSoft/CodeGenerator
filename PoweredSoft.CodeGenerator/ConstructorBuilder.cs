using System;
using System.Collections.Generic;
using System.Text;
using PoweredSoft.CodeGenerator.Constants;
using PoweredSoft.CodeGenerator.Core;

namespace PoweredSoft.CodeGenerator
{
    public class ConstructorBuilder : MethodBuilderBase<ConstructorBuilder>
    {
        private ClassBuilder _class;

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

        
    }
}
