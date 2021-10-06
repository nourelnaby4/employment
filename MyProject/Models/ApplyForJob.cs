using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Models
{
    public class ApplyForJob
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime ApplyDate { get; set; }
        public int JobId { get; set; }
        public string UserId { get; set; }


   


        //نص الاتصال بجدول الوظائف و جدول المستخدمين
        public virtual Job job { get; set; }
        public virtual ApplicationUser user { get; set; }

        //لابد من اضافة هذه النصوص في كلاس الجوب و كلاس اليوزر ابليكيشن
        //وذلك حتي يتم الربط بينهم 
        //   public virtual ICollection<ApplyForJob> ApplyJobs { get; set; }
    }
}