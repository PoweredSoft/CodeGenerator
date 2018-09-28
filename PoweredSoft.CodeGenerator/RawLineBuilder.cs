using System;
using System.Collections.Generic;
using System.Text;
using PoweredSoft.CodeGenerator.Core;

namespace PoweredSoft.CodeGenerator
{
    public class RawLineBuilder : IMultiLineGeneratable
    {
        public string _raw;

        public static RawLineBuilder Create(string rawLine = null)
        {
            var ret = new RawLineBuilder();
            ret.Raw(rawLine);
            return ret;
        }

        public RawLineBuilder Raw(string raw)
        {
            _raw = raw;
            return this;
        }

        public RawLineBuilder Append(string raw)
        {
            _raw += raw;
            return this;
        }

        public List<string> GenerateLines()
        {
            return new List<string>{ $"{_raw};" };
        }
    }
}
