// See https://aka.ms/new-console-template for more information

//facade style builders. Builder that uses other Builders to build things.

using FacetedBuilder.POCOs;

var sometutorbuilder = new TutorBuilder();

TutorTeacher exampletutor = sometutorbuilder.WorkDetails.AddWorkDetails(10, "Catan").PersonalDetails.AddPersonalDetails("Jay", "Mysore", "India");

Console.WriteLine(exampletutor.ToString());
