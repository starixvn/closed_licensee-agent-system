namespace LicenseeAgent.Tests.Fakes
{
    using System.IO;
    using System.Web;

    /// <summary>
    /// FakeHttpPostedFile class.
    /// </summary>
    public class FakeHttpPostedFile : HttpPostedFileBase
    {
        private readonly int _contentLength;
        private readonly string _contentType;
        private readonly string _fileName;
        private readonly Stream _inputStream;

        public FakeHttpPostedFile(int contentLength, string contentType, string fileName, Stream inputStream)
        {
            _contentLength = contentLength;
            _contentType = contentType;
            _fileName = fileName;
            _inputStream = inputStream;
        }

        public override int ContentLength
        {
            get
            {
                return _contentLength;
            }
        }

        public override string ContentType
        {
            get
            {
                return _contentType;
            }
        }

        public override string FileName
        {
            get
            {
                return _fileName;
            }
        }

        public override Stream InputStream
        {
            get
            {
                return _inputStream;
            }
        }
    }
}