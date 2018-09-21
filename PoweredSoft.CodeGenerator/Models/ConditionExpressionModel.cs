namespace PoweredSoft.CodeGenerator.Models
{
    public abstract class ConditionExpressionModel
    {
        public bool And { get; set; } = true;
        public bool Wrap { get; set; } = false;
        public abstract string Generate();
    }
}