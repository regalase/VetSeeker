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
    public class ScheduleController : Controller
    {
        // GET: Schedule
        public ActionResult CreateSchedule()
        {
            var record = new CreateScheduleUIModel();
            //query clinics by user logged in;
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT clinicId, userId, unitHouseNo, street, baranggay, city, clinicname FROM Clinic WHERE userId = @userID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", 13); //replace with user session
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
                                record.ClinicModels.Add(clinic);
                            }
                        }
                    }
                }

            }
            return View(record);
        }

        [HttpPost]
        public ActionResult CreateSchedule(CreateScheduleUIModel record)
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                Debug.WriteLine(record.ScheduleModel.ClinicID);
                Debug.WriteLine(record.ScheduleModel.Date);
                sqlCon.Open();
                string query = @"INSERT INTO Schedule VALUES(@userID, @date, @status, @clinicID);";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", 13);
                    sqlCmd.Parameters.AddWithValue("@date", record.ScheduleModel.Date);
                    sqlCmd.Parameters.AddWithValue("@status", 0);
                    sqlCmd.Parameters.AddWithValue("@clinicID", record.ScheduleModel.ClinicID);

                    sqlCmd.ExecuteNonQuery();

                    return RedirectToAction("Login", "Clinics");// SHOULD BE CHANGED, NOT SURE TO WHAT
                }
            }
        }

        public ActionResult ViewSchedules()
        {
            var record = new List<ViewScheduleUIModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT scheduleID, s.userID, date, status, s.clinicID, clinicName FROM Schedule s INNER JOIN Clinic c ON c.clinicID = s.clinicID WHERE s.userId = @userId";

                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon)) {
                    sqlCmd.Parameters.AddWithValue("@userId", 13);
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if(sqlDr.HasRows)
                        {
                            while(sqlDr.Read())
                            {
                                var viewScheduleUIModel = new ViewScheduleUIModel();
                                viewScheduleUIModel.ScheduleId = int.Parse(sqlDr["scheduleID"].ToString());
                                viewScheduleUIModel.UserID = int.Parse(sqlDr["userID"].ToString());
                                viewScheduleUIModel.Date = DateTime.Parse(sqlDr["date"].ToString());
                                viewScheduleUIModel.ClinicId = int.Parse(sqlDr["clinicID"].ToString());
                                viewScheduleUIModel.ClinicName = sqlDr["clinicName"].ToString();
                                record.Add(viewScheduleUIModel);
                            }
                        }
                    }
                }
            }

            return View(record);
        }
    }
}