using System;
using System.Linq;
using System.Text;
using PoweredSoft.CodeGenerator.Constants;
using PoweredSoft.CodeGenerator.Extensions;

namespace PoweredSoft.CodeGenerator.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new 
            {
                SomeInfo = "ABC"
            };

            var ctx = GenerationContext
                .Create()
                .File(file =>
                {
                    file
                        .Using("System")
                        .Namespace(ns =>
                        {
                            ns
                                .Name("Acme.Dal")
                                .Class(c =>
                                {
                                    c
                                        .Name("Person")
                                        .Partial(true)
                                        .Property(p => p.Name("Id").Type("long").Virtual(true))
                                        .Property(p => p.Name("FirstName").Type("string").Virtual(true))
                                        .Property(p => p.Name("LastName").Type("string").Virtual(true).Meta(a));
                                })
                                .Class(c =>
                                {
                                    c
                                        .Name("PersonModelBase")
                                        .Partial(true)
                                        .Property(p => p.Name("Id").Type("long?").Virtual(true))
                                        .Property(p => p.Name("FirstName").Type("string").Virtual(true))
                                        .Property(p => p.Name("LastName").Type("string").Virtual(true))
                                        .Method(m =>
                                        {
                                            m
                                                .Name("From")
                                                .Virtual(true)
                                                .Parameter(p => p.Name("entity").Type("Person"))
                                                .Add(RawLineBuilder.Create("Id = entity.Id"))
                                                .Add(RawLineBuilder.Create("FirstName = entity.FirstName"))
                                                .Add(RawLineBuilder.Create("LastName = entity.LastName"));
                                        })
                                        .Method(m =>
                                        {
                                            m
                                                .Name("To")
                                                .Virtual(true)
                                                .Parameter(p => p.Name("entity").Type("Person"))
                                                .Add(() =>
                                                {
                                                    return IfBuilder
                                                        .Create()
                                                        .RawCondition(rc => rc.Condition("Id != null"))
                                                        .Add(RawLineBuilder.Create("entity.Id = Id.Value"));
                                                })
                                                .Add(RawLineBuilder.Create("entity.FirstName = FirstName"))
                                                .Add(RawLineBuilder.Create("entity.LastName = LastName"));
                                        });
                                })
                                ;
                        });
                });

            var propertyOfMetaA = ctx.FindClass("Person").FindByMeta<PropertyBuilder>(a);

            var lines = ctx.Files.First().GenerateLines();
            Console.WriteLine(string.Join("\n", lines));
            Console.ReadKey();


            /*
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
                                    .Add(RawLineBuilder.Create("DateOfBirth = entity.DateOfBirth"));
                            })
                            .Add(() =>
                            {
                                return ElseIfBuilder
                                    .Create()
                                    .RawCondition(rc => rc.Condition("entity.DateOfBirth == null"))
                                    .Add(RawLineBuilder.Create("DateOFBirth = DateTime.Now"));
                            })
                            .Add(() =>
                            {
                                return ElseBuilder
                                    .Create()
                                    .Add(RawLineBuilder.Create("entity = null"));
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
            Console.ReadKey();*/
        }
    }
}