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
using static ISPROJ2VetSeeker.FilterConfig;

namespace ISPROJ2VetSeeker.Controllers
{
    [NoDirectAccess]
    public class ClinicController : Controller
    {
        public ActionResult RegisterClinic() //added code lines 20-27
        {
            if (Session[Helper.TYPE_ID_KEY].ToString() == "1")
            {
                return RedirectToAction("ClinicSchedules", "MyAppointment");
            }
            else if (Session[Helper.TYPE_ID_KEY].ToString() == "0")
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            var record = new ClinicModel();
            return View(record);
        }

        [HttpPost]
        public ActionResult RegisterClinic(ClinicModel record)
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"INSERT INTO Clinic VALUES(@userID, @clinicname, @unitHouseNo, @street, @baranggay, @city, @clinicEmail, @clinicContactNo, @latitude, @longitude)";


                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY]);
                    sqlCmd.Parameters.AddWithValue("@clinicname", record.ClinicName);
                    sqlCmd.Parameters.AddWithValue("@unitHouseNo", record.UnitHouseNo);
                    sqlCmd.Parameters.AddWithValue("@street", record.Street);
                    sqlCmd.Parameters.AddWithValue("@baranggay", record.Baranggay);
                    sqlCmd.Parameters.AddWithValue("@city", record.City);
                    sqlCmd.Parameters.AddWithValue("@clinicEmail", record.ClinicEmail);
                    sqlCmd.Parameters.AddWithValue("@clinicContactNo", record.ClinicContactNo);
                    sqlCmd.Parameters.AddWithValue("@latitude", record.Latitude);
                    sqlCmd.Parameters.AddWithValue("@longitude", record.Longitude);
                    sqlCmd.ExecuteNonQuery();

                    return RedirectToAction("ViewClinics", "Clinic");
                }
            }
        }

        // GET: Users
        public ActionResult ListofClinics()
        {
            if (Session[Helper.USER_ID_KEY] != null)
            {
                if (Session[Helper.TYPE_ID_KEY].ToString() == "0")
                {
                    return RedirectToAction("MyProfile", "Clinics");
                }
            }
            var list = new List<ClinicModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT userID, clinicname, unitHouseNo, street, baranggay, city, @clinicEmail, @clinicContactNo, latitude, longitude, FROM clinic";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    //sqlCmd.Parameters.AddWithValue("@status", "Active"); MIGHT NOT NEED
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            list.Add(new ClinicModel
                            {
                                UserID = int.Parse(sqlDr["userID"].ToString()),
                                ClinicName = sqlDr["clinicname"].ToString(),
                                UnitHouseNo = sqlDr["unitHouseNo"].ToString(),
                                Street = sqlDr["street"].ToString(),
                                Baranggay = sqlDr["baranggay"].ToString(),
                                City = sqlDr["city"].ToString(),
                                ClinicEmail = sqlDr["clinicEmail"].ToString(),
                                ClinicContactNo = sqlDr["clinicContactNo"].ToString(),
                                Latitude = sqlDr["latitude"].ToString(),
                                Longitude = sqlDr["longitude"].ToString()
                            });
                        }
                    }
                }

            }
            return View(list);

        }

        public ActionResult ViewClinics()
        {
            var userClinics = new List<ClinicModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT clinicID, userID, clinicname, unitHouseNo, street, baranggay, city, clinicEmail, clinicContactNo FROM Clinic WHERE userID = @userID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY].ToString());
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                var clinicModel = new ClinicModel();
                                clinicModel.ClinicID = int.Parse(sqlDr["clinicID"].ToString());
                                clinicModel.UserID = int.Parse(sqlDr["userID"].ToString());
                                clinicModel.ClinicName = sqlDr["clinicname"].ToString();
                                clinicModel.UnitHouseNo = sqlDr["unitHouseNo"].ToString();
                                clinicModel.Street = sqlDr["street"].ToString();
                                clinicModel.Baranggay = sqlDr["baranggay"].ToString();
                                clinicModel.City = sqlDr["city"].ToString();
                                clinicModel.ClinicEmail = sqlDr["clinicEmail"].ToString();
                                clinicModel.ClinicContactNo = sqlDr["clinicContactNo"].ToString();
                                userClinics.Add(clinicModel);
                            }
                        }
                    }
                }
            }
            return View(userClinics);
        }

        public ActionResult UpdateClinic(int ClinicID)
        {
            var record = new ClinicModel();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT clinicID, userID, clinicname, unitHouseNo, street, baranggay, city, clinicEmail, clinicContactNo, latitude, longitude FROM Clinic WHERE userID = @userID AND clinicID = @clinicID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY].ToString());
                    sqlCmd.Parameters.AddWithValue("@clinicID", ClinicID);
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                record.ClinicID = int.Parse(sqlDr["clinicID"].ToString());
                                record.UserID = int.Parse(sqlDr["userID"].ToString());
                                record.ClinicName = sqlDr["clinicname"].ToString();
                                record.UnitHouseNo = sqlDr["unitHouseNo"].ToString();
                                record.Street = sqlDr["street"].ToString();
                                record.Baranggay = sqlDr["baranggay"].ToString();
                                record.City = sqlDr["city"].ToString();
                                record.ClinicEmail = sqlDr["clinicEmail"].ToString();
                                record.ClinicContactNo = sqlDr["clinicContactNo"].ToString();
                                record.Latitude = sqlDr["latitude"].ToString();
                                record.Longitude = sqlDr["longitude"].ToString();
                            }
                        }
                    }
                }
                sqlCon.Close();
                return View(record);
            }
        }

        [HttpPost]
        public ActionResult UpdateClinic(ClinicModel record)
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"UPDATE Clinic SET clinicname=@clinicname, unitHouseNo=@unitHouseNo, street=@street, baranggay=@baranggay, city=@city, clinicEmail=@clinicEmail, clinicContactNo=@clinicContactNo, latitude=@latitude, longitude=@longitude WHERE userID = @userID and clinicID = @clinicID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@clinicname", record.ClinicName);
                    sqlCmd.Parameters.AddWithValue("@unitHouseNo", record.UnitHouseNo);
                    sqlCmd.Parameters.AddWithValue("@street", record.Street);
                    sqlCmd.Parameters.AddWithValue("@baranggay", record.Baranggay);
                    sqlCmd.Parameters.AddWithValue("@city", record.City);
                    sqlCmd.Parameters.AddWithValue("@clinicEmail", record.ClinicEmail);
                    sqlCmd.Parameters.AddWithValue("@clinicContactNo", record.ClinicContactNo);
                    sqlCmd.Parameters.AddWithValue("@latitude", record.Latitude);
                    sqlCmd.Parameters.AddWithValue("@longitude", record.Longitude);
                    sqlCmd.Parameters.AddWithValue("@userID", record.UserID);
                    sqlCmd.Parameters.AddWithValue("@clinicID", record.ClinicID);
                    sqlCmd.ExecuteNonQuery();
                }
                sqlCon.Close();
            }
            return RedirectToAction("ViewClinics", "Clinic");
        }
        public ActionResult Details(int? id)
        {
            if (id == null) //no record selected
            {
                return RedirectToAction("Index");
            }
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT userID, clinicname, unitHouseNo, street, baranggay, city, clinicEmail, clinicContactNo, latitude, longitude, FROM clinic";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    //sqlCmd.Parameters.AddWithValue("@UserID", id);
                    //sqlCmd.Parameters.AddWithValue("@Status", "Archived");
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            var record = new ClinicModel();
                            while (sqlDr.Read())
                            {
                                record.UserID = int.Parse(sqlDr["userID"].ToString());
                                record.ClinicName = sqlDr["clinicname"].ToString();
                                record.UnitHouseNo = sqlDr["unitHouseNo"].ToString();
                                record.Street = sqlDr["street"].ToString();
                                record.Baranggay = sqlDr["baranggay"].ToString();
                                record.City = sqlDr["city"].ToString();
                                record.ClinicEmail = sqlDr["clinicEmail"].ToString();
                                record.ClinicContactNo = sqlDr["clinicContactNo"].ToString();
                                record.Longitude = sqlDr["longitude"].ToString();
                                record.Latitude = sqlDr["latitude"].ToString();
                            }
                            return View(record);
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }

                    }
                }
            }
        }
        [HttpPost]
        public ActionResult Details(int? id, ClinicModel record)
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"UPDATE Clinic SET UserID=@UserID, clinicname=@clinicname, unitHouseNo=@UnitHouseNo, street=@Street, baranggay=@Baranggay, city=@City,
                        clinicEmail=@clinicEmial, clinicContactNo=@clinicContactNo, latitude=@Latitude, longitude=@Longitude
                        WHERE ClinicID=@ClinicID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@UserID", record.UserID);
                    sqlCmd.Parameters.AddWithValue("@clinicname", record.ClinicName);
                    sqlCmd.Parameters.AddWithValue("@unitHouseNo", record.UnitHouseNo);
                    sqlCmd.Parameters.AddWithValue("@Street", record.Street);
                    sqlCmd.Parameters.AddWithValue("@Baranggay", record.Baranggay);
                    sqlCmd.Parameters.AddWithValue("@City", record.City);
                    sqlCmd.Parameters.AddWithValue("@ClinicEmail", record.ClinicEmail);
                    sqlCmd.Parameters.AddWithValue("@ClinicContactNo", record.ClinicContactNo);
                    sqlCmd.Parameters.AddWithValue("@Latitude", record.Latitude);
                    sqlCmd.Parameters.AddWithValue("@Longitude", record.Longitude);
                    sqlCmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
            }
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"UPDATE Clinic SET Status=@Status, DateModified=@DateModified
                    WHERE ClinicID=@ClinicID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@Status", false);
                    sqlCmd.Parameters.AddWithValue("@DateModified", DateTime.Now);
                    sqlCmd.Parameters.AddWithValue("@ClinicID", id);
                    sqlCmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
            }
        }
    }
}