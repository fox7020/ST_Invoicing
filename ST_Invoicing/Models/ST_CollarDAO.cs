using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ST_Invoicing.Models
{
    public class ST_CollarDAO
    {
        private STDATAEntities dao = new STDATAEntities();

        public ST_Collar Insert(ST_Collar data)
        {
            dao.ST_Collar.Add(data);

            dao.SaveChanges();

            return data;
        }

        public ST_Collar FetchBySerno(int? serno)
        {
            return dao.ST_Collar.Find(serno);
        }

        public List<ST_Collar> GetDataList_NotDel()
        {
            List<ST_Collar> rslt = new List<ST_Collar>();

            rslt = dao.ST_Collar.Where(currCollar => currCollar.del_yn == 0).ToList();

            return rslt;
        }

        public void Dispose()
        {
            dao.Dispose();
        }

    }
}