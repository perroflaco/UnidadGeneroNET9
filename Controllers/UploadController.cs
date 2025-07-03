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
        [HttpGet("download/{anio}/{trimestre}/{area}/{fileName}")]
        public async Task<IActionResult> DescargarArchivo(int anio, int trimestre, string area, string fileName)
        {
            Server = SEVConfigAssistant.Configuration["SambaResource:UDG:Server"];
            Shared = SEVConfigAssistant.Configuration["SambaResource:UDG:Shared"];
            Domain = SEVConfigAssistant.Configuration["SambaResource:UDG:Domain"];
            Usuario = SEVConfigAssistant.Configuration["SambaResource:UDG:User"];
            Clave = SEVConfigAssistant.Configuration["SambaResource:UDG:Password"];

            AuthUser = new NtlmPasswordAuthentication(Domain, Usuario, Clave);
            string trimestreFolder = $"Trimestre{Math.Clamp(trimestre, 1, 4)}";
            string remoteFilePath = $"smb://{Server}/{Shared}/{anio}/{trimestreFolder}/{area}/{fileName}";
            SmbFile remoteFile = new SmbFile(remoteFilePath, AuthUser);

            if (!remoteFile.Exists() || remoteFile.IsDirectory())
            {
            return NotFound("Archivo no encontrado.");
            }

            using (var inputStream = remoteFile.GetInputStream())
            using (var memoryStream = new MemoryStream())
            {
                byte[] buffer = new byte[81920];
                int bytesRead;
                while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    memoryStream.Write(buffer, 0, bytesRead);
                }
                var content = memoryStream.ToArray();
                var contentType = "application/octet-stream";
                return File(content, contentType, fileName);
            }
        }

        [HttpGet("{anio}/{trimestre}")]
        public async Task<IActionResult> GetArchivosPorTrimestre(int anio, int trimestre)
        {
            Server = SEVConfigAssistant.Configuration["SambaResource:UDG:Server"];
            Shared = SEVConfigAssistant.Configuration["SambaResource:UDG:Shared"];
            Domain = SEVConfigAssistant.Configuration["SambaResource:UDG:Domain"];
            Usuario = SEVConfigAssistant.Configuration["SambaResource:UDG:User"];
            Clave = SEVConfigAssistant.Configuration["SambaResource:UDG:Password"];
            Recurso = SEVConfigAssistant.Configuration["Resources:ImageURL"];

            AuthUser = new NtlmPasswordAuthentication(Domain, Usuario, Clave);
            string trimestreFolder = $"Trimestre{Math.Clamp(trimestre, 1, 4)}";
            string remotePath = $"smb://{Server}/{Shared}/{anio}/{trimestreFolder}/";
            SmbFile trimestreDir = new SmbFile(remotePath, AuthUser);

            var resultado = new System.Collections.Generic.List<object>();

            if (trimestreDir.Exists() && trimestreDir.IsDirectory())
            {
                foreach (var areaDir in trimestreDir.ListFiles())
                {
                    if (areaDir.IsDirectory())
                    {
                        var archivos = new System.Collections.Generic.List<object>();
                        foreach (var file in areaDir.ListFiles())
                        {
                            if (!file.IsDirectory())
                            {
                                archivos.Add(new
                                {
                                    Archivo = file.GetName(),
                                    Path = $"{anio}/{trimestreFolder}/{areaDir.GetName().TrimEnd('/')}/{file.GetName()}",
                                    Url = $"{Recurso}{anio}/{trimestreFolder}/{areaDir.GetName().TrimEnd('/')}/{file.GetName()}"
                                });
                            }
                        }
                        resultado.Add(new
                        {
                            Area = areaDir.GetName().TrimEnd('/'),
                            Archivos = archivos
                        });
                    }
                }
            }

            return Ok(new
            {
                Trimestre = trimestre,
                Areas = resultado
            });
        }

        [HttpGet("{anio}")]
        public async Task<IActionResult> GetArchivosPorAnio(int anio)
        {
            Server = SEVConfigAssistant.Configuration["SambaResource:UDG:Server"];
            Shared = SEVConfigAssistant.Configuration["SambaResource:UDG:Shared"];
            Domain = SEVConfigAssistant.Configuration["SambaResource:UDG:Domain"];
            Usuario = SEVConfigAssistant.Configuration["SambaResource:UDG:User"];
            Clave = SEVConfigAssistant.Configuration["SambaResource:UDG:Password"];
            Recurso = SEVConfigAssistant.Configuration["Resources:ImageURL"];

            AuthUser = new NtlmPasswordAuthentication(Domain, Usuario, Clave);
            string remotePath = $"smb://{Server}/{Shared}/{anio}/";
            SmbFile anioDir = new SmbFile(remotePath, AuthUser);

            var resultado = new System.Collections.Generic.List<object>();

            if (anioDir.Exists() && anioDir.IsDirectory())
            {
                foreach (var trimestreDir in anioDir.ListFiles())
                {
                    if (trimestreDir.IsDirectory())
                    {
                        string trimestre = trimestreDir.GetName().TrimEnd('/');
                        var areas = new System.Collections.Generic.List<object>();
                        foreach (var areaDir in trimestreDir.ListFiles())
                        {
                            if (areaDir.IsDirectory())
                            {
                                var archivos = new System.Collections.Generic.List<object>();
                                foreach (var file in areaDir.ListFiles())
                                {
                                    if (!file.IsDirectory())
                                    {
                                        archivos.Add(new
                                        {
                                            Archivo = file.GetName(),
                                            Path = $"{anio}/{trimestre}/{areaDir.GetName().TrimEnd('/')}/{file.GetName()}",
                                            Url = $"{Recurso}{anio}/{trimestre}/{areaDir.GetName().TrimEnd('/')}/{file.GetName()}"
                                        });
                                    }
                                }
                                areas.Add(new
                                {
                                    Area = areaDir.GetName().TrimEnd('/'),
                                    Archivos = archivos
                                });
                            }
                        }
                        resultado.Add(new
                        {
                            Trimestre = trimestre,
                            Areas = areas
                        });
                    }
                }
            }

            return Ok(new
            {
                Anio = anio,
                Trimestres = resultado
            });
        }

        [HttpGet("{anio}/area/{area}")]
        public async Task<IActionResult> GetArchivosPorAreaYAnio(int anio, string area)
        {
            Server = SEVConfigAssistant.Configuration["SambaResource:UDG:Server"];
            Shared = SEVConfigAssistant.Configuration["SambaResource:UDG:Shared"];
            Domain = SEVConfigAssistant.Configuration["SambaResource:UDG:Domain"];
            Usuario = SEVConfigAssistant.Configuration["SambaResource:UDG:User"];
            Clave = SEVConfigAssistant.Configuration["SambaResource:UDG:Password"];
            Recurso = SEVConfigAssistant.Configuration["Resources:ImageURL"];

            AuthUser = new NtlmPasswordAuthentication(Domain, Usuario, Clave);
            string remotePath = $"smb://{Server}/{Shared}/{anio}/";
            SmbFile anioDir = new SmbFile(remotePath, AuthUser);

            var resultado = new System.Collections.Generic.List<object>();

            if (anioDir.Exists() && anioDir.IsDirectory())
            {
                foreach (var trimestreDir in anioDir.ListFiles())
                {
                    if (trimestreDir.IsDirectory())
                    {
                        string trimestre = trimestreDir.GetName().TrimEnd('/');
                        string areaPath = $"{trimestreDir.GetPath()}{area}/";
                        SmbFile areaDir = new SmbFile(areaPath, AuthUser);

                        if (areaDir.Exists() && areaDir.IsDirectory())
                        {
                            var archivos = new System.Collections.Generic.List<object>();
                            foreach (var file in areaDir.ListFiles())
                            {
                                if (!file.IsDirectory())
                                {
                                    archivos.Add(new
                                    {
                                        Archivo = file.GetName(),
                                        Path = $"{anio}/{trimestre}/{area}/{file.GetName()}",
                                        Url = $"{Recurso}{anio}/{trimestre}/{area}/{file.GetName()}"
                                    });
                                }
                            }
                            resultado.Add(new
                            {
                                Trimestre = trimestre,
                                Archivos = archivos
                            });
                        }
                    }
                }
            }

            return Ok(new
            {
                Anio = anio,
                Area = area,
                Trimestres = resultado
            });
        }

        public string Server { get; private set; }
        public string Shared { get; private set; }
        public string Domain { get; private set; }
        public string Usuario { get; private set; }
        public string Clave { get; private set; }
        public string Recurso { get; private set; }
        private NtlmPasswordAuthentication AuthUser { get; set; }

        [HttpPost("{trimestre}/{area}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<UploadResponse> UploadFileAsync([FromForm] UploadRequest request, int trimestre, string area)
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

            var year = DateTime.Now.Year.ToString();
            var trimestreFolder = $"Trimestre{Math.Clamp(trimestre, 1, 4)}";
            var areaFolder = area?.Trim() ?? "General";

            var relativePath = $"{year}/{trimestreFolder}/{areaFolder}/";
            var fileName = request.file.FileName;

            await CrearArchivoAsync(relativePath, fileName, request.file);

            Result.Path = $"{relativePath}{fileName}";
            Result.URL = $"{Recurso}{relativePath}{fileName}";

            return Result;
        }

        public async Task CrearArchivoAsync(string relativePath, string fileName, IFormFile file)
        {
            AuthUser = new NtlmPasswordAuthentication(Domain, Usuario, Clave);

            string remoteDirPath = $"smb://{Server}/{Shared}/{relativePath}";
            string remoteFilePath = $"{remoteDirPath}{fileName}";

            SmbFile remoteDir = new SmbFile(remoteDirPath, AuthUser);
            if (!remoteDir.Exists())
            {
                remoteDir.Mkdirs();
            }

            SmbFile RemoteFile = new SmbFile(remoteFilePath, AuthUser);

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