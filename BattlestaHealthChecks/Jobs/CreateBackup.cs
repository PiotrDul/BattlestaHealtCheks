using BattlestaHealthChecks.Context;
using BattlestaHealthChecks.Interfeces.Jobs;
using BattlestaHealthChecks.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace BattlestaHealthChecks.Jobs
{
    public class CreateBackup : ICreateBackup
    {
        private readonly IServiceScopeFactory _scopeFactory;
        static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "BattlestaHealthChecks";


        public CreateBackup(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task Create()
        {
            string startPath = @".\";
            string zipPath = @"..\" + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".zip";
            ZipFile.CreateFromDirectory(startPath, zipPath);

            UserCredential credential;

            credential = await GetCredentials();
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            UploadFile(zipPath, service);

            var settings = await GetSettings();
            if (settings == null)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                    await dbContext.BackupSettings.AddAsync(new BackupSettings { HourInterval = 24, LastDate = DateTime.Now });
                    await dbContext.SaveChangesAsync();
                }
            }
            else
            {
                settings.LastDate = DateTime.Now;
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                    dbContext.BackupSettings.Update(settings);
                    await dbContext.SaveChangesAsync();
                }
            }


        }

        private async Task<BackupSettings> GetSettings()
        {
            BackupSettings settings;
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                settings = await dbContext.BackupSettings.AsNoTracking().FirstOrDefaultAsync();
            }

            return settings;
        }

        private async Task<UserCredential> GetCredentials()
        {
            UserCredential credential;
            //var crePath = @".\bin\Debug\netcoreapp2.2\credentials.json";
            var crePath = $@"{await GetCredentialPath()}";
            using (var stream = new FileStream(crePath, FileMode.Open, FileAccess.Read))
            {
                var credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

                credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            return credential;
        }

        private static string GetMimeType(string fileName)
        {
            var mimeType = "application/unknown";
            var ext = Path.GetExtension(fileName).ToLower();
            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        private static void UploadFile(string path, DriveService service)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File();
            fileMetadata.Name = Path.GetFileName(path);
            fileMetadata.MimeType = GetMimeType(path);
            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
            {
                request = service.Files.Create(fileMetadata, stream, GetMimeType(path));
                request.Fields = "id";
                request.Upload();
            }

            var file = request.ResponseBody;
            Console.WriteLine("File ID: " + file.Id);

            File.Delete(path);
        }


        private async Task<string> GetCredentialPath()
        {
            var settings = await GetSettings();
            return settings.CredentialPath;
        }

    }
}
