namespace SitecoreMVCSections
{
    using System.Web.Mvc;
    using System.IO;
    using System.Web;
    using System.Web.Routing;
    using Sitecore.Mvc.Helpers;
    using Sitecore.Mvc.Presentation;
    using System;


    public class SitecoreMVCSection : IDisposable
    {
        private readonly TextWriter _writer;
        private bool _disposed;

        public SitecoreMVCSection(ViewContext viewContext, string sectionName)
        {
            _writer = viewContext.Writer;
            this._writer.Write(string.Format("<!--MVCSectionBegin-{0}", sectionName.ToLower()));
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                this._disposed = true;
                this._writer.Write("MVCSectionEnd-->");
            }
        }
    }
}
