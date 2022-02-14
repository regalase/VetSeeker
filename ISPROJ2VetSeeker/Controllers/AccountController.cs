﻿using ISPROJ2VetSeeker.App_Code;
using ISPROJ2VetSeeker.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISPROJ2VetSeeker.Controllers
{
    public class AccountsController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserModel record)
        {
            //Step 1: Open Database
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT userID, typeID FROM users WHERE username=@username AND password=@password";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@username", record.UserName);
                    sqlCmd.Parameters.AddWithValue("@Password", record.Password);
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                Session["userID"] = sqlDr["userID"].ToString();
                                Session["typeID"] = sqlDr["typeID"].ToString();

                            }

                            if (Session["typeID"] != null && Session["userID"] != null ) //user login already
                            {
                                if (Session["typeID"].ToString() != "1")
                                {
                                    return RedirectToAction("MyProfile", "Accounts");
                                }
                            }

                            return RedirectToAction("Dashboard", "Admin");

                        }
                        else
                        {
                            ViewBag.Error = "<div class='alert alert-danger col-lg-4'>Invalid credentials</div>";
                            return View(record);
                        }
                    }
                }
            }
            //Step 2: Execute query to check if email + password match
            //Step 3: If match, proceed to main page
            //Step 4: If not match, display error
        }

        public ActionResult MyProfile()
        {

            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                if (Session["userid"] == null)
                {
                    return RedirectToAction("Login");
                }

                var record = new UserModel();
                sqlCon.Open();
                string query = @"SELECT userID, typeID, firstName, lastName, mobileNo, email, username, password, gender, birthday, city, 
                unitHouseNo, street, barangay, profilePicture, dateAdded, dateModified from User u INNER JOIN UserType ut ON u.typeID = ut.typeId; ";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                record.Type = sqlDr["type"].ToString();
                                record.FirstName = sqlDr["firstName"].ToString();
                                record.LastName = sqlDr["middleName"].ToString();
                                record.MobileNo = sqlDr["lastName"].ToString();
                                record.Email = sqlDr["email"].ToString();
                                record.UserName = sqlDr["username"].ToString();
                                record.Password = sqlDr["password"].ToString();
                                record.Gender = sqlDr["gender"].ToString();
                                record.Birthday = DateTime.Parse(sqlDr["birthday"].ToString());
                                record.City = sqlDr["city"].ToString();
                                record.UnitHouseNo = int.Parse(sqlDr["unitHouseNo"].ToString());
                                record.Street = sqlDr["street"].ToString();
                                record.Baranggay = sqlDr["baranggay"].ToString();
                                //record.ProfilePicture = byte[].parse(sqlDr["profilePicture"].ToString());
                                record.DateAdded = DateTime.Parse(sqlDr["dateJoined"].ToString());
                                record.DateModified = DateTime.Parse(sqlDr["dateJoined"].ToString());
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


        public ActionResult MyProfileInfo()
        {

            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                if (Session["userid"] == null)
                {
                    return RedirectToAction("Login");
                }

                var record = new UserModel();
                sqlCon.Open();
                string query = @"SELECT userID, typeID, firstName, lastName, mobileNo, email, username, password, gender, birthday, city, 
                unitHouseNo, street, barangay, profilePicture, dateAdded, dateModified from User u INNER JOIN UserType ut ON u.typeID = ut.typeId; ";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                record.Type = sqlDr["type"].ToString();
                                record.FirstName = sqlDr["firstName"].ToString();
                                record.LastName = sqlDr["middleName"].ToString();
                                record.MobileNo = sqlDr["lastName"].ToString();
                                record.Email = sqlDr["email"].ToString();
                                record.UserName = sqlDr["username"].ToString();
                                record.Password = sqlDr["password"].ToString();
                                record.Gender = sqlDr["gender"].ToString();
                                record.Birthday = DateTime.Parse(sqlDr["birthday"].ToString());
                                record.City = sqlDr["city"].ToString();
                                record.UnitHouseNo = int.Parse(sqlDr["unitHouseNo"].ToString());
                                record.Street = sqlDr["street"].ToString();
                                record.Baranggay = sqlDr["baranggay"].ToString();
                                //record.ProfilePicture = byte[].parse(sqlDr["profilePicture"].ToString());
                                record.DateAdded = DateTime.Parse(sqlDr["dateJoined"].ToString());
                                record.DateModified = DateTime.Parse(sqlDr["dateJoined"].ToString());
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
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}