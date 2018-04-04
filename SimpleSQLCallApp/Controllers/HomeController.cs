using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.IO;

namespace SimpleSQLCallApp.Controllers
{

   /* public System.Security.Cryptography.X509Certificates.X509Certificate2 GetAPICred_AdminCert()
    {
       
    }*/

    public class HomeController : Controller
    {

        public string ConvertNowToTimeZone(string timeZone)

        {

            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZone);


            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi).ToString();

        }


        public ActionResult Index()
        {


            try
            {

                string certfile = System.IO.Path.Combine(Environment.ExpandEnvironmentVariables("%HOME%"), @"site\wwwroot\BrooksjcCert2017.pfx");
                //string certfile = System.IO.Path.Combine(@".\BrooksjcCert2017.pfx");
                X509Certificate2 cert = new X509Certificate2(certfile, "Pxxxrd!", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
                //return cert; 
                ViewBag.Message= cert.FriendlyName.ToString();

            }
            catch (Exception e)
            {
                ViewBag.Message(e.Message);
            }

            /*foreach(var timeZone = TimeZoneInfo.GetSystemTimeZones())
            {
                ConvertNowToTimeZone(timeZone);
            }*/

            /* if (System.IO.File.Exists("~/BrooksjcCert2017.pfx"))
             {
                 try
                 {

                     var cert = new X509Certificate2(@"BrooksjcCert2017.pfx", "xxx!");
                     var certBytes = cert.RawData;
                     var certString = Convert.ToBase64String(certBytes);

                 }
                 catch (Exception e)
                 {
                     ViewBag.Message(e.Message);
                 }
             }
             else ViewBag.Message("Doesn't Exists");*/

            //string certPath = "BrooksjcCert2017.pfx";
            //string certPass = "Pxxx!";

            // Create a collection object and populate it using the PFX file
            // X509Certificate2Collection collection = new X509Certificate2Collection();
            // collection.Import(certPath, certPass, X509KeyStorageFlags.PersistKeySet);

            /*X509Certificate2 cert = null;
            string certText = GetString(ConfigFields.CREDENTIALS_CERTFILE); // cert file encoded as a base64 string 
            string certSecret = _configService.GetString(ConfigFields.CREDENTIALS_SECRET);
            byte[] certBytes = Convert.FromBase64String(certText);
            try
            {
                cert = new X509Certificate2(certBytes, certSecret, X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            }
            catch (Exception ex)
            {
                ViewBag.Message(string.Format("Could not load cert." ));//(instanceId={0})", InstanceId), ex);
            }*/


            //ViewBag.Message = "HI";

            return View();
        }

        public ActionResult About()
        {
            // var conn = "value";
            var connStr = $"server";
            if (ConfigurationManager.ConnectionStrings["myConnString"] == null)
            {
                connStr = $"Server=tcp:xxx.database.windows.net,1433;Initial Catalog=xxxx;Persist Security Info=False;User ID=xxx;Password=xxxx;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=5;";
               //connStr = new SqlConnection(conn);
                ViewBag.Message = "Using manually inputed connection string...";
            }
            else
            {
                connStr = ConfigurationManager.ConnectionStrings["myConnString"].ConnectionString;
                ViewBag.Message = "Using app setting for the connection string...";
            }

           // var connStr = $"Server=tcp:jebrook-webappwithdatabasedbserver.database.windows.net,1433;Initial Catalog=jebrook-webappwithdatabase_db;Persist Security Info=False;User ID=jebrook;Password=Pa$$w0rd!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            try   //TESTING THE CONNNECTION STRING that it is not null
            {
                //string connStr1 = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            }
            catch(Exception e)
            {
               
                ViewBag.Message = e.ToString();
                return View();
            }

            //string connStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            {
                
               
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                   
                    try
                    {
                        connection.Open();   ///TEST opening the connection to the web app 
                    }

                    catch (Exception e)
                    {
                        ViewBag.Message = (e.ToString() +" " +  connection.DataSource + " "+ connection.Database);
                        return View();
                    }

                }
            }
            ViewBag.Message = "You are conntected to your DB";   //Successfully created a connection to the DB

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            // string labelMachineKey; 
            try
            {
                HttpCookie protectCookie = Request.Cookies["MCENCRYPT"];
                if (protectCookie == null)
                {
                    var protectedText = MachineKey.Protect(System.Text.Encoding.ASCII.GetBytes("EncryptThisText"), "ciphertext");
                    var unprotectedText = System.Text.Encoding.ASCII.GetString(MachineKey.Unprotect(protectedText, "ciphertext"));
                    protectCookie = new HttpCookie("MCENCRYPT");
                    protectCookie.Value = Convert.ToBase64String(protectedText);
                    Response.Cookies.Add(protectCookie);

                    ViewBag.Message = $"Proctected TXT-> {Convert.ToBase64String(protectedText)} : " +
                          $"Unprotected TXT -> {unprotectedText} : ProcessID is {Process.GetCurrentProcess().Id} : " +
                          $"Machine name is {Environment.MachineName}";
                }
                else
                {
                    var unprotectedText = System.Text.Encoding.ASCII
                        .GetString(MachineKey.Unprotect(Convert.FromBase64String(protectCookie.Value), "ciphertext"));
                    ViewBag.Message = $"Unprotected TXT-> {unprotectedText} : " +
                        $"ProcessID is {Process.GetCurrentProcess().Id} : Machine name is {Environment.MachineName}";
                }
            }
            catch(Exception e)
            {
                ViewBag.Message = (e.ToString());
                return View();
            }
            Response.Charset = "ISO-8859-1";

            Response.StatusDescription = "åäö";

            //var blah 
            // Response.StatusCode = 400;

            //var star = new HttpStatusCodeResult(HttpStatusCode.OK, "Status: åäö");

            
            //star.
            //return Response;
            
           // Response.ContentEncoding = ""

            //Response.HttpStatusCodeResult(BadRequest, "Status: åäö");
            
           // response.StatusDescription = "Status: åäö";
           // return response;
            return View();
            
        }
    }
}