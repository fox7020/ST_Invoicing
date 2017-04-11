using ST_Invoicing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ST_Invoicing.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {

        private ST_EmpDAO mST_EmpDAO = new ST_EmpDAO();

        private ST_MaterialDAO mST_MaterialDAO = new ST_MaterialDAO();

        private ST_InStockDAO mST_InStockDAO = new ST_InStockDAO();

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // <summary>
        /// 呈現後台使用者登入頁
        /// </summary>
        /// <param name="ReturnUrl">使用者原本Request的Url</param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {
            //ReturnUrl字串是使用者在未登入情況下要求的的Url
            LoginVM vm = new LoginVM() { ReturnUrl = ReturnUrl };
            return View(vm);
        }
        /// <summary>
        /// 後台使用者進行登入
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="u">使用者原本Request的Url</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginVM vm)
        {
            //沒通過Model驗證(必填欄位沒填，DB無此帳密)
            if (!ModelState.IsValid)
            {
                return View(vm);
            }


            //都成功...
            //進行表單登入 ※之後User.Identity.Name的值就是vm.Account帳號的值
            //導向預設Url(Web.config裡的defaultUrl定義)或使用者原先Request的Url
            FormsAuthentication.RedirectFromLoginPage(vm.Account, false);
            //剛剛已導向，此行不會執行到
            return Redirect(FormsAuthentication.GetRedirectUrl(vm.Account, false));
        }
        /// <summary>
        /// 後台使用者登出動作
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Logout()
        {
            //清除Session中的資料
            Session.Abandon();
            //登出表單驗證
            FormsAuthentication.SignOut();
            //導至登入頁
            return RedirectToAction("Login", "Home");
        }
        /// <summary>
        /// 後台首頁 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //取得目前登入者的帳號
            //string Account = User.Identity.Name;
            //可以依帳號到DB抓登入者的資料...

            ST_Emp currUser = mST_EmpDAO.FetchByAccount(User.Identity.Name);

            /*檢查有無700、850、點心盒、便當盒*/
            mST_MaterialDAO.InsertBasicMaterial();

            Session["user"] = currUser.emp_name;

            Session["user_guid"] = currUser.guid.ToString();

            Session["id"] = currUser.serno;

            if (currUser != null)
            {
                ViewData["user"] = currUser.emp_name;
            }
            else
            {
                ViewData["user"] = "Unknow User";
            }

            return View();
        }

        public ActionResult Index2()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mST_EmpDAO.Dispose();
                mST_MaterialDAO.Dispose();
                mST_InStockDAO.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}