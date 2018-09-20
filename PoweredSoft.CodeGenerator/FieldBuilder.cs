using System;
using System.Collections.Generic;
using System.Text;
using PoweredSoft.CodeGenerator.Core;
using PoweredSoft.CodeGenerator.Extensions;
using PoweredSoft.CodeGenerator.Models;

namespace PoweredSoft.CodeGenerator
{
    public class FieldBuilder : ClassMemberBuilder<FieldModel, FieldBuilder>, IGeneratable
    {
        public override List<string> GenerateLines()
        {
            var fieldLine = $"{Model.AccessModifier.Generate()}";

            if (Model.IsStatic)
                fieldLine += " static";

            fieldLine += $" {Model.Type} {Model.Name}";

            if (!string.IsNullOrWhiteSpace(Model.DefaultValue))
                fieldLine += $" = {Model.DefaultValue}";

            fieldLine += ";";
            return new List<string> {fieldLine};
        }
    }
}
