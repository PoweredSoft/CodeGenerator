using PoweredSoft.CodeGenerator.Constants;

namespace PoweredSoft.CodeGenerator.Models
{
    public class PropertyModel : ClassMemberModel
    {
        public AccessModifiers SetAccessModifier { get; set; } = AccessModifiers.Public;
        public bool CanSet { get; set; } = true;
        public string UnderlyingMember { get; set; }
        public bool IsVirtual { get; set; }
    }
}
