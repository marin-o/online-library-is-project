﻿using AdminApplication.Models;
using ClosedXML.Excel;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace AdminApplication.Controllers
{
    public class BorrowingHistoryController : Controller
    {
        public BorrowingHistoryController()
        {
            GemBox.Document.ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            string URL = "https://onlinelibraryintegratedsystems.azurewebsites.net/api/Admin/GetAllBorrowingHistories";

            HttpResponseMessage response = client.GetAsync(URL).Result;
            var data = response.Content.ReadAsAsync<List<BorrowingHistory>>().Result;
            return View(data);
        }

        public IActionResult Details(string id)
        {
            HttpClient client = new HttpClient();
            //added in next aud
            string URL = "https://onlinelibraryintegratedsystems.azurewebsites.net/api/Admin/GetDetailsForBorrowingHistory";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<BorrowingHistory>().Result;

            return View(result);

        }



        public FileContentResult CreateInvoice(string id)
        {
            HttpClient client = new HttpClient();

            string URL = "https://onlinelibraryintegratedsystems.azurewebsites.net/api/Admin/GetDetailsForBorrowingHistory";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<BorrowingHistory>().Result;

            var assembly = Assembly.GetExecutingAssembly();
            var resource = "AdminApplication.Invoice.docx";

            using (var stream = assembly.GetManifestResourceStream(resource))
            {
                var document = DocumentModel.Load(stream, GemBox.Document.LoadOptions.DocxDefault);

                document.Content.Replace("{{BorrowingHistoryNumber}}", result?.Id.ToString());
                document.Content.Replace("{{UserName}}", result?.Member?.UserName);

                StringBuilder stringBuilder = new StringBuilder();
                var total = result.Books?.Count;
                foreach (var item in result.Books)
                {
                    stringBuilder.AppendLine("The book " + item.Book?.Title + " from author " + item.Book?.Author?.Name + " from category " + item.Book?.Category?.Name
                        + ", was borrowed on " + item.BorrowedAt.ToString() + ", and " + (item.Returned ? "returned at " : "has not yet been returned.") + (item.Returned ? item.ReturnedAt.ToString() : ""));

                }
                document.Content.Replace("{{BookList}}", stringBuilder.ToString());
                document.Content.Replace("{{TotalBooks}}", total.ToString());

                var strm = new MemoryStream();
                document.Save(strm, new PdfSaveOptions());
                return File(strm.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
            }


        }

        [HttpGet]
        public FileContentResult ExportAllBorrowingHistories()
        {
            string fileName = "BorrowingHistorys.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("BorrowingHistorys");
                worksheet.Cell(1, 1).Value = "BorrowingHistoryID";
                worksheet.Cell(1, 2).Value = "Member UserName";
                worksheet.Cell(1, 3).Value = "Total Books Borrowed";
                HttpClient client = new HttpClient();
                string URL = "https://onlinelibraryintegratedsystems.azurewebsites.net/api/Admin/GetAllBorrowingHistories";

                HttpResponseMessage response = client.GetAsync(URL).Result;
                var data = response.Content.ReadAsAsync<List<BorrowingHistory>>().Result;

                for (int i = 0; i < data.Count(); i++)
                {
                    var item = data[i];
                    worksheet.Cell(i + 2, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 2, 2).Value = item.Member?.UserName;
                    for (int j = 0; j < item.Books?.Count(); j++)
                    {
                        worksheet.Cell(1, 4 + j).Value = "Book - " + (j + 1);
                        worksheet.Cell(i + 2, 4 + j).Value = item.Books.ElementAt(j).Book.Title;
                    }
                    worksheet.Cell(i + 2, 3).Value = item.Books?.Count;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }

        }
    }
}
