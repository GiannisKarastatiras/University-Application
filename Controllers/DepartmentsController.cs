using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BootCampApp.DAL;
using BootCampApp.Models;
using System.Data.Entity.Infrastructure;

namespace BootCampApp.Controllers
{
    public class DepartmentsController : Controller
    {
        private BootCampDbContext db = new BootCampDbContext();

        // GET: Departments
        public async Task<ActionResult> Index() //opou feugei request mpainei to async
        {
            var departments = db.Departments.Include(d => d.Administrator); //kataskeuazei ton tropo pou tha parei ta departments
            return View(await departments.ToListAsync());   //edw ekteleite --- otan tha trexei to tolistasync tote tha parw ta departments
            //await--signal--perimene ta xwrizei se dyo kommatia
            //tha ta dwsei otan tha ta exei---den tha dwsei ena timeout opws sto allo--den tha dwsei pote an den ta parei ola ta  departments
        }

        // GET: Departments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await db.Departments.FindAsync(id); //tha perimenei to department na ektelestei kai meta tha proxwrhsei o kwdikas
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DepartmentID,Name,Budget,StartDate,InstructorID")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await db.Departments.FindAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "DepartmentID,Name,Budget,StartDate,InstructorID")] Department department)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(department).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", department.InstructorID);
        //    return View(department);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {
            //check id null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //These are the fields to sent To TryToUpdateModel
            string[] fieldsToBind = new string[] { "Name", "Budget", "StartDate", "InstructorID", "RowVersion" };

            // Go find the department to update
            var departmentToUpdate = await db.Departments.FindAsync(id);
            //Check if department has already been deleted
            if (departmentToUpdate == null)
            {
                // Stay here, show a message, and values that you wanted to update
                Department deletedDepartment = new Department();
                TryUpdateModel(deletedDepartment, fieldsToBind);
                ModelState.AddModelError(string.Empty, "Unable to save changes. The department was deleted.");
                ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", deletedDepartment.InstructorID);

                return View(deletedDepartment);
            }
            // Department exists
            if (TryUpdateModel(departmentToUpdate, fieldsToBind))
            {
                // Normal update ... no one has changed my DB record
                try
                {
                    db.Entry(departmentToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    //Find Client Values
                    var entry = ex.Entries.Single();
                    var clientValues = (Department)entry.Entity;
                    // Find DB Values
                    var databaseEntry = entry.GetDatabaseValues();
                    //Do I need to check for null in DB?
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to Save Changes. Department was deleted.");
                    }
                    // Compare values and show what we have on DB if we have difference
                    else
                    {
                        var databaseValues = (Department)databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                        {
                            ModelState.AddModelError("Name", "Current Value: " + databaseValues.Name);
                        }
                        if (databaseValues.Budget != clientValues.Budget)
                        {
                            ModelState.AddModelError("Budget", "Current Value: " + databaseValues.Budget);
                        }
                        if (databaseValues.StartDate != clientValues.StartDate)
                        {
                            ModelState.AddModelError("StartDate", "Current Value: " + databaseValues.StartDate);
                        }
                        if (databaseValues.InstructorID != clientValues.InstructorID)
                        {
                            ModelState.AddModelError("InstructorID", "Current Value: " + db.Instructors.Find(databaseValues.InstructorID).FullName);
                        }
                        //Set RowVersion to the right state
                        departmentToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }   
            }

            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", departmentToUpdate.InstructorID);
            return View(departmentToUpdate);
        }

        // GET: Departments/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await db.Departments.FindAsync(id);
            if (department == null)
            {
                // 
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction("Index");
                }
                return HttpNotFound();
            }

            //Code for concurrency
            if (concurrencyError.GetValueOrDefault())
            {
                ViewBag.ConcurrencyMessage = "The Department you try to delete has changed."
                    + "Delete operation has cancelled.";
            }
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Department department)
        {
            
            // Normal delete of a department
            try
            {
                db.Entry(department).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            // Concurrency exception
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { id = department.DepartmentID, concurrencyError = true });                   
            }
            //Any other exception
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unable Delete Department");
                return View(department);
            }
            
            
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
