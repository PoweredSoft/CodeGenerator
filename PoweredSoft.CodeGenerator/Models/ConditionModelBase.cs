using System.Collections.Generic;
using PoweredSoft.CodeGenerator.Core;

namespace PoweredSoft.CodeGenerator.Models
{
    public abstract class ConditionModelBase
    {
        public abstract string ConditionType { get; }
        public List<ConditionExpressionModel> Expressions { get; set; } = new List<ConditionExpressionModel>();
        public List<IGeneratable> Children { get; set; } = new List<IGeneratable>();
    }
}