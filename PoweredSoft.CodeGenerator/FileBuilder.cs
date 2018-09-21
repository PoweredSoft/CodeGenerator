using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PoweredSoft.CodeGenerator.Core;
using PoweredSoft.CodeGenerator.Models;

namespace PoweredSoft.CodeGenerator
{
    public class FileBuilder : IGeneratable
    {
        public static FileBuilder Create() => new FileBuilder();
        public FileModel Model { get; protected set; } = new FileModel();

        public FileBuilder Namespace(Action<NamespaceBuilder> action)
        {
            var ns = NamespaceBuilder.Create();
            Model.Children.Add(ns);
            action(ns);
            return this;
        }

        public FileBuilder Add(IGeneratable child)
        {
            Model.Children.Add(child);
            return this;
        }

        public FileBuilder Using(string usingNamespace)
        {
            Model.Usings.Add(usingNamespace);
            return this;
        }

        public void SaveToFile(string path, Encoding encoding)
        {
            var lines = GenerateLines();
            File.WriteAllLines(path, lines, encoding);
        }

        public List<string> GenerateLines()
        {
            var ret = new List<string>();
            ret.AddRange(Model.Usings.Select(t => $"using {t};"));
            ret.AddRange(Model.Children.SelectMany(t =>
            { 
                var partLines = t.GenerateLines();
                partLines.Insert(0, "");
                return partLines;
            }).ToList());

            return ret;
        }
    }
}
