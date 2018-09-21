using System;
using System.Collections.Generic;
using System.Text;
using PoweredSoft.CodeGenerator.Constants;
using PoweredSoft.CodeGenerator.Core;

namespace PoweredSoft.CodeGenerator.Models
{
    public class MethodModel : IHasName
    {
        public string ReturnType { get; set; } = "void";
        public string Name { get; set; }
        public List<IGeneratable> Children { get; set; } = new List<IGeneratable>();
        public List<ParameterModel> Parameters { get; set; } = new List<ParameterModel>();
        public bool IsStatic { get; set; }
        public bool IsPartial { get; set; }
        public bool IsVirtual { get; set; }
        public AccessModifiers AccessModifier { get; set; } = AccessModifiers.Public;
        public bool IsAbstract { get; set; }
    }
}
