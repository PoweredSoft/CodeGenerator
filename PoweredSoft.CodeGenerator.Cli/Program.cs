using System;
using PoweredSoft.CodeGenerator.Constants;

namespace PoweredSoft.CodeGenerator.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = NamespaceBuilder
                .Create()
                .Name("Acme.Models")
                .AddClass(c => c
                    .Partial(true)
                    .Name("Person")
                    //.Field(f => f.AccessModifier(AccessModifiers.Private).Name("_firstName").Type("string"))
                    //.Field(f => f.AccessModifier(AccessModifiers.Private).Name("_lastName").Type("string"))
                    //.Field(f => f.AccessModifier(AccessModifiers.Private).Name("_dateOfBirth").Type("DateTime?").DefaultValue("null"))
                    .Property(p => p.Name("FirstName").Type("string"))
                    .Property(p => p.Name("LastName").Type("string"))
                    .Property(p => p.Name("DateOfBirth").Type("DateTime?").DefaultValue("null").SetAccessModifier(AccessModifiers.Internal))
                )
                .GenerateLines();

            Console.WriteLine(string.Join("\n", lines));
            Console.ReadKey();
        }
    }
}