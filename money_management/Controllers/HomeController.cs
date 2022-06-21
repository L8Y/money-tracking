
using Microsoft.AspNetCore.Mvc;
using money_management.Interface;
using money_tracker_lib;


namespace money_management.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRegisterService _registerService;
        private readonly IBank_detailsService _bank_DetailsService;
        private readonly IReportServices _reportServices;
        public HomeController(IRegisterService registerService, IBank_detailsService bank_DetailsService, IReportServices reportServices)
        {
              _registerService = registerService;
             _bank_DetailsService = bank_DetailsService;
            _reportServices = reportServices;
        }
        [HttpGet]
        public ActionResult home()
        {
            
            ViewBag.Users = HttpContext.Session.GetInt32("login_userId");
            ViewBag.name = HttpContext.Session.GetString("login_email");
            var userId = (int)HttpContext.Session.GetInt32("login_userId");
            var accountDetails = _bank_DetailsService.getAccounts(userId);
            ViewBag.accountDetails = accountDetails;
            
            return View();
        }
        [HttpPost]
        public ActionResult home(string Reason, int Amount, EventArgs e)
        {
            int balance = 0;
            string transaction_type = Request.Form["Transaction_type"];
            string user_bankid = Request.Form["User_BankId"];
            DateTime currentDate = DateTime.UtcNow.Date;
            var userId = (int)HttpContext.Session.GetInt32("login_userId");
            var accountDetails = _bank_DetailsService.getAccounts(userId);

            
            ViewBag.accountDetails = accountDetails;

            if(Convert.ToInt32(transaction_type) == 0)
            {
                foreach (var item in accountDetails)
                {
                    balance = Convert.ToInt32(item.Initial_Balance) + Amount;
                }
            }
            else
            {
                foreach (var item in accountDetails)
                {
                    balance = Convert.ToInt32(item.Initial_Balance) - Amount;
                }

            }

            

            var isCredited = _reportServices.AddTransaction(Reason, Amount, balance, Convert.ToByte(transaction_type), currentDate, userId, Convert.ToInt32(user_bankid));
            
            if(isCredited > 0)
            {
                var isBalanceUpdated = _bank_DetailsService.updateInitialBalance(userId, Convert.ToInt32(user_bankid), balance);
                ViewBag.Users = "Added";
            }
            else
            {
                ViewBag.Users = "Not successfull";

            }


            return View();
        }
        [HttpGet]
        public ActionResult register()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult register(string Name, string EmailId, string Password)
        {
            int isUserCreated = _registerService.createUser(Name, EmailId, Password);
            

            if(isUserCreated > 0)
            {
                @ViewData["Message"] = "Added successfully";
            }
            else
            {
                @ViewData["Message"] = "Not successfull";
            }
            return View();
        }

        [HttpGet]
        public ActionResult login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult login(string email, string password)
        {
            var isValidUser = _registerService.isValidUser(email, password);

      
              foreach(var item in isValidUser)
              {
                if(item.count > 0)  
                {

                    HttpContext.Session.SetString("login_email", email);

                    var currentUserId = _registerService.GetUserId(HttpContext.Session.GetString("login_email"));
                    

                    foreach (var users in currentUserId)
                    {
                        HttpContext.Session.SetInt32("login_userId", users.UserId);

                    }
                    return RedirectToAction("home");

                }
                else
                {
                    @ViewData["Message"] = "Invalid user";
                }
                   
              }
              
           
            return View();

        }

        [HttpGet]

        public ActionResult AddAccount()
        {
            if (HttpContext.Session.GetString("login_email") == null)
            {
                return RedirectToAction("login");
            }

           
            return View();
        }
        [HttpPost]

        public ActionResult AddAccount(string Account_Name, string Initial_Balance)
        {

            var userId = (int)HttpContext.Session.GetInt32("login_userId");



            int isAccountAdded = _bank_DetailsService.AddBankAccount(Account_Name, Initial_Balance, userId);

            if (isAccountAdded > 0)
            {
                @ViewData["Message"] = "Added successfully";
            }
            else
            {
                @ViewData["Message"] = "Not successfull";
            }
            return View();
           
        }


    }
}