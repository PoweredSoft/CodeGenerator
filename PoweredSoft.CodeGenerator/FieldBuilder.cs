using System;
using System.Collections.Generic;
using System.Text;
using PoweredSoft.CodeGenerator.Core;
using PoweredSoft.CodeGenerator.Extensions;

namespace PoweredSoft.CodeGenerator
{
    public class FieldBuilder : ClassMemberBuilder<FieldBuilder>
    {
        public override List<string> GenerateLines()
        {
            var fieldLine = $"{_accessModifier.Generate()}";

            if (_isStatic)
                fieldLine += " static";

            fieldLine += $" {_type} {_name}";

            if (!string.IsNullOrWhiteSpace(_defaultValue))
                fieldLine += $" = {_defaultValue}";

            fieldLine += ";";
            return new List<string> {fieldLine};
        }
    }
}
