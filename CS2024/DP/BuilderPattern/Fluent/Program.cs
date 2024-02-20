// See https://aka.ms/new-console-template for more information
using Vanilla.POCOs;

//same as Vanilla, with, Fluent ability

var fluentHTMLbuilder = new HtmlBuilderFluent("ul");

fluentHTMLbuilder.AddChildHtmlElement("li", "hello").AddChildHtmlElement("li", "world");

Console.WriteLine(fluentHTMLbuilder.ToString());

