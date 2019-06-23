using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using XmlTest.Models;

namespace XmlTest.Helper
{
    public class XmlHelper
    {
        #region Load XML doc
        public static XDocument GetXmlDoc()
        {
            try
            {
                XDocument myDoc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath("~/Customer.xml"));
                return myDoc;
            }
            catch (System.IO.FileNotFoundException ex)
            {
                return null;
            }
        }
        #endregion

        public static List<Customer> GetXmlData()
        {
            List<Customer> listCs = new List<Customer>();
            XDocument myDoc = XmlHelper.GetXmlDoc();
            if (myDoc != null)
            {
                var query = from c in myDoc.Descendants("Customer")
                            select new Customer
                            {
                                ID=c.FirstAttribute.Value,
                                UseName = c.Element("Usename").Value,
                                FirstName = c.Element("FirstName").Value,
                                LastName = c.Element("LastName").Value,
                                Phone = c.Element("PhoneNo").Value
                            };
                foreach (var item in query)
                {
                    listCs.Add(item);
                }
            }
            return listCs;
        }

        public static void DeleteElement(string id)
        {
            try
            {
                XDocument myDoc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath("~/Customer.xml"));
                IEnumerable<XElement> customer = from c in myDoc.Descendants("Customer") where c.FirstAttribute.Value == id select c;
                if (customer.Count() > 0)
                {
                    customer.First().Remove();
                }
                myDoc.Save(HttpContext.Current.Server.MapPath("~/Customer.xml"));
            }
            catch (Exception ex)
            {
                //logger.Info(ex.Message + "  " + DateTime.Now.ToString());
            }

        }

        #region Insert new item
        public static void InsertNewElement(Customer cs)
        {
            try
            {
                // Load current document. 
                XDocument myDoc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath("~/Customer.xml"));
                Random r = new Random();
                // add new element 
                XElement newElement = new XElement("Customer",new XAttribute("ID", r.Next(5000)),
                          new XElement("Usename", cs.UseName),
                          new XElement("FirstName", cs.FirstName),
                          new XElement("LastName", cs.LastName),
                          new XElement("PhoneNo", cs.Phone));

                myDoc.Descendants("Customers").First().Add(newElement);
                // Save changes.
                myDoc.Save(System.Web.HttpContext.Current.Server.MapPath("~/Customer.xml"));
            }
            catch (Exception ex)
            {
                
            }

        }
        #endregion

        public static void EditElement(Customer cs)
        {
            try
            {
                // Load current document. 
                XDocument myDoc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath("~/Customer.xml"));

                IEnumerable<XElement> products = from c in myDoc.Descendants("Customer") where c.FirstAttribute.Value == cs.ID select c;
                if (products.Count() > 0)
                {
                    XElement product = products.First();
                    //replace xml element
                    product.ReplaceNodes( new XElement("Usename", cs.UseName), new XElement("FirstName", cs.FirstName), new XElement("LastName", cs.LastName), new XElement("PhoneNo", cs.Phone));
                }
                myDoc.Save(HttpContext.Current.Server.MapPath("~/Customer.xml"));
            }
            catch (Exception ex)
            {
                
            }

        }

        #region Query customer
        public static Customer FindCustomerByID(string id)
        {
            try
            {
                Customer cs = new Customer();
                // Load current document. 
                XDocument myDoc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath("~/Customer.xml"));

                var query = from c in myDoc.Root.Elements("Customer")
                            where (string)c.FirstAttribute.Value == id
                            select new Customer
                            {
                                ID = c.FirstAttribute.Value,
                                UseName = c.Element("Usename").Value,
                                FirstName = c.Element("FirstName").Value,
                                LastName = c.Element("LastName").Value,
                                Phone = c.Element("PhoneNo").Value
                            };

                foreach (var item in query)
                {
                    cs.ID = item.ID;
                    cs.UseName = item.UseName;
                    cs.FirstName = item.FirstName;
                    cs.LastName = item.LastName;
                    cs.Phone = item.Phone;
                }
                return cs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

    }
}