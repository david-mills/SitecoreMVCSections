namespace SitecoreMVCSections
{
    using System;
    using System.Text;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;

    public class SitecoreMVCSectionFilter : Stream
    {

        private static Regex _entryRegex = new Regex(@"<!--MVCSectionBegin-(?<Name>[^\s]+)(?<Content>[\s\S]+?)MVCSectionEnd-->",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace |
            RegexOptions.Multiline);

        private static Regex _endOfFile = new Regex("</html>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly Stream responseStream;

        private long _position;

        private readonly StringBuilder responseHtml;

        public SitecoreMVCSectionFilter(Stream inputStream)
        {
            this.responseStream = inputStream;
            this.responseHtml = new StringBuilder();
        }

        #region Filter overrides

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override void Close()
        {
            this.responseStream.Close();
            base.Close();
        }

        public override void Flush()
        {
            this.responseStream.Flush();
        }

        public override long Length
        {
            get
            {
                return 0;
            }
        }

        public override long Position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.responseStream.Seek(offset, origin);
        }

        public override void SetLength(long length)
        {
            this.responseStream.SetLength(length);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.responseStream.Read(buffer, offset, count);
        }

        #endregion

        private string _last = string.Empty;

        public override void Write(byte[] buffer, int offset, int count)
        {
            string strBuffer = Encoding.UTF8.GetString(buffer, offset, count);

            this.responseHtml.Append(strBuffer);
            if (_endOfFile.IsMatch(string.Format("{0}{1}", _last, strBuffer)))
            {
                var finalHtml = this.responseHtml.ToString();

                var matches = _entryRegex.Matches(finalHtml);

                foreach (Match match in matches)
                {
                    var name = match.Groups["Name"].Value;
                    var groupContent = match.Groups["Content"].Value;
                    var index = finalHtml.IndexOf(string.Format("<!--MVCSection-{0}-->", name.ToLower()), comparisonType: StringComparison.InvariantCultureIgnoreCase);

                    if (index > 0)
                    {
                        index = index + 18 + name.Length;
                        finalHtml = finalHtml.Insert(index, groupContent);
                    }
                }

                finalHtml = _entryRegex.Replace(finalHtml, string.Empty);

                var data = Encoding.UTF8.GetBytes(finalHtml);
                this.responseStream.Write(data, 0, data.Length);
            }

            this._last = strBuffer;
        }
    }
}
