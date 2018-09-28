namespace PoweredSoft.CodeGenerator
{
    public class IfBuilder : ConditionBuilder<IfBuilder>
    {
        protected override string ConditionType => "if";
    }
}