using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public List<ST_Emp> GetDataList_NotDel()
        {
            List<ST_Emp> rslt = new List<ST_Emp>();

            rslt = dao.ST_Emp.Where(currEmp => currEmp.del_yn == 0).ToList();

            return rslt;
        }

        public ST_Emp Update(ST_Emp data)
        {
            dao.Entry(data).State = EntityState.Modified;

            dao.SaveChanges();

            return data;
        }

        public void Soft_Delete(ST_Emp data)
        {
            data.password2 = data.password;

            data.del_yn = 1;

            data.deleted_at = DateTime.Now;

            dao.Entry(data).State = EntityState.Modified;


            dao.SaveChanges();

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
                throw new Exception("兩筆以上的account");
            }

            return isUni;
        }
    }
}