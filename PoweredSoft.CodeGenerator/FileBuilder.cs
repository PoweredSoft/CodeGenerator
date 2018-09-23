using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
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

        public IEnumerable<NamespaceBuilder> Namespaces => Model.Children.Where(t => t is NamespaceBuilder).Cast<NamespaceBuilder>();

        public FileBuilder Namespace(string name, bool createIfNotExisting, Action<NamespaceBuilder> action)
        {
            var ns =  Namespaces.FirstOrDefault(t => t.Model.Name == name);
            if (ns == null && !createIfNotExisting)
                throw new Exception($"Could not find a namespace with name: {name}");

            if (ns == null)
            {
                return Namespace(t =>
                {
                    t.Name(name);
                    action(t);
                });
            }

            action(ns);
            return this;
        }

        public FileBuilder Namespace(Action<NamespaceBuilder> action)
        {
            var ns = NamespaceBuilder.Create();
            Model.Children.Add(ns);
            action(ns);
            return this;
        }

        public FileBuilder Path(string path)
        {
            Model.Path = path;
            return this;
        }

        public FileBuilder Add(IGeneratable child)
        {
            Model.Children.Add(child);
            return this;
        }

        public FileBuilder Using(string usingNamespace)
        {
            if (!Model.Usings.Contains(usingNamespace))
                Model.Usings.Add(usingNamespace);
            return this;
        }

        public void SaveToFile(Encoding encoding)
        {
            var lines = GenerateLines();
            File.WriteAllLines(Model.Path, lines, encoding);
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
