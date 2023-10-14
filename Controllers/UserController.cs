using BharatMirror.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Reflection.Metadata.Ecma335;
using MailKit;

namespace BharatMirror.Controllers
{
    public class UserController : Controller
    {
        const string SessionId = "_UserId";
        private readonly IUserRepository _userRepo;
        private readonly Models.IMailService _mailService;
        private readonly IAdvertisementRepository _advertisementRepository;
       public UserController(IUserRepository userRepo, IAdvertisementRepository advertisementRepo, Models.IMailService mailService)
        {
            _userRepo = userRepo;
            _advertisementRepository = advertisementRepo;
            _mailService = mailService;
        }
   

        public IActionResult Index()
        {
            int id = HttpContext.Session.GetInt32(SessionId) ?? 0;
            if (id == -1)
                return new NotFoundResult();
            if (id == 0)
            {
                return RedirectToAction("Login");
            }

            Users user = _userRepo.GetUsers(id);
            ViewBag.user = user;
            var Model = _advertisementRepository.GetAdvertisements_user(id);
            return View(Model);


            
        }

        [HttpGet]
        public IActionResult Create()
        {


            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email , string password)
        {
            Users user = _userRepo.GetUserByEmail(email);
            if (user != null && user.Password == password)
            {
               
                if (user.Role == "Admin")
                {
                    HttpContext.Session.SetInt32(SessionId, -1);
                    return RedirectToAction("Index", "Admin");
                 }
                HttpContext.Session.SetInt32(SessionId, user.Id);
                MailRequest mailrq = new MailRequest()
                {
                    ToEmail = "bhadiyadraharikrishna@gmail.com",
                    Subject = "Login Attemp From New Device",
                    Body = "User made new Login  attemp, please verify.",
                    UserName = "Hari",
                    Attachments = null
                };

                await _mailService.SendEmailAsync(mailrq);
                return RedirectToAction("index");
            }
            //ViewBag.error = "Enter a Valid Credential !";
            return View();


        }

        [HttpPost]

        public IActionResult Create(Users user)
        {
            if (ModelState.IsValid)
            {
                user.Role = "User";
                Users newStudent = _userRepo.Add(user);
                MailRequest mailrq = new MailRequest()
                {
                    ToEmail = "bhadiyadraharikrishna@gmail.com",
                    Subject = "Login Attemp From New Device",
                    UserName = user.Name,
                    Body = "User made new Login  attemp, please verify.",
                    Attachments = null
                };

                _mailService.SendWelcomeEmailAsync(mailrq);
                return RedirectToAction("index" , new {id = newStudent.Id});

            }
            return View();
        }


        [HttpGet]
        public IActionResult AddAdvertisement()
        {

            if (HttpContext.Session.GetInt32(SessionId) == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddAdvertisement(Advertisement advertisement)
        {
            int id = HttpContext.Session.GetInt32(SessionId) ?? 0;
            if (id == 0)
            {
                return RedirectToAction("Login");
            }
            advertisement.user_id = id;
            advertisement.payment = "False";
            advertisement.reason = "";
            advertisement.status = "pending";
            if (advertisement.start_time.Date != advertisement.end_time.Date)
            {
                ViewBag.error = "Time Period must be within same day.";
                return View();
            }
            _advertisementRepository.Add(advertisement);
            //String url = "payment/" + id.ToString();
            return RedirectToAction("payment", new { Id = advertisement.Id });
        }

        [HttpGet]
        public ViewResult EditAdvertisement(int id)
        {
            Advertisement ad = _advertisementRepository.GetAdvertisement(id);
            return View(ad);
        }

        [HttpPost]
        public IActionResult EditAdvertisement(Advertisement advertisement)
        {

            Advertisement ad = _advertisementRepository.GetAdvertisement(advertisement.Id);
            ad.image_url = advertisement.image_url;
            ad.business_url = advertisement.business_url;
            ad.category = advertisement.category;
            ad.start_time = advertisement.start_time;
            ad.end_time = advertisement.end_time;
            ad.title = advertisement.title;
            ad.description = advertisement.description;
            _advertisementRepository.Update(ad);

            return RedirectToAction("index");
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            if (HttpContext.Session.GetInt32(SessionId) == null)
            {
                return RedirectToAction("Login");
            }
            var Model = _advertisementRepository.GetAdvertisement(Id);
            if (Model == null)
            {
                return RedirectToAction("index");
            }
            return View(Model);
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            if (HttpContext.Session.GetInt32(SessionId) == null)
            {
                return RedirectToAction("Login");
            }
            var Model = _advertisementRepository.GetAdvertisement(Id);
            if (Model == null)
            {
                return RedirectToAction("index");
            }
            return View(Model);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirm(int Id)
        {
            if (HttpContext.Session.GetInt32(SessionId) == null)
            {
                return RedirectToAction("Login");
            }
            _advertisementRepository.Delete(Id);
            return RedirectToAction("index");
        }
        [HttpGet]
        public IActionResult payment(int Id)
        {

            Advertisement ad = _advertisementRepository.GetAdvertisement(Id);
            if (ad.payment == "Done")
            {
                return RedirectToAction("index");
            }
            ViewBag.Id = Id;

            return View();
        }

        [HttpPost, ActionName("payment")]
        public IActionResult paymentConfirm(int Id)
        {
            Advertisement ad = _advertisementRepository.GetAdvertisement(Id);
            ad.payment = "Done";
            _advertisementRepository.Update(ad);

            return RedirectToAction("index");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }



    }
}
