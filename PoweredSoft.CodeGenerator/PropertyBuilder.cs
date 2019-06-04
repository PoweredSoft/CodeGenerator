﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using PoweredSoft.CodeGenerator.Constants;
using PoweredSoft.CodeGenerator.Extensions;

namespace PoweredSoft.CodeGenerator
{
    public class PropertyBuilder : ClassMemberBuilder<PropertyBuilder>
    {
        protected string _underlyingMember;
        private bool _canSet = true;
        private AccessModifiers _setAccessModifier = AccessModifiers.Public;
        private bool _isVirtual;

        public override List<string> GenerateLines()
        {
            if (!string.IsNullOrWhiteSpace(_underlyingMember))
                return GenerateUnderlyingProperty();

            return GenerateSimpleProperty();
        }

        protected List<string> GenerateUnderlyingProperty()
        {
            var ret = new List<string>();

            if (Attributes?.Any() == true)
                ret.AddRange(GetAttributesLines());

            // property line.,
            var propertyLine = $"{_accessModifier.Generate()}";

            if (_isStatic)
                propertyLine += " static";
            
            propertyLine += $" {_type} {_name}";
            propertyLine = propertyLine.TrimStart();

            ret.Add(propertyLine);

            // open.
            ret.Add("{");

            // get
            ret.Add("    get");
            ret.Add("    {");
            ret.Add($"        return {_underlyingMember};");
            ret.Add("    }");

            if (_canSet)
            {
                var setAccess = _accessModifier != AccessModifiers.Omit && _setAccessModifier != _accessModifier ? $"{_setAccessModifier.Generate()} " : "";
                ret.Add($"    {setAccess}set");
                ret.Add("    {");
                ret.Add($"        {_underlyingMember} = value;");
                ret.Add("    }");
            }

            // end
            ret.Add("}");
            return ret;
        }

        protected List<string> GenerateSimpleProperty()
        {
            var ret = new List<string>();

            if (Attributes?.Any() == true)
                ret.AddRange(GetAttributesLines());

            var line = $"{_accessModifier.Generate()}";

            if (_isStatic)
                line += " static";

            if (_isVirtual)
                line += " virtual";

            line += $" {_type} {_name}";
            line += " { get;";

            if (_canSet)
            {
                if (_accessModifier != AccessModifiers.Omit && _setAccessModifier != _accessModifier)
                    line += $" {_setAccessModifier.Generate()}";

                line += " set;";
            }

            line += " }";

            if (_canSet && !string.IsNullOrWhiteSpace(_defaultValue))
                line += $" = {_defaultValue};";

            if (!string.IsNullOrWhiteSpace(_comment))
                line += $"// {_comment}";

            line = line.TrimStart();
            ret.Add(line);
            return ret;
        }

        public PropertyBuilder Virtual(bool isVirtual)
        {
            _isVirtual = isVirtual;
            return this;
        }

        public PropertyBuilder CanSet(bool canSet)
        {
            _canSet = canSet;
            return this;
        }

        public PropertyBuilder SetAccessModifier(AccessModifiers am)
        {
            _setAccessModifier = am;
            return this;
        }

        public PropertyBuilder UnderlyingMember(string underlyingMember)
        {
            _underlyingMember = underlyingMember;
            return this;
        }
    }
}
