using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ST_Invoicing.Models
{
    public class ST_VendorDAO
    {
        private STDATAEntities dao = new STDATAEntities();

        public ST_Vendor Insert(ST_Vendor data)
        {
            dao.ST_Vendor.Add(data);

            dao.SaveChanges();

            return data;
        }

        public ST_Vendor FetchBySerno(int? serno)
        {
            return dao.ST_Vendor.Find(serno);
        }

        public ST_Vendor FetchByGuid(string guid)
        {
            List<ST_Vendor> rslt = new List<ST_Vendor>();

            rslt = dao.ST_Vendor.Where(currVendor => currVendor.guid == Guid.Parse(guid)).ToList();

            if (rslt.Count == 1)
            {

                return rslt[0];
            }
            else if (rslt.Count > 1)
            {

                throw new Exception("兩筆以上的ST_Material in Fetch");
            }

            return null;
        }

        public List<ST_Vendor> GetDataList_NotDel()
        {
            List<ST_Vendor> rslt = new List<ST_Vendor>();

            rslt = dao.ST_Vendor.Where(currVendor => currVendor.del_yn == 0).ToList();

            return rslt;
        }

        public ST_Vendor Update(ST_Vendor data)
        {
            dao.Entry(data).State = EntityState.Modified;

            dao.SaveChanges();

            return data;
        }

        public void Soft_Delete(ST_Vendor data)
        {
            data.del_yn = 1;

            data.deleted_at = DateTime.Now;

            //todo

            dao.Entry(data).State = EntityState.Modified;

            dao.SaveChanges();

        }

        public void Dispose()
        {
            dao.Dispose();
        }
    }
}