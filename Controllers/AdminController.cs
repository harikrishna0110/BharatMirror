using BharatMirror.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace BharatMirror.Controllers
{
    public class AdminController : Controller
    {
        const string SessionId = "_UserId";
        private readonly IAdvertisementRepository _advertisementRepository;

        public AdminController(IAdvertisementRepository advertisementRepo)
        {
            _advertisementRepository = advertisementRepo;
        }
        public IActionResult Index()
        {
            int id = HttpContext.Session.GetInt32(SessionId) ?? 0;
            if(id == 0)
            {
                return RedirectToAction("Index", "User");
            }
            if (id != -1)
            {
                return new NotFoundResult();
            }
            var Model = _advertisementRepository.GetAdvertisements_admin("pending");
            return View(Model);
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Advertisement ad = _advertisementRepository.GetAdvertisement(id);
            ViewBag.accept = "Accepted";
            return View(ad);
        }

        [HttpPost]
        public IActionResult Edit(Advertisement advertisement)
        {

            Advertisement ad = _advertisementRepository.GetAdvertisement(advertisement.Id);
            ad.status = advertisement.status;
            ad.reason = advertisement.reason;
            
            _advertisementRepository.Update(ad);

            return RedirectToAction("index");
        }

        public ViewResult Reject(int id)
        {
            Advertisement ad = _advertisementRepository.GetAdvertisement(id);
            ViewBag.reject = "Rejected";
            return View(ad);
        }

        [HttpPost]
        public IActionResult Reject(Advertisement advertisement)
        {

            Advertisement ad = _advertisementRepository.GetAdvertisement(advertisement.Id);
            ad.status = advertisement.status;
            ad.reason = advertisement.reason;
            ad.payment = "Refund Completed";
            _advertisementRepository.Update(ad);

            return RedirectToAction("index");
        }
    }
}
