using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BootCampApp.DAL;
using BootCampApp.Models;

namespace BootCampApp.Controllers
{
    public class CoursesController : Controller //mou fairnei apo thn pleura tou course related data me to departm
    {
        private BootCampDbContext db = new BootCampDbContext();

        // GET: Courses
        public ActionResult Index()
        {
            //include lexh kleidi----Eager Loading(Include)----anadpodo apo to lazy loading(virtual)---eager paei mia fora sthn vazh na kanei to join kai ta fernei Ola
            //mporei na ginei method sto model kai na klhthei edw mesa---syxronh methodos!!!edw to vazei epeidh einai palio to documentation
            var courses = db.Courses.Include(c => c.Department); //edwpairnei kai to (1) navigation[Department] property,pou vrisketai sth class Course (kanei to inner join twn dyo pinakwn
            return View(courses.ToList());

            //Eager Loading(Include)
            //Lazy Loading(virtual)
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            //ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name");  //h axia sto dropdownlist tha nai to name alla egw dexomai to departmentID
            PopulateDepartmentsDropDownList();
            return View();
        }
        
        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,Hours,DepartmentID")] Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Courses.Add(course);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to Save Changes");
                
            }


            //ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
            PopulateDepartmentsDropDownList();
            return View(course);
        }

        private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            var departmentQuery = from d in db.Departments
                                  orderby d.Name
                                  select d;
            ViewBag.DepartmentID = new SelectList(departmentQuery, "DepartmentID", "Name", selectedDepartment);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            //ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
            PopulateDepartmentsDropDownList(course.DepartmentID);   //otan kanw edit me ayto tha petyxw na erthei me etoimo to department
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? courseID)
        {
            //Must!!
            if (courseID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var courseToUpdate = db.Courses.Find(courseID);
            if (TryUpdateModel(courseToUpdate, "",      //mono ta paidia ayta tha ginoun update Title, hours, departmentID
                new string[] { "Title", "Hours", "DepartmentID"}))  //Whitelist tha dexthei apo to view mono ayta ta tria pedia---mono auta tha prospathisei na kanei update
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    ModelState.AddModelError("", "Unable to Save Changes");
                }
                
            }
            //ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
            PopulateDepartmentsDropDownList(courseToUpdate.DepartmentID);
            return View(courseToUpdate);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
