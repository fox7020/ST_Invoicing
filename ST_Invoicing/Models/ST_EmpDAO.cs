using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ST_Invoicing.Models
{
    public class ST_EmpDAO
    {
        private STDATAEntities dao = new STDATAEntities();

        public ST_Emp Insert(ST_Emp data)
        {
            dao.ST_Emp.Add(data);

            dao.SaveChanges();

            return data;
        }

        public bool IsUniID(string account)
        {
            bool isUni = false;

            List<ST_Emp> rslt = new List<ST_Emp>();

            rslt = dao.ST_Emp.Where(currEmp => currEmp.account == account).ToList();

            if (rslt.Count == 1)
            {
                isUni = false;
            }
            else if (rslt.Count == 0)
            {
                isUni = true;
            }
            else
            {
                throw new Exception("兩筆以上的ID");
            }

            return isUni;
        }
    }
}