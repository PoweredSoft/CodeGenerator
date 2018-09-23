using System;
using System.Collections.Generic;
using System.Text;
using PoweredSoft.CodeGenerator.Core;

namespace PoweredSoft.CodeGenerator.Models
{
    public class FileModel
    {
        public List<string> Usings { get; set; } = new List<string>();
        public List<IGeneratable> Children = new List<IGeneratable>();
        public string Path { get; set; }
    }
}
