using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VSchool.Models;

namespace VSchool.ModelView
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Nullable<int> Genderd { get; set; }
        public string Photo { get; set; }

        public virtual Gender Gender { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentGrade> StudentGrades { get; set; }

        public HttpPostedFileBase studentImage { get; set; }
    }
}