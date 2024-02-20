using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanilla.POCOs
{
    internal class HtmlBuilder
    {
        HtmlElement root = new HtmlElement();
        private readonly string rootName;

        public HtmlBuilder(string rootName)
        {
            this.rootName = rootName;
            root.NameOfTag = rootName;
        }

        public void AddChildHtmlElement(string childTagName, string childTagText)
        {
            var e = new HtmlElement(childTagName, childTagText);
            root.ListOFHTMLElements.Add(e);
        }

        public override string ToString()
        {
            return root.ToString();
        }

        public void Clear()
        {
            root = new HtmlElement { NameOfTag = rootName };
        }



    }
}
