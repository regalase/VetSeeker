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

namespace ISPROJ2VetSeeker.Controllers
{
    public class ServiceController : Controller
    {
        public ActionResult CreateService()
        {
            var record = new ServiceTypeModel();
            return View(record);
        }

        [HttpPost]
        public ActionResult CreateService(ServiceTypeModel record)
        {

            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"INSERT INTO ServiceType VALUES(@userID, @serviceName, @price)";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY].ToString());
                    sqlCmd.Parameters.AddWithValue("@serviceName", record.ServiceName);
                    sqlCmd.Parameters.AddWithValue("@price", record.Price);

                    sqlCmd.ExecuteNonQuery();

                    return RedirectToAction("VetProfile", "Accounts");
                }
            }
        }



        public ActionResult ListofServices()
        {
            var list = new List<ServiceTypeModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT userID, serviceName, price FROM ServiceType WHERE userID = @userID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY].ToString());
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            list.Add(new ServiceTypeModel
                            {
                                UserID = int.Parse(sqlDr["userID"].ToString()),
                                ServiceName = sqlDr["serviceName"].ToString(),
                                Price = decimal.Parse(sqlDr["price"].ToString())
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
                string query = @"SELECT userID, serviceName, price FROM ServiceType";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {

                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            var record = new ServiceTypeModel();
                            while (sqlDr.Read())
                            {
                                record.UserID = int.Parse(sqlDr["userID"].ToString());
                                record.ServiceName = sqlDr["serviceName"].ToString();
                                record.Price = decimal.Parse(sqlDr["price"].ToString());
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
        public ActionResult Details(int? id, ServiceTypeModel record)
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"UPDATE ServiceType SET userID=@UserID, serviceName=@ServiceName, price=@Price
                                WHERE serviceTypeID=@ServiceTypeID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", record.UserID);
                    sqlCmd.Parameters.AddWithValue("@serviceName", record.ServiceName);
                    sqlCmd.Parameters.AddWithValue("@price", record.Price);
                    sqlCmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
            }
        }
        public ActionResult Delete(int? id, ServiceTypeModel record)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"UPDATE serviceType SET userID=@userID, serviceName=@serviceName, price=@price
                            WHERE serviceTypeID=@ServiceTypeID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", record.UserID);
                    sqlCmd.Parameters.AddWithValue("@serviceName", record.ServiceName);
                    sqlCmd.Parameters.AddWithValue("@price", record.Price);
                    sqlCmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
            }
        }
    }
}