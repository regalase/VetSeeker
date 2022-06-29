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
using static ISPROJ2VetSeeker.FilterConfig;

namespace ISPROJ2VetSeeker.Controllers
{
    [NoDirectAccess]
    public class MyAppointmentController : Controller
    {
        // GET: MyAppointment
        public ActionResult ClinicSchedules(string searchCity)
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
                    string query = @"SELECT s.scheduleId, s.userID, s.date, s.status, s.clinicID, c.city FROM Schedule s INNER JOIN Clinic c ON c.clinicID = s.clinicID WHERE c.city like '%" + searchCity + "%' AND s.clinicID = @clinicID AND s.status = 0  AND s.date >= @date ORDER BY s.date ASC";
                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                    {
                        //Add long lat params from gmaps
                        sqlCmd.Parameters.AddWithValue("@clinicID", model.ClinicID); //replace with user session
                        sqlCmd.Parameters.AddWithValue("@date", DateTime.Today);
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
                                    schedule.City = sqlDr["city"].ToString();
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

        public ActionResult BookAppointment(int SelectedScheduleID)
        {
            Debug.WriteLine("Selected ScheduleID: " + SelectedScheduleID);
            var record = new ClientViewScheduleModel();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT s.scheduleID, s.userID, s.date, s.status, s.clinicID FROM Schedule s WHERE s.scheduleID=@scheduleID ";

                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@scheduleID", SelectedScheduleID);
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                record.BookedScheduleModel.ScheduleID = int.Parse(sqlDr["scheduleID"].ToString());
                                record.BookedScheduleModel.ClinicID = int.Parse(sqlDr["clinicID"].ToString());
                                record.BookedScheduleModel.UserID = int.Parse(sqlDr["userID"].ToString());
                                record.BookedScheduleModel.Date = DateTime.Parse(sqlDr["date"].ToString());
                                record.BookedScheduleModel.Status = sqlDr["status"].ToString();
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
                                record.PetsForBooking.Add(PetModel);
                            }
                        }
                    }
                }
                sqlCon.Close();
            }
            return View(record);
        }

        [HttpPost]
        public ActionResult BookAppointment(ClientViewScheduleModel record)
        {
            Debug.WriteLine("Selected PET: " + record.SelectedPetId);
            Debug.WriteLine("SELECTED SCHEDULE: " + record.SelectedScheduleID);
            Debug.WriteLine("test: " + record.SpecificProblem);
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
            return RedirectToAction("ViewPending", "MyAppointment");
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
                                                WHERE m.userID = @userID AND s.status = 3 ";
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
                                 WHERE c.clinicID=@clinicID AND s.scheduleID=@scheduleID AND s.status != 3";

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

        public ActionResult ViewPending()
        {
            var record = new List<ViewScheduleUIModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT clinicName, date, s.status, s.scheduleID, s.clinicID
                                 FROM Schedule s 
                                 INNER JOIN Clinic c ON c.clinicID = s.clinicID 
                                 INNER JOIN MyAppointment ma ON ma.scheduleID = s.scheduleID 
                                 WHERE ma.userID = @userID AND s.status = 1 OR s.status = 2 ";

                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userId", Session[Helper.USER_ID_KEY]);
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                var viewScheduleUIModel = new ViewScheduleUIModel();
                                viewScheduleUIModel.ClinicName = sqlDr["clinicName"].ToString();
                                viewScheduleUIModel.Date = DateTime.Parse(sqlDr["date"].ToString());
                                viewScheduleUIModel.Status = sqlDr["status"].ToString();
                                viewScheduleUIModel.ScheduleId = int.Parse(sqlDr["scheduleID"].ToString());
                                viewScheduleUIModel.ClinicId = int.Parse(sqlDr["clinicID"].ToString());
                                record.Add(viewScheduleUIModel);
                            }
                        }
                    }
                }
            }

            return View(record);
        }

        public ActionResult ListofAllServices(string search)
        {
            var list = new List<ServiceTypeModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT s.serviceTypeID, s.userID, s.serviceName, s.serviceDescription, s.price, c.clinicName FROM ServiceType s
                                    INNER JOIN Clinic c ON c.userID = s.userID WHERE s.serviceName like '%" + search + "%'";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
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
                                Price = decimal.Parse(sqlDr["price"].ToString()),
                                ClinicName = sqlDr["clinicName"].ToString()
                            });
                        }
                    }
                }
                sqlCon.Close();
            }
            return View(list);

        }
    }
}