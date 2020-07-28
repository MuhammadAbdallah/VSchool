using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VSchool.Models;
using VSchool.ModelView;


namespace VSchool.Controllers
{
    public class StudentController : Controller
    {
        TestEntities db = new TestEntities();
        // GET: Student
        public ActionResult Index()
        {
           // List<Student> students = db.Students.ToList();
            return View();
        }

        public ActionResult SearchStudent (string  searchName, string searchEmail)
        {

            //List<Student> students = db.Students.Where(std => (std.Name.Contains(searchName) || (std.Name == null||std.Name=="")) &&
            //(std.Email.Contains(searchEmail) || (std.Email == null || std.Name==""))).ToList();

            List<Student> students = db.Students.Where(student => (student.Name.Contains(searchName) || searchName == null ||searchName=="") && (student.Email.Contains(searchEmail) || searchEmail == null || searchEmail == "")).ToList();

            return View(students);
        }

        public ActionResult Create()
        {
            ViewBag.genderId = new SelectList(db.Genders.ToList(), "Id", "Name","Select Gender");
            return View();
        }

        [HttpPost]
        public ActionResult Create(StudentViewModel passedStudent)
        {
            Student student = new Student();
            student.Name = passedStudent.Name;
            student.Email = passedStudent.Email;
            student.Genderd = passedStudent.Genderd;
            
            //Uploading Image for student
            //File Attatchments
            if (passedStudent.studentImage != null)
            {
                if (!Directory.Exists(Server.MapPath("~/UploadFiles/StudentImage/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/UploadFiles/StudentImage/"));
                }

                string attachmentNumber = Guid.NewGuid().ToString();
                string InputFileName = Path.GetFileNameWithoutExtension(passedStudent.studentImage.FileName);
                //if (InputFileName.Length > 30)
                //{
                //    InputFileName = InputFileName.Substring(0, 30);
                //}

                var InputExtension = Path.GetExtension(passedStudent.studentImage.FileName);


                var ServerSavePath = Path.Combine(Server.MapPath("~/UploadFiles/StudentImage/") + attachmentNumber + InputFileName + InputExtension);

                // save attachment in folder
                passedStudent.studentImage.SaveAs(ServerSavePath);
                student.Photo = attachmentNumber + InputFileName + InputExtension;
            }

            db.Students.Add(student);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        public ActionResult Edit(int id)
        {
           
            StudentViewModel oldStudent = db.Students.Where(st => st.Id == id).Select(std => new StudentViewModel
            {
                
                Name = std.Name,
                Email = std.Email,
                Photo = std.Photo,
                Genderd = std.Genderd,
                
            }).FirstOrDefault();
            ViewBag.genderId = new SelectList(db.Genders.ToList(), "Id", "Name",id);
            return View(oldStudent);

        }

        [HttpPost]
        public ActionResult Edit(StudentViewModel passedStudent)
        {
            Student student = db.Students.Where(std => std.Id == passedStudent.Id).FirstOrDefault();
            student.Name = passedStudent.Name;
            student.Email = passedStudent.Email;
            student.Genderd = passedStudent.Genderd;
            if(passedStudent.studentImage != null)
            {
                if (!Directory.Exists(Server.MapPath("~/UploadFiles/StudentImage/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/UploadFiles/StudentImage/"));
                }

                string attachNumber = Guid.NewGuid().ToString();
                string InputFileName = Path.GetFileNameWithoutExtension(passedStudent.studentImage.FileName);
                string InputFileExtension = Path.GetExtension(passedStudent.studentImage.FileName);

                var severSavePath = Path.Combine(Server.MapPath("~/UploadFiles/StudentImage/")) + attachNumber + InputFileName + InputFileExtension;

                passedStudent.studentImage.SaveAs(severSavePath);

                student.Photo = attachNumber + InputFileName + InputFileExtension;

            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Delete(int id)
        {
            Student student = db.Students.Where(std => std.Id == id).FirstOrDefault();
            List<StudentGrade> st = db.StudentGrades.Where(std => std.StudentId == id).ToList();

            foreach(var std in st)
            {
                var s = db.StudentGrades.Where(ss=>ss.StudentId == std.StudentId).FirstOrDefault();
                db.StudentGrades.Remove(s);
            }
           
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
       


    }
}