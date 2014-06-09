namespace SitecoreMVCSections
{
    using System.Collections.Generic;

    using Sitecore.Data;
    using Sitecore.Pipelines.HttpRequest;

    public class SitecoreMVCSectionPipeline : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            if (Sitecore.Context.Site != null && Sitecore.Context.Item != null)
            {
                if (Sitecore.Context.Item.Paths.IsContentItem)
                {
                    args.Context.Response.Filter = new SitecoreMVCSectionFilter(args.Context.Response.Filter);
                }
            }
        }
    }
}

