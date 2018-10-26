using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using POETraderWeb.Models;
using System.Threading.Tasks;

namespace POETraderWeb.Controllers
{
    public class poe_itemController : Controller
    {
        private poe_traderEntities db = new poe_traderEntities();

        // GET: poe_item
        public async Task<ActionResult> Index(string itemName)
        {
            var poe_item = from i in db.POE_ITEM select i;
            if (String.IsNullOrEmpty(itemName))
            {
                poe_item = poe_item.OrderByDescending(p => p.UNIQUE_ID);
                poe_item = poe_item.Take(25);
            }
            else
            {
                poe_item = poe_item.Where(p => p.ITEM_NAME.Contains(itemName)).OrderBy(p => p.UNIQUE_ID);
            }
            return View(await (poe_item.ToListAsync()));
        }

        // GET: poe_item/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            POE_ITEM poe_item = db.POE_ITEM.Find(id);
            if (poe_item == null)
            {
                return HttpNotFound();
            }
            return View(poe_item);
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