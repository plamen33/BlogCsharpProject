using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogJuneMVC.Extensions
{
    public static class MyHelpers
    {
        public static IHtmlString Image(this HtmlHelper htmlHelper, string url)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var img = new TagBuilder("img");
            img.Attributes["alt"] = "[No Photo is Available for this post]";
            img.Attributes["src"] = UrlHelper.GenerateContentUrl(url, htmlHelper.ViewContext.HttpContext);
            img.Attributes["height"] = "300";
            return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
        }
    }

}