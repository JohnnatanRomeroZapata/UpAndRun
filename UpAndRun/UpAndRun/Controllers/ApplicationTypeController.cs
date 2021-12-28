using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UpAndRun.Data;
using UpAndRun.Models;

namespace UpAndRun.Controllers
{
    public class ApplicationTypeController : Controller
    {
        private readonly UpAndRunDbContext _upAndRunDbContext;

        public ApplicationTypeController(UpAndRunDbContext upAndRunDbContext)
        {
            _upAndRunDbContext = upAndRunDbContext;
        }

        [HttpGet]
        public IActionResult IndexApplicationType()
        {
            IEnumerable<ApplicationType> applicationTypeList = _upAndRunDbContext.ApplicationTypes;

            return View(applicationTypeList);
        }

        [HttpGet]
        public IActionResult CreateApplicationType()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateApplicationType(ApplicationType applicationType)
        {
            if (ModelState.IsValid)
            {
                _upAndRunDbContext.ApplicationTypes.Add(applicationType);
                _upAndRunDbContext.SaveChanges();
                return RedirectToAction("IndexApplicationType");
            }

            return View(applicationType);

        }

        [HttpGet]
        public IActionResult UpdateApplicationType(int? Id)
        {
            if (Id is null || Id == 0)
            {
                return NotFound();
            }

            ApplicationType applicationType = _upAndRunDbContext.ApplicationTypes.Find(Id);

            if (applicationType is null)
            {
                return NotFound();
            }

            return View(applicationType);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult UpdateApplicationType(ApplicationType applicationType)
        {
            if (ModelState.IsValid)
            {
                _upAndRunDbContext.ApplicationTypes.Update(applicationType);
                _upAndRunDbContext.SaveChanges();
                return RedirectToAction("IndexApplicationType");
            }

            return View(applicationType);
        }

        [HttpGet]
        public IActionResult DeleteApplicationType(int? Id)
        {
            if (Id is null || Id == 0)
            {
                return NotFound();
            }

            ApplicationType applicationType = _upAndRunDbContext.ApplicationTypes.Find(Id);

            if (applicationType is null)
            {
                return NotFound();
            }

            return View(applicationType);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult DeleteApplicationTypePost(int? Id)
        {
            if (Id is null || Id == 0)
            {
                return NotFound();
            }

            ApplicationType applicationTypeToDelete = _upAndRunDbContext.ApplicationTypes.Find(Id);

            if (applicationTypeToDelete is null)
            {
                return NotFound();
            }
            _upAndRunDbContext.ApplicationTypes.Remove(applicationTypeToDelete);
            _upAndRunDbContext.SaveChanges();
            return RedirectToAction("IndexApplicationType");
        }
    }
}
