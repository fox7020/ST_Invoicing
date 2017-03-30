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

        public ST_InStock FetchByMaterialGuid(string material_guid)
        {
            List<ST_InStock> rslt = new List<ST_InStock>();

            rslt = dao.ST_InStock.Where(currInStock => currInStock.del_yn == 0).Where(currInStock => currInStock.material_guid.Equals(material_guid)).ToList();

            if (rslt.Count == 1)
            {
                return rslt[0];
            }
            else if (rslt.Count > 1)
            {
                throw new Exception("2筆以上相同原物料的庫存!!!");
            }

            return null;
        }

        public ST_InStock Update(ST_InStock data)
        {
            dao.Entry(data).State = EntityState.Modified;

            dao.SaveChanges();

            return data;
        }

        public double AddStockCount(string material_guid, double count)
        {

            List<ST_InStock> rslt = new List<ST_InStock>();

            rslt = dao.ST_InStock.Where(currInStock => currInStock.del_yn == 0).Where(currInStock => currInStock.material_guid.Equals(material_guid)).ToList();

            if (rslt.Count == 1)
            {
                rslt[0].count += count;

                Update(rslt[0]);

                return rslt[0].count;
            }
            else if (rslt.Count > 1)
            {
                throw new Exception("2筆以上相同原物料的庫存!!!");
            }

            return 0;
        }

        public void Dispose()
        {
            dao.Dispose();
        }
    }
}