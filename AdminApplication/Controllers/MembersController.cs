using AdminApplication.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AdminApplication.Controllers
{
    public class MembersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ImportMembers(IFormFile file)
        {
            string pathToUpload = $"{Path.GetTempPath()}\\{file.FileName}";

            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            List<Member> users = getAllMembersFromFile(file.FileName);
            HttpClient client = new HttpClient();
            string URL = "https://onlinelibraryintegratedsystems.azurewebsites.net/api/Admin/ImportAllUsers";

            HttpContent content = new StringContent(JsonConvert.SerializeObject(users), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<bool>().Result;

            return RedirectToAction("Index", "Home");

        }

        private List<Member> getAllMembersFromFile(string fileName)
        {
            List<Member> members = new List<Member>();
            string filePath = $"{Path.GetTempPath()}\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        members.Add(new Models.Member
                        {
                            Email = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            ConfirmPassword = reader.GetValue(2).ToString()
                        });
                    }

                }
            }
            return members;

        }
    }
}
