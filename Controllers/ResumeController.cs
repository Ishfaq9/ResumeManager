using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeManager.Models;
using System.Net.Mime;

namespace ResumeManager.Controllers
{
    public class ResumeController : Controller
    {
        private readonly ResumeDbContext _context;

        public ResumeController(ResumeDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Applicant> applicants;
            applicants = _context.Applicants.ToList();
            return View(applicants);
        }

        [HttpGet]
        public IActionResult Create()
        {
            Applicant applicant = new Applicant();
            applicant.Experiences.Add(new Experience() { ExperienceId = 1 });
            //applicant.Experiences.Add(new Experience() { ExperienceId = 2 });
            //applicant.Experiences.Add(new Experience() { ExperienceId = 3 });
            return View(applicant);
        }


        [HttpPost]
        public IActionResult Create(Applicant applicant)
        {

            //string uniqueFileName = GetUploadedFileName(applicant);
            //applicant.PhotoUrl = uniqueFileName;
           // applicant.Experiences.ForEach(experience => experience.Designation = "okay");
            _context.Add(applicant);
            _context.SaveChanges();
            return RedirectToAction("index");

        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            Applicant applicant= _context.Applicants.Include(e=> e.Experiences).Where(a=> a.Id == Id).SingleOrDefault()!;

            return View(applicant);

        }

    
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            Applicant applicant = _context.Applicants.Include(e => e.Experiences).Where(a => a.Id == Id).SingleOrDefault()!;

