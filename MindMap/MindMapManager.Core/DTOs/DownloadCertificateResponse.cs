using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.DTOs
{
    public class DownloadCertificateResponse
    {
        public string physicalPath {  get; set; }
        public string contentType { get; set; }
        public string fileName { get; set; }
    }
}
