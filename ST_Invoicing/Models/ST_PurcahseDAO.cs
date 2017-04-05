using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ST_Invoicing.Models
{
    public class ST_PurcahseDAO
    {
        private STDATAEntities dao = new STDATAEntities();

        public ST_Purchase Insert(ST_Purchase data)
        {
            dao.ST_Purchase.Add(data);

            dao.SaveChanges();

            return data;
        }

        public ST_Purchase FetchBySerno(int? serno)
        {
            return dao.ST_Purchase.Find(serno);
        }


        public List<ST_Purchase>GetDataByDate(DateTime rec_date)
        {
            List<ST_Purchase> rslt = new List<ST_Purchase>();

            rslt = dao.ST_Purchase.Where(currPurcahse => currPurcahse.del_yn == 0).Where(currPurcahse => currPurcahse.purchase_date.Equals(rec_date)).ToList();

            return rslt;
        }

        public List<ST_Purchase>GetDataByDateRange(DateTime start_Date, DateTime end_Date)
        {
            List<ST_Purchase> rslt = new List<ST_Purchase>();

            rslt = dao.ST_Purchase.Where(currPurcahse => currPurcahse.del_yn == 0).Where(currPurcahse => currPurcahse.purchase_date >= start_Date).Where(currPurcahse => currPurcahse.purchase_date <= end_Date).ToList();

            return rslt;
        }
        public ST_Purchase Update(ST_Purchase data)
        {
            dao.Entry(data).State = EntityState.Modified;

            dao.SaveChanges();

            return data;
        }

        public void Delete(ST_Purchase data)
        {
            dao.ST_Purchase.Remove(data);

            dao.SaveChanges();
        }

        public void Dispose()
        {
            dao.Dispose();
        }
    }
}