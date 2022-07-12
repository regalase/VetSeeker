using System;
using ISPROJ2VetSeeker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using ISPROJ2VetSeeker.App_Code;

namespace ISPROJ2VetSeeker.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult ListofUsers()//For Admin
        {
            if (Session[Helper.USER_ID_KEY] == null)
            {
                return RedirectToAction("Login");
            }
            var list = new List<UserModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT userID, ut.typeID, firstName, lastName, mobileNo, email, username, password, gender, birthday, city, 
                unitHouseNo, street, baranggay, profilePicture, dateAdded, dateModified, emailConfirmed, securityQuestion, securityAnswer from Users u INNER JOIN UserType ut ON u.typeID = ut.typeID";
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
                                DateModified = DateTime.Parse(sqlDr["dateModified"].ToString()),
                                EmailConfirmed = sqlDr["emailConfirmed"].ToString(),
                                SecurityQuestion = sqlDr["securityQuestion"].ToString(),
                                SecurityAnswer = sqlDr["securityAnswer"].ToString()
                            });
                        }
                    }
                }

            }
            return View(list);
        }
        public ActionResult AuditLog()
        {
            if (Session[Helper.USER_ID_KEY] == null)
            {
                return RedirectToAction("Login");
            }
            var list = new List<AdminModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT auditLogID, userID, typeID, dateOfLogin, dateOfLogout FROM UserSessionLog";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            list.Add(new AdminModel
                            {
                                AuditLogID = long.Parse(sqlDr["auditLogID"].ToString()),
                                UserID = int.Parse(sqlDr["userID"].ToString()),
                                TypeID = int.Parse(sqlDr["typeID"].ToString()),
                                DateOfLogin = DateTime.Parse(sqlDr["dateOfLogin"].ToString()),
                                DateOfLogout = DateTime.Parse(sqlDr["dateOfLogout"].ToString())
                            });
                        }
                    }
                }

            }
            return View(list);
        }
        public ActionResult ListofClinics()
        {
            if (Session[Helper.USER_ID_KEY] == null)
            {
                return RedirectToAction("Login");
            }
            var list = new List<ClinicModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT * FROM clinic";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    //sqlCmd.Parameters.AddWithValue("@status", "Active"); MIGHT NOT NEED
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            list.Add(new ClinicModel
                            {
                                ClinicID = long.Parse(sqlDr["userID"].ToString()),
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
        public ActionResult SelectVet()
        {
            if (Session[Helper.USER_ID_KEY] == null)
            {
                return RedirectToAction("Login");
            }
            var list = new List<UserModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT userID, typeID, firstName, lastName, email, emailConfirmed FROM Users WHERE typeID = '2' AND emailConfirmed = 'false'";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    //sqlCmd.Parameters.AddWithValue("@status", "Active"); MIGHT NOT NEED
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
                                Email = sqlDr["email"].ToString(),
                                EmailConfirmed = sqlDr["emailConfirmed"].ToString()
                            });
                        }
                    }
                }

            }
            return View(list);
        }
        public ActionResult UpdateVet()
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT userID, typeID, firstName, lastName, email, emailConfirmed FROM Users WHERE typeID = '2' AND emailConfirmed = 'false'";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    var record = new UserModel();
                    //sqlCmd.Parameters.AddWithValue("@status", "Active"); MIGHT NOT NEED
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                {
                                    record.UserID = int.Parse(sqlDr["userID"].ToString());
                                    record.TypeID = int.Parse(sqlDr["typeID"].ToString());
                                    record.FirstName = sqlDr["firstName"].ToString();
                                    record.LastName = sqlDr["lastName"].ToString();
                                    record.Email = sqlDr["email"].ToString();
                                    record.EmailConfirmed = sqlDr["emailConfirmed"].ToString();
                                }


                            }
                        }

                    }
                    sqlCon.Close();
                    return View(record);
                }

            }

        }
        [HttpPost]
        public ActionResult UpdateVet(UserModel record)
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                if (Session[Helper.USER_ID_KEY] == null)
                {
                    return RedirectToAction("Login");
                }
                sqlCon.Open();
                string query = @"UPDATE Users SET emailConfirmed = @emailConfirmed WHERE userID = @userID";
                {
                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                    {
                        //Debug.WriteLine("user id:" + record.UserID);
                        sqlCmd.Parameters.AddWithValue("@emailConfirmed", "true");
                        sqlCmd.Parameters.AddWithValue("@userID", record.UserID);
                        sqlCmd.ExecuteNonQuery();
                    }
                    sqlCon.Close();
                }
            }

            return RedirectToAction("SelectVet", "Admin");

        }
    }
}

