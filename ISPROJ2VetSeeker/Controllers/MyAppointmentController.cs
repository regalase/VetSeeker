using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISPROJ2VetSeeker.App_Code;
using ISPROJ2VetSeeker.Models;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace ISPROJ2VetSeeker.Controllers
{
    public class MyAppointmentController : Controller
    {
        // GET: MyAppointment
        public ActionResult ClinicSchedules()
        {
            var record = new List<ClinicModel>();
            //query clinics by user logged in;
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                //Change later with claculations to nearest clinic
                string query = @"SELECT clinicId, userId, unitHouseNo, street, baranggay, city, clinicname, longitude, latitude FROM Clinic ORDER BY longitude, latitude DESC";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    //Add long lat params from gmaps
                    //sqlCmd.Parameters.AddWithValue("@userID", 13); //replace with user session
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                var clinic = new ClinicModel();
                                clinic.ClinicID = int.Parse(sqlDr["clinicID"].ToString());
                                clinic.UserID = int.Parse(sqlDr["userID"].ToString());
                                clinic.UnitHouseNo = sqlDr["unitHouseNo"].ToString();
                                clinic.Street = sqlDr["street"].ToString();
                                clinic.Baranggay = sqlDr["baranggay"].ToString();
                                clinic.City = sqlDr["city"].ToString();
                                clinic.ClinicName = sqlDr["clinicname"].ToString();
                                record.Add(clinic);
                            }
                        }
                    }
                }

            }
            return View(record);
        }

        [HttpPost]
        public ActionResult ClinicSchedules(ClinicModel record)
        {
            var clinicSchedules = new ClinicScheduleUIModel();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                string query = @"SELECT scheduleID, s.userID, date, status, s.clinicID FROM Schedule s WHERE s.clinicID = @clinicID";

                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@clinicID", record.ClinicID);
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                var ScheduleModel = new ScheduleModel();
                                ScheduleModel.ScheduleID = int.Parse(sqlDr["scheduleID"].ToString());
                                ScheduleModel.UserID = int.Parse(sqlDr["userID"].ToString());
                                ScheduleModel.Date = DateTime.Parse(sqlDr["date"].ToString());
                                ScheduleModel.ClinicID = int.Parse(sqlDr["clinicID"].ToString());
                                clinicSchedules.ScheduleModels.Add(ScheduleModel);
                            }
                        }
                    }
                }
            }
            //Change with return to another view, with the ClinicScheduleUIModel
            return RedirectToAction("BookAppointment", record);
        }

        public ActionResult BookAppointment(ClinicScheduleUIModel record)
        {
            return View(record);
        }

        /*
        [HttpPost]
        public ActionResult BookAppointment(ClinicScheduleUIModel record)
        {
            //Save the appointment + Schedule, refer to query.
            return View(record);
        }*/
    }
}