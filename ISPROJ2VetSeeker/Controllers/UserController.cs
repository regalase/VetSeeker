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
using static ISPROJ2VetSeeker.FilterConfig;
using Microsoft.AspNet.Identity;
using Microsoft.PowerBI.Api.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ISPROJ2VetSeeker.Controllers
{
    //[NoDirectAccess]
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
                                UnitHouseNo = sqlDr["unitHouseNo"].ToString(),
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
                            list.Add(new UserTypeModel
                            {
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

                var date = DateTime.Now.AddYears(-18);
                if (record.Birthday > date)
                {
                    ViewBag.Error = "test";
                    return RedirectToAction("Register", "User");
                }
                using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
                {
                    sqlCon.Open();
                    /*string checkEmail = "SELECT COUNT(*) FROM Users WHERE email = @email";
                    SqlCommand Cmd = new SqlCommand(checkEmail, sqlCon);
                    Int32 count = Convert.ToInt32(Cmd.ExecuteScalar());
                    if (count > 0) {
                        ViewBag.Error = "Email is existing";
                    }*/
                    string query = @"INSERT INTO Users VALUES(@typeId, @firstName, @lastName, @mobileNo, @email, @username, @password, @gender, @birthday, @city, @unitHouseNo, @street, @baranggay, @profilePicture, @dateAdded, @dateModified, @emailConfirmed, @securityQuestion, @securityAnswer)";
                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                    {

                        sqlCmd.Parameters.AddWithValue("@typeID", record.TypeID);
                        sqlCmd.Parameters.AddWithValue("@firstName", record.FirstName);
                        sqlCmd.Parameters.AddWithValue("@lastName", record.LastName);
                        sqlCmd.Parameters.AddWithValue("@mobileNo", record.MobileNo);
                        sqlCmd.Parameters.AddWithValue("@email", record.Email);
                        sqlCmd.Parameters.AddWithValue("@username", record.UserName);
                        //string hashed = Helper.Hash(record.Password);
                        sqlCmd.Parameters.AddWithValue("@password", record.Password);
                        sqlCmd.Parameters.AddWithValue("@securityQuestion", record.SecurityQuestion);
                        sqlCmd.Parameters.AddWithValue("@securityAnswer", record.SecurityAnswer);
                        sqlCmd.Parameters.AddWithValue("@gender", record.Gender);
                        sqlCmd.Parameters.AddWithValue("@birthday", record.Birthday);
                        sqlCmd.Parameters.AddWithValue("@city", record.City);
                        sqlCmd.Parameters.AddWithValue("@unitHouseNo", record.UnitHouseNo);
                        sqlCmd.Parameters.AddWithValue("@street", record.Street);
                        sqlCmd.Parameters.AddWithValue("@baranggay", record.Baranggay);
                        sqlCmd.Parameters.AddWithValue("@profilePicture", record.ProfilePicture);
                        sqlCmd.Parameters.AddWithValue("@dateAdded", DateTime.Now);
                        sqlCmd.Parameters.AddWithValue("@dateModified", DateTime.Now);
                        sqlCmd.Parameters.AddWithValue("@emailConfirmed", "false");
                        Helper.SendEmail(record.Email, "emailconfirm", "Hello! Please Confirm your email here https://mail.google.com/mail/u/1/#inbox");
                        sqlCmd.ExecuteNonQuery();
                        return RedirectToAction("Login", "Accounts");
                    }
                }
            }  
        }

        public ActionResult EmailConfirmation()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EmailConfirmation(UserModel record)
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT userID, typeID, city, firstName, lastName, email FROM Users WHERE username=@username OR email=@username AND password=@password";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@username", record.UserName);
                    //string hashed = Helper.Hash(record.Password);
                    sqlCmd.Parameters.AddWithValue("@password", record.Password);
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                Session[Helper.USER_ID_KEY] = sqlDr[Helper.USER_ID_KEY].ToString();
                                Session[Helper.TYPE_ID_KEY] = sqlDr[Helper.TYPE_ID_KEY].ToString();
                                Session[Helper.USER_CITY_KEY] = sqlDr[Helper.USER_CITY_KEY].ToString();
                            }

                            if (Session[Helper.TYPE_ID_KEY] != null && Session[Helper.USER_ID_KEY] != null) //user login already
                            {
                                int AuditLogID = Helper.RecordUserSessionLogin(int.Parse(Session[Helper.USER_ID_KEY].ToString()), int.Parse(Session[Helper.TYPE_ID_KEY].ToString()));
                                Session[Helper.AUDIT_ID_KEY] = AuditLogID.ToString();
                                if (Session[Helper.TYPE_ID_KEY].ToString() == "1")
                                {
                                    return RedirectToAction("VerifyEmail", "User");
                                }
                                else
                                {
                                    return RedirectToAction("VetEmail", "User");
                                }
                            }
                            else
                            {
                                ViewBag.Error = "<div class='alert text-danger col-lg-4'>Invalid credentials</div>";

                            }
                        }
                    }
                }
            }
            return View(record);
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(UserModel record)
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT userID, typeID, email FROM Users WHERE email=@email";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@email", record.Email);
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                Session[Helper.USER_ID_KEY] = sqlDr[Helper.USER_ID_KEY].ToString();
                                Session[Helper.TYPE_ID_KEY] = sqlDr[Helper.TYPE_ID_KEY].ToString();
                                Debug.WriteLine("Email: " + record.Email);
                            }

                            if (Session[Helper.TYPE_ID_KEY] != null && Session[Helper.USER_ID_KEY] != null) //user login already
                            {
                                if (Session[Helper.TYPE_ID_KEY].ToString() != "0")
                                {
                                    return RedirectToAction("SelectQuestion", "User");
                                }
                                else
                                {
                                    return RedirectToAction("ForgotPassword", "User");
                                }
                            }
                            else
                            {
                                ViewBag.Error = "<div class='alert text-danger col-lg-4'>Invalid credentials</div>";

                            }
                        }
                    }
                }
            }
            return View(record);
        }

        public ActionResult SelectQuestion()
        {
            var record = new UserModel();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string getSecurityQuestionQuery = @"SELECT userID, securityQuestion FROM Users WHERE userID = @userID";
                using (SqlCommand sqlCmd = new SqlCommand(getSecurityQuestionQuery, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY].ToString());
                    //sqlCmd.Parameters.AddWithValue("@securityAnswer", record.SecurityAnswer);
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                Debug.WriteLine("Question: " + sqlDr["securityQuestion"].ToString());
                                record.SecurityQuestion = sqlDr["securityQuestion"].ToString();
                                record.UserID = int.Parse(sqlDr["userID"].ToString());
                                Debug.WriteLine("Question1: " + record.SecurityQuestion);
                            }
                        }
                    }
                }
                sqlCon.Close();
                return View(record);
            }
        }

        [HttpPost]
        public ActionResult SelectQuestion(UserModel record)
        { 
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string getSecurityAnswerQuery = @"SELECT userID FROM Users WHERE userID = @userID AND securityAnswer = @securityAnswer";
                using (SqlCommand sqlCmd = new SqlCommand(getSecurityAnswerQuery, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY].ToString());
                    sqlCmd.Parameters.AddWithValue("@securityAnswer", record.SecurityAnswer);
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                record.UserID = int.Parse(sqlDr["userID"].ToString());
                                Debug.WriteLine("Answer: " + record.SecurityAnswer);
                                return RedirectToAction("ResetPassword", "User");
                            }
                        }
                        else
                        {
                            ViewBag.Error = "<div class='alert text-danger col-lg-4'>Invalid Credentials</div>";
                            return RedirectToAction("SelectQuestion", "User");
                            //return View(record);
                        }
                    }
                }
                sqlCon.Close();
                return View(record);
            }
        }

        public ActionResult ResetPassword()
        {
            var record = new UserModel();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT userID, typeID, password FROM Users WHERE userID = @userID";
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
                                record.Password = sqlDr["password"].ToString();
                            }
                        }
                    }
                }
                sqlCon.Close();
                return View(record);
            }
        }

        [HttpPost]
        public ActionResult ResetPassword(UserModel record)
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"UPDATE Users SET password=@password WHERE userID = @userID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    //string hashed = Helper.Hash(record.Password);
                    sqlCmd.Parameters.AddWithValue("@password", record.Password);
                    sqlCmd.Parameters.AddWithValue("@userID", record.UserID);
                    sqlCmd.ExecuteNonQuery();
                }
                sqlCon.Close();
            }

            if (Session["TypeID"].ToString() != "0")
            {
                return RedirectToAction("Login", "Accounts");//HOMEPAGE
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }

        }
        public ActionResult VetEmail()
        {
            return View();
        }

        public ActionResult VerifyEmail()
        {
            var record = new UserModel();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT userID, emailConfirmed FROM Users WHERE userID = @userID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY].ToString());
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                Debug.WriteLine("user id:" + sqlDr["userID"].ToString());

                                record.UserID = int.Parse(sqlDr["userID"].ToString());
                                record.EmailConfirmed = sqlDr["emailConfirmed"].ToString();
                            }
                        }
                    }
                }
                sqlCon.Close();
                return View(record);
            }
        }


        [HttpPost]
        public ActionResult VerifyEmail(UserModel record)
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"UPDATE Users SET emailConfirmed = @emailConfirmed WHERE userID = @userID";
                {
                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                    {
                        Debug.WriteLine("user id:" + record.UserID);
                        sqlCmd.Parameters.AddWithValue("@emailConfirmed", "true");
                        sqlCmd.Parameters.AddWithValue("@userID", record.UserID);
                        sqlCmd.ExecuteNonQuery();
                    }
                    sqlCon.Close();
                }
            }

            return RedirectToAction("EmailVerified", "User");

        }

        public ActionResult EmailVerified()
        {
            return View();
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
                                UnitHouseNo = sqlDr["unitHouseNo"].ToString(),
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
                string query = @"SELECT u.username, f.feedback, f.rating, f.dateAdded, f.dateModified,f.myAppointmentID, ma.myAppointmentID, c.clinicID, f.clinicID
                                 FROM Feedback f 
                                 INNER JOIN MyAppointment ma ON f.myAppointmentID = ma.myAppointmentID 
                                 INNER JOIN Schedule s ON ma.scheduleID = s.scheduleID 
                                 INNER JOIN Users u ON u.userID = ma.userID
                                 INNER JOIN Clinic c ON c.clinicID = f.clinicID
                                 WHERE c.clinicID = f.clinicID";
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
                string query = @"SELECT userID, typeID, firstName, lastName, mobileNo, email, username, password, gender, birthday, city, unitHouseNo, street, baranggay, profilePicture, dateModified, securityQuestion, securityAnswer FROM Users WHERE userID = @userID";
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
                                record.UnitHouseNo = sqlDr["unitHouseNo"].ToString();
                                record.Street = sqlDr["street"].ToString();
                                record.Baranggay = sqlDr["baranggay"].ToString();
                                record.ProfilePicture = sqlDr["profilePicture"].ToString();
                                record.DateModified = DateTime.Parse(sqlDr["dateModified"].ToString());
                                record.SecurityQuestion = sqlDr["securityQuestion"].ToString();
                                record.SecurityAnswer = sqlDr["securityAnswer"].ToString();
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
                string query = @"UPDATE Users SET mobileNo=@mobileNo, email=@email, username=@username, password=@password, gender=@gender, birthday=@birthday, city=@city, unitHouseNo=@unitHouseNo, street=@street, baranggay=@baranggay, profilePicture=@profilePicture, dateModified=@dateModified, securityQuestion=@securityQuestion, securityAnswer=@securityAnswer WHERE userID = @userID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@mobileNo", record.MobileNo);
                    sqlCmd.Parameters.AddWithValue("@email", record.Email);
                    sqlCmd.Parameters.AddWithValue("@username", record.UserName);
                    //string hashed = Helper.Hash(record.Password);
                    sqlCmd.Parameters.AddWithValue("@password", record.Password);
                    sqlCmd.Parameters.AddWithValue("@gender", record.Gender);
                    sqlCmd.Parameters.AddWithValue("@birthday", record.Birthday);
                    sqlCmd.Parameters.AddWithValue("@city", record.City);
                    sqlCmd.Parameters.AddWithValue("@unitHouseNo", record.UnitHouseNo);
                    sqlCmd.Parameters.AddWithValue("@street", record.Street);
                    sqlCmd.Parameters.AddWithValue("@baranggay", record.Baranggay);
                    sqlCmd.Parameters.AddWithValue("@profilePicture", record.ProfilePicture);
                    sqlCmd.Parameters.AddWithValue("@dateModified", DateTime.Now);
                    sqlCmd.Parameters.AddWithValue("@securityQuestion", record.SecurityQuestion);
                    sqlCmd.Parameters.AddWithValue("@securityAnswer", record.SecurityAnswer);
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