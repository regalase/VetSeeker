﻿using ISPROJ2VetSeeker.App_Code;
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
    public class ClinicController : Controller
    {
        public ActionResult RegisterClinic()
        {
            var record = new ClinicModel();
            return View(record);
        }

        [HttpPost]
        public ActionResult RegisterClinic(ClinicModel record)
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"INSERT INTO Clinic VALUES(@userID, @clinicname, @unitHouseNo, @street, @baranggay, @city, @latitude, @longitude);";
                Debug.WriteLine(record.ClinicName);
                Debug.WriteLine(record.UnitHouseNo);
                Debug.WriteLine(record.Street);
                Debug.WriteLine(record.Baranggay);
                Debug.WriteLine(record.City);
                Debug.WriteLine(record.Latitude);
                Debug.WriteLine(record.Longitude);

                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", 13);
                    sqlCmd.Parameters.AddWithValue("@clinicname", record.ClinicName);
                    sqlCmd.Parameters.AddWithValue("@unitHouseNo", record.UnitHouseNo);
                    sqlCmd.Parameters.AddWithValue("@street", record.Street);
                    sqlCmd.Parameters.AddWithValue("@baranggay", record.Baranggay);
                    sqlCmd.Parameters.AddWithValue("@city", record.City);
                    sqlCmd.Parameters.AddWithValue("@latitude", record.Latitude);
                    sqlCmd.Parameters.AddWithValue("@longitude", record.Longitude);
                    sqlCmd.ExecuteNonQuery();

                    return RedirectToAction("Login", "Clinics");// SHOULD BE CHANGED, NOT SURE TO WHAT
                }
            }
        }

        // GET: Users
        public ActionResult ListofClinics()//For Admin
        {
            if (Session[Helper.USER_ID_KEY] != null) //user login already
            {
                if (Session[Helper.TYPE_ID_KEY].ToString() == "0")//ADMIN
                {
                    return RedirectToAction("MyProfile", "Clinics");
                }
            }
            var list = new List<ClinicModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT userID, clinicname, unitHouseNo, street, baranggay, city, latitude, longitude, FROM clinic";
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
                                Latitude = sqlDr["latitude"].ToString(),
                                Longitude = sqlDr["longitude"].ToString()
                            });
                        }
                    }
                }

            }
            return View(list);

        }
        public ActionResult Details(int? id)//For Admin
        {
            if (id == null) //no record selected
            {
                return RedirectToAction("Index");
            }
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT userID, clinicname, unitHouseNo, street, baranggay, city, latitude, longitude, FROM clinic";
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
                        latitude=@Latitude, longitude=@Longitude
                        WHERE ClinicID=@ClinicID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@UserID", record.UserID);
                    sqlCmd.Parameters.AddWithValue("@clinicname", record.ClinicName);
                    sqlCmd.Parameters.AddWithValue("@unitHouseNo", record.UnitHouseNo);
                    sqlCmd.Parameters.AddWithValue("@Street", record.Street);
                    sqlCmd.Parameters.AddWithValue("@Baranggay", record.Baranggay);
                    sqlCmd.Parameters.AddWithValue("@City", record.City);
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