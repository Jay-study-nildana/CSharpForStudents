// See https://aka.ms/new-console-template for more information
using Vanilla.POCOs;

//The bare bones builder pattern. good for understanding the basic concept.

//we are really just trying to do what string builder does, but, with the scene changed to HTML Tree of DOM elements

var builder = new HtmlBuilder("ul");
builder.AddChildHtmlElement("li", "hello");
builder.AddChildHtmlElement("li", "world");
Console.WriteLine(builder.ToString());




