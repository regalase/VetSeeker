﻿using ISPROJ2VetSeeker.App_Code;
using ISPROJ2VetSeeker.Models;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
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
                string query = @"SELECT userID, typeID, city, firstName, lastName, email, emailConfirmed FROM Users WHERE username=@username OR email=@username AND password=@password";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@username", record.UserName);
                    //string hashed = Helper.Hash(record.Password);
                    sqlCmd.Parameters.AddWithValue("@password", record.Password);
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            Debug.WriteLine("Sean1: " + record.EmailConfirmed);
                            while (sqlDr.Read())
                            {
                                Session[Helper.USER_ID_KEY] = sqlDr[Helper.USER_ID_KEY].ToString();
                                Session[Helper.TYPE_ID_KEY] = sqlDr[Helper.TYPE_ID_KEY].ToString();
                                Session[Helper.USER_CITY_KEY] = sqlDr[Helper.USER_CITY_KEY].ToString();
                                record.EmailConfirmed = sqlDr["emailConfirmed"].ToString();
                            }

                            if (Session[Helper.TYPE_ID_KEY] != null && Session[Helper.USER_ID_KEY] != null && record.EmailConfirmed == "true") //user login already
                            {
                                int AuditLogID = Helper.RecordUserSessionLogin(int.Parse(Session[Helper.USER_ID_KEY].ToString()), int.Parse(Session[Helper.TYPE_ID_KEY].ToString()));
                                Session[Helper.AUDIT_ID_KEY] = AuditLogID.ToString();
                                if (Session[Helper.TYPE_ID_KEY].ToString() == "1")
                                {
                                    return RedirectToAction("ClinicSchedules", "MyAppointment");
                                }
                                else if (Session[Helper.TYPE_ID_KEY].ToString() == "2")
                                {
                                    return RedirectToAction("ViewSchedules", "Schedule");
                                }
                                else
                                {
                                    return RedirectToAction("Dashboard", "Admin");
                                }
                            }
                            else
                            {
                                ViewBag.Error = "<div class='alert text-danger col-lg-4'>Account not verified</div>";
                                return View(record);
                                //return RedirectToAction("Login", "Accounts");
                            }
                        }
                        else
                        {
                            ViewBag.Error = "<div class='alert text-danger col-lg-4'>Invalid credentials</div>";
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
                if (Session[Helper.USER_ID_KEY] == null)
                {
                    return RedirectToAction("Login");
                }

                var record = new UserModel();
                sqlCon.Open();
                string query = @"SELECT userID, ut.typeID, firstName, lastName, mobileNo, email, username, password, gender, birthday, city, 
                unitHouseNo, street, baranggay, profilePicture, dateAdded, dateModified from Users u INNER JOIN UserType ut ON u.typeID = ut.typeID WHERE userId = @UserID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@UserID", Session[Helper.USER_ID_KEY].ToString());
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                record.TypeID = int.Parse(sqlDr["typeID"].ToString());
                                record.FirstName = sqlDr["firstName"].ToString();
                                record.LastName = sqlDr["lastName"].ToString();
                                record.MobileNo = sqlDr["mobileNo"].ToString();
                                record.Email = sqlDr["email"].ToString();
                                record.UserName = sqlDr["username"].ToString();
                                record.Password = sqlDr["password"].ToString();
                                record.Gender = sqlDr["gender"].ToString();
                                string dateTime = sqlDr["birthday"].ToString();
                                record.Birthday = System.DateTime.Parse(dateTime);
                                record.City = sqlDr["city"].ToString();
                                record.UnitHouseNo = sqlDr["unitHouseNo"].ToString();
                                record.Street = sqlDr["street"].ToString();
                                record.Baranggay = sqlDr["baranggay"].ToString();
                                record.ProfilePicture = sqlDr["profilePicture"].ToString();
                                record.DateAdded = DateTime.Parse(sqlDr["dateAdded"].ToString());
                                record.DateModified = DateTime.Parse(sqlDr["dateModified"].ToString());
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

        //Change to Client Info
        public ActionResult MyProfileInfo()
        {

            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                if (Session[Helper.USER_ID_KEY] == null)
                {
                    return RedirectToAction("Login");
                }

                var record = new UserModel();
                sqlCon.Open();
                string query = @"SELECT userID, typeID, firstName, lastName, mobileNo, email, username, password, gender, birthday, city, 
                unitHouseNo, street, baranggay, profilePicture, dateAdded, dateModified from User u INNER JOIN UserType ut ON u.typeID = ut.typeId; ";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@UserID", Session[Helper.USER_ID_KEY].ToString());
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                record.Type = sqlDr["type"].ToString();
                                record.FirstName = sqlDr["firstName"].ToString();
                                record.LastName = sqlDr["lastName"].ToString();
                                record.MobileNo = sqlDr["mobileNo"].ToString();
                                record.Email = sqlDr["email"].ToString();
                                record.UserName = sqlDr["username"].ToString();
                                record.Password = sqlDr["password"].ToString();
                                record.Gender = sqlDr["gender"].ToString();
                                record.Birthday = DateTime.Parse(sqlDr["birthday"].ToString());
                                record.City = sqlDr["city"].ToString();
                                record.UnitHouseNo = sqlDr["unitHouseNo"].ToString();
                                record.Street = sqlDr["street"].ToString();
                                record.Baranggay = sqlDr["baranggay"].ToString();
                                record.ProfilePicture = sqlDr["profilePicture"].ToString();
                                record.DateAdded = DateTime.Parse(sqlDr["dateJoined"].ToString());
                                record.DateModified = DateTime.Parse(sqlDr["dateModified"].ToString());
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

        public ActionResult VetProfile()
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                if (Session[Helper.USER_ID_KEY] == null)
                {
                    return RedirectToAction("Login");
                }

                var record = new UserModel();
                sqlCon.Open();
                string query = @"SELECT userID, ut.typeID, firstName, lastName, mobileNo, email, username, password, gender, birthday, city, 
                unitHouseNo, street, baranggay, profilePicture, dateAdded, dateModified from Users u INNER JOIN UserType ut ON u.typeID = ut.typeID WHERE userId = @UserID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@UserID", Session[Helper.USER_ID_KEY].ToString());
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                record.TypeID = int.Parse(sqlDr["typeID"].ToString());
                                record.FirstName = sqlDr["firstName"].ToString();
                                record.LastName = sqlDr["lastName"].ToString();
                                record.MobileNo = sqlDr["mobileNo"].ToString();
                                record.Email = sqlDr["email"].ToString();
                                record.UserName = sqlDr["username"].ToString();
                                record.Password = sqlDr["password"].ToString();
                                record.Gender = sqlDr["gender"].ToString();
                                string dateTime = sqlDr["birthday"].ToString();
                                record.Birthday = System.DateTime.Parse(dateTime);
                                record.City = sqlDr["city"].ToString();
                                record.UnitHouseNo = sqlDr["unitHouseNo"].ToString();
                                record.Street = sqlDr["street"].ToString();
                                record.Baranggay = sqlDr["baranggay"].ToString();
                                record.ProfilePicture = sqlDr["profilePicture"].ToString();
                                record.DateAdded = DateTime.Parse(sqlDr["dateAdded"].ToString());
                                record.DateModified = DateTime.Parse(sqlDr["dateModified"].ToString());
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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            Helper.RecordUserSessionLogout(int.Parse(Session[Helper.USER_ID_KEY].ToString()), int.Parse(Session[Helper.TYPE_ID_KEY].ToString()), int.Parse(Session[Helper.AUDIT_ID_KEY].ToString()));
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}