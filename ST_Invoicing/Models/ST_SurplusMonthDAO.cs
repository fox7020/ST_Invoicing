using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ST_Invoicing.Models
{
    public class ST_SurplusMonthDAO
    {
        private STDATAEntities dao = new STDATAEntities();

        public ST_Surplus_Month Insert(ST_Surplus_Month data)
        {
            dao.ST_Surplus_Month.Add(data);

            dao.SaveChanges();

            return data;
        }

        public ST_Surplus_Month FetchBySerno(int? serno)
        {
            return dao.ST_Surplus_Month.Find(serno);
        }

        public ST_Surplus_Month FetchByMonth(DateTime rec_Month)
        {
            List<ST_Surplus_Month> rslt = new List<ST_Surplus_Month>();

            rslt = dao.ST_Surplus_Month.Where(currSurplusMonth => currSurplusMonth.del_yn == 0).Where(currSurplusMonth => currSurplusMonth.rec_month.Equals(rec_Month)).ToList();

            if (rslt.Count == 1)
            {
                return rslt[0];
            }
            else if (rslt.Count > 1)
            {
                throw new Exception("兩筆以上的月報表!!!");
            }

            return null;
        }

        public void Dispose()
        {
            dao.Dispose();
        }
    }
}