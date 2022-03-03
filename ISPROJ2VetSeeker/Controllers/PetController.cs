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
    public class PetController : Controller
    {
        // GET: Pet
        public List<UserModel> GetUsers()
        {
            var list = new List<UserModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT userID, firstName, lastName, dateAdded, dateModified FROM Users";

                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            list.Add(new UserModel
                            {
                                UserID = long.Parse(sqlDr["userID"].ToString()),
                                FirstName = sqlDr["firstName"].ToString(),
                                LastName = sqlDr["lastName"].ToString(),
                                DateAdded = DateTime.Parse(sqlDr["dateAdded"].ToString()),
                                DateModified = DateTime.Parse(sqlDr["dateModified"].ToString())
                            });
                        }
                        return list;
                    }
                }
            }
        }

        public ActionResult PetRegister()
        {
            var record = new PetModel();
            record.Users = GetUsers();
            return View(record);
        }

        [HttpPost]
        public ActionResult PetRegister(PetModel record)
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"INSERT INTO Pet VALUES(@userID, @petName, @breed, @sex, @color, @dateOfBirth, @petChipNo, @guardian, @petProfilePic, @dateAdded, @dateModified)";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY]);
                    sqlCmd.Parameters.AddWithValue("@petName", record.PetName);
                    sqlCmd.Parameters.AddWithValue("@breed", record.Breed);
                    sqlCmd.Parameters.AddWithValue("@sex", record.Sex);
                    sqlCmd.Parameters.AddWithValue("@color", record.Color);
                    sqlCmd.Parameters.AddWithValue("@dateOfBirth", record.Birthday);
                    sqlCmd.Parameters.AddWithValue("@petChipNo", record.PetChipNo);
                    sqlCmd.Parameters.AddWithValue("@guardian", record.Guardian);

                    byte[] petProfilePic = new byte[32];
                    sqlCmd.Parameters.AddWithValue("@petProfilePic", petProfilePic);
                    sqlCmd.Parameters.AddWithValue("@dateAdded", DateTime.Now);
                    sqlCmd.Parameters.AddWithValue("@dateModified", DateTime.Now);
                    sqlCmd.ExecuteNonQuery();

                    return RedirectToAction("MyProfile", "Accounts");
                }
            }
            // return View(GetPets());
        }

        public ActionResult ViewPets()
        {
            var userPets = new List<PetModel>();
            using ( SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT petID, userID, petName, breed, sex, color, dateOfBirth, petChipNo, guardian, petProfilePic, dateAdded, dateModified FROM Pet WHERE userID = @userID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY].ToString());
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                var petModel = new PetModel();
                                petModel.PetID = int.Parse(sqlDr["petID"].ToString());
                                petModel.UserID = int.Parse(sqlDr["userID"].ToString());
                                petModel.PetName = sqlDr["petName"].ToString();
                                petModel.Breed = sqlDr["breed"].ToString();
                                petModel.Sex = sqlDr["sex"].ToString();
                                petModel.Color = sqlDr["color"].ToString();
                                petModel.Birthday = DateTime.Parse(sqlDr["dateOfBirth"].ToString());
                                petModel.PetChipNo = sqlDr["petChipNo"].ToString();
                                petModel.Guardian = sqlDr["guardian"].ToString();
                                //petModel.PetProfilePic = byte[].parse(sqlDr["petProfilePic"].ToString());
                                petModel.DateAdded = DateTime.Parse(sqlDr["dateAdded"].ToString());
                                petModel.DateModified = DateTime.Parse(sqlDr["dateModified"].ToString());
                                userPets.Add(petModel);
                            }
                        }
                    }
                }
            }
            return View(userPets);
        }

        public ActionResult UpdatePetProfile(int petID)
        {
            var record = new PetModel();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT petID, userID, petName, breed, sex, color, dateOfBirth, petChipNo, guardian, petProfilePic, dateAdded, dateModified FROM Pet WHERE userID = @userID AND petID = @petID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", Session[Helper.USER_ID_KEY].ToString());
                    sqlCmd.Parameters.AddWithValue("@petID", petID);
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                record.PetID = int.Parse(sqlDr["petID"].ToString());
                                record.UserID = int.Parse(sqlDr["userID"].ToString());
                                record.PetName = sqlDr["petName"].ToString();
                                record.Breed = sqlDr["breed"].ToString();
                                record.Sex = sqlDr["sex"].ToString();
                                record.Color = sqlDr["color"].ToString();
                                record.Birthday = DateTime.Parse(sqlDr["dateOfBirth"].ToString());
                                record.PetChipNo = sqlDr["petChipNo"].ToString();
                                record.Guardian = sqlDr["guardian"].ToString();
                                //record.PetProfilePic = byte[].parse(sqlDr["petProfilePic"].ToString());
                                record.DateAdded = DateTime.Parse(sqlDr["dateAdded"].ToString());
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
        public ActionResult UpdatePetProfile(PetModel record)
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"UPDATE Pet SET petName=@petName, breed=@breed, sex=@sex, color=@color, dateOfBirth=@dateOfBirth, petChipNo=@petChipNo, guardian=@guardian, petProfilePic=@petProfilePic, dateModified=@dateModified WHERE userID = @userID and petID = @petID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@petName", record.PetName);
                    sqlCmd.Parameters.AddWithValue("@breed", record.Breed);
                    sqlCmd.Parameters.AddWithValue("@sex", record.Sex);
                    sqlCmd.Parameters.AddWithValue("@color", record.Color);
                    sqlCmd.Parameters.AddWithValue("@dateOfBirth", record.Birthday);
                    sqlCmd.Parameters.AddWithValue("@petChipNo", record.PetChipNo);
                    sqlCmd.Parameters.AddWithValue("@guardian", record.Guardian);

                    byte[] petProfilePic = new byte[32];
                    sqlCmd.Parameters.AddWithValue("@petProfilePic", petProfilePic);
                    sqlCmd.Parameters.AddWithValue("@dateModified", DateTime.Now);
                    sqlCmd.Parameters.AddWithValue("@userID", record.UserID);
                    sqlCmd.Parameters.AddWithValue("@petID", record.PetID);
                    sqlCmd.ExecuteNonQuery();
                }
                sqlCon.Close();
            }
            return RedirectToAction("ViewPets", "Pet");
        }

        public ActionResult ViewMedicalHistory(int PetID)
        {
            var ViewMedicalHistoryModel = new ViewMedicalHistoryModel();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {

                sqlCon.Open();
                string query = @"SELECT m.medicalHistoryID, m.petID, m.serviceTypeID, m.diagnosis, s.serviceName, u.userID, u.firstName, u.lastName, u.mobileNo, u.email, p.petName
                                 FROM MedicalHistory m 
                                 INNER JOIN serviceType s ON s.serviceTypeID = m.serviceTypeID 
                                 INNER JOIN users u ON u.userID = s.userID 
                                 INNER JOIN pet p ON p.petID = m.petID 
                                 WHERE m.petID = @petID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@petID", PetID.ToString());
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                ViewMedicalHistoryModel.PetID = int.Parse(sqlDr["petID"].ToString());
                                ViewMedicalHistoryModel.MedicalHistoryID = int.Parse(sqlDr["medicalHistoryID"].ToString());
                                ViewMedicalHistoryModel.ServiceTypeID = int.Parse(sqlDr["serviceTypeID"].ToString());
                                ViewMedicalHistoryModel.Diagnosis = sqlDr["diagnosis"].ToString();
                                ViewMedicalHistoryModel.ServiceName = sqlDr["serviceName"].ToString();
                                ViewMedicalHistoryModel.UserID = int.Parse(sqlDr["userID"].ToString());
                                ViewMedicalHistoryModel.FirstName = sqlDr["firstName"].ToString();
                                ViewMedicalHistoryModel.LastName = sqlDr["lastName"].ToString();
                                ViewMedicalHistoryModel.MobileNo = sqlDr["mobileNo"].ToString();
                                ViewMedicalHistoryModel.Email = sqlDr["email"].ToString();
                                ViewMedicalHistoryModel.PetName = sqlDr["petName"].ToString();
                            }
                        }
                    }
                }
                sqlCon.Close();
            }
            return View(ViewMedicalHistoryModel);
        }

    }
}