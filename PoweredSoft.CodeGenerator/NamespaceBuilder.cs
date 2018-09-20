using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using PoweredSoft.CodeGenerator.Core;
using PoweredSoft.CodeGenerator.Extensions;
using PoweredSoft.CodeGenerator.Models;

namespace PoweredSoft.CodeGenerator
{
    public class NamespaceBuilder : IGeneratable
    {
        protected NamespaceModel Model { get; set; } = new NamespaceModel();

        public static NamespaceBuilder Create() => new NamespaceBuilder();

        public NamespaceBuilder Name(string name)
        {
            Model.Name = name;
            return this;
        }

        public NamespaceBuilder AddClass(Action<ClassBuilder> configurator)
        {
            var child = ClassBuilder.Create();
            Model.Children.Add(child);
            configurator(child);
            return this;
        }

        public NamespaceBuilder AddSubNameSpace(Action<NamespaceBuilder> configurator)
        {
            var child = NamespaceBuilder.Create();
            Model.Children.Add(child);
            configurator(child);
            return this;
        }

        public List<string> GenerateLines()
        {
            var ret = new List<string>();
            ret.Add($"namespace {Model.Name}");
            ret.Add("{");
            ret.AddRange(Model.Children.IdentChildren());
            ret.Add("}");
            return ret;
        }
    }
}
