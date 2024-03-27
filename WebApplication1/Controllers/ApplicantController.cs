using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace Master_Detail.Controllers
{
    public class ApplicantController : Controller
    {
        readonly Retac_Master_DEntities db = new Retac_Master_DEntities();
        // GET: Applicant
        public ActionResult Index()
        {
            return View(db.Applicants.ToList());
        }
        public ActionResult Create()
        {
            Applicant applicant = new Applicant();
            applicant.ApplicantExes.Add(new ApplicantEx
            {
                CompanyName = "",
                Designation = "",
                YearOfEx = 0
            });

            return View(applicant);
        }
        [HttpPost]
        public ActionResult Create(Applicant applicant, string btn)
        {
            if (btn == "Add")
            {
                applicant.ApplicantExes.Add(new ApplicantEx());
            }
            if (btn == "Create")
            {
                if (applicant.Picture != null)
                {
                    string ext = Path.GetExtension(applicant.Picture.FileName);
                    if (ext == ".jpg" || ext == ".png")
                    {
                        string rootpath = Server.MapPath("~/");
                        string fileTosave = Path.Combine(rootpath, "Pictures", applicant.Picture.FileName);
                        applicant.Picture.SaveAs(fileTosave);
                        applicant.PicPath = "~/Pictures/" + applicant.Picture.FileName;

                        applicant.TotalEx = applicant.ApplicantExes.Sum(s => s.YearOfEx);
                        db.Applicants.Add(applicant);
                        if (db.SaveChanges() > 0)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Please provide valid Image");
                        return View(applicant);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please provide valid Image");
                    return View(applicant);
                }
            }
            return View(applicant);
        }

        public ActionResult Edit(int id)
        {
            Applicant applicant = db.Applicants.Find(id);


            return View(applicant);
        }
        [HttpPost]
        public ActionResult Edit(Applicant applicant, string btn)
        {
            var updateApp = db.Applicants.Find(applicant.Id);
            if (btn == "Add")
            {
                updateApp.ApplicantExes.Add(new ApplicantEx());
            }
            if (btn == "Update")
            {

                if (applicant.Picture != null)
                {
                    string ext = Path.GetExtension(applicant.Picture.FileName);
                    if (ext == ".jpg" || ext == ".png")
                    {
                        string rootpath = Server.MapPath("~/");
                        string fileTosave = Path.Combine(rootpath, "Pictures", applicant.Picture.FileName);
                        applicant.Picture.SaveAs(fileTosave);
                        updateApp.PicPath = "~/Pictures/" + applicant.Picture.FileName;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Please provide valid Image");
                        return View(applicant);
                    }
                }
                else
                {
                    updateApp.PicPath = applicant.PicPath;
                }

                updateApp.DOB = applicant.DOB;
                updateApp.Id = applicant.Id;
                updateApp.Name = applicant.Name;
                updateApp.IsAvailable = applicant.IsAvailable;

                db.ApplicantExes.RemoveRange(db.ApplicantExes.Where(a => a.AppId.Equals(updateApp.Id)));
                db.SaveChanges();
                updateApp.ApplicantExes = applicant.ApplicantExes;
                updateApp.TotalEx = applicant.ApplicantExes.Sum(s => s.YearOfEx);
                db.Entry(updateApp).State = System.Data.Entity.EntityState.Modified;

                if (db.SaveChanges() > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(updateApp);
        }
        public ActionResult Delete(int id)
        {
            db.Applicants.Remove(db.Applicants.Find(id));

            db.SaveChanges();

            return RedirectToAction("Index");

        }
    }
}