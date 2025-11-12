using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ABC_Retail.Models;

namespace ABC_Retail.Services
{
    public class BlobStorageService
    {
        private readonly HttpClient _http;
        private readonly string _blobFunctionUrl;

        public BlobStorageService(StorageOptions opt)
        {
            _http = new HttpClient();
            _blobFunctionUrl = opt.BlobFunctionUrl; 
        }

        // Upload file via BlobFunction and return the response message
        public async Task<string> UploadAsync(IFormFile file)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.Position = 0;

            var content = new ByteArrayContent(ms.ToArray());
            content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Headers.Add("filename", file.FileName); // BlobFunction reads this header

            var response = await _http.PostAsync(_blobFunctionUrl, content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
