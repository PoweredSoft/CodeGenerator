namespace PoweredSoft.CodeGenerator
{
    public class ElseIfBuilder : ConditionBuilder<ElseIfBuilder>
    {
        protected override string ConditionType => "else if";
    }
}