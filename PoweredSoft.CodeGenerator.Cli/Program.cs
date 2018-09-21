using System;
using System.Linq;
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
                .Class(c => c
                    .Partial(true)
                    .Name("PersonModelBase")
                    //.Field(f => f.AccessModifier(AccessModifiers.Private).Name("_firstName").Type("string"))
                    //.Field(f => f.AccessModifier(AccessModifiers.Private).Name("_lastName").Type("string"))
                    .Property(p => p.Name("FirstName").Type("string").Virtual(true))
                    .Property(p => p.Name("LastName").Type("string").Virtual(true))
                    .Property(p => p.Name("DateOfBirth").Type("DateTime?").Virtual(true))
                    .Method(fromMethod =>
                    {
                        fromMethod
                            .Name("From")
                            .Virtual(true)
                            .ReturnType("void")
                            .Parameter(p => p.Name("entity").Type("Acme.Dal.Person"))
                            .Add(() =>
                            {
                                return IfBuilder
                                    .Create()
                                    .RawCondition(rc => rc.Condition("entity.DateOfBirth != null"))
                                    .Add(RawLineBuilder.Create("DateOfBirth = entity.DateOfBirth"))
                                    .Add(RawLineBuilder.Create("entity = entity"));
                            });
                    })
                );

            
            var lines = FileBuilder
                .Create()
                .Using("System")
                .Add(ns)
                .GenerateLines();

                //.SaveToFile("C:\\test\\Person.cs", Encoding.UTF8);

            Console.WriteLine(string.Join("\n", lines));
            Console.ReadKey();
        }
    }
}