// See https://aka.ms/new-console-template for more information
using FluentInheritanceRecursiveGenerics.POCOs;

//Similar to Fluent but now with Recursing Generics and Inheritance

var SomeTutor = TutorTeacher.New.UpdateTutorName("Jay").TopicOfTeaching("Board Games").UpdateNextDateOfWorkshop(DateTime.Now).Build();
Console.WriteLine(SomeTutor);
