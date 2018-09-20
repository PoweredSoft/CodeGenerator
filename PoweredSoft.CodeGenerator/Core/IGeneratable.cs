using System.Collections.Generic;

namespace PoweredSoft.CodeGenerator.Core
{
    public interface IGeneratable
    {
        List<string> GenerateLines();
    }
}
