using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanilla.POCOs
{
    internal class HtmlElement
    {
        public string NameOfTag, TextContentInsideTag;
        public List<HtmlElement> ListOFHTMLElements = new List<HtmlElement>();
        private const int indentSize = 2;
        private const int rootLocation = 0;

        public HtmlElement()
        {
            //empty constructor
        }

        public HtmlElement(string name, string text)
        {
            NameOfTag = name;
            TextContentInsideTag = text;
        }

        private string ToStringImplementation(int indent)
        {
            var sb = new StringBuilder();
            //calculate the amount of indent space for each tag.
            //the deeper we go inside the tag, the more will be in the indentation
            var indentspace = new string(' ', indentSize * indent);
            sb.Append($"{indentspace}<{NameOfTag}>\n"); //opening tag.
            if (!string.IsNullOrWhiteSpace(TextContentInsideTag))
            {
                sb.Append(new string(' ', indentSize * (indent + 1)));
                sb.Append(TextContentInsideTag);
                sb.Append("\n");
            }

            foreach (var e in ListOFHTMLElements)
                sb.Append(e.ToStringImplementation(indent + 1));

            sb.Append($"{indentspace}</{NameOfTag}>\n"); //closing tag
            return sb.ToString();
        }

        public override string ToString()
        {
            return ToStringImplementation(rootLocation);
        }
    }
}
