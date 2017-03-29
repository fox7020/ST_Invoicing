using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ST_Invoicing.Models
{
    public class ST_MaterialDAO
    {
        private STDATAEntities dao = new STDATAEntities();      

        public ST_Material Insert(ST_Material data)
        {
            dao.ST_Material.Add(data);

            dao.SaveChanges();           

            return data;
        }

        public ST_Material FetchBySerno(int? serno)
        {
            return dao.ST_Material.Find(serno);
        }

        public ST_Material FetchByGuid(string guid)
        {
            List<ST_Material> rslt = new List<ST_Material>();

            rslt = dao.ST_Material.Where(currMaterial => currMaterial.guid == Guid.Parse(guid)).ToList();

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

        public List<ST_Material> GetDataList_NotDel()
        {
            List<ST_Material> rslt = new List<ST_Material>();

            rslt = dao.ST_Material.Where(currMaterial => currMaterial.del_yn == 0).ToList();
       
            return rslt;
        }

        public ST_Material Update(ST_Material data)
        {
            dao.Entry(data).State = EntityState.Modified;

            dao.SaveChanges();          

            return data;
        }

        public void Soft_Delete(ST_Material data)
        {
            data.del_yn = 1;

            data.deleted_at = DateTime.Now;

            data.item_name = data.item_name.Trim();

            data.item_species = data.item_species.Trim();

            data.utem_unit = data.utem_unit.Trim();

            dao.Entry(data).State = EntityState.Modified;

            dao.SaveChanges();
       
        }

        public bool IsUniItem(ST_Material data)
        {
            bool isUni = false;

            List<ST_Material> rslt = new List<ST_Material>();

            rslt = dao.ST_Material.Where(currMaterial => currMaterial.del_yn == 0).Where(currMaterial => currMaterial.item_name == data.item_name).ToList();

            if (data.serno == 0)
            {
               
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
                    throw new Exception("兩筆以上的item_name");
                }
            }
            else
            {
                
                if (rslt.Count == 1 && rslt[0].serno == data.serno)
                {
                    isUni = true;
                }
                else if (rslt.Count == 1 && rslt[0].serno != data.serno)
                {
                    isUni = false;
                }
                else if (rslt.Count == 0)
                {
                    isUni = true;
                }
            }
       
            return isUni;
        }

        public void Dispose()
        {
            dao.Dispose();
        }
    }
}