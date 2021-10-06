using Microsoft.AspNet.Identity;
using MyProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;




namespace MyProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            var list = db.Categories.ToList();
            return View(list);
        }

        //not apartial view ,use page layout only
        public ActionResult Details(int JobId)
        {
            var job = db.Jobs.Find(JobId);
            if (job == null)
            {
                return HttpNotFound();
            }

            //to transfare id to apply action
            Session["JobId"] = JobId;
            return View(job);
        }
        [Authorize]
        public ActionResult Apply()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Apply(string Message)
        {
            //user identity getUserid it return the id of the user that are registration now so we use authorize in this action
            var UserId = User.Identity.GetUserId();

            //we should transfare session data to int "typing cast"
            var JobId =(int) Session["JobId"];


            //حتي لايسجل نفس المستخدم نفس العمل اكثر من مرة
            var Check = db.ApplyForJobs.Where(a => a.UserId==UserId && a.JobId==JobId).ToList();
            if (Check.Count() < 1)
            {
                
                var job = new ApplyForJob();
                job.UserId = UserId;
                job.JobId = JobId;
                job.Message = Message;
                job.ApplyDate = DateTime.Now;


                db.ApplyForJobs.Add(job);
                db.SaveChanges();
                ViewBag.Result = "لقد تم التسجيل بنجاح";
            }
            else
            {
                ViewBag.Result = "المعذرة لقد سبق ان تقدمت الي نفس الوظيفة";
            }
            return View();
        }

        //action for retrive all jobs that useraccount applyed fot it
        //templete for this action is (list)
        public ActionResult GetUserJobs()
        {
            var UserId = User.Identity.GetUserId();
            var Jobs = db.ApplyForJobs.Where(a => a.UserId == UserId);
            return View(Jobs.ToList());
        }


        //هذه الميثود خاصة بعرض تفاصيل العمل الذي قام المستخد بالفعل بالتقدم اليه 
        public ActionResult DetailsForMyJob( int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }

            return View(job);
        }
        [HttpGet]
        //this action for للتعديل علي الوظائف التي قمت انا كمستخدم بالتقدم عليها
        public ActionResult EditForMyJob(int id)
        {
            var job=db.ApplyForJobs.Find(id);
            if (job== null)
            {
                return HttpNotFound();
            }
            
                return View(job);
            
        }
        [HttpPost]
        public ActionResult EditForMyJob(ApplyForJob job)
        {
            if(ModelState.IsValid)
            {

                //راجع فيديو رقم 31
                job.ApplyDate = DateTime.Now;
                //System.Data.Entity.EntityState.Modified;
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();

                //RedirectionToAction becouse it will transfer to another view action
                return RedirectToAction("GetUserJobs");
            }
            
            return View(job);
        }
        [HttpGet]
        public ActionResult DeleteForMyJob(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: /Delete/5
        [HttpPost]
        public ActionResult DeleteForMyJob(ApplyForJob job)
        {
            if (ModelState.IsValid)
            {
                var MyJob = db.ApplyForJobs.Find(job.Id);
                db.ApplyForJobs.Remove(MyJob);
                db.SaveChanges();
                return RedirectToAction("GetUserJobs");
            }
            return View(job);
        }
        public ActionResult GetJobsByPublisher()
        {
            var UserId = User.Identity.GetUserId();
            // بطريقة اخري يممكنا جلب وظائف التي نشرها ناشر معين
            /*
             var MyJob=from j 
             
            var MyJob = db.ApplyForJobs.Where(a => a.UserId == UserId).ToList();  XXXincorrect
            */
            var jobs = from app in db.ApplyForJobs
                       join job in db.Jobs
                       on app.JobId equals job.Id
                       where job.User.Id == UserId
                       select app;
            return View(jobs.ToList());
        }
        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(string SearchName)
        {
            //model Class =>Jobs Templete =>List in the View
            //ولكننا هنقوم بتغير شكل العرض الي الشكل الفيو بتاع اكشن الاندكس اللي في الهوم فهناخد الفيو بتاعها نعدل عليه 
            
            var result = db.Jobs.Where(a => a.JobTitle.Contains(SearchName) ||
              a.JobContent.Contains(SearchName) || a.JobContent.Contains(SearchName) ||
              a.Category.CategoryName.Contains(SearchName) || 
              a.Category.CategoryDescription.Contains(SearchName)).ToList();
           
            return View(result);
        }
        [HttpGet]
        public ActionResult EditProfile()
        {
            //View model Edit...ClassMdoel=>EditProfileModel
            EditProfileViewModel Profile = new EditProfileViewModel();
            var UserId = User.Identity.GetUserId();
            var CurrentUser = db.Users.Where(a => a.Id == UserId).SingleOrDefault();
            Profile.username = CurrentUser.UserName;
            Profile.Email = CurrentUser.Email;
            return View(Profile);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}