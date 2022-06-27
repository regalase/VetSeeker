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
                string query = @"INSERT INTO ServiceType VALUES(@userID, @serviceName, @serviceDescription, @price)";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY].ToString());
                    sqlCmd.Parameters.AddWithValue("@serviceName", record.ServiceName);
                    sqlCmd.Parameters.AddWithValue("@serviceDescription", record.ServiceDescription);
                    sqlCmd.Parameters.AddWithValue("@price", record.Price);

                    sqlCmd.ExecuteNonQuery();

                    return RedirectToAction("ListofServices", "Service");
                }
            }
        }

        public ActionResult ListofServices()
        {
            var list = new List<ServiceTypeModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT serviceTypeID, userID, serviceName, serviceDescription, price FROM ServiceType WHERE userID = @userID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY].ToString());
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            list.Add(new ServiceTypeModel
                            {
                                ServiceTypeID = int.Parse(sqlDr["serviceTypeID"].ToString()),
                                UserID = int.Parse(sqlDr["userID"].ToString()),
                                ServiceName = sqlDr["serviceName"].ToString(),
                                ServiceDescription = sqlDr["serviceDescription"].ToString(),
                                Price = decimal.Parse(sqlDr["price"].ToString())
                            });
                        }
                    }
                }

            }
            return View(list);

        }
        public ActionResult UpdateService(int? id)
        {
            if (id == null) //no record selected
            {
                return RedirectToAction("ListofServices");
            }
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT userID, serviceName, serviceDescription, price FROM ServiceType";
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
                                record.ServiceDescription = sqlDr["serviceDescription"].ToString();
                                record.Price = decimal.Parse(sqlDr["price"].ToString());
                            }
                            sqlCon.Close();
                            return View(record);
                        }
                        else
                        {
                            sqlCon.Close();
                            return RedirectToAction("ListofServices", "Service");
                        }

                    }
                }
            }
        }
        [HttpPost]
        public ActionResult UpdateService(int? id, ServiceTypeModel record)
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"UPDATE ServiceType SET serviceName=@ServiceName, serviceDescription=@ServiceDescription, price=@Price
                                WHERE serviceTypeID=@ServiceTypeID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    Debug.WriteLine(" ID" + id);
                    Debug.WriteLine("SCHED ID" + record.ServiceName);
                    Debug.WriteLine("Clinic ID" + record.Price);
                    sqlCmd.Parameters.AddWithValue("@serviceName", record.ServiceName);
                    sqlCmd.Parameters.AddWithValue("@serviceDescription", record.ServiceDescription);
                    sqlCmd.Parameters.AddWithValue("@price", record.Price);
                    sqlCmd.Parameters.AddWithValue("@ServiceTypeID", id);
                    sqlCmd.ExecuteNonQuery();
                }
                sqlCon.Close();
            }
            return RedirectToAction("ListofServices", "Service");
        }

        public ActionResult DeleteService(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"DELETE FROM ServiceType WHERE serviceTypeID=@serviceTypeID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@serviceTypeID", id);
                    sqlCmd.ExecuteNonQuery();
                }
                sqlCon.Close();
            }
            return RedirectToAction("ListofServices", "Service");
        }
    }
}
