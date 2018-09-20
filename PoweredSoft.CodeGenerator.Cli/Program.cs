using System;
using System.Text;
using PoweredSoft.CodeGenerator.Constants;

namespace PoweredSoft.CodeGenerator.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var ns = NamespaceBuilder
                .Create()
                .Name("Acme.Models")
                .AddClass(c => c
                    .Partial(true)
                    .Name("Person")
                    //.Field(f => f.AccessModifier(AccessModifiers.Private).Name("_firstName").Type("string"))
                    //.Field(f => f.AccessModifier(AccessModifiers.Private).Name("_lastName").Type("string"))
                    .Field(f => f.AccessModifier(AccessModifiers.Private).Name("_dateOfBirth").Type("DateTime?"))
                    .Property(p => p.Name("FirstName").Type("string"))
                    .Property(p => p.Name("LastName").Type("string"))
                    .Property(p =>
                        p.Name("DateOfBirth").Type("DateTime?").DefaultValue("null").UnderlyingMember("_dateOfBirth"))
                );

            FileBuilder
                .Create()
                .Using("System")
                .Add(ns)
                .SaveToFile("C:\\test\\Person.cs", Encoding.UTF8);
        }
    }
}