using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SEV.Library;
using SharpCifs.Smb;
using SharpCifs.Util;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Upload.Controllers
{
    public class UploadResponse
    {
        public string Path { get; set; }
        public string URL { get; set; }
    }

    public class UploadRequest
    {
        public IFormFile file { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        public string Server { get; private set; }
        public string Shared { get; private set; }
        public string Domain { get; private set; }
        public string Usuario { get; private set; }
        public string Clave { get; private set; }
        public string Recurso { get; private set; }
        private NtlmPasswordAuthentication AuthUser { get; set; }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public UploadResponse UploadFileAsync([FromForm] UploadRequest request)
        {
            UploadResponse Result = new UploadResponse();

            Server = SEVConfigAssistant.Configuration["SambaResource:UDG:Server"];
            Shared = SEVConfigAssistant.Configuration["SambaResource:UDG:Shared"];
            Domain = SEVConfigAssistant.Configuration["SambaResource:UDG:Domain"];
            Usuario = SEVConfigAssistant.Configuration["SambaResource:UDG:User"];
            Clave = SEVConfigAssistant.Configuration["SambaResource:UDG:Password"];
            Recurso = SEVConfigAssistant.Configuration["Resources:ImageURL"];

            var extension = ObtenerExtension(request.file).ToLower();

            if (!extension.Equals(".ppt") && !extension.Equals(".pptx"))
            {
                Result.Path = "Solo se permiten archivos .ppt y .pptx";
                Result.URL = Result.Path;
                return Result;
            }

            if (!CheckLengthFile(request.file))
            {
                Result.Path = "El archivo excede el tamaño máximo permitido de 5MB";
                Result.URL = Result.Path;
                return Result;
            }

            CrearArchivoAsync(request.file.FileName, request.file).Wait();

            Result.Path = request.file.FileName;
            Result.URL = Recurso + request.file.FileName;

            return Result;
        }

        public async Task CrearArchivoAsync(string FileName, IFormFile file)
        {
            AuthUser = new NtlmPasswordAuthentication(Domain, Usuario, Clave);


            //SMBResource = "smb://fs36.in.sev.gob.mx/SisFs/UDG/";
            string remotePath = $"smb://{Server}/{Shared}/{FileName}";
            SmbFile RemoteFile = new SmbFile(remotePath, AuthUser);

            if (!RemoteFile.Exists())
            {
                RemoteFile.CreateNewFile();
            }

            byte[] FileContent;
            using (var memoryStream = new MemoryStream())
            {
                await file.OpenReadStream().CopyToAsync(memoryStream);
                FileContent = memoryStream.ToArray();
            }

            using (var outputStream = RemoteFile.GetOutputStream())
            {
                outputStream.Write(FileContent);
                outputStream.Dispose();
                outputStream.Close();
            }
        }

        private bool CheckLengthFile(IFormFile file)
        {
            return file.Length <= (5 * 1024 * 1024); // 5MB
        }

        private string ObtenerExtension(IFormFile file)
        {
            return Path.GetExtension(file.FileName);
        }
    }
}