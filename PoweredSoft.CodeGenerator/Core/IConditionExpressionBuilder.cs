using System;
using System.Collections.Generic;
using System.Text;

namespace PoweredSoft.CodeGenerator.Core
{
    public interface IConditionExpressionBuilder : IInlineGeneretable
    {
        bool IsAnd();
        bool ShouldWrap();
    }
}
