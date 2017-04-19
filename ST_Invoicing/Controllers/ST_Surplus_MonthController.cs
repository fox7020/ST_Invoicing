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
            List<ST_SurplusDay> dataList_day = mST_SurplusDayDAO.GetDataLessThanThisMonth(true);

            List<DateTime> month_List_day = GetMonthList(dataList_day);

            foreach (DateTime month in month_List_day)
            {
                if (mST_SurplusMonthDAO.FetchByMonth(month) == null)
                {
                    ST_Surplus_Month data = CreateNewMonthData(month);

                    data.guid = Guid.NewGuid();

                    mST_SurplusMonthDAO.Insert(data);
                }
            }
            #endregion

            #region
            List<ST_Surplus_Month> dataList_Month = mST_SurplusMonthDAO.GetDataListNotDel();

            /*依照月份排序*/
            dataList_Month.OrderBy(o => o.rec_month).ToList();
            #endregion

            #region DropDown List資料

            List<string> queryItems = new List<string>();

            foreach (ST_Surplus_Month data in dataList_Month)
            {
                string strMonth = data.rec_month.ToString("yyyy-MM");

                queryItems.Add(strMonth);
            }

            ViewData["query_Item"] = queryItems;
            #endregion

            List<string> select_Draw = new List<string>();

            select_Draw.Add("支出比重");
            select_Draw.Add("日收入與支出");


            ViewData["draw_item"] = select_Draw;

            ViewData["user"] = Session["user"];

            return View(dataList_Month[0]);
        }

        [HttpPost]
        public ActionResult Index(string selMonth)
        {
            ViewData["user"] = Session["user"];

            #region
            List<ST_Surplus_Month> dataList_Month = mST_SurplusMonthDAO.GetDataListNotDel();

            /*依照月份排序*/
            dataList_Month.OrderBy(o => o.rec_month).ToList();
            #endregion

            #region DropDown List資料

            List<string> queryItems = new List<string>();

            foreach (ST_Surplus_Month data in dataList_Month)
            {
                string strMonth = data.rec_month.ToString("yyyy-MM");

                queryItems.Add(strMonth);
            }

            ViewData["query_Item"] = queryItems;
            #endregion

            List<string> select_Draw = new List<string>();

            select_Draw.Add("支出比重");
            select_Draw.Add("日收入與支出");


            ViewData["draw_item"] = select_Draw;

            if (selMonth != "")
            {
                string year = selMonth.Substring(0, 4);
                string month = selMonth.Substring(5, 2);

                string rec_Month = year + "-" + month + "-01";

                DateTime recMonth = DateTime.Parse(rec_Month);

                ST_Surplus_Month data = mST_SurplusMonthDAO.FetchByMonth(recMonth);
                      
                return View(data);
            }



            return View(dataList_Month[0]);
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

        public JsonResult Draw(string selMonth, string method)
        {
            /*取得查看的年月份&要畫的圖表 =>使用引數*/
            //string selMonth

            List<int> rslt = new List<int>();

            if (selMonth != "---查看月份---")
            {
                string year = selMonth.Substring(0, 4);
                string month = selMonth.Substring(5, 2);

                string rec_Month = year + "-" + month + "-01";

                DateTime recMonth = DateTime.Parse(rec_Month);

                ST_Surplus_Month currData = mST_SurplusMonthDAO.FetchByMonth(recMonth);

                switch (method)
                {
                    case "支出比重":
                        List<int> expenditure_List = new List<int>();

                        /*食材*/
                        rslt.Add(currData.food_month);

                        /*耗材*/
                        rslt.Add(currData.supplie_month);

                        /*其他*/
                        rslt.Add(currData.other_month);
                        break;
                    case "日收入與支出":
                        #region 取得整月每天的資料
                        DateTime firstDay = new DateTime(recMonth.Year, recMonth.Month, 1);
                        DateTime lastDay = new DateTime(recMonth.AddMonths(1).Year, recMonth.AddMonths(1).Month, 1).AddDays(-1);

                        List<ST_SurplusDay> day_List = mST_SurplusDayDAO.GetDataByDateRange(firstDay, lastDay);

                        #region 計算每日支出
                        foreach (ST_SurplusDay data in day_List)
                        {
                            List<ST_Purchase> purrchase_List = mST_PurcahseDAO.GetDataByDate(data.rec_date);

                            data.expenditure = 0;

                            foreach (ST_Purchase currPurchase in purrchase_List)
                            {
                                data.expenditure += currPurchase.purchase_price;
                            }
                        }
                        #endregion
                        #endregion
                        return Json(day_List, JsonRequestBehavior.AllowGet);
                }
            }


            return Json(rslt, JsonRequestBehavior.AllowGet);
        }

        private List<DateTime> GetMonthList(List<ST_SurplusDay> dataList)
        {
            List<DateTime> dateList = new List<DateTime>();

            #region 將所有日報表資料日期先轉成當月的1號
            foreach (ST_SurplusDay SurplusDay in dataList)
            {
                dateList.Add(new DateTime(SurplusDay.rec_date.Year, SurplusDay.rec_date.Month, 1));
            }
            #endregion

            #region 取得總共幾個月份
            Dictionary<DateTime, int> uniqueStore = new Dictionary<DateTime, int>();
            List<DateTime> finalList = new List<DateTime>();

            foreach (DateTime currValue in dateList)
            {
                if (!uniqueStore.ContainsKey(currValue))
                {
                    uniqueStore.Add(currValue, 0);
                    finalList.Add(currValue);
                }
            }
            #endregion

            return finalList;
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

            /*計算早班850cc數量*/
            newData.month_850_am = GetMonth_850(dataList, "am");

            /*計算晚班850cc數量*/
            newData.month_850_pm = GetMonth_850(dataList, "pm");

            /*計算早班700cc數量*/
            newData.month_700_am = GetMonth_700(dataList, "am");

            /*計算晚班700cc數量*/
            newData.month_700_pm = GetMonth_700(dataList, "pm");

            /*計算早班點心盒數量*/
            newData.month_meal_am = GetMonth_Meal(dataList, "am");

            /*計算晚班點心盒數量*/
            newData.month_meal_pm = GetMonth_Meal(dataList, "pm");

            /*計算早班便當盒數量*/
            newData.month_box_am = GetMonth_Box(dataList, "am");

            /*計算晚班便當盒數量*/
            newData.month_box_pm = GetMonth_Box(dataList, "pm");

            /*計算早班內用大大碗*/
            newData.month_inner_xl_am = GetInner_XL(dataList, "am");

            /*計算晚班內用大大碗*/
            newData.month_inner_xl_pm = GetInner_XL(dataList, "pm");

            /*計算早班內用大碗*/
            newData.month_inner_l_am = GetInner_L(dataList, "am");

            /*計算晚班內用大碗*/
            newData.month_inner_l_pm = GetInner_L(dataList, "pm");

            /*計算早班內用小碗*/
            newData.month_inner_s_am = GetInner_S(dataList, "am");

            /*計算晚班內用小碗*/
            newData.month_inner_s_pm = GetInner_S(dataList, "pm");

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

        public int GetMonth_850(List<ST_SurplusDay> dataList, string timeSection)
        {
            int tota_Count = 0;

            if (timeSection == "am")
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    tota_Count += dataList[i].count_850_use_am;
                }
            }
            else if (timeSection == "pm")
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    tota_Count += dataList[i].count_850_use_pm;
                }
            }


            return tota_Count;
        }

        public int GetMonth_700(List<ST_SurplusDay> dataList, string timeSection)
        {
            int tota_Count = 0;

            if (timeSection == "am")
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    tota_Count += dataList[i].count_700_use_am;
                }
            }
            else if (timeSection == "pm")
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    tota_Count += dataList[i].count_700_use_pm;
                }
            }


            return tota_Count;
        }

        public int GetMonth_Meal(List<ST_SurplusDay> dataList, string timeSection)
        {
            int tota_Count = 0;

            if (timeSection == "am")
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    tota_Count += dataList[i].count_meal_use_am;
                }
            }
            else if (timeSection == "pm")
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    tota_Count += dataList[i].count_meal_use_pm;
                }
            }


            return tota_Count;
        }

        public int GetMonth_Box(List<ST_SurplusDay> dataList, string timeSection)
        {
            int tota_Count = 0;

            if (timeSection == "am")
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    tota_Count += dataList[i].count_box_use_am;
                }
            }
            else if (timeSection == "pm")
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    tota_Count += dataList[i].count_box_use_pm;
                }
            }


            return tota_Count;
        }

        public int GetInner_XL(List<ST_SurplusDay> dataList, string timeSection)
        {
            int tota_Count = 0;

            if (timeSection == "am")
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    tota_Count += dataList[i].inner_xl_am;
                }

            }
            else if (timeSection == "pm")
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    tota_Count += dataList[i].inner_xl_pm;
                }
            }


            return tota_Count;
        }

        public int GetInner_L(List<ST_SurplusDay> dataList, string timeSection)
        {
            int tota_Count = 0;

            if (timeSection == "am")
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    tota_Count += dataList[i].inner_l_am;
                }
            }
            else if (timeSection == "pm")
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    tota_Count += dataList[i].inner_l_pm;
                }
            }


            return tota_Count;
        }

        public int GetInner_S(List<ST_SurplusDay> dataList, string timeSection)
        {
            int tota_Count = 0;

            if (timeSection == "am")
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    tota_Count += dataList[i].inner_s_am;
                }
            }
            else if (timeSection == "pm")
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    tota_Count += dataList[i].inner_s_pm;
                }
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
