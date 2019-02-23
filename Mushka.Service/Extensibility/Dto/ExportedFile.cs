using System;
using System.IO;

namespace Mushka.Service.Extensibility.Dto
{
    public class ExportedFile : IDisposable
    {
        public string Name { get; }

        public string ContentType { get; }

        public Stream FileContent { get; }

        public ExportedFile(string name, string contentType, Stream fileContent)
        {
            Name = name;
            ContentType = contentType;
            FileContent = fileContent;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                FileContent.Dispose();
            }
        }
    }
}