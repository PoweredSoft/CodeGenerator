using System;
using System.Collections.Generic;
using System.Text;
using PoweredSoft.CodeGenerator.Core;

namespace PoweredSoft.CodeGenerator
{
    public class RawLineBuilder : ISingleLineGeneratable
    {
        public string _raw;
        private bool _noEndOfLine;
        private string _comment;

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

        public RawLineBuilder NoEndOfLine()
        {
            _noEndOfLine = true;
            return this;
        }

        public RawLineBuilder Comment(string comment)
        {
            _comment = comment;
            return this;
        }

        public string GenerateLine()
        {
            var l = $"{_raw}{(_noEndOfLine ? "" : ";")}";
            if (!string.IsNullOrWhiteSpace(_comment))
                l += $" // {_comment}";
            return l;
        }
    }
}
