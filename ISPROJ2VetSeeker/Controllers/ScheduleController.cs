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
using GMap;

namespace ISPROJ2VetSeeker.Controllers
{
    public class ScheduleController : Controller
    {
        // GET: Schedule
        public ActionResult CreateSchedule()
        {
            var record = new CreateScheduleUIModel();
            //query clinics by user logged in;
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT clinicId, userId, unitHouseNo, street, baranggay, city, clinicname FROM Clinic WHERE userId = @userID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY]); //replace with user session
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                var clinic = new ClinicModel();
                                clinic.ClinicID = int.Parse(sqlDr["clinicID"].ToString());
                                clinic.UserID = int.Parse(sqlDr["userID"].ToString());
                                clinic.UnitHouseNo = sqlDr["unitHouseNo"].ToString();
                                clinic.Street = sqlDr["street"].ToString();
                                clinic.Baranggay = sqlDr["baranggay"].ToString();
                                clinic.City = sqlDr["city"].ToString();
                                clinic.ClinicName = sqlDr["clinicname"].ToString();
                                record.ClinicModels.Add(clinic);
                            }
                        }
                    }
                }

            }
            return View(record);
        }

        [HttpPost]
        public ActionResult CreateSchedule(CreateScheduleUIModel record)
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                Debug.WriteLine(record.ScheduleModel.ClinicID);
                Debug.WriteLine(record.ScheduleModel.Date);
                sqlCon.Open();
                string query = @"INSERT INTO Schedule VALUES(@userID, @petID, @date, @status, @clinicID)";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY]);
                    sqlCmd.Parameters.AddWithValue("@petID", DBNull.Value);
                    sqlCmd.Parameters.AddWithValue("@date", record.ScheduleModel.Date);
                    sqlCmd.Parameters.AddWithValue("@status", 0);
                    sqlCmd.Parameters.AddWithValue("@clinicID", record.ScheduleModel.ClinicID);

                    sqlCmd.ExecuteNonQuery();

                    return RedirectToAction("VetProfile", "Accounts");// SHOULD BE CHANGED, NOT SURE TO WHAT
                }
            }
        }

        public ActionResult ViewSchedules()
        {
            var record = new List<ViewScheduleUIModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT scheduleID, s.userID, date, status, s.clinicID, clinicName FROM Schedule s INNER JOIN Clinic c ON c.clinicID = s.clinicID WHERE s.userId = @userId";

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
                                viewScheduleUIModel.ScheduleId = int.Parse(sqlDr["scheduleID"].ToString());
                                viewScheduleUIModel.UserID = int.Parse(sqlDr["userID"].ToString());
                                viewScheduleUIModel.Date = DateTime.Parse(sqlDr["date"].ToString());
                                viewScheduleUIModel.Status = sqlDr["status"].ToString();
                                viewScheduleUIModel.ClinicId = int.Parse(sqlDr["clinicID"].ToString());
                                viewScheduleUIModel.ClinicName = sqlDr["clinicName"].ToString();
                                record.Add(viewScheduleUIModel);
                            }
                        }
                    }
                }
            }

            return View(record);
        }

        public ActionResult ManageSchedules(int id, string status)
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string scheduleUpdateQuery = @"UPDATE Schedule SET status = @status WHERE scheduleID = @scheduleID";
                using (SqlCommand sqlCmd = new SqlCommand(scheduleUpdateQuery, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@status", status);
                    sqlCmd.Parameters.AddWithValue("@scheduleID", id);
                    sqlCmd.ExecuteNonQuery();
                }
                sqlCon.Close();
            }
            return RedirectToAction("VetProfile", "Accounts");
        }

        public ActionResult CompleteSchedule(int id, string status)
        {
            CompleteScheduleModel completeScheduleModel = new CompleteScheduleModel();

            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string getScheduleQuery = @"SELECT scheduleID, userID, petID, date, status, clinicID FROM Schedule WHERE scheduleID = @scheduleID";
                using (SqlCommand sqlCmd = new SqlCommand(getScheduleQuery, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@scheduleID", id);
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                completeScheduleModel.ScheduleModel.ScheduleID = int.Parse(sqlDr["scheduleID"].ToString());
                                completeScheduleModel.ScheduleModel.UserID = int.Parse(sqlDr["userID"].ToString());
                                completeScheduleModel.ScheduleModel.PetID = int.Parse(sqlDr["petID"].ToString());
                                completeScheduleModel.ScheduleModel.Date = DateTime.Parse(sqlDr["date"].ToString());
                                completeScheduleModel.ScheduleModel.Status = sqlDr["status"].ToString();
                                completeScheduleModel.ScheduleModel.ClinicID = int.Parse(sqlDr["clinicID"].ToString());
                            }
                        }
                    }
                }
                sqlCon.Close();
            }

            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string getServiceTypesQuery = @"SELECT serviceTypeID, serviceName, price FROM ServiceType WHERE userID = @userID";
                using (SqlCommand sqlCmd = new SqlCommand(getServiceTypesQuery, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY].ToString());
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                var ServiceTypeModel = new ServiceTypeModel();
                                ServiceTypeModel.ServiceTypeID = int.Parse(sqlDr["serviceTypeID"].ToString());
                                ServiceTypeModel.ServiceName = sqlDr["serviceName"].ToString();
                                ServiceTypeModel.Price = decimal.Parse(sqlDr["price"].ToString());

                                completeScheduleModel.ServiceTypeModels.Add(ServiceTypeModel);
                            }
                        }
                    }
                }
                sqlCon.Close();
            }

            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string getPetQuery = @"SELECT petID, userID, petName, breed, sex, color, dateOfBirth, petChipNo, guardian, petProfilePic, dateAdded, dateModified FROM Pet WHERE petId = @petID";
                using (SqlCommand sqlCmd = new SqlCommand(getPetQuery, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@petID", completeScheduleModel.ScheduleModel.PetID);
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                completeScheduleModel.PetModel.PetID = int.Parse(sqlDr["petID"].ToString());
                                completeScheduleModel.PetModel.UserID = int.Parse(sqlDr["userID"].ToString());
                                completeScheduleModel.PetModel.PetName = sqlDr["petName"].ToString();
                                completeScheduleModel.PetModel.Breed = sqlDr["breed"].ToString();
                                completeScheduleModel.PetModel.Sex = sqlDr["sex"].ToString();
                                completeScheduleModel.PetModel.Color = sqlDr["color"].ToString();
                                completeScheduleModel.PetModel.Birthday = DateTime.Parse(sqlDr["dateOfBirth"].ToString());
                                completeScheduleModel.PetModel.PetChipNo = sqlDr["petChipNo"].ToString();
                                completeScheduleModel.PetModel.Guardian = sqlDr["guardian"].ToString();
                                //completeScheduleModel.PetModel.PetProfilePic = sqlDr["petProfilePic"].ToString();
                                completeScheduleModel.PetModel.DateAdded = DateTime.Parse(sqlDr["dateAdded"].ToString());
                                completeScheduleModel.PetModel.DateModified = DateTime.Parse(sqlDr["dateModified"].ToString());
                            }
                        }
                    }
                }
                sqlCon.Close();
            }

            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string getScheduleQuery = @"SELECT myAppointmentID FROM MyAppointment WHERE scheduleID = @scheduleID";
                using (SqlCommand sqlCmd = new SqlCommand(getScheduleQuery, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@scheduleID", id);
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                completeScheduleModel.MyAppointmentID = int.Parse(sqlDr["myAppointmentID"].ToString());
                            }
                        }
                    }
                }
                sqlCon.Close();
            }
            Debug.WriteLine(completeScheduleModel + " " + completeScheduleModel.ScheduleModel.Date);
            return View(completeScheduleModel);
        }

        [HttpPost]
        public ActionResult CompleteSchedule(CompleteScheduleModel model)
        {
            //TODO: Invoice
            //TODO: Medical History
            int CreatedMedicalHistoryID = 0;
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"INSERT INTO MedicalHistory VALUES(@petID, @serviceTypeID, @diagnosis); SELECT SCOPE_IDENTITY();";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    Debug.WriteLine(model.SelectedPetID);
                    sqlCmd.Parameters.AddWithValue("@petID", model.SelectedPetID);
                    sqlCmd.Parameters.AddWithValue("@serviceTypeID", model.SelectedService.ServiceTypeID);
                    sqlCmd.Parameters.AddWithValue("@diagnosis", model.Diagnosis);

                    CreatedMedicalHistoryID = Convert.ToInt32(sqlCmd.ExecuteScalar());
                    Debug.WriteLine("CreatedMedicalHistoryID: " + CreatedMedicalHistoryID);
                    

                }
                sqlCon.Close();
            }
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon())) //fix date
            {
                sqlCon.Open();
                string query = @"INSERT INTO Invoice VALUES(@myAppointmentID, @medicalHistoryID, @professionalFee, @date, @totalPrice, @dateAdded, @dateModified)";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    Debug.WriteLine(model.MyAppointmentID);
                    sqlCmd.Parameters.AddWithValue("@myAppointmentID", model.MyAppointmentID);
                    sqlCmd.Parameters.AddWithValue("@medicalHistoryID", CreatedMedicalHistoryID);
                    sqlCmd.Parameters.AddWithValue("@professionalFee", model.ProfessionalFee);
                    sqlCmd.Parameters.AddWithValue("@date", DateTime.Now);
                    sqlCmd.Parameters.AddWithValue("@totalPrice", model.TotalPrice);
                    sqlCmd.Parameters.AddWithValue("@dateAdded", DateTime.Now);
                    sqlCmd.Parameters.AddWithValue("@dateModified", DateTime.Now);

                    sqlCmd.ExecuteNonQuery();

                }
                sqlCon.Close();
            }


            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string scheduleUpdateQuery = @"UPDATE Schedule SET status = 3 WHERE scheduleID = @scheduleID";
                using (SqlCommand sqlCmd = new SqlCommand(scheduleUpdateQuery, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@scheduleID", model.SelectedScheduleID);
                    sqlCmd.ExecuteNonQuery();
                }
                sqlCon.Close();
            }

            return RedirectToAction("VetProfile", "Accounts");
        }
    }
}