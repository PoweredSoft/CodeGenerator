using System.Collections.Generic;
using PoweredSoft.CodeGenerator.Constants;
using PoweredSoft.CodeGenerator.Core;
using PoweredSoft.CodeGenerator.Models;

namespace PoweredSoft.CodeGenerator
{
    public abstract class ClassMemberBuilder<TModel, TBuilder> : IGeneratable
        where TBuilder : class, new()
        where TModel : ClassMemberModel, new()
    {
        public TModel Model { get; protected set; } = new TModel();

        public static TBuilder Create() => new TBuilder();

        public TBuilder Type(string typeName)
        {
            Model.Type = typeName;
            return this as TBuilder;
        }

        public TBuilder Comment(string comment)
        {
            Model.Comment = comment;
            return this as TBuilder;
        }

        public TBuilder Name(string name)
        {
            Model.Name = name;
            return this as TBuilder;
        }

        public TBuilder AccessModifier(AccessModifiers access)
        {
            Model.AccessModifier = access;
            return this as TBuilder;
        }

        public TBuilder DefaultValue(string defaultValue)
        {
            Model.DefaultValue = defaultValue;
            return this as TBuilder;
        }

        public TBuilder Static(bool isStatic)
        {
            Model.IsStatic = isStatic;
            return this as TBuilder;
        }

        public abstract List<string> GenerateLines();
    }
}