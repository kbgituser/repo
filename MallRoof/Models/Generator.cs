using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace MallRoof.Models
{
    public class Generator
    {
        
        public Generator()
        {
            
        }
        public IReadOnlyCollection<SitemapNode> GetSitemapNodes(UrlHelper urlHelper)
        {



            //List<string> nodes = new List<string>();

            //nodes.Add(urlHelper.AbsoluteRouteUrl("Default", new { controller = "Home", action = "Index" }));
            //nodes.Add(urlHelper.AbsoluteRouteUrl("Default", new { controller = "Home", action = "About" }));
            //nodes.Add(urlHelper.AbsoluteRouteUrl("Default", new { controller = "Home", action = "Contact" }));
            //nodes.Add(urlHelper.AbsoluteRouteUrl("Default", new { controller = "Home", action = "Instruction" }));
            //nodes.Add(urlHelper.AbsoluteRouteUrl("Default", new { controller = "Malls", action = "Index" }));
            //nodes.Add(urlHelper.AbsoluteRouteUrl("Default", new { controller = "Demands", action = "Index" }));
            //nodes.Add(urlHelper.AbsoluteRouteUrl("Default", new { controller = "Account", action = "Login" }));


            List<SitemapNode> nodes = new List<SitemapNode>();

            nodes.Add(new SitemapNode(){Url = urlHelper.AbsoluteRouteUrl("Default", new { controller = "", action = "" }), Priority = 1});
            nodes.Add(new SitemapNode(){Url = urlHelper.AbsoluteRouteUrl("Default", new { controller = "Home", action = "About" }), Priority = 0.9});
            nodes.Add(new SitemapNode() { Url = urlHelper.AbsoluteRouteUrl("Default", new { controller = "Home", action = "Contact" }), Priority = 0.9 });
            nodes.Add(new SitemapNode() { Url = urlHelper.AbsoluteRouteUrl("Default", new { controller = "Home", action = "Instruction" }), Priority = 0.9 });
            nodes.Add(new SitemapNode() { Url = urlHelper.AbsoluteRouteUrl("Default", new { controller = "Malls", action = "Index" }), Priority = 0.8 });
            nodes.Add(new SitemapNode() { Url = urlHelper.AbsoluteRouteUrl("Default", new { controller = "Demands", action = "Index" }), Priority = 0.8 });
            nodes.Add(new SitemapNode() { Url = urlHelper.AbsoluteRouteUrl("Default", new { controller = "Account", action = "Login" }), Priority = 0.7 });
            

            return nodes;
        }

        public string GetSitemapDocument(IEnumerable<SitemapNode> sitemapNodes)
        {
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XElement root = new XElement(xmlns + "urlset");

            foreach (SitemapNode sitemapNode in sitemapNodes)
            {
                XElement urlElement = new XElement(
            xmlns + "url",
            new XElement(xmlns + "loc", Uri.EscapeUriString(sitemapNode.Url)),
            sitemapNode.LastModified == null ? null : new XElement(
                xmlns + "lastmod",
                sitemapNode.LastModified.Value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")),
            sitemapNode.Frequency == null ? null : new XElement(
                xmlns + "changefreq",
                sitemapNode.Frequency.Value.ToString().ToLowerInvariant()),
            sitemapNode.Priority == null ? null : new XElement(
                xmlns + "priority",
                sitemapNode.Priority.Value.ToString("F1", System.Globalization.CultureInfo.InvariantCulture)));
                root.Add(urlElement);
            }

            XDocument document = new XDocument(root);
            return document.ToString();
        }
    }
    public static class UrlHelperExtensions
    {
        public static string AbsoluteRouteUrl(this UrlHelper urlHelper,
            string routeName, object routeValues = null)
        {
            string scheme = urlHelper.RequestContext.HttpContext.Request.Url.Scheme;
            return urlHelper.RouteUrl(routeName, routeValues, scheme);
        }
    }
}