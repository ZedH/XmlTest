using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XmlTest.Helper;
using XmlTest.Models;

namespace XmlTest.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            List<Customer> listCs = new List<Customer>();
            listCs = XmlHelper.GetXmlData();
            return View(listCs);
        }

        public ActionResult Create()
        {
            return View(new Customer());
        }

        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            XmlHelper.InsertNewElement(customer);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            Customer cs = new Customer();
            cs = XmlHelper.FindCustomerByID(id.ToString());
            return View(cs);
        }

        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            XmlHelper.EditElement(customer);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            XmlHelper.DeleteElement(id.ToString());
            return RedirectToAction("Index");
        }
    }
}