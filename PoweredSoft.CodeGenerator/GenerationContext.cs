using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoweredSoft.CodeGenerator
{
    public class GenerationContext
    {
        public string OutputDirectory { get; set; }
        public bool OverrideIfExists { get; set; } = false;
        public List<FileBuilder> Files { get; set; } = new List<FileBuilder>();

        public IEnumerable<NamespaceBuilder> NameSpaces => Files
            .SelectMany(t => t.Model.Children)
            .Where(t => t is NamespaceBuilder)
            .Cast<NamespaceBuilder>();

        private IEnumerable<ClassBuilder> FileClasses => Files
            .SelectMany(t => t.Model.Children)
            .Where(t => t is ClassBuilder).Cast<ClassBuilder>();

        private IEnumerable<ClassBuilder> NameSpaceClasses => NameSpaces
            .SelectMany(t => t.Model.Children)
            .Where(t => t is ClassBuilder)
            .Cast<ClassBuilder>();

        public IEnumerable<ClassBuilder> Classes => FileClasses.Union(NameSpaceClasses);

        public GenerationContext File(Action<FileBuilder> action)
        {
            var file = FileBuilder.Create();
            Files.Add(file);
            action(file);
            return this;
        }
    }
}
