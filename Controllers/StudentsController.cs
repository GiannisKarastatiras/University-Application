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
using PagedList;

namespace BootCampApp.Controllers
{
    public class StudentsController : Controller
    {
        private BootCampDbContext db = new BootCampDbContext();

        // GET: Students
        //public ActionResult Index(string sortOrder, string searchString)//timh sortOrder oti exei to ViewBag(elegxei thn timh tou sortOrder)
        //{
        //    var students = from s in db.Students
        //                   select s;

        //    ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ? "name_desc" : ""; //Proetoimazetai gia thn epomenh fora to antitheto
        //    ViewBag.DateSortParam = sortOrder == "Date" ? "date_desc" : "Date"; //kerdos sto koumpi pou patietai wste na paizei me tis periptwseis

        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        students = students.Where(s => s.LastName.Contains(searchString)
        //                                  || s.FirstName.Contains(searchString));
        //    }

        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            students = students.OrderByDescending(s => s.LastName);
        //            break;
        //        case "Date":
        //            students = students.OrderBy(s => s.EnrollmentDate);
        //            break;
        //        case "date_desc":
        //            students = students.OrderByDescending(s => s.EnrollmentDate);
        //            break;
        //        default:
        //            students = students.OrderBy(s => s.LastName);
        //            break;
        //    }


        //    return View(students.ToList());
        //}

        //public ActionResult Index(string sortOrder, string searchString)//timh sortOrder oti exei to ViewBag(elegxei thn timh tou sortOrder)
        //{
        //    var students = from s in db.Students
        //                   select s;

        //    ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ? "name_desc" : ""; //Proetoimazetai gia thn epomenh fora to antitheto
        //    ViewBag.DateSortParam = sortOrder == "Date" ? "date_desc" : "Date"; //kerdos sto koumpi pou patietai wste na paizei me tis periptwseis
        //    ViewBag.SearchString = searchString;

        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        students = students.Where(s => s.LastName.Contains(searchString)
        //                                  || s.FirstName.Contains(searchString));
        //    }

        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            students = students.OrderByDescending(s => s.LastName);
        //            break;
        //        case "Date":
        //            students = students.OrderBy(s => s.EnrollmentDate);
        //            break;
        //        case "date_desc":
        //            students = students.OrderByDescending(s => s.EnrollmentDate);
        //            break;
        //        default:
        //            students = students.OrderBy(s => s.LastName);
        //            break;
        //    }


        //    return View(students.ToList());
        //}

        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)//timh sortOrder oti exei to ViewBag(elegxei thn timh tou sortOrder)
        {
            var students = from s in db.Students
                           select s;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ? "name_desc" : ""; //Proetoimazetai gia thn epomenh fora to antitheto
            ViewBag.DateSortParam = sortOrder == "Date" ? "date_desc" : "Date"; //kerdos sto koumpi pou patietai wste na paizei me tis periptwseis
            //ViewBag.SearchString = searchString;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.Contains(searchString)
                                          || s.FirstName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            int pageNumber = (page ?? 1);   //null qoalicing operator ??---an einai null tha to kanei 1 an den einai den ton endiaferei
            int pageSize = 4;

            return View(students.ToPagedList(pageNumber, pageSize));
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id); //Obj pou gemise apo linq, select from students where ID = ...
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]  
        public ActionResult Create([Bind(Include = /*ID,*/"LastName,FirstName,EnrollmentDate")] Student student)
        {//Bind-include--mono auta tha perasoun(whitelist)---security ths method,prostateuw to model mou apo kakovoules energeies(security best practisies)
         //bind one level of defensive programming

            //defensive technic h try
            //neos tropos gia apothhkeysh sthn DB
            try   //breakpoint edw an ftasei edw o kwdikas exw server side validation (opou exw C# einai serverside)
            {                
                if (ModelState.IsValid)
                {
                    db.Students.Add(student);   //to kanei add se mia lista linq
                    // entity elegxei entities kai markarei(ws ADDED) tn student afou ginei add sto epomeno db.saveChenges
                    //An ginei allagh status:modified k sto epomeno SaveChanges 
                    //By default UNCHANGED
                    //Entity status: ADDED,UNCHANGED,MODIFIED,DETACHED(ADIAFORIA APO TO ENTITY),DELETED
                    db.SaveChanges();           //to pernaei sthn DB
                    return RedirectToAction("Index");
                }            
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save Changes");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]  //ActionName dioti allaxa to onoma etsi wste na akousei ws Edit
        [ValidateAntiForgeryToken]  //auto kai sto CreateView (@Html.antiforgerytoken) apagoreuoun tis epitheseis CrossSideScripting kai xss
        public ActionResult EditPost(int? id)
        {
            if (id == null) //Na meinei panta dw autos o elegxos se opoiodhpote refactoring
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var studentToUpdate = db.Students.Find(id); //Phgaine sthn DB kai vres auto to id student
            //Antistoixo if modelstate is valid
            //kanei validation sthn DB
            //tryupdatemodel kanei oti leei
            if (TryUpdateModel(studentToUpdate, "", //2os tropos san thn bind
                new string[] { "LastName", "FirstName", "EnrollmentDate" })) //edw grafetai h whitelist
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Unable to save Changes");
                }
            }


            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]  //auto kai sto CreateView (@Html.antiforgerytoken) apagoreuoun tis epitheseis CrossSideScripting kai xss
        ////public ActionResult Edit([Bind(Include = "ID,LastName,FirstName,EnrollmentDate")] Student student)
        ////{
        ////    if (ModelState.IsValid)
        ////    {
        ////        db.Entry(student).State = EntityState.Modified;
        ////        db.SaveChanges();
        ////        return RedirectToAction("Index");
        ////    }
        ////    return View(student);
        ////}


        // GET: Students/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)//prosthetw mia param etsi wste an erthei apo thn post delete
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())   //an erthei true einai h periptwsh pou erxetai apo thn postDelete sthn eidikh periptwsh
            {
                ViewBag.ErrorMessage = "Delete FAILED";
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {   //old code---paei sthn DB KAI Kanei kanei query(2 fores sthn DB)
                //Student student = db.Students.Find(id);
                //db.Students.Remove(student); ---flaggarei to state tou se DELETED---
                //kai tha ginei delete sthn DB sthn apo katw grammh sthn grammh saveChanges();


                //new code(extra pou egrapsa)
                //to idio me to apo panw kalutera grammeno
                //--petyxainw ton idio skopo--
                //kaluteros tropos
                //paw mia fora sthn DB
                Student studentToDelete = new Student() { ID = id };
                db.Entry(studentToDelete).State = EntityState.Deleted;  

                db.SaveChanges();
                
            }
            catch (DataException)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true }); //gia na tairiaxei me thn getdelete //an den paei kala na xanagyrisei sto DELETE(get)
            }

            return RedirectToAction("Index");   //An ola pane kala Tha epistrepsei sto index me -1 student
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
