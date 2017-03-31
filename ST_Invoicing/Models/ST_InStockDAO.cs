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

        public ST_InStock FetchByMaterialGuid(Guid material_guid)
        {
            List<ST_InStock> rslt = new List<ST_InStock>();

            rslt = dao.ST_InStock.Where(currInStock => currInStock.del_yn == 0).Where(currInStock => currInStock.material_guid == material_guid).ToList();

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

        public double AddStockCount(Guid material_guid, double count)
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

        public double DecreseStockCount(Guid material_guid, double count)
        {
            List<ST_InStock> rslt = new List<ST_InStock>();

            rslt = dao.ST_InStock.Where(currInStock => currInStock.del_yn == 0).Where(currInStock => currInStock.material_guid.Equals(material_guid)).ToList();

            if (rslt.Count == 1)
            {
                rslt[0].count -= count;

                Update(rslt[0]);

                return rslt[0].count;
            }
            else if (rslt.Count > 1)
            {
                throw new Exception("2筆以上相同原物料的庫存!!!");
            }

            return 0;
        }

        public void InsertBasicInStock()
        {
            /*Insert 700、850、點心盒*/
            ST_InStock bowl_700 = new ST_InStock(Guid.Parse("E5564142-5B2C-472F-8EDB-3E957AA1BBE3"));
            ST_InStock bowl_850 = new ST_InStock(Guid.Parse("5981CDD4-C4A7-45A0-9832-E16574D1689C"));
            ST_InStock meat = new ST_InStock(Guid.Parse("258921A7-9EB2-4978-9689-6284B906230D"));

            if (FetchByMaterialGuid(bowl_700.material_guid) == null)
            {
                Insert(bowl_700);
            }

            if (FetchByMaterialGuid(bowl_850.material_guid) == null)
            {
                Insert(bowl_850);
            }

            if (FetchByMaterialGuid(meat.material_guid) == null)
            {
                Insert(meat);
            }

        }
        public void Dispose()
        {
            dao.Dispose();
        }
    }
}