using System;
using System.Collections.Generic;
using System.Text;
using PoweredSoft.CodeGenerator.Core;

namespace PoweredSoft.CodeGenerator
{
    public class RawInlineBuilder : IInlineGeneratable
    {
        public static RawInlineBuilder Create() => new RawInlineBuilder();
        public static RawInlineBuilder Create(string raw) => RawInlineBuilder.Create().Raw(raw);

        private string _raw;

        public string GenerateInline() => _raw;

        public RawInlineBuilder Raw(string raw)
        {
            _raw = raw;
            return this;
        }
    }
}
