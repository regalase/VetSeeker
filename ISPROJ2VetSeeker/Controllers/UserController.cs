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
using CaptchaMvc.HtmlHelpers;
using System.Security.Cryptography;

namespace ISPROJ2VetSeeker.Controllers
{
    public class UserController : Controller
    {
        public List<UserModel> GetUsersByType(long type)
        {
            var list = new List<UserModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT userID, typeID, firstName, lastName, mobileNo, email, username, password, gender, birthday, city, 
                unitHouseNo, street,barangay, profilePicture, dateAdded, dateModified  from User WHERE typeID = @type"; 

                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@type", type);
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            list.Add(new UserModel
                            {
                                UserID = long.Parse(sqlDr["userID"].ToString()),
                                TypeID = int.Parse(sqlDr["typeID"].ToString()),
                                FirstName = sqlDr["firstName"].ToString(),
                                LastName = sqlDr["lastName"].ToString(),
                                MobileNo = sqlDr["mobileNo"].ToString(),
                                Email = sqlDr["email"].ToString(),
                                UserName = sqlDr["username"].ToString(),
                                Gender = sqlDr["gender"].ToString(),
                                Birthday = DateTime.Parse(sqlDr["Birthday"].ToString()),
                                City = sqlDr["city"].ToString(),
                                UnitHouseNo = int.Parse(sqlDr["unitHouseNo"].ToString()),
                                Street = sqlDr["street"].ToString(),
                                Baranggay = sqlDr["baranggay"].ToString(),
                                ProfilePicture = sqlDr["profilePicture"].ToString(),
                                DateAdded = DateTime.Parse(sqlDr["dateAdded"].ToString()),
                                DateModified = DateTime.Parse(sqlDr["dateModified"].ToString())
                            });
                        }
                        return list;
                    }
                }
            }
        }

        public List<UserTypeModel> GetUserTypeModels()
        {
            var list = new List<UserTypeModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT type, typeId FROM UserType WHERE typeID != 0";

                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            list.Add(new UserTypeModel {
                                TypeID = int.Parse(sqlDr["typeId"].ToString()),
                                Type = sqlDr["type"].ToString()
                            });
                        }
                        return list;
                    }
                }
            }
        }

        public ActionResult Register()
        {
            var record = new UserModel();
            List<UserTypeModel> userTypeModel = GetUserTypeModels();
            record.Types = userTypeModel;
            return View(record);
        }

        [HttpPost]
        public ActionResult Register(UserModel record, HttpPostedFileBase file)
        {
            if (!this.IsCaptchaValid(""))
            {
                ViewBag.error = "Invalid Captcha";
                return RedirectToAction("Register", "User");
            }
            else
            {
                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/") + file.FileName);
                    record.ProfilePicture = file.FileName;
                }
                else
                {
                    record.ProfilePicture = "";
                }


                using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
                {
                    
                    sqlCon.Open();
                    string query = @"INSERT INTO Users VALUES(@typeId, @firstName, @lastName, @mobileNo, @email, @username, @password, @gender, @birthday, @city, @unitHouseNo, @street, @baranggay, @profilePicture, @dateAdded, @dateModified)";
                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                    {

                        sqlCmd.Parameters.AddWithValue("@typeID", record.TypeID);
                        sqlCmd.Parameters.AddWithValue("@firstName", record.FirstName);
                        sqlCmd.Parameters.AddWithValue("@lastName", record.LastName);
                        sqlCmd.Parameters.AddWithValue("@mobileNo", record.MobileNo);
                        sqlCmd.Parameters.AddWithValue("@email", record.Email);
                        sqlCmd.Parameters.AddWithValue("@username", record.UserName);
                        string hashed = Helper.Hash(record.Password);
                        sqlCmd.Parameters.AddWithValue("@password", hashed);
                        sqlCmd.Parameters.AddWithValue("@gender", record.Gender);
                        sqlCmd.Parameters.AddWithValue("@birthday", record.Birthday);
                        sqlCmd.Parameters.AddWithValue("@city", record.City);
                        sqlCmd.Parameters.AddWithValue("@unitHouseNo", record.UnitHouseNo);
                        sqlCmd.Parameters.AddWithValue("@street", record.Street);
                        sqlCmd.Parameters.AddWithValue("@baranggay", record.Baranggay);
                        sqlCmd.Parameters.AddWithValue("@profilePicture", record.ProfilePicture);
                        sqlCmd.Parameters.AddWithValue("@dateAdded", DateTime.Now);
                        sqlCmd.Parameters.AddWithValue("@dateModified", DateTime.Now);
                        sqlCmd.ExecuteNonQuery();
                        return RedirectToAction("Login", "Accounts");
                    }
                    
                }
            }


        }

        public ActionResult ListofUsers()//For Admin
        {
            if (Session["TypeID"] != null) //user login already
            {
                if (Session["UserTypeID"].ToString() != "1")
                {
                    return RedirectToAction("MyProfile", "Accounts");
                }
            }
            var list = new List<UserModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT userID, typeID, firstName, lastName, mobileNo, email, username, gender, birthday, city, 
                unitHouseNo, street, baranggay, profilePicture, dateAdded, dateModified from User u INNER JOIN UserType ut ON u.typeID = ut.typeId";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            list.Add(new UserModel
                            {
                                UserID = long.Parse(sqlDr["userID"].ToString()),
                                TypeID = int.Parse(sqlDr["typeID"].ToString()),
                                FirstName = sqlDr["firstName"].ToString(),
                                LastName = sqlDr["lastName"].ToString(),
                                MobileNo = sqlDr["mobileNo"].ToString(),
                                Email = sqlDr["email"].ToString(),
                                UserName = sqlDr["username"].ToString(),
                                Gender = sqlDr["gender"].ToString(),
                                Birthday = DateTime.Parse(sqlDr["birthday"].ToString()),
                                City = sqlDr["city"].ToString(),
                                UnitHouseNo = int.Parse(sqlDr["unitHouseNo"].ToString()),
                                Street = sqlDr["street"].ToString(),
                                Baranggay = sqlDr["baranggay"].ToString(),
                                //ProfilePicture = byte[].Parse(sqlDr["profilePicture"].ToString()),
                                DateAdded = DateTime.Parse(sqlDr["dateAdded"].ToString()),
                                DateModified = DateTime.Parse(sqlDr["dateModified"].ToString())
                            });
                        }
                    }
                }

            }
            return View(list);

        }

        public ActionResult ViewClientFeedback()//For User
        {
            if (Session["TypeID"] != null) //user login already
            {
                if (Session["TypeID"].ToString() != "2")
                {
                    return RedirectToAction("MyProfile", "Accounts");//HOMEPAGE
                }
            }
            var list = new List<ViewFeedbackModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT u.username, f.feedback, f.rating, f.dateAdded, f.dateModified, ma.myAppointmentID
                                 FROM Feedback f 
                                 INNER JOIN MyAppointment ma ON f.myAppointmentID = ma.myAppointmentID 
                                 INNER JOIN Schedule s ON ma.scheduleID = s.scheduleID 
                                 INNER JOIN Users u ON u.userID = s.userID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            list.Add(new ViewFeedbackModel
                            {
                                Username = sqlDr["username"].ToString(),
                                Feedback = sqlDr["feedback"].ToString(),
                                Rating = decimal.Parse(sqlDr["rating"].ToString()),
                                DateAdded = DateTime.Parse(sqlDr["dateAdded"].ToString()),//add author name..?
                                DateModified = DateTime.Parse(sqlDr["dateModified"].ToString()),
                                MyAppointmentID = int.Parse(sqlDr["myAppointmentID"].ToString()),
                            });
                        }
                    }
                }

            }
            return View(list);

        }

        public ActionResult UpdateUserProfile()
        {
            var record = new UserModel();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT userID, typeID, firstName, lastName, mobileNo, email, username, password, gender, birthday, city, unitHouseNo, street, baranggay, profilePicture, dateModified FROM Users WHERE userID = @userID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY].ToString());
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                record.UserID = int.Parse(sqlDr["userID"].ToString());
                                record.TypeID = int.Parse(sqlDr["typeID"].ToString());
                                record.FirstName = sqlDr["firstName"].ToString();
                                record.LastName = sqlDr["lastName"].ToString();
                                record.MobileNo = sqlDr["mobileNo"].ToString();
                                record.Email = sqlDr["email"].ToString();
                                record.UserName = sqlDr["username"].ToString();
                                record.Password = sqlDr["password"].ToString();
                                record.Gender = sqlDr["gender"].ToString();
                                record.Birthday = DateTime.Parse(sqlDr["birthday"].ToString());
                                record.City = sqlDr["city"].ToString();
                                record.UnitHouseNo = int.Parse(sqlDr["unitHouseNo"].ToString());
                                record.Street = sqlDr["street"].ToString();
                                record.Baranggay = sqlDr["baranggay"].ToString();
                                record.ProfilePicture = sqlDr["profilePicture"].ToString();
                                record.DateModified = DateTime.Parse(sqlDr["dateModified"].ToString());
                            }
                        }
                    }
                }
                sqlCon.Close();
                return View(record);
            }
        }

        [HttpPost]
        public ActionResult UpdateUserProfile(UserModel record, HttpPostedFileBase file)
        {
            //Debug.WriteLine("PP:" + file.FileName);
            if (file != null)
            {
                file.SaveAs(HttpContext.Server.MapPath("~/Images/") + file.FileName);
                record.ProfilePicture = file.FileName;
            }
            else
            {
                record.ProfilePicture = "";
            }
            

            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"UPDATE Users SET firstName=@firstName, lastName=@lastName, mobileNo=@mobileNo, email=@email, username=@username, password=@password, gender=@gender, birthday=@birthday, city=@city, unitHouseNo=@unitHouseNo, street=@street, baranggay=@baranggay, profilePicture=@profilePicture, dateModified=@dateModified WHERE userID = @userID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@firstName", record.FirstName);
                    sqlCmd.Parameters.AddWithValue("@lastName", record.LastName);
                    sqlCmd.Parameters.AddWithValue("@mobileNo", record.MobileNo);
                    sqlCmd.Parameters.AddWithValue("@email", record.Email);
                    sqlCmd.Parameters.AddWithValue("@username", record.UserName);
                    string hashed = Helper.Hash(record.Password);
                    sqlCmd.Parameters.AddWithValue("@password", hashed);
                    sqlCmd.Parameters.AddWithValue("@gender", record.Gender);
                    sqlCmd.Parameters.AddWithValue("@birthday", record.Birthday);
                    sqlCmd.Parameters.AddWithValue("@city", record.City);
                    sqlCmd.Parameters.AddWithValue("@unitHouseNo", record.UnitHouseNo);
                    sqlCmd.Parameters.AddWithValue("@street", record.Street);
                    sqlCmd.Parameters.AddWithValue("@baranggay", record.Baranggay);
                    sqlCmd.Parameters.AddWithValue("@profilePicture", record.ProfilePicture);
                    sqlCmd.Parameters.AddWithValue("@dateModified", DateTime.Now);
                    sqlCmd.Parameters.AddWithValue("@userID", record.UserID);
                    sqlCmd.ExecuteNonQuery();
                }
                sqlCon.Close();
            }

            if (Session["TypeID"].ToString() == "1")
                {
                    return RedirectToAction("MyProfile", "Accounts");//HOMEPAGE
                }
            else
                {
                    return RedirectToAction("VetProfile", "Accounts");
                }
           
        }
    }
}