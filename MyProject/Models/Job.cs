using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyProject.Models
{
    public class Job
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="اسم الوظيفة")]
        public string JobTitle { get; set; }
        [Required]
        [Display(Name = "وصف الوظيفة")]
        public string JobContent { get; set; }
      //لو انا حطيت ريكويرد في الملف دا هيديني ايرور لما اجي اختار الصورة
        [Display(Name = "صورة الوظيفة")]
        public string JobImage { get; set; }

        //foreign key
        [Required]
        [Display(Name = "نوع الوظيفة")]
        public int CategoryId { get; set; }

        //video 32
        public string UserID { get; set; }
        public virtual Category Category { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<ApplyForJob> ApplyJobs { get; set; }
    }
}