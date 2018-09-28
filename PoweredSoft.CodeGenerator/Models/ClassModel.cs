using System.Collections.Generic;
using PoweredSoft.CodeGenerator.Constants;
using PoweredSoft.CodeGenerator.Core;

namespace PoweredSoft.CodeGenerator.Models
{
    public class ClassModel : IHasName, IHasGeneratableChildren, IHasMeta
    {
        public string Name { get; set; }
        public AccessModifiers AccessModifier { get; set; } = AccessModifiers.Public;
        public bool IsStatic { get; set; } = false;
        public bool IsPartial { get; set; } = false;
        public List<IGeneratable> Children { get; set; } = new List<IGeneratable>();
        public object Meta { get; set; }
    }
}
