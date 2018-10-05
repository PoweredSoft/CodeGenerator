using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using PoweredSoft.CodeGenerator.Core;
using PoweredSoft.CodeGenerator.Extensions;

namespace PoweredSoft.CodeGenerator
{
    public class NamespaceBuilder : IMultiLineGeneratable, IHasGeneratableChildren, IHasName
    {
        private string _name;
        public List<IGeneratable> Children { get; } = new List<IGeneratable>();
        public IEnumerable<ClassBuilder> Classes => Children.Where(t => t is ClassBuilder).Cast<ClassBuilder>();
        public IEnumerable<InterfaceBuilder> Interfaces => Children.Where(t => t is InterfaceBuilder).Cast<InterfaceBuilder>();
        public static NamespaceBuilder Create() => new NamespaceBuilder();

        public NamespaceBuilder Name(string name)
        {
            _name = name;
            return this;
        }

        public NamespaceBuilder Class(Action<ClassBuilder> configurator)
        {
            var child = ClassBuilder.Create();
            Children.Add(child);
            configurator(child);
            return this;
        }

        public NamespaceBuilder Class(string name, bool createIfNotExist, Action<ClassBuilder> action)
        {
            var child = Classes.FirstOrDefault(t => t.GetName() == name);
            if (child == null && !createIfNotExist)
                throw new Exception($"Could not find class with name: {name}");

            if (child == null)
            {
                return Class(t =>
                {
                    t.Name(name);
                    action(t);
                });
            }

            action(child);
            return this;
        }

        public NamespaceBuilder NameSpace(Action<NamespaceBuilder> configurator)
        {
            var child = NamespaceBuilder.Create();
            Children.Add(child);
            configurator(child);
            return this;
        }

        public List<string> GenerateLines()
        {
            var ret = new List<string>();
            ret.Add($"namespace {_name}");
            ret.Add("{");
            ret.AddRange(Children.IdentChildren());
            ret.Add("}");
            return ret;
        }

        public string GetName() => _name;

        public NamespaceBuilder Interface(Action<InterfaceBuilder> action)
        {
            var i = InterfaceBuilder.Create();
            Children.Add(i);
            action(i);
            return this;
        }

        public NamespaceBuilder Interface(string name, bool createIfNotExist, Action<InterfaceBuilder> action)
        {
            var child = Interfaces.FirstOrDefault(t => t.GetName() == name);
            if (child == null && !createIfNotExist)
                throw new Exception($"Could not find interface with name: {name}");

            if (child == null)
            {
                return Interface(t =>
                {
                    t.Name(name);
                    action(t);
                });
            }

            action(child);
            return this;
        }
    }
}
