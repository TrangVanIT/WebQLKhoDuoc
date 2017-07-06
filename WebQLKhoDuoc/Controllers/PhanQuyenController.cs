using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebQLKhoDuoc.Models;
using WebQLKhoDuoc.Context;

namespace WebQLKhoDuoc.Controllers
{
    [WebQLKhoDuoc.Models.Authorization]
    public class PhanQuyenController : Controller
    {
        private QLKhoDuocContext db = new QLKhoDuocContext();

        // GET: /PhanQuyen/
        public ActionResult Index(string id)
        {
            var phanquyens = db.PhanQuyens.Where(x=>x.MaDMQuyen==id);
            return View(phanquyens.ToList());
        }

        // GET: /PhanQuyen/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanQuyen phanquyen = db.PhanQuyens.Find(id);
            if (phanquyen == null)
            {
                return HttpNotFound();
            }
            return View(phanquyen);
        }

        // GET: /PhanQuyen/Create
        public ActionResult Create()
        {
            ViewBag.MaDMQuyen = new SelectList(db.DanhMucQuyens, "MaDMQuyen", "TenDMQuyen");
            return View();
        }

        // POST: /PhanQuyen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="MaQuyen,TenQuyen,Mota,MaDMQuyen")] PhanQuyen phanquyen)
        {
            if (ModelState.IsValid)
            {
                db.PhanQuyens.Add(phanquyen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaDMQuyen = new SelectList(db.DanhMucQuyens, "MaDMQuyen", "TenDMQuyen", phanquyen.MaDMQuyen);
            return View(phanquyen);
        }

        // GET: /PhanQuyen/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanQuyen phanquyen = db.PhanQuyens.Find(id);
            if (phanquyen == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDMQuyen = new SelectList(db.DanhMucQuyens, "MaDMQuyen", "TenDMQuyen", phanquyen.MaDMQuyen);
            return View(phanquyen);
        }

        // POST: /PhanQuyen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaQuyen,TenQuyen,Mota,MaDMQuyen")] PhanQuyen phanquyen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phanquyen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id=phanquyen.MaDMQuyen});
            }
            ViewBag.MaDMQuyen = new SelectList(db.DanhMucQuyens, "MaDMQuyen", "TenDMQuyen", phanquyen.MaDMQuyen);
            return View(phanquyen);
        }

        // GET: /PhanQuyen/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanQuyen phanquyen = db.PhanQuyens.Find(id);
            if (phanquyen == null)
            {
                return HttpNotFound();
            }
            return View(phanquyen);
        }

        // POST: /PhanQuyen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhanQuyen phanquyen = db.PhanQuyens.Find(id);
            db.PhanQuyens.Remove(phanquyen);
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
