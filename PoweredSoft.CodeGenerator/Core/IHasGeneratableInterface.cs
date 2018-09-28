using System;
using System.Collections.Generic;
using System.Text;

namespace PoweredSoft.CodeGenerator.Core
{
    public interface IHasGeneratableChildren
    {
        List<IGeneratable> Children { get; set; }
    }
}