            return View(applicant);

        }
        [HttpPost]
        public IActionResult Delete(Applicant applicant)
        {
           _context.Applicants.Remove(applicant);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            Applicant applicant = _context.Applicants.Include(e => e.Experiences).Where(a => a.Id == Id).SingleOrDefault()!;

            return View(applicant);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Applicant model)
        {
            //afif bro
            //Applicant entity = _context.Applicants.Include(i => i.Experiences).FirstOrDefault(w => w.Id == model.Id);

            //entity.Name = model.Name;
            //entity.Age = model.Age;
            //entity.Gender = model.Gender;
            //entity.Qualification = model.Qualification;
            //entity.TotalExperience = model.TotalExperience;
            //entity.PhotoUrl = model.PhotoUrl;

            //entity.Experiences.AddRange(model.Experiences.Where(w => w.ExperienceId == 0 && !w.IsDeleted));
            ////entity.Experiences.RemoveAll(w => !model.Experiences.Any(p2 => p2.ExperienceId == w.ExperienceId));
            //entity.Experiences.RemoveAll(w => model.Experiences.Any(p2 => p2.ExperienceId == w.ExperienceId && p2.IsDeleted == true));
            //foreach (var item in entity.Experiences.Where(w => w.ExperienceId != 0 && model.Experiences.Any(w2 => w2.ExperienceId == w.ExperienceId)))
            //{
            //    var newEx = model.Experiences.FirstOrDefault(w => w.ExperienceId == item.ExperienceId);
            //    item.CompanyName = newEx.CompanyName;
            //    item.Designation = newEx.Designation;
            //    item.YearsWorked = newEx.YearsWorked;
            //}
            //_context.Applicants.Update(entity);
            //_context.SaveChanges();


            //ishfaq
            var applicantdb = _context.Applicants.Where(a => a.Id == model.Id).FirstOrDefault();

            applicantdb.Age = model.Age;
            applicantdb.Qualification = model.Qualification;
            applicantdb.Name = model.Name;
            applicantdb.Gender = model.Gender;
            applicantdb.TotalExperience = model.TotalExperience;
            applicantdb.PhotoUrl = model.PhotoUrl;

            foreach (var modelExperience in model.Experiences)
            {
                var existingExperience = _context.Experiences.FirstOrDefault(e => e.ExperienceId == modelExperience.ExperienceId);

                if (existingExperience != null && !modelExperience.IsDeleted)
                {
                    existingExperience.CompanyName = modelExperience.CompanyName;
                    existingExperience.Designation = modelExperience.Designation;
                    existingExperience.YearsWorked = modelExperience.YearsWorked;
                    _context.Entry(existingExperience).State = EntityState.Modified;
                }
                if (modelExperience.ExperienceId == 0 && !modelExperience.IsDeleted )
                {
                    modelExperience.ApplicantId = model.Id;
                    _context.Experiences.Add(modelExperience);
                }
                if (existingExperience != null && modelExperience.IsDeleted)
                {
                    _context.Experiences.Remove(existingExperience);
                }
            }
            _context.Applicants.Update(applicantdb);
            _context.SaveChanges();

            // Remove experiences marked as deleted from the database
            // Check ModelState validity
            // Remove deleted experiences
            //Applicant applicant1 = _context.Applicants.Include(e => e.Experiences).Where(a => a.Id == applicant.Id).SingleOrDefault()!;
            //Applicant applicant1 = _context.Applicants.Where(a => a.Id == applicant.Id).SingleOrDefault()!;
            //Applicant applicant1 = _context.Applicants.Include(e => e.Experiences).Where(a => a.Id == applicant.Id).SingleOrDefault()!;


            ////Experience model = _context.Experiences.SingleOrDefault(a => a.ApplicantId == applicant.Id)!;

            //foreach (var experience in applicant.Experiences)
            //{
            //    int applicantId = applicant.Id;

            //    if (experience1.ExperienceId == experience.ExperienceId)
            //    {
            //        experience1.ApplicantId = applicantId;
            //        experience1.CompanyName = experience.CompanyName;
            //        experience1.Designation = experience.Designation;
            //        experience1.YearsWorked = experience.YearsWorked;
            //        experience1.IsActive = experience.IsActive;
            //    }
            //    if (experience.ExperienceId == 0)
            //    {
            //        experience1.ApplicantId = applicantId;
            //        _context.Experiences.Add(experience);
            //    }
            //}
            ////update the master class
            //applicant1.Id = applicant.Id;
            //applicant1.Name = applicant.Name;
            //applicant1.Age = applicant.Age;
            //applicant1.Gender = applicant.Gender;
            //applicant1.Qualification = applicant.Qualification;
            //applicant1.TotalExperience = applicant.TotalExperience;
            //applicant1.PhotoUrl = applicant.PhotoUrl;

            //Update existing experiences
            //foreach (var experience in applicant.Experiences)
            //{
            //    if (experience.ExperienceId == 0)
            //    {
            //        int applicantId = applicant.Id;
            //        experience.ApplicantId = applicantId;
            //        _context.Experiences.Add(experience);
            //    }
            //    else
            //    {
            //        _context.Entry(experience).State = EntityState.Modified;
            //    }
            //}
            //Add new experiences
            //if (NewExperiences != null)
            //{
            //    int applicantId = applicant.Id; // Get the ApplicantId

            //    foreach (var newExperience in NewExperiences)
            //    {
            //        if (newExperience.IsDeleted != true)
            //        {
            //            newExperience.ApplicantId = applicantId; // Set the ApplicantId for each new experience
            //            _context.Experiences.Add(newExperience);
            //        }
            //    }
            //}
            // applicant.Experiences.RemoveAll(e => e.IsDeleted == true);
            // Update applicant
            //_context.Update(applicant);
            //// Save changes to the database
            //_context.SaveChanges();
            //var experiencesToDelete = applicant.Experiences.Where(e => e.IsDeleted).ToList();
            //_context.Experiences.RemoveRange(experiencesToDelete);
            //_context.SaveChanges();

            // Redirect to the Index action

            
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult CreateEx()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateEx(Experience experience)
        {
            // Check if the combination already exists in the database
            var existingExperience = _context.Experiences
                .FirstOrDefault(e => e.ApplicantId == experience.ApplicantId &&
                                     e.CompanyName == experience.CompanyName &&
                                     e.IsActive == experience.IsActive);

            if (existingExperience != null)
            {
                return BadRequest("Data already exists");
            }

            _context.Experiences.Add(experience);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home"); // Redirect to home page or any other page
        }

        
    }
}
