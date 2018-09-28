using System.Collections.Generic;

namespace PoweredSoft.CodeGenerator.Core
{
    public interface IGeneratable
    {

    }

    public interface IMultiLineGeneratable : IGeneratable
    {
        List<string> GenerateLines();
    }

    public interface ISingleLineGeneratable : IGeneratable
    {
        string GenerateLine();
    }

    public interface IInlineGeneretable : IGeneratable
    {
        string GenerateInline();
    }
}
