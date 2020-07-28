using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VSchool.Models;

namespace VSchool.Controllers
{
    public class StudentsGradesController : Controller
    {
        TestEntities db = new TestEntities();

        // GET: StudentsGrades
        public ActionResult Index()
        {
           // List<StudentGrade> students = db.StudentGrades.ToList();
            return View();
        }

        public ActionResult SubmitDegree()
        {
            ViewBag.students = new SelectList(db.Students.ToList(), "Id", "Name");
            ViewBag.subjects = new SelectList(db.Subjects.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult SubmitDegree(StudentGrade passedStudentGrade)
        {
            StudentGrade stdGrade = new StudentGrade();
            stdGrade.StudentId = passedStudentGrade.StudentId;
            stdGrade.SubjectId = passedStudentGrade.SubjectId;
            stdGrade.Degree = passedStudentGrade.Degree;
            db.StudentGrades.Add(stdGrade);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult SearchDegree(string studentName, string subjectName)
        {
            List<StudentGrade> students = db.StudentGrades.Where(studentGrade => (studentGrade.Student.Name.Contains(studentName) || studentName == null || studentName == "") && (studentGrade.Subject.Name.Contains(subjectName) || subjectName == null || subjectName=="")).ToList();

            return View(students);

        }

        public ActionResult Edit(int id)
        {
            
            StudentGrade student = db.StudentGrades.Where(stdGrd => stdGrd.Id == id).FirstOrDefault();
            ViewBag.students = new SelectList(db.Students.ToList(), "Id", "Name", id);
            ViewBag.subjects = new SelectList(db.Subjects.ToList(), "Id", "Name", id);
            return View(student);
        }

        [HttpPost]
        public ActionResult Edit(StudentGrade passedStudentGrade)
        {
            StudentGrade student = db.StudentGrades.Where(stdGrd => stdGrd.Id == passedStudentGrade.Id).FirstOrDefault();
            student.StudentId = passedStudentGrade.StudentId;
            student.SubjectId = passedStudentGrade.SubjectId;
            student.Degree = passedStudentGrade.Degree;
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult Delete(int id)
        {
            StudentGrade stdGrade = db.StudentGrades.Where(studentGrade => studentGrade.Id == id).FirstOrDefault();
            db.StudentGrades.Remove(stdGrade);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}