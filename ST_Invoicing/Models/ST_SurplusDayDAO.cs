using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ST_Invoicing.Models
{
    public class ST_SurplusDayDAO
    {
        private STDATAEntities dao = new STDATAEntities();

        public ST_SurplusDay Insert(ST_SurplusDay data)
        {
            dao.ST_SurplusDay.Add(data);

            dao.SaveChanges();

            return data;
        }

        public ST_SurplusDay FetchBySerno(int? serno)
        {
            return dao.ST_SurplusDay.Find(serno);
        }


        public List<ST_SurplusDay> GetDataByDate(DateTime rec_date)
        {
            List<ST_SurplusDay> rslt = new List<ST_SurplusDay>();

            rslt = dao.ST_SurplusDay.Where(currSurplusDay => currSurplusDay.del_yn == 0).Where(currSurplusDay => currSurplusDay.rec_date.Equals(rec_date)).ToList();

            return rslt;
        }

        public List<ST_SurplusDay> GetDataByDateRange(DateTime start_Date, DateTime end_Date)
        {
            List<ST_SurplusDay> rslt = new List<ST_SurplusDay>();

            rslt = dao.ST_SurplusDay.Where(currSurplusDay => currSurplusDay.del_yn == 0).Where(currSurplusDay => currSurplusDay.rec_date >= start_Date).Where(currSurplusDay => currSurplusDay.rec_date <= end_Date).ToList();

            return rslt;
        }

        public List<ST_SurplusDay> GetThisMonthData(DateTime rec_date)
        {
            DateTime firstDay = new DateTime(rec_date.Year, rec_date.Month, 1);
            DateTime lastDay = new DateTime(rec_date.AddMonths(1).Year, rec_date.AddMonths(1).Month, 1).AddDays(-1);

            List<ST_SurplusDay> rslt = new List<ST_SurplusDay>();

            rslt = dao.ST_SurplusDay.Where(currSurplusDay => currSurplusDay.del_yn == 0).Where(currSurplusDay => currSurplusDay.rec_date >= firstDay).Where(currSurplusDay => currSurplusDay.rec_date <= lastDay).ToList();

            return rslt;
        }

        public List<ST_SurplusDay> GetDataWhichDateGreateThanSpecifyDate(DateTime rec_date)
        {
            List<ST_SurplusDay> rslt = new List<ST_SurplusDay>();

            rslt = dao.ST_SurplusDay.Where(currSurplusDay => currSurplusDay.del_yn == 0).Where(currSurplusDay => currSurplusDay.rec_date > rec_date).ToList();

            return rslt;
        }

        public ST_SurplusDay Update(ST_SurplusDay data)
        {
            dao.Entry(data).State = EntityState.Modified;

            dao.SaveChanges();

            return data;
        }

        public void SoftDelete(ST_SurplusDay data)
        {
            data.del_yn = 1;

            dao.Entry(data).State = EntityState.Modified;

            dao.SaveChanges();
        }

        public void Dispose()
        {
            dao.Dispose();
        }
    }
}