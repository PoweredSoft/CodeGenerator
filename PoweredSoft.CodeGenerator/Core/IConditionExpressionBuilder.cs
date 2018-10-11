using System;
using System.Collections.Generic;
using System.Text;

namespace PoweredSoft.CodeGenerator.Core
{
    public interface IConditionExpressionBuilder : IInlineGeneratable
    {
        bool IsAnd();
        bool ShouldWrap();
    }
}
