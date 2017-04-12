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
    public class ST_Surplus_MonthController : Controller
    {
        private ST_SurplusMonthDAO mST_SurplusMonthDAO = new ST_SurplusMonthDAO();
        private ST_SurplusDayDAO mST_SurplusDayDAO = new ST_SurplusDayDAO();
        private ST_PurcahseDAO mST_PurcahseDAO = new ST_PurcahseDAO();
        private ST_MaterialDAO mST_MaterialDAO = new ST_MaterialDAO();

        // GET: ST_Surplus_Month
        public ActionResult Index()
        {
            #region 檢查是否有未製作的月報表
            List<ST_SurplusDay> dataList = mST_SurplusDayDAO.GetDataLessThanThisMonth(true);

            List<DateTime> month_List = GetMonthList(dataList);

            foreach (DateTime month in month_List)
            {
                if (mST_SurplusMonthDAO.FetchByMonth(month) == null)
                {
                    ST_Surplus_Month data = CreateNewMonthData(month);

                    data.guid = Guid.NewGuid();

                    mST_SurplusMonthDAO.Insert(data);
                }
            }
            #endregion
            return View();
        }

        // GET: ST_Surplus_Month/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ST_Surplus_Month data = mST_SurplusMonthDAO.FetchBySerno(id);
            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }

        private List<DateTime> GetMonthList(List<ST_SurplusDay> dataList)
        {
            List<DateTime> dates = new List<DateTime>();

            foreach (ST_SurplusDay data in dataList)
            {

                DateTime recMonth = new DateTime(data.rec_date.Year, data.rec_date.Month, 1);

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

        public ST_Surplus_Month CreateNewMonthData(DateTime rec_Month)
        {
            DateTime firstDay = new DateTime(rec_Month.Year, rec_Month.Month, 1);
            DateTime lastDay = new DateTime(rec_Month.AddMonths(1).Year, rec_Month.AddMonths(1).Month, 1).AddDays(-1);

            List<ST_SurplusDay> dataList = mST_SurplusDayDAO.GetDataByDateRange(firstDay, lastDay);

            #region 將資料寫入月報表
            ST_Surplus_Month newData = new ST_Surplus_Month();

            newData.rec_month = rec_Month;

            newData.work_days = dataList.Count;

            /*計算月收入累計&月支出累計前要先依照日期排序*/
            dataList = dataList.OrderBy(o => o.rec_date).ToList();

            /*計算累計支出*/
            newData.expenditure_month = GetExpenditure_Month(dataList);

            /*計算累計收入*/
            newData.turnover_month = GetTurnover_Month(dataList);

            /*計算累計盈餘*/
            newData.surplus_month = newData.turnover_month - newData.expenditure_month;

            /*計算飯桶數*/
            newData.rice_month = GetRice_month(dataList);

            /*計算羹桶數*/
            newData.soup_month = GetSoup_month(dataList);

            /*計算魚數量*/
            newData.fish_month = GetFish_month(dataList);

            /*計算850cc數量*/
            newData.month_850 = GetMonth_850(dataList);

            /*計算700cc數量*/
            newData.month_700 = GetMonth_700(dataList);

            /*計算點心盒數量*/
            newData.month_meal = GetMonth_Meal(dataList);

            /*計算便當盒數量*/
            newData.month_box = GetMonth_Box(dataList);

            /*計算內用大大碗*/
            newData.month_inner_xl = GetInner_XL(dataList);

            /*計算內用大碗*/
            newData.month_inner_l = GetInner_L(dataList);

            /*計算內用小碗*/
            newData.month_inner_s = GetInner_S(dataList);

            /*計算支出食材費用*/
            newData.food_month = GetPurchase_Month(firstDay, lastDay, "食材");

            /*計算支出耗材費用*/
            newData.supplie_month = GetPurchase_Month(firstDay, lastDay, "耗材");

            /*計算支出其他費用*/
            newData.other_month = GetPurchase_Month(firstDay, lastDay, "其他");
            #endregion

            int month_other = newData.other_month;

            return newData;
        }

        public int GetExpenditure_Month(List<ST_SurplusDay> dataList)
        {           
            #region 計算每日支出
            foreach (ST_SurplusDay data in dataList)
            {
                List<ST_Purchase> purrchase_List = mST_PurcahseDAO.GetDataByDate(data.rec_date);

                data.expenditure = 0;

                foreach (ST_Purchase currPurchase in purrchase_List)
                {
                    data.expenditure += currPurchase.purchase_price;
                }
            }
            #endregion

            #region  加總月份總支出
            #endregion 
            int total_Expenditure = 0;

            for (int i = 0; i < dataList.Count; i++)
            {  
                total_Expenditure += dataList[i].expenditure;
            }

            return total_Expenditure;
        }

        public int GetTurnover_Month(List<ST_SurplusDay> dataList)
        {
            int total_Turnover = 0;

            for (int i = 0; i < dataList.Count; i++)
            {
                total_Turnover += dataList[i].turnover;
            }

            return total_Turnover;
        }

        public double GetRice_month(List<ST_SurplusDay> dataList)
        {
            double tota_Count = 0;

            for (int i = 0; i < dataList.Count; i++)
            {
                tota_Count += dataList[i].count_rice;
            }

            return tota_Count;
        }

        public double GetSoup_month(List<ST_SurplusDay> dataList)
        {
            double tota_Count = 0;

            for (int i = 0; i < dataList.Count; i++)
            {
                tota_Count += dataList[i].count_soup;
            }

            return tota_Count;
        }

        public double GetFish_month(List<ST_SurplusDay> dataList)
        {
            double tota_Count = 0;

            for (int i = 0; i < dataList.Count; i++)
            {
                tota_Count += dataList[i].count_fish;
            }

            return tota_Count;
        }

        public int GetMonth_850(List<ST_SurplusDay> dataList)
        {
            int tota_Count = 0;

            for (int i = 0; i < dataList.Count; i++)
            {
                tota_Count += dataList[i].count_850_use_am;
                tota_Count += dataList[i].count_850_use_pm;
            }

            return tota_Count;
        }

        public int GetMonth_700(List<ST_SurplusDay> dataList)
        {
            int tota_Count = 0;

            for (int i = 0; i < dataList.Count; i++)
            {
                tota_Count += dataList[i].count_700_use_am;
                tota_Count += dataList[i].count_700_use_pm;
            }

            return tota_Count;
        }

        public int GetMonth_Meal(List<ST_SurplusDay> dataList)
        {
            int tota_Count = 0;

            for (int i = 0; i < dataList.Count; i++)
            {
                tota_Count += dataList[i].count_meal_use_pm;
                tota_Count += dataList[i].count_meal_use_am;
            }

            return tota_Count;
        }

        public int GetMonth_Box(List<ST_SurplusDay> dataList)
        {
            int tota_Count = 0;

            for (int i = 0; i < dataList.Count; i++)
            {
                tota_Count += dataList[i].count_box_use_pm;
                tota_Count += dataList[i].count_box_use_am;
            }

            return tota_Count;
        }

        public int GetInner_XL(List<ST_SurplusDay> dataList)
        {
            int tota_Count = 0;

            for (int i = 0; i < dataList.Count; i++)
            {
                tota_Count += dataList[i].inner_xl_am;
                tota_Count += dataList[i].inner_xl_pm;
            }

            return tota_Count;
        }

        public int GetInner_L(List<ST_SurplusDay> dataList)
        {
            int tota_Count = 0;

            for (int i = 0; i < dataList.Count; i++)
            {
                tota_Count += dataList[i].inner_l_am;
                tota_Count += dataList[i].inner_l_pm;
            }

            return tota_Count;
        }

        public int GetInner_S(List<ST_SurplusDay> dataList)
        {
            int tota_Count = 0;

            for (int i = 0; i < dataList.Count; i++)
            {
                tota_Count += dataList[i].inner_s_am;
                tota_Count += dataList[i].inner_s_pm;
            }

            return tota_Count;
        }

        public int GetPurchase_Month(DateTime firstDay, DateTime lastDay, string itemType)
        {

            List<ST_Purchase> purchaseList = mST_PurcahseDAO.GetDataByDateRange(firstDay, lastDay);

            #region 移除非指定類別的資料
            for (int i = 0; i < purchaseList.Count; i++)
            {
                ST_Material currMaterial = mST_MaterialDAO.FetchByGuid(purchaseList[i].material_guid);

                if (!currMaterial.item_species.Trim().Equals(itemType))
                {
                    purchaseList.RemoveAt(i);
                    i--;
                }
            }
            #endregion

            #region 計算加總值
            int total_Fee = 0;

            foreach (ST_Purchase data in purchaseList)
            {
                total_Fee += data.purchase_price;
            }
            #endregion

            return total_Fee;

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mST_SurplusMonthDAO.Dispose();
                mST_SurplusDayDAO.Dispose();
                mST_PurcahseDAO.Dispose();
                mST_MaterialDAO.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
