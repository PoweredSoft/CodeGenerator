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
            var ret = new List<string>();

            // property line.,
            var propertyLine = $"{Model.AccessModifier.Generate()}";

            if (Model.IsStatic)
                propertyLine += " static";
            
            propertyLine += $" {Model.Type} {Model.Name}";

            ret.Add(propertyLine);

            // open.
            ret.Add("{");

            // get
            ret.Add("    get");
            ret.Add("    {");
            ret.Add($"        return {Model.UnderlyingMember};");
            ret.Add("    }");

            if (Model.CanSet)
            {
                var setAccess = Model.SetAccessModifier != Model.AccessModifier ? $"{Model.SetAccessModifier.Generate()} " : "";
                ret.Add($"    {setAccess}set");
                ret.Add("    {");
                ret.Add($"        {Model.UnderlyingMember} = value;");
                ret.Add("    }");
            }

            // end
            ret.Add("}");
            return ret;
        }

        protected List<string> GenerateSimpleProperty()
        {
            var line = $"{Model.AccessModifier.Generate()}";

            if (Model.IsStatic)
                line += " static";

            if (Model.IsVirtual)
                line += " virtual";

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

            if (!string.IsNullOrWhiteSpace(Model.Comment))
                line += $"// {Model.Comment}";

            return new List<string> { line };
        }

        public PropertyBuilder Virtual(bool isVirtual)
        {
            Model.IsVirtual = isVirtual;
            return this;
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
