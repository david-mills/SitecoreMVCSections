namespace SitecoreMVCSections
{
    using System.Web.Mvc;
    using System.Web;
    using System.Web.Routing;
    using Sitecore.Mvc.Helpers;
    using Sitecore.Mvc.Presentation;

    public static class SitecoreMVCSectionExtensions
    {
        public static SitecoreMVCSection BeginSitecoreSection(this HtmlHelper htmlHelper, string sectionName)
        {
            return new SitecoreMVCSection(htmlHelper.ViewContext, sectionName.ToLower());
        }

        public static IHtmlString SitecoreSectionPlaceholder(this HtmlHelper htmlHelper, string sectionName)
        {
            return new HtmlString(string.Format("<!--MVCSection-{0}-->", sectionName.ToLower()));
        }
    }
}
