using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ST_Invoicing.Models;

namespace ST_Invoicing.Controllers
{
    [Authorize]
    public class ST_PurchaseController : Controller
    {
        private ST_MaterialDAO mST_MaterialDAO = new ST_MaterialDAO();

        private ST_VendorDAO mST_VendorDAO = new ST_VendorDAO();

        private ST_PurcahseDAO mST_PurcahseDAO = new ST_PurcahseDAO();

        private ST_EmpDAO mST_EmpDAO = new ST_EmpDAO();

        // GET: ST_Purchase
        public ActionResult Index()
        {
            List<ST_Purchase> purchase_List = new List<ST_Purchase>();

            purchase_List = mST_PurcahseDAO.GetThisMonthData(DateTime.Today);

            SetOtherProperty(ref purchase_List);

            purchase_List = purchase_List.OrderBy(o => o.purchase_date).ToList();

            ViewData["user"] = Session["user"];

            return View(purchase_List);
        }

        [HttpPost]
        public ActionResult Index(string query)
        {

            List<ST_Purchase> rslt = mST_PurcahseDAO.GetThisMonthData(DateTime.Today);

            SetOtherProperty(ref rslt);

            if (query != null)
            {
                QueryByKeyWord(ref rslt, query);
            }

            List<ST_Purchase> SortedList = rslt.OrderBy(o => o.purchase_date).ToList();

            ViewData["user"] = Session["user"];

            return View(SortedList);
        }

        private void QueryByKeyWord(ref List<ST_Purchase> datalist, string query)
        {
            for (int i = 0; i < datalist.Count; i++)
            {
                if (datalist[i].item_name != null)
                {
                    if (!datalist[i].item_name.Contains(query))
                    {
                        datalist.RemoveAt(i);
                        i--;
                    }
                }
                else if (datalist[i].item_name == null)
                {
                    datalist.RemoveAt(i);
                    i--;
                }

            }
        }

        // GET: ST_Purchase/Create
        public ActionResult Create()
        {

            ViewData["food_Items"] = GetMaterialItems("食材");

            ViewData["supplies_Items"] = GetMaterialItems("耗材");

            ViewData["other_Items"] = GetMaterialItems("其他");

            ViewData["user"] = Session["user"];

            return View();
        }

        // POST: ST_Purchase/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Purchase_DayAll data)
        {
            if (ModelState.IsValid && data.purchase_List != null)
            {
                List<ST_Purchase> prucahse_List = data.purchase_List.Cast<ST_Purchase>().ToList();

                foreach (ST_Purchase currPurchase in prucahse_List)
                {
                    currPurchase.guid = Guid.NewGuid();

                    currPurchase.purchase_date = data.rec_date;

                    ST_Material currMaterial = mST_MaterialDAO.FetchByItemName(currPurchase.item_name);

                    if (currMaterial != null)
                    {
                        currPurchase.material_guid = currMaterial.guid;
                    }
                    else
                    {   /*Material表非固定品項的Guid*/
                        currPurchase.material_guid = Guid.Parse("DF7E2E4F-F2F0-4232-8E10-665FA99048B1");

                        currPurchase.special_item = currPurchase.item_name;
                    }

                    currPurchase.emp_guid = mST_EmpDAO.FetchByAccount(User.Identity.Name).guid;

                    mST_PurcahseDAO.Insert(currPurchase);
                }

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "至少新增一筆採購項目");

            ViewData["food_Items"] = GetMaterialItems("食材");

            ViewData["supplies_Items"] = GetMaterialItems("耗材");

            ViewData["other_Items"] = GetMaterialItems("其他");

            ViewData["user"] = Session["user"];

            return View(data);
        }

        // GET: ST_Purchase/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ST_Purchase data = mST_PurcahseDAO.FetchBySerno(id);

            if (data == null)
            {
                return HttpNotFound();
            }

            data.special_mark = data.special_mark.Trim();

            /*非固定品項*/
            if (data.material_guid.Equals(Guid.Parse("DF7E2E4F-F2F0-4232-8E10-665FA99048B1")))
            {
                data.item_name = data.special_item;
            }
            else
            {
                data.item_name = mST_MaterialDAO.FetchByGuid(data.material_guid).item_name;
            }

            ViewData["user"] = Session["user"];

