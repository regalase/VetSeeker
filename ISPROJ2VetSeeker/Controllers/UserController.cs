using ISPROJ2VetSeeker.App_Code;
using ISPROJ2VetSeeker.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                unitHouseNo, street,barangay, profilePicture, dateAdded, dateModified from User WHERE typeID = @type";

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
                                TypeID = long.Parse(sqlDr["typeID"].ToString()),
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
                                //ProfilePicture = byte[].Parse(sqlDr["profilePicture"].ToString()),
                                DateAdded = DateTime.Parse(sqlDr["dateAdded"].ToString()),
                                DateModified = DateTime.Parse(sqlDr["dateModified"].ToString())
                            });
                        }
                        return list;
                    }
                }
            }
        }


        [HttpPost]
        public ActionResult Register(UserModel record)
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"INSERT INTO Users VALUES(@userId, @typeId, @firstName, @lastName, @mobileNo, @email, @username, @password, @gender, @birthday, @city, @unitHouseNo, @street, @baranggay, @profilePicture, @dateAdded, @dateModified);";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@typeID", record.TypeID);
                    sqlCmd.Parameters.AddWithValue("@firstName", record.FirstName);
                    sqlCmd.Parameters.AddWithValue("@lastName", record.LastName);
                    sqlCmd.Parameters.AddWithValue("@mobileNo", record.MobileNo);
                    sqlCmd.Parameters.AddWithValue("@email", record.Email);
                    sqlCmd.Parameters.AddWithValue("@username", record.UserName);
                    sqlCmd.Parameters.AddWithValue("@password", record.Password);
                    sqlCmd.Parameters.AddWithValue("@gender", record.Gender);
                    sqlCmd.Parameters.AddWithValue("@birthday", record.Birthday);
                    sqlCmd.Parameters.AddWithValue("@city", record.City);
                    sqlCmd.Parameters.AddWithValue("@unitHouseNo", record.UnitHouseNo);
                    sqlCmd.Parameters.AddWithValue("@street", record.Street);
                    sqlCmd.Parameters.AddWithValue("@baranggay", record.Baranggay);
                    sqlCmd.Parameters.AddWithValue("@profilePicture", record.ProfilePicture);
                    sqlCmd.Parameters.AddWithValue("@dateAdded", DateTime.Now);
                    sqlCmd.Parameters.AddWithValue("@dateModified", DBNull.Value);
                    sqlCmd.ExecuteNonQuery();

                    return RedirectToAction("Login", "Accounts");
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
                                TypeID = long.Parse(sqlDr["typeID"].ToString()),
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
    }
}