namespace PoweredSoft.CodeGenerator.Models
{
    public class RawConditionExpressionModel : ConditionExpressionModel
    {
        public string RawCondition { get; set; }

        public override string Generate()
        {
            return RawCondition;
        }
    }
}