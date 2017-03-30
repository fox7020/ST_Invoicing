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

        public void Dispose()
        {
            dao.Dispose();
        }

    }
}