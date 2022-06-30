﻿
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
            if (HttpContext.Session.GetString("login_email") == null)
            {
                return RedirectToAction("login");
            }

            
            ViewBag.name = HttpContext.Session.GetString("login_email");

            var userId = (int)HttpContext.Session.GetInt32("login_userId");
            var accountDetails = _bank_DetailsService.getAccounts(userId);
            ViewBag.accountDetails = accountDetails;
            var displayReportsById = _reportServices.displayReportsById(userId);
            ViewBag.displayReports = displayReportsById;

            
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

            bool isAmountValid = false;
            if(Convert.ToInt32(transaction_type) == 0)
            {
                foreach (var item in accountDetails)
                {
                    isAmountValid = true;
                    balance = Convert.ToInt32(item.Initial_Balance) + Amount;
                }
                    
            }
            else
            {
                foreach (var item in accountDetails)
                {
                    if (Amount < Convert.ToInt32(item.Initial_Balance))
                    {
                        isAmountValid = true;
                        balance = Convert.ToInt32(item.Initial_Balance) - Amount;
                        
                    }
                    else
                    {
                        isAmountValid = false;
                        ViewBag.Error = "Your expense is greater than the amount";
                    }
                }

            }

            if (isAmountValid == true)
            {
                /*var isCredited = _reportServices.AddTransaction(Reason, Amount, balance, Convert.ToByte(transaction_type), currentDate, userId, Convert.ToInt32(user_bankid));*/
                var isCredited = _reportServices.AddTransaction(Reason, Amount, balance, Convert.ToByte(transaction_type), currentDate, userId, Convert.ToInt32(user_bankid));
                var isBalanceUpdated = _bank_DetailsService.updateInitialBalance(userId, Convert.ToInt32(user_bankid), balance);
                if (isCredited == 0)
                {
                    ViewBag.Users = "Not successfull";

                }

            }

            var displayReportsById = _reportServices.displayReportsById(userId);
            ViewBag.displayReports = displayReportsById;

            var viewAccounts = _bank_DetailsService.getAccounts(userId);
            ViewBag.accountDetails = viewAccounts;




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
            bool isEmailUnique = _registerService.isEmailUnique(EmailId);

            if (isEmailUnique)
            {
                int isUserCreated = _registerService.createUser(Name, EmailId, Password);




                if (isUserCreated > 0)
                {
                    ViewData["Message"] = "Added successfully, please login";
                }
                else
                {
                    ViewData["Message"] = "Not successfull";
                }
            }
            else
            {
                ViewData["Message"] = "EmailId already exists";
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

            bool isAccountNameQuery = _bank_DetailsService.isBankAccountUnique(Account_Name, userId);



            if (isAccountNameQuery)
            {
                int isAccountAdded = _bank_DetailsService.AddBankAccount(Account_Name, Initial_Balance, userId);

                if (isAccountAdded > 0)
                {
                    ViewData["Message"] = "Added successfully";
                }
                else
                {
                    ViewData["Message"] = "Not successfull";
                }
            }
            else
            {
                ViewData["Message"] = "Account Name already exists";
            }
            return View();
           
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("login_email");
            HttpContext.Session.Remove("login_userId");
            return RedirectToAction("login");

        }
        [HttpGet]
        public ActionResult Report()
        {
            if (HttpContext.Session.GetString("login_email") == null)
            {
                return RedirectToAction("login");
            }

            var userId = (int)HttpContext.Session.GetInt32("login_userId");
            var accountDetails = _bank_DetailsService.getAccounts(userId);
            ViewBag.accountDetails = accountDetails;
           
            var displayReportsById = _reportServices.displayReportsById(userId);
            ViewBag.displayReports = displayReportsById;
            return View();

        }
        [HttpPost]
        public ActionResult Report(EventArgs e)
        {
            string dropdown_bankAccount = Request.Form["User_BankId"];
            var userId = (int)HttpContext.Session.GetInt32("login_userId");
            var accountDetails = _bank_DetailsService.getAccounts(userId);
            ViewBag.accountDetails = accountDetails;

            if (dropdown_bankAccount == "All Accounts")
            {
                var displayReportsById = _reportServices.displayReportsById(userId);
                ViewBag.displayReports = displayReportsById;
            }
            else
            {
                var displayReports = _reportServices.displayReports(userId, Convert.ToInt32(dropdown_bankAccount));
                ViewBag.displayReports = displayReports;

            }
                    
            return View();
        }


    }
}