using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ST_Invoicing.Models
{
    public class ST_InStockDAO
    {
        private STDATAEntities dao = new STDATAEntities();

        public ST_InStock Insert(ST_InStock data)
        {
            dao.ST_InStock.Add(data);

            dao.SaveChanges();

            return data;
        }

        public ST_InStock FetchBySerno(int? serno)
        {
            return dao.ST_InStock.Find(serno);
        }
        
        public ST_InStock FetchByDate(DateTime rec_date)
        {
            List<ST_InStock> rslt = dao.ST_InStock.Where(currStock => currStock.del_yn == 0).Where(currStock => currStock.rec_date == rec_date).ToList();

            if(rslt.Count == 1)
            {
                return rslt[0];
            }else if (rslt.Count > 1)
            {
                throw new Exception("2筆以上的相同日期庫存!!!!");
            }

            return null;
        }

        public ST_InStock FetchRecentData(DateTime rec_date)
        {         
            return dao.ST_InStock.Where(currStock => currStock.del_yn == 0).Where(currStock => currStock.rec_date < rec_date).OrderByDescending(currStock => currStock.rec_date).FirstOrDefault();        
        }

        public ST_InStock FetchLastDateData()
        {
            return dao.ST_InStock.Where(currStock => currStock.del_yn == 0).OrderByDescending(currStock => currStock.rec_date).FirstOrDefault();        
        }

        public List<ST_InStock> GetDataList_NotDel()
        {
            List<ST_InStock> rslt = new List<ST_InStock>();

            rslt = dao.ST_InStock.Where(currInStock => currInStock.del_yn == 0).ToList();

            return rslt;
        }

        public ST_InStock Update(ST_InStock data)
        {
            dao.Entry(data).State = EntityState.Modified;

            dao.SaveChanges();

            return data;
        }

     
       
        public void Dispose()
        {
            dao.Dispose();
        }
    }
}