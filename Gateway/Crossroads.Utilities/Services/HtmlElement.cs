using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;

namespace Crossroads.Utilities.Services
{
    public class HtmlElement
    {
        
        private readonly String elementStart;
        private readonly String elementText;
        private readonly String elementEnd;

        private readonly List<HtmlElement> childrenElements;

        public HtmlElement(String el)
        {            
            this.elementStart = "<" + el + ">";
            this.elementEnd = "</" + el + ">";
        }

        public HtmlElement(String el, String text) : this(el)
        {
            this.elementText = text;
        }

        public HtmlElement(String el, List<HtmlElement> children) : this(el)
        {
            this.childrenElements = children;
        }

        public String Build(String el, Func<HtmlElement, String> func = null)
        {
            return "";
        }

        public override String ToString()
        {
            return "";
        }
    }
}