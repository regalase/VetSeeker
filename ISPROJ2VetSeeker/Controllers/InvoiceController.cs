using ISPROJ2VetSeeker.App_Code;
using ISPROJ2VetSeeker.Models;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Diagnostics;
using GMap;

namespace ISPROJ2VetSeeker.Controllers
{
    public class InvoiceController : Controller
    {
        // GET: Invoice
        public ActionResult Invoices()
        {
            List<InvoiceModel> Invoices = new List<InvoiceModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string getInvoicesQuery = @"SELECT i.invoiceID, i.myAppointmentID, i.medicalHistoryID, i.professionalFee, i.date, i.totalPrice FROM Invoice i INNER JOIN MyAppointment m on m.myAppointmentID = i.myAppointmentID WHERE m.userID = @userID";
                using (SqlCommand sqlCmd = new SqlCommand(getInvoicesQuery, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY].ToString());
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                InvoiceModel invoiceModel = new InvoiceModel();
                                invoiceModel.InvoiceID = int.Parse(sqlDr["invoiceID"].ToString());
                                invoiceModel.MyAppointmentID = int.Parse(sqlDr["myAppointmentID"].ToString());
                                invoiceModel.MedicalHistoryID = int.Parse(sqlDr["medicalHistoryID"].ToString());
                                invoiceModel.ProfessionalFee = decimal.Parse(sqlDr["professionalFee"].ToString());
                                invoiceModel.Date = DateTime.Parse(sqlDr["date"].ToString());
                                invoiceModel.TotalPrice = decimal.Parse(sqlDr["totalPrice"].ToString());
                                Invoices.Add(invoiceModel);
                            }
                        }
                    }
                }
                sqlCon.Close();
            }
            return View(Invoices);
        }

        public ActionResult VetInvoices()
        {
            List<InvoiceModel> Invoices = new List<InvoiceModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string getVetInvoicesQuery = @"SELECT i.invoiceID, i.myAppointmentID, i.medicalHistoryID, i.professionalFee, i.date, i.totalPrice 
                                             FROM Invoice i 
                                             INNER JOIN MyAppointment m on m.myAppointmentID = i.myAppointmentID 
                                             INNER JOIN Schedule s on s.scheduleID = i.scheduleID
                                             WHERE s.userID = @userID";
                using (SqlCommand sqlCmd = new SqlCommand(getVetInvoicesQuery, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY].ToString());
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                InvoiceModel invoiceModel = new InvoiceModel();
                                invoiceModel.InvoiceID = int.Parse(sqlDr["invoiceID"].ToString());
                                invoiceModel.MyAppointmentID = int.Parse(sqlDr["myAppointmentID"].ToString());
                                invoiceModel.MedicalHistoryID = int.Parse(sqlDr["medicalHistoryID"].ToString());
                                invoiceModel.ProfessionalFee = decimal.Parse(sqlDr["professionalFee"].ToString());
                                invoiceModel.Date = DateTime.Parse(sqlDr["date"].ToString());
                                invoiceModel.TotalPrice = decimal.Parse(sqlDr["totalPrice"].ToString());
                                Invoices.Add(invoiceModel);
                            }
                        }
                    }
                }
                sqlCon.Close();
            }
                return View(Invoices);
        }
    }
}