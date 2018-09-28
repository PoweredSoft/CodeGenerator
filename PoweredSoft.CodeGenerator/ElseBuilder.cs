namespace PoweredSoft.CodeGenerator
{
    public class ElseBuilder : ConditionBuilder<ElseBuilder>
    {
        protected override string ConditionType => "else";
    }
}