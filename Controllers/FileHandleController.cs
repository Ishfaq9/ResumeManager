using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using ResumeManager.Models;
using System;



namespace ResumeManager.Controllers
{
    public class FileHandleController : Controller
    {
        private readonly ResumeDbContext _resumeDbContext;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public FileHandleController(IWebHostEnvironment hostingEnvironment, ResumeDbContext resumeDbContext)
        {
            _hostingEnvironment = hostingEnvironment;
            _resumeDbContext = resumeDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportMembers(IFormFile fileupload, string TemplateName, string Remarks)
        {
            string loggedInUserId = Guid.NewGuid().ToString();
            int i_menu = 0;
            int fSize = 0;
            int fileSize = 0;
            String fSizewithUnit = null;
            string fname = null;
            string message = "";
            int ShowAudioDiv = 0;
            try
            {
                if (fileupload != null)
                {
                    // Checking for Internet Explorer  
                    string userAgent = Request.Headers["User-Agent"].ToString();
                    // Perform user-agent parsing to detect Internet Explorer
                    if (userAgent.Contains("MSIE") || userAgent.Contains("Trident"))
                    {
                        string[] testfiles = fileupload.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = fileupload.FileName;
                    }
                    string ext = Path.GetExtension(fname);
                    fileSize = (int)fileupload.Length;
                    fSize = fileSize / 1000000;
                    if (fSize <= 5)
                    {
                        bool isValidFileType = IsValidCSVFile(fileupload, ext);

                        if (isValidFileType/* && group != null*/)
                        {
                            string path = Path.Combine(_hostingEnvironment.WebRootPath, "UploadedCSVs", loggedInUserId);
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            string fPath = Path.Combine(path) + fname;

                            // Save the uploaded file to the specified path
                            using (var fileStream = new FileStream(fPath, FileMode.Create))
                            {
                                fileupload.CopyTo(fileStream);
                            }

                            string csvData = System.IO.File.ReadAllText(fPath);
                            int memberCounter = 0;
                            var rows = csvData.Split('\n');
                            if (rows.Count() > 1)
                            {
                                var phoneNumberColumn = rows[0].Trim();

                                if (phoneNumberColumn == "MobileNumber")
                                {

                                    Group group = new Group();
                                    group.Name = TemplateName;
                                    group.Description = Remarks;
                                    group.InsertedDate = DateTime.Now;
                                    group.CampaignType = "General";
                                    _resumeDbContext.Groups.Add(group);
                                    _resumeDbContext.SaveChanges();
                                    List<Member> bulkInsert = new List<Member>();
                                    for (int i = 1; i < rows.Count(); i++)
                                    {
                                        var row = rows[i].Replace("\r", "");
                                        if (!string.IsNullOrEmpty(row) && row.All(Char.IsDigit))
                                        {
                                            var member = new Member
                                            {
                                                PhoneNumber = row.Trim(),
                                                GroupId = group.Id,
                                                Name = loggedInUserId,
                                                InsertedDate = DateTime.Now
                                            };
                                            bulkInsert.Add(member);
                                            memberCounter++;
                                        }
                                    }
                                    _resumeDbContext.Members.AddRange(bulkInsert);
                                    _resumeDbContext.SaveChanges();
                                }
                                else
                                {
                                    message = "CSV format is not correct. Please download sample members.";
                                }
                            }
                            if (memberCounter > 0)
                            {
                                message = memberCounter.ToString() + " members imported.";

                            }
                            else
                            {
                                message = memberCounter.ToString() + " members imported.";
                            }
                            ShowAudioDiv = 1;
                        }
                        else
                        {
                            message = "Please select comma separated CSV file.";
                            ShowAudioDiv = 2;
                        }
                    }
                    else
                    {
                        message = "Maximum file size limit is 5 MB";
                        ShowAudioDiv = 2;
                    }
                }
                else
                {
                    ShowAudioDiv = 2;
                    message = "Empty file no data found.";
                }

            }
            catch (Exception ex)
            {
                ShowAudioDiv = 2;
                message = "CSV Upload failed.";
                Console.WriteLine("RetailSMS/GroupsController/ImportMembers failed  For  user id:" + loggedInUserId + "Error : " + ex.Message);
                Console.WriteLine("RetailSMS/GroupsController/ImportMembers/ failed For  user id:" + loggedInUserId + "Error : " + ex.Message, "ImportMembers failed For user id:" + loggedInUserId);

            }
            if (fSize <= 0)
            {
                fileSize = fileSize / 1000;
                fSizewithUnit = fileSize + "KB";
            }
            else
            {
                fSizewithUnit = fSize + "MB";
            }

            ViewBag.message = message;
            //ViewBag.phoneNumber = loggedInUser.PhoneNumber;
            ViewBag.showAudioDiv = ShowAudioDiv;
            ViewBag.size = fSizewithUnit;
            ViewBag.filename = fname;

            return View("Index");
        }

        [NonAction]
        private bool IsValidCSVFile(IFormFile uploadFile, string ext)
        {
            bool isValid = false;
            try
            {
                if (ext.ToLower() == ".csv" || ext.ToLower() == ".txt" || ext.ToLower() == ".text")
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception occurred in IsValidFileType for fileName: " + uploadFile.FileName + " Error: " + ex.Message);
            }
            return isValid;

        }


        [HttpPost]
        public ActionResult ImportMembers2(Group model)
        {

            string insertedByValue = "your_value_here"; 
            model.Name = insertedByValue;

           // model.Members.Select(c => { c.InsertedDate = DateTime.Now;return c});
            model.Members.All(c => { c.Name = insertedByValue; c.InsertedDate=DateTime.Now; return true; });

           // model.Members.Select(s => s.Name = insertedByValue);

            //var details = model.Members.Select(w =>
            //{
            //    w.Name = insertedByValue; return w;
            //});

            //_resumeDbContext.Members.AddRange(details);
            //   _resumeDbContext.Members.AddRange(model.Members);
            _resumeDbContext.Add(model);
            _resumeDbContext.SaveChanges();


            //string loggedInUserId = Guid.NewGuid().ToString();
            //string message = "";
            //int ShowAudioDiv = 0;
            //try
            //{
            //    if (numbers != null)
            //    {
            //        int memberCounter = 0;
            //        if (numbers.Count() > 0)
            //        {
            //            Group group = new Group();
            //            group.Name = "okay";
            //            group.Description = "123";
            //            group.InsertedDate = DateTime.Now;
            //            group.CampaignType = "General";
            //            _resumeDbContext.Groups.Add(group);
            //            _resumeDbContext.SaveChanges();
            //            List<Member> bulkInsert = new List<Member>();
            //            for (int i = 0; i < numbers.Count(); i++)
            //            {
            //                var row = numbers[i];
            //                var member = new Member
            //                {
            //                    PhoneNumber = row.Trim(),
            //                    GroupId = group.Id,
            //                    Name = loggedInUserId,
            //                    InsertedDate = DateTime.Now
            //                };
            //                bulkInsert.Add(member);
            //                memberCounter++;
            //            }
            //            _resumeDbContext.Members.AddRange(bulkInsert);
            //            _resumeDbContext.SaveChanges();
            //        }
            //        if (memberCounter > 0)
            //        {
            //            message = memberCounter.ToString() + " members imported.";
            //        }
            //        else
            //        {
            //            message = memberCounter.ToString() + " members imported.";
            //        }
            //        ShowAudioDiv = 1;
            //    }
            //    else
            //    {
            //        ShowAudioDiv = 2;
            //        message = "Empty file no data found.";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ShowAudioDiv = 2;
            //    message = "CSV Upload failed.";
            //    Console.WriteLine("RetailSMS/GroupsController/ImportMembers failed  For  user id:" + loggedInUserId + "Error : " + ex.Message);
            //    Console.WriteLine("RetailSMS/GroupsController/ImportMembers/ failed For  user id:" + loggedInUserId + "Error : " + ex.Message, "ImportMembers failed For user id:" + loggedInUserId);
            //}
            //ViewBag.message = message;
            //ViewBag.showAudioDiv = ShowAudioDiv;
            return View("Index");
        }


        public ActionResult ImportMembers3()
        {
            return View();

        }

        [HttpPost]
        public ActionResult ImportMembers3(Group model)
        {
            return View(model);
        }
    }
}
