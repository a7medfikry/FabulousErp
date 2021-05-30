using FabulousDB.DB_Context;
using FabulousDB.Models;
using FabulousDB.Models.Attachment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers
{
    public class UploadFileController : Controller
    {
        public PartialViewResult UploadFile(string Title="")
        {
            ViewBag.Title = Title;
            return PartialView();
        } 
        public PartialViewResult GeneralFileUpload(string Title="")
        {
            ViewBag.Title = Title;
            return PartialView();
        }
        DBContext db = new DBContext();
        //Get All Images 
        public ActionResult GetAllImages(string Page, int? Po, int? Relation_id, string FileKey = null)
        {
            List<ImageUpload> Other = new List<ImageUpload>();
            if (string.IsNullOrWhiteSpace(FileKey))
            {
                FileKey = null;
            }
            foreach (Attachment_files i in db.Attachment_head.Where(x => /*x.Page == Page &&*/ x.C_PostingNumber == Po&&x.Relation_id==Relation_id/*&&x.Attachment_files.Any(z=>z.File_key== FileKey)*/).SelectMany(x => x.Attachment_files).ToList())
            {
                string Thumb = i.File;
                if (!IsImage(i.File))
                {
                    Thumb = "/Images/File.png";
                }
                Other.Add(new ImageUpload { url = i.File, Id = i.Id, thumbnailUrl = Thumb, name = i.File.Replace("/UploadedFiles/",""), type = "", deleteUrl = $"/UploadFile/DeleteFile/{i.Id}", deleteType = "POST", updateType = "POST", updateUrl = $"" });
            }
            return Json(new ImageUploadFile { files = Other }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInvImage(int ItemId)
        {
            List<ImageUpload> Other = new List<ImageUpload>();
            try
            {
                foreach (Inv_item_option i in db.Inv_item_option.Where(x => x.Inv_item_id == ItemId).ToList())
                {

                    Other.Add(new ImageUpload
                    {
                        url = i.Image,
                        thumbnailUrl = i.Image,
                        name = i.Image.Replace("/InvItems/", ""),
                        type = "",
                        deleteUrl = $"/UploadFile/DeleteInveImage/{i.Id}",
                        deleteType = "POST",
                        updateType = "POST",
                        updateUrl = $""
                    });
                }
                return Json(new ImageUploadFile { files = Other }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new ImageUploadFile { files = new List<ImageUpload> { } }, JsonRequestBehavior.AllowGet);

            }

        }


        //Upload New Image 
        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public ActionResult Create(Attachment_files File,string Page,int? PO,int? Relation_id, string FileKey)
        {
            if (ModelState.IsValid)
            {
                Page = string.Join("",Page.Where(x => (x < '0' || x > '9'))).Replace("Create","")
                    .Replace("Edit","").Replace("Details","").Replace("Index","");
                while (Page.LastOrDefault() == '/')
                {
                    Page=Page.Remove(Page.Length-1,1);
                }
                if (PO < 0)
                {
                    PO = null;
                }
                Attachment_head Head = db.Attachment_head.FirstOrDefault(x =>x.Relation_id== Relation_id&& x.Page == Page && x.C_PostingNumber == PO);

                if (Head==null)
                {
                    db.Attachment_head.Add(new Attachment_head
                    {
                        Page=Page,
                        C_PostingNumber= PO,
                        Relation_id= Relation_id
                    });
                    db.SaveChanges();
                    Head = db.Attachment_head.FirstOrDefault(x => x.Relation_id == Relation_id && x.Page == Page && x.C_PostingNumber == PO);
                }
                HttpPostedFileBase file = Request.Files[0];
                string filename = (file.FileName);
                File.File = filename;
                File.Attachment_id = Head.Id;
                File.File_key = FileKey;
                db.Attachment_files.Add(File);
                db.SaveChanges();
                UploadImage(File);
                Attachment_files ThisCat = db.Attachment_files.Find(File.Id);
                return Json(new ImageUploadFile { files = new List<ImageUpload> { new ImageUpload { url = ThisCat.File, thumbnailUrl = ThisCat.File, name = ThisCat.File, type = "image/jpeg", deleteUrl = $"/Admin/OtherImages/DeleteImage/{ThisCat.Id}", deleteType = "POST", updateType = "POST", updateUrl = $"/Admin/Hotel_rooms_images/edit/{ThisCat.Id}" } } });
            }

            return View(File);
        }

        bool IsImage(string FilePath)
        {
            string FileExt = System.IO.Path.GetExtension(FilePath).ToLower();
            if (FileExt == ".jpg" && FileExt != ".png" && FileExt != ".gif" && FileExt != ".jpeg")
            {
                return true;
            }
            return false;
        }
        //Save Image
        public ActionResult UploadImage( Attachment_files category)
        {
            try
            {
                Attachment_files ThisCat = db.Attachment_files.Find(category.Id);

                HttpPostedFileBase file = Request.Files[0];
                string filename = (file.FileName);
                string fileExtension = System.IO.Path.GetExtension(file.FileName);
               
                string savepath = Server.MapPath("/UploadedFiles");

                // filename = filename.Trim(fileExtension.ToCharArray()) + fileExtension;
                // If Image Name Is Dublicated
                if (System.IO.File.Exists(savepath + @"\" + filename))
                {
                    int counter = 1;
                    string FileName = System.IO.Path.GetFileNameWithoutExtension(filename) + counter + fileExtension;
                    while (System.IO.File.Exists(savepath + @"\" + FileName))
                    {
                        // if a file with this name already exists,
                        // prefix the filename with a number.
                        counter++;
                        FileName = System.IO.Path.GetFileNameWithoutExtension(filename) + counter + fileExtension;

                    }
                    filename = FileName;
                }
                file.SaveAs(savepath + @"\" + filename);
                ThisCat.File = "/UploadedFiles/" + filename;
                db.SaveChanges();
                return null;
            }
            catch
            {
                return Json(0);
            }
        }
         
        public ActionResult UploadInvItemImage(int ItemId)
        {
            try
            {
                Inv_item_option ThisItem = db.Inv_item_option.FirstOrDefault(x => x.Inv_item_id == ItemId);
                HttpPostedFileBase file = Request.Files[0];
                string filename = (file.FileName);
                string fileExtension = System.IO.Path.GetExtension(file.FileName);
               
                string savepath = Server.MapPath("/InvItems");

                // filename = filename.Trim(fileExtension.ToCharArray()) + fileExtension;
                // If Image Name Is Dublicated
                if (System.IO.File.Exists(savepath + @"\" + filename))
                {
                    int counter = 1;
                    string FileName = System.IO.Path.GetFileNameWithoutExtension(filename) + counter + fileExtension;
                    while (System.IO.File.Exists(savepath + @"\" + FileName))
                    {
                        // if a file with this name already exists,
                        // prefix the filename with a number.
                        counter++;
                        FileName = System.IO.Path.GetFileNameWithoutExtension(filename) + counter + fileExtension;

                    }
                    filename = FileName;
                }
                file.SaveAs(savepath + @"\" + filename);
                ThisItem.Image = "/InvItems/" + filename;
                db.SaveChanges();
                return Json(1);
            }
            catch
            {
                return Json(0);
            }
        }


        //Delete Image 
        public ActionResult DeleteFile(int Id)
        {
            Attachment_files ThisOther = db.Attachment_files.Find(Id);
            if (System.IO.File.Exists(Server.MapPath(ThisOther.File)))
            {
                System.IO.File.Delete(Server.MapPath(ThisOther.File));
            }
            db.Attachment_files.Remove(ThisOther);
            db.SaveChanges();
            return Json(1);
        } 
        public ActionResult DeleteInveImage(int Id)
        {
            Inv_item_option ThisOption = db.Inv_item_option.Find(Id);
            if (System.IO.File.Exists(Server.MapPath(ThisOption.Image)))
            {
                System.IO.File.Delete(Server.MapPath(ThisOption.Image));
            }
            ThisOption.Image = "";
            db.SaveChanges();
            return Json(1);
        }

        //Image Upload Class Class 
        class ImageUploadFile
        {
            public List<ImageUpload> files { get; set; }
        }
        class ImageUpload
        {
            public int Id { get; set; }
            public string url { get; set; }
            public string thumbnailUrl { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string deleteUrl { get; set; }
            public string deleteType { get; set; }
            public string updateType { get; set; }
            public string updateUrl { get; set; }
            public string Image_alt { get; set; }
            public string Image_alt_en { get; set; }
        }
    }
}