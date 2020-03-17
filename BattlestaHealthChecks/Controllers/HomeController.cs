using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BattlestaHealthChecks.Models;
using BattlestaHealthChecks.Interfeces.Services;
using BattlestaHealthChecks.Interfeces.Jobs;

namespace BattlestaHealthChecks.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISampleSettingsService _settingsService;
        private readonly ISampleService _sampleService;
        private readonly ICreateBackup _createBackup;
        public HomeController(ISampleSettingsService settingsService, ISampleService sampleService, ICreateBackup createBackup)
        {
            _settingsService = settingsService;
            _sampleService = sampleService;
            _createBackup = createBackup;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            var raports = await _sampleService.GetSamples(page);
            return View(raports);
        }

        public async Task<IActionResult> ShowDetails(int sampleId)
        {
            if (sampleId == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            var sample = await _sampleService.GetSample(sampleId);

            return View(sample);
        }

        public async Task<IActionResult> Backup()
        {
            var model = await _settingsService.GetBackupSettings();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BackupSave(BackupSettings settings)
        {

            if (ModelState.IsValid)
            {
                await _settingsService.UpdateBackupSettings(settings);
            }
            return RedirectToAction("Backup", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> BackupNow(string backup)
        {
            if (backup == "backup_now")
            {
                await _createBackup.Create();
            }
            return RedirectToAction("Backup", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> ShowSettings(SampleSettings settings) {
            var model = settings;
            if (model.TimeInterval == 0)
            {
                model = await _settingsService.GetSampleSettings();
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveSettings(SampleSettings settings)
        { 
            var newSettings = new SampleSettings();
            if(ModelState.IsValid){
                newSettings = await _settingsService.UpdateSampleSettings(settings);
                ViewBag.UpdateSucceeded = "Zaktualizowano ustawienia";
            }
            return RedirectToAction("ShowSettings", "Home", newSettings);
        }
    }
}
