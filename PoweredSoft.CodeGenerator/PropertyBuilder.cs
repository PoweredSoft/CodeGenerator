using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using PoweredSoft.CodeGenerator.Constants;
using PoweredSoft.CodeGenerator.Extensions;
using PoweredSoft.CodeGenerator.Models;

namespace PoweredSoft.CodeGenerator
{
    public class PropertyBuilder : ClassMemberBuilder<PropertyModel, PropertyBuilder>
    {
        public override List<string> GenerateLines()
        {
            if (!string.IsNullOrWhiteSpace(Model.UnderlyingMember))
                return GenerateUnderlyingProperty();

            return GenerateSimpleProperty();
        }

        protected List<string> GenerateUnderlyingProperty()
        {
            var line = $"{Model.AccessModifier.Generate()}";

            if (Model.IsStatic)
                line += " static";

            line += $" {Model.Type} {Model.Name}";
            line += " {";


            // get


            // set

            if (Model.CanSet)
            {
                line += " { get; ";

                if (Model.SetAccessModifier != AccessModifiers.Public)
                    line += $"{Model.SetAccessModifier.Generate()} set; }}";
                else
                    line += " set; }";

                if (!string.IsNullOrWhiteSpace(Model.DefaultValue))
                    line += $" = {Model.DefaultValue};";
            }
            else
            {
                line += " { get; }";
            }


            return new List<string> {line};
        }

        protected List<string> GenerateSimpleProperty()
        {
            var line = $"{Model.AccessModifier.Generate()}";

            if (Model.IsStatic)
                line += " static";

            line += $" {Model.Type} {Model.Name}";
            line += " { get;";

            if (Model.CanSet)
            {
                if (Model.SetAccessModifier != Model.AccessModifier)
                    line += $" {Model.SetAccessModifier.Generate()}";

                line += " set;";
            }

            line += " }";

            if (Model.CanSet && !string.IsNullOrWhiteSpace(Model.DefaultValue))
                line += $" = {Model.DefaultValue};";

            return new List<string> { line };
        }

        public PropertyBuilder CanSet(bool canSet)
        {
            Model.CanSet = canSet;
            return this;
        }

        public PropertyBuilder SetAccessModifier(AccessModifiers am)
        {
            Model.SetAccessModifier = am;
            return this;
        }

        public PropertyBuilder UnderlyingMember(string underlyingMember)
        {
            Model.UnderlyingMember = underlyingMember;
            return this;
        }
    }
}
