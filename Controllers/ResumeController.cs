using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeManager.Models;

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

<<<<<<< HEAD
=======
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            Applicant applicant = _context.Applicants.Include(e => e.Experiences).Where(a => a.Id == Id).SingleOrDefault()!;

>>>>>>> 2910feb24d028988df5de4bf32a91ecd9aa9cfe2
            return View(applicant);

        }

        [HttpPost]
<<<<<<< HEAD
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Applicant applicant, List<Experience> NewExperiences)
        {

            // Remove experiences marked as deleted from the database
            // Check ModelState validity
            // Remove deleted experiences
=======
        public IActionResult Edit(Applicant applicant)
        {
            List<Experience> experiences = _context.Experiences.Where(e=>e.ApplicantId == applicant.Id).ToList();
            _context.Experiences.RemoveRange(experiences);
            _context.SaveChanges();


            applicant.Experiences.RemoveAll(e => e.IsDeleted == true);

            _context.Update(applicant);
            _context.Experiences.AddRange(applicant.Experiences);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
>>>>>>> 2910feb24d028988df5de4bf32a91ecd9aa9cfe2



            // Update existing experiences
            foreach (var experience in applicant.Experiences)
            {
                _context.Entry(experience).State = EntityState.Modified;
            }

            // Add new experiences
            if (NewExperiences != null)
            {
                int applicantId = applicant.Id; // Get the ApplicantId
                
                foreach (var newExperience in NewExperiences)
                {
                    if (newExperience.IsDeleted != true)
                    {
                        newExperience.ApplicantId = applicantId; // Set the ApplicantId for each new experience
                        _context.Experiences.Add(newExperience);
                    }
                }
            }
           // applicant.Experiences.RemoveAll(e => e.IsDeleted == true);
            // Update applicant
            _context.Update(applicant);

            // Save changes to the database
            _context.SaveChanges();
            var experiencesToDelete = applicant.Experiences.Where(e => e.IsDeleted).ToList();
            _context.Experiences.RemoveRange(experiencesToDelete);
            _context.SaveChanges();

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
