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
            //scrollbar, dropdown
            var accountDetails = _bank_DetailsService.getAccounts(userId);
            ViewBag.accountDetails = accountDetails;
            //chatarea
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

            var accountDetails = _bank_DetailsService.getCurrentBalance(userId, Convert.ToInt32(user_bankid));

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

                //row affected in report table
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
            bool isAccountNameUnique = _bank_DetailsService.isBankAccountUnique(Account_Name, userId);

            if (isAccountNameUnique)
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
            ViewBag.filterTitle = null;
            var userId = (int)HttpContext.Session.GetInt32("login_userId");
            //dropdown
            var accountDetails = _bank_DetailsService.getAccounts(userId);
            ViewBag.accountDetails = accountDetails;
            //complete table
            var displayReportsById = _reportServices.displayReportsById(userId);
            ViewBag.displayReports = displayReportsById;
            TempData["currentAccount"] = null;
            TempData.Keep();
            return View();

        }
        [HttpPost]
        public ActionResult Report(EventArgs e)
        {
            string dropdown_bankAccount = Request.Form["User_BankId"];          
            if(dropdown_bankAccount == "All Accounts")
            {
                TempData["currentAccount"] = null;
                ViewBag.filterTitle = null ;
            }
            else
            {
                TempData["currentAccount"] = dropdown_bankAccount;
                ViewBag.filterTitle = Convert.ToInt32(dropdown_bankAccount);
            }
            TempData.Keep();
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
                //pass both userId and the selected account Id
                var displayReports = _reportServices.displayReports(userId, Convert.ToInt32(dropdown_bankAccount));
                ViewBag.displayReports = displayReports;
            }       
            return View();
        }
        [HttpPost]
        public ActionResult reportSearch(string search)
        {
            var userId = (int)HttpContext.Session.GetInt32("login_userId");
            if (TempData["currentAccount"] != null)
            {
                var displaySearchByAccount = _reportServices.searchReportByAccount(userId, search, Convert.ToInt32(TempData["currentAccount"]));
                ViewBag.displayReports = displaySearchByAccount;
                ViewBag.filterTitle = Convert.ToInt32(TempData["currentAccount"]);
                
            }
            else
            {
                var displaySearchByAllAccounts = _reportServices.searchReportByAllAccounts(userId, search);
                ViewBag.displayReports = displaySearchByAllAccounts;
                ViewBag.filterTitle = null;
            }
            
            var accountDetails = _bank_DetailsService.getAccounts(userId);
            ViewBag.accountDetails = accountDetails;
            
            TempData.Keep();
            return View("Report");
        }
    }
}