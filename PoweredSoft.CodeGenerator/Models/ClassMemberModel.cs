using PoweredSoft.CodeGenerator.Constants;
using PoweredSoft.CodeGenerator.Core;

namespace PoweredSoft.CodeGenerator.Models
{
    public abstract class ClassMemberModel : IHasName
    {
        public AccessModifiers AccessModifier { get; set; } = AccessModifiers.Public;
        public string Type { get; set; }
        public string Name { get; set; }
        public string DefaultValue { get; set; }
        public bool IsStatic { get; set; } = false;
    }
}
