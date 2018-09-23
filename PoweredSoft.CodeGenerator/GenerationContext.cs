using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoweredSoft.CodeGenerator
{
    public class GenerationContext
    {
        public List<FileBuilder> Files { get; set; } = new List<FileBuilder>();

        public IEnumerable<NamespaceBuilder> NameSpaces => Files
            .SelectMany(t => t.Model.Children)
            .Where(t => t is NamespaceBuilder)
            .Cast<NamespaceBuilder>();

        public static GenerationContext Create() => new GenerationContext();

        private IEnumerable<ClassBuilder> FileClasses => Files
            .SelectMany(t => t.Model.Children)
            .Where(t => t is ClassBuilder).Cast<ClassBuilder>();

        private IEnumerable<ClassBuilder> NameSpaceClasses => NameSpaces
            .SelectMany(t => t.Model.Children)
            .Where(t => t is ClassBuilder)
            .Cast<ClassBuilder>();

        public IEnumerable<ClassBuilder> Classes => FileClasses.Union(NameSpaceClasses);

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
                return Classes.FirstOrDefault(t => t.Model.Name == name);

            return NameSpaces
                .Where(t => t.Model.Name == filterNamespace)
                .SelectMany(t => t.Model.Children)
                .Where(t => t is ClassBuilder)
                .Cast<ClassBuilder>()
                .FirstOrDefault(t => t.Model.Name == name);
        }

        public GenerationContext File(Action<FileBuilder> action)
        {
            var file = FileBuilder.Create();
            Files.Add(file);
            action(file);
            return this;
        }

        public GenerationContext SaveToDisk(Encoding encoding)
        {
            Files.ForEach(file =>
            {
                file.SaveToFile(encoding);
            });
            return this;
        }
    }
}