            return View(data);
        }

        // POST: ST_Purchase/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ST_Purchase data)
        {
            if (ModelState.IsValid)
            {

                mST_PurcahseDAO.Update(data);

                return RedirectToAction("Index");
            }

            ViewData["user"] = Session["user"];

            return View(data);
        }

        // GET: ST_Purchase/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ST_Purchase data = mST_PurcahseDAO.FetchBySerno(id);

            if (data == null)
            {
                return HttpNotFound();
            }

            data.special_mark = data.special_mark.Trim();

            data.item_name = mST_MaterialDAO.FetchByGuid(data.material_guid).item_name;

            data.emp_name = mST_EmpDAO.FetchByGuid(data.emp_guid).emp_name;

            ViewData["user"] = Session["user"];

            return View(data);
        }

        // POST: ST_Purchase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ST_Purchase data = mST_PurcahseDAO.FetchBySerno(id);

            if (data != null)
            {
                mST_PurcahseDAO.Delete(data);
            }


            return RedirectToAction("Index");
        }

        public ActionResult GetFoodView()
        {
            ViewData["food_Items"] = GetMaterialItems("食材");

            return PartialView("FoodView");
        }

        public ActionResult GetItemView()
        {

            ViewData["supplies_Items"] = GetMaterialItems("耗材");

            return PartialView("ItemView");
        }

        public ActionResult GetOtherView()
        {
            ViewData["other_Items"] = GetMaterialItems("其他");

            return PartialView("OtherView");
        }

        public ActionResult GetNonFixView()
        {
            return PartialView("NonFixedView");
        }

        [HttpGet]
        public ActionResult HistroryIndex()
        {
            #region 取得本月之前的舊資料
            List<ST_Purchase> purchase_List = mST_PurcahseDAO.GetDataLessThanThisMonth(true);

            SetOtherProperty(ref purchase_List);

            purchase_List = purchase_List.OrderBy(o => o.purchase_date).ToList();
            #endregion

            #region DropDown List資料
            List<DateTime> month_List = GetMonthList(purchase_List);

            List<string> queryItems = new List<string>();

            foreach (DateTime month in month_List)
            {
                string strMonth = month.ToString("yyyy-MM");

                queryItems.Add(strMonth);
            }

            ViewData["query_Item"] = queryItems;
            #endregion

            return View(purchase_List);
        }

        [HttpPost]
        public ActionResult HistroryIndex(string selMonth)
        {

            #region 取得本月之前的舊資料
            List<ST_Purchase> purchase_List = mST_PurcahseDAO.GetDataLessThanThisMonth(true);          
            #endregion

            #region 設定要查詢的月份資料
            if (selMonth != "")
            {
                string strMonth = selMonth + "-1";

                DateTime Month = DateTime.Parse(strMonth);

                GetSelMonthData(ref purchase_List, Month);
            }
            #endregion


            #region DropDown List資料
            List<DateTime> month_List = GetMonthList(purchase_List);

            List<string> queryItems = new List<string>();

            foreach (DateTime month in month_List)
            {
                string strMonth = month.ToString("yyyy-MM");

                queryItems.Add(strMonth);
            }

            ViewData["query_Item"] = queryItems;
            #endregion

            SetOtherProperty(ref purchase_List);

            purchase_List = purchase_List.OrderBy(o => o.purchase_date).ToList();

            return View(purchase_List);
        }

        [HttpGet]
        public ActionResult Unsettlement()
        {
            #region 取得本月之前的舊資料
            List<ST_Purchase> rslt = mST_PurcahseDAO.GetDataLessThanThisMonth(false);
            #endregion

            //#region 將取得的資料依照月份分配
            //List<DateTime> month_list = new List<DateTime>();
            //month_list = GetMonthList(rslt);
            //#endregion

            //#region 取得上個月的資料
            //GetSelMonthData(ref rslt, month_list[0]);
            //#endregion

            SetOtherProperty(ref rslt);

            rslt = rslt.OrderBy(o => o.purchase_date).ToList();

            ViewData["user"] = Session["user"];

            return View(rslt);
        }

        [HttpPost, ActionName("Unsettlement")]
        public ActionResult Settlement()
        {
            #region 取得本月之前的舊資料
            List<ST_Purchase> rslt = mST_PurcahseDAO.GetDataLessThanThisMonth(false);
            #endregion

            SetOtherProperty(ref rslt);

            foreach (ST_Purchase data in rslt)
            {
                data.finish_yn = 1;

                mST_PurcahseDAO.Update(data);
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult HistoryEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ST_Purchase data = mST_PurcahseDAO.FetchBySerno(id);

            if (data == null)
            {
                return HttpNotFound();
            }

            data.special_mark = data.special_mark.Trim();

            /*非固定品項*/
            if (data.material_guid.Equals(Guid.Parse("DF7E2E4F-F2F0-4232-8E10-665FA99048B1")))
            {
                data.item_name = data.special_item;
            }
            else
            {
                data.item_name = mST_MaterialDAO.FetchByGuid(data.material_guid).item_name;
            }

            ViewData["user"] = Session["user"];

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HistoryEdit(ST_Purchase data)
        {
            if (ModelState.IsValid)
            {

                mST_PurcahseDAO.Update(data);

                return RedirectToAction("Unsettlement");
            }

            ViewData["user"] = Session["user"];

            return View(data);
        }



        public ActionResult HistoryDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ST_Purchase data = mST_PurcahseDAO.FetchBySerno(id);

            if (data == null)
            {
                return HttpNotFound();
            }

            data.special_mark = data.special_mark.Trim();

            data.item_name = mST_MaterialDAO.FetchByGuid(data.material_guid).item_name;

            data.emp_name = mST_EmpDAO.FetchByGuid(data.emp_guid).emp_name;

            ViewData["user"] = Session["user"];

            return View(data);
        }

        // POST: ST_Purchase/Delete/5
        [HttpPost, ActionName("HistoryDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult HistoryDeleteConfirmed(int id)
        {
            ST_Purchase data = mST_PurcahseDAO.FetchBySerno(id);

            if (data != null)
            {
                mST_PurcahseDAO.Delete(data);
            }


            return RedirectToAction("Unsettlement");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mST_MaterialDAO.Dispose();
                mST_VendorDAO.Dispose();
                mST_PurcahseDAO.Dispose();
                mST_EmpDAO.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<string> GetMaterialItems(string species)
        {
            List<ST_Material> materials = mST_MaterialDAO.GetDataListBySpecies(species);

            List<string> material_Items = new List<string>();

            foreach (ST_Material material in materials)
            {
                material_Items.Add(material.item_name);
            }

            return material_Items;
        }

        private void SetOtherProperty(ref List<ST_Purchase> dataList)
        {
            foreach (ST_Purchase purchase in dataList)
            {
                if (purchase.special_item != null)
                {
                    purchase.item_name = purchase.special_item;
                }
                else
                {
                    purchase.item_name = mST_MaterialDAO.FetchByGuid(purchase.material_guid).item_name;
                }

                purchase.emp_name = mST_EmpDAO.FetchByGuid(purchase.emp_guid).emp_name;


                if (purchase.special_mark.Trim() == "是")
                {
                    purchase.font_Color = "red";
                }
                else
                {
                    purchase.font_Color = "black";
                }


            }
        }

        private List<DateTime> GetMonthList(List<ST_Purchase> dataList)
        {
            List<DateTime> dates = new List<DateTime>();

            foreach (ST_Purchase data in dataList)
            {

                DateTime recMonth = new DateTime(data.purchase_date.Year, data.purchase_date.Month, 1);

                if (dates.Count == 0)
                {
                    dates.Add(recMonth);
                }

                foreach (DateTime date in dates)
                {
                    if (date != recMonth)
                    {
                        dates.Add(recMonth);
                    }
                }
            }

            return dates;
        }

        private void GetSelMonthData(ref List<ST_Purchase> rslt, DateTime selMonth)
        {
            DateTime firstDay = new DateTime(selMonth.Year, selMonth.Month, 1);
            DateTime lastDay = new DateTime(selMonth.AddMonths(1).Year, selMonth.AddMonths(1).Month, 1).AddDays(-1);

            for (int i = 0; i < rslt.Count; i++)
            {
                if (rslt[i].purchase_date < firstDay || rslt[i].purchase_date > lastDay)
                {
                    rslt.RemoveAt(i);
                    i--;
                }
            }

        }

    }
}
