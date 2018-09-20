using PoweredSoft.CodeGenerator.Core;

namespace PoweredSoft.CodeGenerator.Models
{
    public class ParameterModel : IHasName
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string DefaultValue { get; set; }
    }
}
