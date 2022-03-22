using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISPROJ2VetSeeker.App_Code;
using ISPROJ2VetSeeker.Models;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace ISPROJ2VetSeeker.Controllers
{
    public class MyAppointmentController : Controller
    {
        // GET: MyAppointment
        public ActionResult ClinicSchedules()
        {
            var record = new ClientViewScheduleModel();
            //query clinics by user logged in;
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT clinicID, userID, unitHouseNo, street, baranggay, city, clinicname, longitude, latitude FROM Clinic ORDER BY longitude, latitude DESC";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    //sqlCmd.Parameters.AddWithValue("@userID", 13); //replace with user session
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                var clinic = new ClinicScheduleUIModel();
                                clinic.ClinicID = int.Parse(sqlDr["clinicID"].ToString());
                                clinic.UserID = int.Parse(sqlDr["userID"].ToString());
                                clinic.UnitHouseNo = sqlDr["unitHouseNo"].ToString();
                                clinic.Street = sqlDr["street"].ToString();
                                clinic.Baranggay = sqlDr["baranggay"].ToString();
                                clinic.City = sqlDr["city"].ToString();
                                clinic.ClinicName = sqlDr["clinicname"].ToString();
                                record.ClinicScheduleUIModels.Add(clinic);
                            }
                        }
                    }

                }
                sqlCon.Close();
            }
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                //Change later with claculations to nearest clinic
                foreach (ClinicScheduleUIModel model in record.ClinicScheduleUIModels)
                {
                    string query = @"SELECT s.scheduleId, s.userID, s.date, s.status, s.clinicID FROM Schedule s INNER JOIN Clinic c ON c.clinicID = s.clinicID WHERE s.clinicID = @clinicID AND s.status = 0 ORDER BY s.date ASC";
                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                    {
                        //Add long lat params from gmaps
                        sqlCmd.Parameters.AddWithValue("@clinicID", model.ClinicID); //replace with user session
                        using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                        {
                            if (sqlDr.HasRows)
                            {
                                while (sqlDr.Read())
                                {
                                    var schedule = new ScheduleModel();
                                    schedule.ScheduleID = int.Parse(sqlDr["scheduleID"].ToString());
                                    schedule.UserID = int.Parse(sqlDr["userID"].ToString());
                                    schedule.Date = DateTime.Parse(sqlDr["date"].ToString());
                                    schedule.Status = sqlDr["status"].ToString();
                                    model.ScheduleModels.Add(schedule);
                                }
                            }
                        }
                    }
                }

                sqlCon.Close();
            }
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                //Change later with claculations to nearest clinic
                foreach (ClinicScheduleUIModel model in record.ClinicScheduleUIModels)
                {
                    string query = @"SELECT petID, userID, petName FROM Pet WHERE userID = @userID";
                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                    {
                        sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY].ToString());
                        using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                        {
                            if (sqlDr.HasRows)
                            {
                                while (sqlDr.Read())
                                {
                                    var PetModel = new PetModel();
                                    PetModel.PetID = int.Parse(sqlDr["petID"].ToString());
                                    PetModel.UserID = int.Parse(sqlDr["userID"].ToString());
                                    PetModel.PetName = sqlDr["petName"].ToString();
                                    model.PetModels.Add(PetModel);
                                }
                            }
                        }
                    }
                }

                sqlCon.Close();
            }
            return View(record);
        }

        [HttpPost]
        public ActionResult ClinicSchedules(ClientViewScheduleModel record)
        {
            Debug.WriteLine("Selected PET: " + record.SelectedPetId);
            Debug.WriteLine("SELECTED SCHEDULE: " + record.SelectedScheduleID);
            return RedirectToAction("BookAppointment", record);
        }

        public ActionResult BookAppointment(ClientViewScheduleModel record)
        {
            Debug.WriteLine("Selected PET: " + record.SelectedPetId);
            Debug.WriteLine("SELECTED SCHEDULE: " + record.SelectedScheduleID);
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string bookAppointmentQuery = @"INSERT INTO MyAppointment VALUES(@scheduleID, @userID, @specificProblem)";

                using (SqlCommand sqlCmd = new SqlCommand(bookAppointmentQuery, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@scheduleID", record.SelectedScheduleID);
                    sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY].ToString());
                    sqlCmd.Parameters.AddWithValue("@specificProblem", record.SpecificProblem);
                    sqlCmd.ExecuteNonQuery();
                }

                string scheduleUpdateQuery = @"UPDATE Schedule SET status = 1, petID = @petID WHERE scheduleID = @scheduleID";
                using (SqlCommand sqlCmd = new SqlCommand(scheduleUpdateQuery, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@scheduleID", record.SelectedScheduleID);
                    sqlCmd.Parameters.AddWithValue("@petID", record.SelectedPetId);
                    sqlCmd.ExecuteNonQuery();
                }
                sqlCon.Close();
            }
            return RedirectToAction("ViewAppointments", "MyAppointment");
        }

        public ActionResult ViewAppointments()
        {
            var ViewAppointmentsModels = new List<ViewAppointmentsModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string ViewAppointmentQuery = @"SELECT m.myAppointmentID, p.petID, p.petName, c.clinicName, c.clinicID, s.scheduleID, s.date, s.status, u.firstName, u.lastName, u.userID 
                                                FROM MyAppointment m  
                                                INNER JOIN Schedule s ON s.scheduleID = m.scheduleID 
                                                INNER JOIN Users u ON u.userID = s.userID 
                                                INNER JOIN Clinic c ON c.clinicID = s.clinicID 
                                                INNER JOIN Pet p ON s.petID = p.petID 
                                                WHERE m.userID = @userID
                                                ORDER BY m.myAppointmentID DESC";
                using (SqlCommand sqlCmd = new SqlCommand(ViewAppointmentQuery, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY].ToString());
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                var ViewAppointmentsModel = new ViewAppointmentsModel();
                                ViewAppointmentsModel.MyAppointmentID = int.Parse(sqlDr["myAppointmentID"].ToString());
                                ViewAppointmentsModel.PetID = int.Parse(sqlDr["petID"].ToString());
                                ViewAppointmentsModel.PetName = sqlDr["petName"].ToString();
                                ViewAppointmentsModel.ClinicName = sqlDr["clinicName"].ToString();
                                ViewAppointmentsModel.ClinicID = int.Parse(sqlDr["clinicID"].ToString());
                                ViewAppointmentsModel.ScheduleID = int.Parse(sqlDr["scheduleID"].ToString());
                                ViewAppointmentsModel.Date = DateTime.Parse(sqlDr["date"].ToString());
                                ViewAppointmentsModel.Status = sqlDr["status"].ToString();
                                ViewAppointmentsModel.FirstName = sqlDr["firstName"].ToString();
                                ViewAppointmentsModel.LastName = sqlDr["lastName"].ToString();
                                ViewAppointmentsModel.VetID = int.Parse(sqlDr["userID"].ToString());
                                ViewAppointmentsModels.Add(ViewAppointmentsModel);
                            }
                        }
                    }
                }
            }
            return View(ViewAppointmentsModels);
        }

        public ActionResult CreateFeedback(int id)
        {
            var record = new FeedbackModel();
            record.MyAppointmentID = id;
            return View(record);
        }

        [HttpPost]
        public ActionResult CreateFeedback(FeedbackModel record)
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"INSERT INTO Feedback VALUES(@myAppointmentID, @feedback, @rating, @dateAdded, @dateModified);";
                Debug.WriteLine(record.MyAppointmentID);
                Debug.WriteLine(record.Feedback);
                Debug.WriteLine(record.Rating);
                Debug.WriteLine(record.DateAdded);
                Debug.WriteLine(record.DateModified);

                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@myAppointmentID", record.MyAppointmentID);
                    sqlCmd.Parameters.AddWithValue("@feedback", record.Feedback);
                    sqlCmd.Parameters.AddWithValue("@rating", record.Rating);
                    sqlCmd.Parameters.AddWithValue("@dateAdded", DateTime.Now);
                    sqlCmd.Parameters.AddWithValue("@dateModified", DateTime.Now);
                    sqlCmd.ExecuteNonQuery();

                    return RedirectToAction("MyProfile", "Accounts");// open alert box and send to.. homepage?
                }
            }
        }

        public ActionResult ViewVetDetails(int id, int ScheduleID)
        {
            var record = new ViewVetDetailsModel();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT c.clinicID, c.clinicname, c.unitHouseNo, c.street, c.baranggay, c.city, c.latitude, c.longitude, u.firstName, u.lastName, u.mobileNo, u.email, s.status, u.userID
                                 FROM Clinic c 
                                 INNER JOIN Users u ON u.userID=c.userID
                                 INNER JOIN Schedule s ON s.userID=u.userID
                                 WHERE c.clinicID=@clinicID AND s.scheduleID=@scheduleID AND s.status = 0";

                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    Debug.WriteLine("sheduleID: " + id);
                    Debug.WriteLine("clinicID: " + id);
                    sqlCmd.Parameters.AddWithValue("@scheduleID", ScheduleID);
                    sqlCmd.Parameters.AddWithValue("@clinicID", id);
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                var viewVetDetailsModel = new ViewVetDetailsModel();
                                viewVetDetailsModel.ClinicModel.ClinicID = int.Parse(sqlDr["clinicID"].ToString());
                                viewVetDetailsModel.ClinicModel.ClinicName = sqlDr["clinicname"].ToString();
                                viewVetDetailsModel.ClinicModel.UnitHouseNo = sqlDr["unitHouseNo"].ToString();
                                viewVetDetailsModel.ClinicModel.Street = sqlDr["street"].ToString();
                                viewVetDetailsModel.ClinicModel.Baranggay = sqlDr["baranggay"].ToString();
                                viewVetDetailsModel.ClinicModel.City = sqlDr["city"].ToString();
                                viewVetDetailsModel.ClinicModel.Latitude = sqlDr["latitude"].ToString();
                                viewVetDetailsModel.ClinicModel.Longitude = sqlDr["longitude"].ToString();
                                viewVetDetailsModel.UserModel.FirstName = sqlDr["firstname"].ToString();
                                viewVetDetailsModel.UserModel.LastName = sqlDr["lastname"].ToString();
                                viewVetDetailsModel.UserModel.MobileNo = sqlDr["mobileNo"].ToString();
                                viewVetDetailsModel.UserModel.Email = sqlDr["Email"].ToString();
                                viewVetDetailsModel.UserModel.UserID = int.Parse(sqlDr["userID"].ToString());
                                record = viewVetDetailsModel;
                            }
                        }
                    }
                }
                sqlCon.Close();
            }

            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT serviceName, price
                                 FROM ServiceType
                                 WHERE userID = @userID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", record.UserModel.UserID);
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                var ServiceTypeModel = new ServiceTypeModel();
                                ServiceTypeModel.ServiceName = sqlDr["serviceName"].ToString();
                                ServiceTypeModel.Price = decimal.Parse(sqlDr["price"].ToString());
                                record.ServiceTypeModels.Add(ServiceTypeModel);
                            }
                        }
                    }
                }
                sqlCon.Close();
            }

            return View(record);
        }
    }
}