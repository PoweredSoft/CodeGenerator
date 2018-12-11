using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoweredSoft.CodeGenerator.Core;

namespace PoweredSoft.CodeGenerator
{
    public class GenerationContext
    {
        public List<FileBuilder> Files { get; set; } = new List<FileBuilder>();

        public IEnumerable<NamespaceBuilder> Namespaces => Files
            .SelectMany(t => t.Children)
            .Where(t => t is NamespaceBuilder)
            .Cast<NamespaceBuilder>();

        public static GenerationContext Create() => new GenerationContext();

        private IEnumerable<ClassBuilder> FileClasses => Files
            .SelectMany(t => t.Children)
            .Where(t => t is ClassBuilder).Cast<ClassBuilder>();

        private IEnumerable<ClassBuilder> NamespaceClasses => Namespaces
            .SelectMany(t => t.Children)
            .Where(t => t is ClassBuilder)
            .Cast<ClassBuilder>();

        private IEnumerable<InterfaceBuilder> FileInterfaces => Files
            .SelectMany(t => t.Children)
            .Where(t => t is InterfaceBuilder).Cast<InterfaceBuilder>();

        private IEnumerable<InterfaceBuilder> NamespaceInterfaces => Namespaces
            .SelectMany(t => t.Children)
            .Where(t => t is InterfaceBuilder)
            .Cast<InterfaceBuilder>();

        public IEnumerable<ClassBuilder> Classes => FileClasses.Union(NamespaceClasses);
        public IEnumerable<InterfaceBuilder> Interfaces => FileInterfaces.Union(NamespaceInterfaces);

        public GenerationContext SingleFile(Action<FileBuilder> action)
        {
            if (Files.Any())
            {
                action(Files.First());
                return this;
            }

            var file = FileBuilder.Create();
            Files.Add(file);
            action(file);
            return this;
        }

        public ClassBuilder FindClass(string name, string filterNamespace = null)
        {
            if (null == filterNamespace)
                return Classes.FirstOrDefault(t => t.GetName() == name);

            return Namespaces
                .Where(t => t.GetName() == filterNamespace)
                .SelectMany(t => t.Children)
                .Where(t => t is ClassBuilder)
                .Cast<ClassBuilder>()
                .FirstOrDefault(t => t.GetName() == name);
        }

        public GenerationContext File(Action<FileBuilder> action)
        {
            var file = FileBuilder.Create();
            Files.Add(file);
            action(file);
            return this;
        }

        public GenerationContext FileIfPathIsSet(Action<FileBuilder> action)
        {
            var file = FileBuilder.Create();
            action(file);
            if (file.HasPathConfigured())
                Files.Add(file);
            return this;
        }

        public GenerationContext SaveToDisk(Encoding encoding, bool normalizeNewLines = false)
        {
            Files.ForEach(file =>
            {
                file.SaveToFile(encoding, normalizeNewLines: normalizeNewLines);
            });
            return this;
        }

        public GenerationContext File(string path, Action<FileBuilder> action)
        {
            var existing = Files.FirstOrDefault(t => t.GetPath().Equals(path));
            if (null != existing)
            {
                action(existing);
                return this;
            }

            return File(fb =>
            {
                fb.Path(path);
                action(fb);
            });
        }
    }
}
