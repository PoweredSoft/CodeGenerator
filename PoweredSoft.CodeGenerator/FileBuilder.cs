using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Text;
using PoweredSoft.CodeGenerator.Core;
using PoweredSoft.CodeGenerator.Extensions;

namespace PoweredSoft.CodeGenerator
{
    public class FileBuilder : IMultiLineGeneratable, IHasGeneratableChildren
    {
        private string _path;

        public static FileBuilder Create() => new FileBuilder();
        public List<IGeneratable> Children { get; } = new List<IGeneratable>();
        public List<string> Usings { get; } = new List<string>();

        public IEnumerable<NamespaceBuilder> Namespaces => Children.Where(t => t is NamespaceBuilder).Cast<NamespaceBuilder>();

        public FileBuilder Namespace(string name, bool createIfNotExisting, Action<NamespaceBuilder> action)
        {
            var ns =  Namespaces.FirstOrDefault(t => t.GetName() == name);
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
            Children.Add(ns);
            action(ns);
            return this;
        }

        public FileBuilder Path(string path)
        {
            _path = path;
            return this;
        }

        public FileBuilder Add(IGeneratable child)
        {
            Children.Add(child);
            return this;
        }

        public FileBuilder Using(string usingNamespace)
        {
            if (!Usings.Contains(usingNamespace))
                Usings.Add(usingNamespace);
            return this;
        }

        public void SaveToFile(Encoding encoding)
        {
            var lines = GenerateLines();
            File.WriteAllLines(_path, lines, encoding);
        }

        public List<string> GenerateLines()
        {
            var ret = new List<string>();
            ret.AddRange(Usings.Select(t => $"using {t};"));
            ret.AddRange(Children.SelectMany(t =>
            { 
                var partLines = t.ToLines();
                partLines.Insert(0, "");
                return partLines;
            }).ToList());

            return ret;
        }

        public bool HasPathConfigured() => !string.IsNullOrWhiteSpace(_path);

        public string GetPath() => _path;
    }
}
