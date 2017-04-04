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

        public ST_Material FetchByGuid(Guid guid)
        {
            List<ST_Material> rslt = new List<ST_Material>();

            rslt = dao.ST_Material.Where(currMaterial => currMaterial.guid.Equals(guid)).ToList();

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

        public ST_Material FetchByItemName(string item_name)
        {
            List<ST_Material> rslt = new List<ST_Material>();

            rslt = dao.ST_Material.Where(currMaterial => currMaterial.del_yn == 0).Where(currMaterial => currMaterial.item_name.Equals(item_name)).ToList();

            if (rslt.Count == 1)
            {
                return rslt[0];
            }
            else if (rslt.Count > 1)
            {
                throw new Exception("2筆以上的原物料名稱!!!");
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

            data.item_unit = data.item_unit.Trim();

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

        public void InsertBasicMaterial()
        {
            /*檢查700、850、點心盒有無資料*/

            ST_Material bowl_700 = new ST_Material(Guid.Parse("E5564142-5B2C-472F-8EDB-3E957AA1BBE3"), "700CC碗", "個", "耗材");
            ST_Material bowl_850 = new ST_Material(Guid.Parse("5981CDD4-C4A7-45A0-9832-E16574D1689C"), "850CC碗", "個", "耗材");
            ST_Material meat = new ST_Material(Guid.Parse("258921A7-9EB2-4978-9689-6284B906230D"), "850CC碗", "個", "耗材");
            ST_Material rice_Box = new ST_Material(Guid.Parse("ecf1e82d-163e-4117-bca5-125ba0118f8c"), "便當盒", "個", "耗材");

            if (FetchByGuid(bowl_700.guid) == null)
            {
                Insert(bowl_700);
            }

            if (FetchByGuid(bowl_850.guid) == null)
            {
                Insert(bowl_850);
            }

            if (FetchByGuid(meat.guid) == null)
            {
                Insert(meat);
            }

            if (FetchByGuid(rice_Box.guid) == null)
            {
                Insert(rice_Box);
            }

        }

        public void Dispose()
        {
            dao.Dispose();
        }
    }
}