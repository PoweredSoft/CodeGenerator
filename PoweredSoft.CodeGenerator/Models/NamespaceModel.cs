using System;
using System.Collections.Generic;
using System.Text;
using PoweredSoft.CodeGenerator.Core;

namespace PoweredSoft.CodeGenerator.Models
{
    public class NamespaceModel : IHasName
    {
        public string Name { get; set; }
        public IEnumerable<string> NamespaceParts => Name.Split('.');
        public List<IGeneratable> Children { get; set; } = new List<IGeneratable>();

        public string Generate()
        {
            throw new NotImplementedException();
        }
    }
}
