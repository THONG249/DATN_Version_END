using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Context;
using static WebBanHang.Common;

namespace WebBanHang.Controllers
{
   
    public class TinhTrangGiaoHangController : Controller
    {
        TNSTOREEntities objTNStoreEntity = new TNSTOREEntities();
        // GET: TinhTrangGiaoHang
        public ActionResult Index()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                int id = int.Parse(Session["Id"].ToString());
                var lisOrder = objTNStoreEntity.Orders.Where(n => n.UserId == id).ToList();
                if(lisOrder.Count == 0)
                {
                    ViewBag.Tb = "Bạn chưa đặt hàng";
                }

                return View(lisOrder);
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            this.LoadData();
            var obj = objTNStoreEntity.Orders.Where(n => n.Id == id).FirstOrDefault();
            return View(obj);
            
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Order obj)
        {
            this.LoadData();
            obj.UserId = (int)Session["Id"];
            objTNStoreEntity.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            objTNStoreEntity.SaveChanges();
            return RedirectToAction("Index");
        }
        void LoadData()
        {
            Common objCommon = new Common();
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            List<Status> lstStatus = new List<Status>();
            Status objType = new Status();
            objType.Id = 1;
            objType.Name = "Chờ giao hàng";
            lstStatus.Add(objType);

            objType = new Status();
            objType.Id = 2;
            objType.Name = "Đã nhận hàng";
            lstStatus.Add(objType);

            DataTable dtProductType = converter.ToDataTable(lstStatus);
            ViewBag.ProductType = objCommon.ToSelectList(dtProductType, "Id", "Name");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (objTNStoreEntity != null)

                    objTNStoreEntity.Dispose();
                objTNStoreEntity.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}