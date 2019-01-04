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
using BootCampApp.ViewModels;

namespace BootCampApp.Controllers
{
    public class InstructorsController : Controller
    {
        private BootCampDbContext db = new BootCampDbContext();

        // GET: Instructors
        public ActionResult Index(int? id, int? courseID)   //id --- instructor
        {
            var viewModel = new InstructorsData();

            //Query
            //inner join
            //Eager loading
            viewModel.Instructors = db.Instructors
                .Include(i => i.OfficeAssignment)
                //.Include(i => i.Courses.Select(c => c.Department))-----navigate mesw ths class course na parw to Department
                .OrderBy(i => i.LastName);

            if (id != null)
            {
                ViewBag.InstructorID = id.Value;
                viewModel.Courses = viewModel.Instructors   //Ta Courses Tha ta ferei logw "Lazy-Loading" epeidh tou zhththikan
                    .Where(i => i.ID == id.Value).Single().Courses;          //single tha ferei ena instructors apo th list viewmodel.Instuctors pou epilexame
                //tha gemisei thn list apo courses gia enan Instructors                    
            }

            if (courseID != null)
            {
                ViewBag.CourseID = courseID.Value;
                //Lazy Loading
                viewModel.Enrollments = viewModel.Courses   //Ta Enrollments Tha ta ferei logw "Lazy-Loading" epeidh tou zhththikan
                    .Where(c => c.CourseID == courseID).Single().Enrollments;

                ////1h epilogh Lazy
                //2h Eager
                //Advanced texnikh Explicit
                //Explicit Loading----eite collection(lexh)eite load(lexh)---ayta pou xreiazomai akrivws----

                //var selectedCourse = viewModel.Courses
                //    .Where(x => x.CourseID == courseID).Single();
                //db.Entry(selectedCourse).Collection(x => x.Enrollments).Load(); //collection ta polla gia ena to load
                //foreach (Enrollment enrollment in selectedCourse.Enrollments)
                //{
                //    db.Entry(enrollment).Reference(x => x.Student).Load();  //reference ena tha parw
                //}
                //viewModel.Enrollments = selectedCourse.Enrollments;
            }

            return View(viewModel);
        }

        // GET: Instructors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // GET: Instructors/Create
        public ActionResult Create()
        {
            //ViewBag.ID = new SelectList(db.OfficeAssignments, "InstructorID", "Location");
            var instructor = new Instructor();
            instructor.Courses = new List<Course>();
            PopulateAssignedCourseData(instructor);
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName,FirstName,HireDate, OfficeAssignment")] Instructor instructor, string[] selectedCourses)
        {
            //fill courses list on Instructor
            if (selectedCourses != null)
            {
                instructor.Courses = new List<Course>();
                foreach (var course in selectedCourses)
                {
                    var courseToAdd = db.Courses.Find(int.Parse(course));       //int.parse gt h find fernei int---to course einai to course id
                    instructor.Courses.Add(courseToAdd);
                }
                
                if (ModelState.IsValid)
                {
                    db.Instructors.Add(instructor);
                    db.SaveChanges();       //Instructor kai meta courses 
                    return RedirectToAction("Index");
                }
            }
            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        // GET: Instructors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Instructor instructor = db.Instructors.Find(id);
            Instructor instructor = db.Instructors  //fernei tous instructors paketo me ta officeassigments
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .Where(i => i.ID == id)
                .Single();  // or SingleOrDefault();

            
            if (instructor == null)
            {
                return HttpNotFound();
            }

            PopulateAssignedCourseData(instructor);   //mporei na paei k ston instructor kai sta courses

            //ViewBag.ID = new SelectList(db.OfficeAssignments, "InstructorID", "Location", instructor.ID);
            return View(instructor);
        }

        private void PopulateAssignedCourseData(Instructor instructor)
        {
            // Find courses
            var allCourses = db.Courses;
            //Find Instructor Courses
            var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.CourseID));
            //HashSet----List with no duplicate elements and unorder----kalytero performance
            //https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1?view=netframework-4.7.2

            //Fill assignedCourseData ViewModel
            var viewModel = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.CourseID)
                });
            }
            //Give viewModel To View (ViewBag)
            ViewBag.Courses = viewModel;                
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedCourses)     //selected courses logw tou oti etsi onomazetai sto view---erxetai apto view
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var instructorToUpdate = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)            //kouvalei ta courses pou eixe 
                .Where(i => i.ID == id)
                .Single();
            
            //Tha mporouse na ginei elegxos gia ton instructorToUpdate if its NULL

            if (TryUpdateModel(instructorToUpdate, "",          //elenxei thn Whitelist
                new string[] { "LastName", "FirstName", "HireDate", "OfficeAssignment" }))
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment.Location))
                    {
                        //trigarei to entity oti tha markaristei ws state deleted
                        //intructorToUpdate---updated
                        //officeassigment---deleted
                        instructorToUpdate.OfficeAssignment = null;
                    }

                    // Update Instructor Courses
                    UpdateInstructorCourses(instructorToUpdate, selectedCourses);

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Unable to Save Changes");
                }
            }

            PopulateAssignedCourseData(instructorToUpdate);
            return View(instructorToUpdate);    //an exei exception tha trexei ayth
        }

        private void UpdateInstructorCourses(Instructor instructorToUpdate, string[] selectedCourses)
        {
            //Check if selected courses is NULL
            if (selectedCourses == null)
            {
                instructorToUpdate.Courses = new List<Course>();
                return;
            }

            //Maybe Im gonna need InstructorsCourses
            var selectedCoursesHS = new HashSet<string>(selectedCourses);//this is the list that comes from the view
            var instructorsCourses = new HashSet<int>(instructorToUpdate.Courses.Select(c => c.CourseID));//this is the list that comes from DB
            //Loop all courses
            //1.IF selectedCourses Contains specific Course
            //2.If InstructorCourses NOT Contains spesific Course
            //3.add it to Instructor
            foreach (var course in db.Courses)
            {
                //mporei na ginei me &&
                if (selectedCoursesHS.Contains(course.CourseID.ToString()))     //epidh mas stelnei to view value to courseID
                {
                    if (!instructorsCourses.Contains(course.CourseID))
                    {
                        instructorToUpdate.Courses.Add(course);
                    }
                }
                //ELSE
                // If InstructorCourses Contains spesific Course
                //Remove Course
                else
                {
                    if (instructorsCourses.Contains(course.CourseID))
                    {
                        instructorToUpdate.Courses.Remove(course);
                    }
                }
            }

            
        }

        // GET: Instructors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Instructor instructor = db.Instructors.Find(id);
            Instructor instructor = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Where(i => i.ID == id)
                .Single();

            db.Instructors.Remove(instructor);

            var department = db.Departments
                .Where(d => d.InstructorID == id)
                .SingleOrDefault();

            if (department != null)
                department.InstructorID = null;

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
