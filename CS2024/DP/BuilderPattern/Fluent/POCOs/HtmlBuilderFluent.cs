using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanilla.POCOs
{
    internal class HtmlBuilderFluent
    {
        HtmlElement root = new HtmlElement();
        private readonly string rootName;

        public HtmlBuilderFluent(string rootName)
        {
            this.rootName = rootName;
            root.NameOfTag = rootName;
        }

        //So, when you compare this specific function with the non-fluent counterpart
        //the key difference is we are returning a reference to the object using this
        public HtmlBuilderFluent AddChildHtmlElement(string childTagName, string childTagText)
        {
            var e = new HtmlElement(childTagName, childTagText);
            root.ListOFHTMLElements.Add(e);
            return this;
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
