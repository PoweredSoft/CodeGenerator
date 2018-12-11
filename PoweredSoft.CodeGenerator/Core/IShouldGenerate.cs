using System;
using System.Collections.Generic;
using System.Text;

namespace PoweredSoft.CodeGenerator.Core
{
    public interface IShouldGenerateBuilder
    {
        bool ShouldGenerate();
    }

    public interface IShouldGenerateBuilder<TBuilder> : IShouldGenerateBuilder
    {
        TBuilder Generate(bool generate);
    }
}
