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
    public class ST_SurplusDayController : Controller
    {

        private ST_SurplusDayDAO mST_SurplusDayDAO = new ST_SurplusDayDAO();

        private ST_EmpDAO mST_EmpDAO = new ST_EmpDAO();

        private ST_PurcahseDAO mST_PurcahseDAO = new ST_PurcahseDAO();

        private ST_InStockDAO mST_InStockDAO = new ST_InStockDAO();

        // GET: ST_SurplusDay
        public ActionResult Index()
        {
            List<ST_SurplusDay> dataList = mST_SurplusDayDAO.GetThisMonthData(DateTime.Today);

            SetExpenditure(ref dataList);

            SetSurplus(ref dataList);

            /*計算月收入累計&月支出累計前要先依照日期排序*/
            dataList = dataList.OrderBy(o => o.rec_date).ToList();

            /*計算累計支出*/
            SetExpenditure_Month(ref dataList);

            /*計算累計收入*/
            SetTurnover_Month(ref dataList);

            /*計算累計盈餘*/
            SetSurplus_Month(ref dataList);
          
            ViewData["user"] = Session["user"];

            return View(dataList);
        }

        [HttpPost]
        public ActionResult Index(string query)
        {

            List<ST_SurplusDay> dataList = mST_SurplusDayDAO.GetThisMonthData(DateTime.Today);

            if (query != "")
            {
                QueryByKeyWord(ref dataList, query);
            }
            
            SetExpenditure(ref dataList);

            SetSurplus(ref dataList);

            /*計算月收入累計&月支出累計前要先依照日期排序*/
            dataList = dataList.OrderBy(o => o.rec_date).ToList();

            /*計算累計支出*/
            SetExpenditure_Month(ref dataList);

            /*計算累計收入*/
            SetTurnover_Month(ref dataList);

            /*計算累計盈餘*/
            SetSurplus_Month(ref dataList);

            ViewData["user"] = Session["user"];

            return View(dataList);
        }


        // GET: ST_SurplusDay/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ST_SurplusDay data = mST_SurplusDayDAO.FetchBySerno(id);

            if (data == null)
            {
                return HttpNotFound();
            }

            #region 計算當日支出
            List<ST_Purchase> purrchase_List = mST_PurcahseDAO.GetDataByDate(data.rec_date);

            data.expenditure = 0;

            foreach (ST_Purchase currPurchase in purrchase_List)
            {
                data.expenditure += currPurchase.purchase_price;
            }
            #endregion

            #region 計算當日盈餘
            data.surplus = data.turnover - data.expenditure;
            #endregion
           
            ViewData["user"] = Session["user"];

            data.emp_name = mST_EmpDAO.FetchByGuid(data.emp_guid).emp_name;

            return View(data);
        }

        // GET: ST_SurplusDay/Create
        public ActionResult Create()
        {
            ViewData["user"] = Session["user"];

            return View();
        }

        // POST: ST_SurplusDay/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ST_SurplusDay data)
        {
            if (ModelState.IsValid)
            {
                data.guid = Guid.NewGuid();

                data.emp_guid = mST_EmpDAO.FetchByAccount(User.Identity.Name).guid;

                mST_SurplusDayDAO.Insert(data);

                SetStock(data);

                return RedirectToAction("Index");
            }

            return View(data);
        }

        // GET: ST_SurplusDay/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ST_SurplusDay data = mST_SurplusDayDAO.FetchBySerno(id);

            if (data == null)
            {
                return HttpNotFound();
            }

            ViewData["user"] = Session["user"];
           
            return View(data);
        }

        // POST: ST_SurplusDay/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ST_SurplusDay data)
        {
            if (ModelState.IsValid)
            {
                mST_SurplusDayDAO.Update(data);

                #region 修改庫存
                SetStock(data);
                #endregion
                #region 修改比data日期還大的庫存

                List<ST_SurplusDay> edit_List = mST_SurplusDayDAO.GetDataWhichDateGreateThanSpecifyDate(data.rec_date);

                foreach (ST_SurplusDay editData in edit_List)
                {
                    SetStock(editData);

                }
                #endregion

                return RedirectToAction("Index");
            }
      
            return View(data);
        }

        // GET: ST_SurplusDay/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ST_SurplusDay data = mST_SurplusDayDAO.FetchBySerno(id);

            if (data == null)
            {
                return HttpNotFound();
            }

            ViewData["user"] = Session["user"];

            return View(data);
        }

        // POST: ST_SurplusDay/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ST_SurplusDay data = mST_SurplusDayDAO.FetchBySerno(id);

            mST_SurplusDayDAO.SoftDelete(data);

            return RedirectToAction("Index");
        }

       
        public void SetExpenditure(ref List<ST_SurplusDay> dataList)
        {
            foreach (ST_SurplusDay currData in dataList)
            {
                List<ST_Purchase> purrchase_List = mST_PurcahseDAO.GetDataByDate(currData.rec_date);

                currData.expenditure = 0;

                foreach (ST_Purchase currPurchase in purrchase_List)
                {
                    currData.expenditure += currPurchase.purchase_price;
                }
            }
        }

        public void SetSurplus(ref List<ST_SurplusDay> dataList)
        {
            foreach (ST_SurplusDay currData in dataList)
            {
                currData.surplus = currData.turnover - currData.expenditure;
            }
        }

        public void SetSurplus_Month(ref List<ST_SurplusDay> dataList)
        {
            int total_Surplus = 0;

            for (int i = 0; i < dataList.Count; i++)
            {
                total_Surplus += dataList[i].surplus;

                dataList[i].surplus_month = total_Surplus;
            }
        }

        public void SetTurnover_Month(ref List<ST_SurplusDay> dataList)
        {
            int total_Turnover = 0;

            for (int i = 0; i < dataList.Count; i++)
            {
                total_Turnover += dataList[i].turnover;

                dataList[i].turnover_month = total_Turnover;
            }
        }

        public void SetExpenditure_Month(ref List<ST_SurplusDay> dataList)
        {
            int total_Expenditure = 0;

            for (int i = 0; i < dataList.Count; i++)
            {
                total_Expenditure += dataList[i].expenditure;

                dataList[i].expenditure_month = total_Expenditure;
            }
        }

        public void SetStock(ST_SurplusDay data)
        {
            ST_InStock currStock = mST_InStockDAO.FetchByDate(data.rec_date);

            ST_InStock recentStock = mST_InStockDAO.FetchRecentData(data.rec_date);

            if (currStock != null)
            {
                currStock.add_700 = data.count_700_add;
                currStock.add_850 = data.count_850_add;
                currStock.add_meal = data.count_meal_add;
                currStock.add_box = data.count_box_add;

                currStock.use_700 = data.count_700_use_am + data.count_700_use_pm;
                currStock.use_850 = data.count_850_use_am + data.count_850_use_pm;
                currStock.use_meal = data.count_meal_use_am + data.count_meal_use_pm;
                currStock.use_box = data.count_box_use_am + data.count_box_use_pm;
                
                if(recentStock != null)
                {
                    currStock.count_700 = recentStock.count_700 + currStock.add_700 - currStock.use_700;
                    currStock.count_850 = recentStock.count_850 + currStock.add_850 - currStock.use_850;
                    currStock.count_meal = recentStock.count_meal + currStock.add_meal - currStock.use_meal;
                    currStock.count_box = recentStock.count_box + currStock.add_box - currStock.use_box;
                }
                else
                {
                    currStock.count_700 = currStock.add_700 - currStock.use_700;
                    currStock.count_850 = currStock.add_850 - currStock.use_850;
                    currStock.count_meal = currStock.add_meal - currStock.use_meal;
                    currStock.count_box = currStock.add_box - currStock.use_box;
                }
               

                mST_InStockDAO.Update(currStock);
            }
            else if (currStock == null)
            {
                currStock = new ST_InStock();

                currStock.rec_date = data.rec_date;

                currStock.guid = Guid.NewGuid();

                currStock.add_700 = data.count_700_add;
                currStock.add_850 = data.count_850_add;
                currStock.add_meal = data.count_meal_add;
                currStock.add_box = data.count_box_add;

                currStock.use_700 = data.count_700_use_am + data.count_700_use_pm;
                currStock.use_850 = data.count_850_use_am + data.count_850_use_pm;
                currStock.use_meal = data.count_meal_use_am + data.count_meal_use_pm;
                currStock.use_box = data.count_box_use_am + data.count_box_use_pm;
          
                if (recentStock != null)
                {
                    currStock.count_700 = recentStock.count_700 + currStock.add_700 - currStock.use_700;
                    currStock.count_850 = recentStock.count_850 + currStock.add_850 - currStock.use_850;
                    currStock.count_meal = recentStock.count_meal + currStock.add_meal - currStock.use_meal;
                    currStock.count_box = recentStock.count_box + currStock.add_box - currStock.use_box;
                }
                else
                {
                    currStock.count_700 = currStock.add_700 - currStock.use_700;
                    currStock.count_850 = currStock.add_850 - currStock.use_850;
                    currStock.count_meal = currStock.add_meal - currStock.use_meal;
                    currStock.count_box = currStock.add_box - currStock.use_box;
                }

                mST_InStockDAO.Insert(currStock);           
            }          
        }

        private void QueryByKeyWord(ref List<ST_SurplusDay>datalist, string query)
        {
            for (int i = 0; i < datalist.Count; i++)
            {
                if (datalist[i].remark != null)
                {
                    if (!datalist[i].remark.Contains(query))
                    {
                        datalist.RemoveAt(i);
                        i--;
                    }
                }
                else if (datalist[i].remark == null)
                {
                      datalist.RemoveAt(i);
                      i--;
                }
               
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mST_SurplusDayDAO.Dispose();
                mST_EmpDAO.Dispose();
                mST_PurcahseDAO.Dispose();
                mST_InStockDAO.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
