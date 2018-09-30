using System;
using System.Collections.Generic;
using System.Text;
using PoweredSoft.CodeGenerator.Core;

namespace PoweredSoft.CodeGenerator
{
    public class SingleLineCommentBuilder : ISingleLineGeneratable
    {
        private string _comment;

        public static SingleLineCommentBuilder Create() => new SingleLineCommentBuilder();
        public static SingleLineCommentBuilder Create(string comment) => Create().Comment(comment);

        public SingleLineCommentBuilder Comment(string comment)
        {
            _comment = comment;
            return this;
        }

        public string GenerateLine()
        {
            return "// " + _comment;
        }
    }
}
