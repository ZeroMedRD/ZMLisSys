using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using ZMLisSys.Models;
using LinqToExcel;

namespace BlogSample.Infrastructure.Helpers
{
    public class ImportDataHelper
    {
        /// <summary>
        /// 檢查匯入的 Excel 資料.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="importZipCodes">The import zip codes.</param>
        /// <returns></returns>
        public CheckResult CheckImportData(
            string fileName,
            List<Listest> importZipCodes)
        {
            var result = new CheckResult();

            var targetFile = new FileInfo(fileName);

            if (!targetFile.Exists)
            {
                result.ID = Guid.NewGuid();
                result.Success = false;
                result.ErrorCount = 0;
                result.ErrorMessage = "匯入的資料檔案不存在";
                return result;
            }

            var excelFile = new ExcelQueryFactory(fileName);

            //欄位對映
            excelFile.AddMapping<Listest>(x => x.ID, "ID");
            excelFile.AddMapping<Listest>(x => x.TestName, "TestName");
            excelFile.AddMapping<Listest>(x => x.P_name, "P_name");
            excelFile.AddMapping<Listest>(x => x.Sex_id, "Sex_id");
            excelFile.AddMapping<Listest>(x => x.Idno_id, "Idno_id");
            //excelFile.AddMapping<Listest>(x => x.Idno_id, "CreateDate");

            //SheetName
            var excelContent = excelFile.Worksheet<Listest>("UCL_Report");

            int errorCount = 0;
            int rowIndex = 1;
            var importErrorMessages = new List<string>();

            //檢查資料
            foreach (var row in excelContent)
            {
                var errorMessage = new StringBuilder();
                var zipCode = new Listest();

                zipCode.ID = row.ID;
                zipCode.TestName = row.TestName;
                zipCode.P_name = row.P_name;
                zipCode.Sex_id = row.Sex_id;
                zipCode.Idno_id = row.Idno_id;
                zipCode.CreateDate = DateTime.Now;

                //TestName
                //if (string.IsNullOrWhiteSpace(row.TestName))
                //{
                //    errorMessage.Append("檢驗ID - 不可空白. ");
                //}
                //zipCode.TestName = row.TestName;

                ////Town
                //if (string.IsNullOrWhiteSpace(row.Town))
                //{
                //    errorMessage.Append("鄉鎮市區名稱 - 不可空白. ");
                //}
                //zipCode.Town = row.Town;

                //=============================================================================
                if (errorMessage.Length > 0)
                {
                    errorCount += 1;
                    importErrorMessages.Add(string.Format(
                        "第 {0} 列資料發現錯誤：{1}{2}",
                        rowIndex,
                        errorMessage,
                        "<br/>"));
                }
                importZipCodes.Add(zipCode);
                rowIndex += 1;
            }

            try
            {
                result.ID = Guid.NewGuid();
                result.Success = errorCount.Equals(0);
                result.RowCount = importZipCodes.Count;
                result.ErrorCount = errorCount;

                string allErrorMessage = string.Empty;

                foreach (var message in importErrorMessages)
                {
                    allErrorMessage += message;
                }

                result.ErrorMessage = allErrorMessage;

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Saves the import data.
        /// </summary>
        /// <param name="importZipCodes">The import zip codes.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SaveImportData(IEnumerable<Listest> importZipCodes)
        {
            try
            {
                //先砍掉全部資料
                using (var db = new ZMListestEntities())
                {
                    foreach (var item in db.Listests.OrderBy(x => x.ID))
                    {
                        db.Listests.Remove(item);
                    }
                    db.SaveChanges();
                }

                //再把匯入的資料給存到資料庫
                using (var db = new ZMListestEntities())
                {
                    foreach (var item in importZipCodes)
                    {
                        db.Listests.Add(item);
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}