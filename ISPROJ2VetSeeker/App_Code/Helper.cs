using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;
using System.Diagnostics;

namespace ISPROJ2VetSeeker.App_Code
{
    public class Helper
    {
        public const string USER_ID_KEY = "userID";
        public const string TYPE_ID_KEY = "typeID";
        public const string USER_CITY_KEY = "city";
        public const string AUDIT_ID_KEY = "AuditLogID";

        private static Dictionary<String, double> CITY_LONGITUDE_MAP = new Dictionary<String, double>();

        private static Dictionary<String, double> CITY_LATITUDE_MAP = new Dictionary<String, double>();

        public static void init()
        {
            CITY_LONGITUDE_MAP.Add("Makati", 121.0244);
            CITY_LATITUDE_MAP.Add("Makati", 14.5547);
            CITY_LONGITUDE_MAP.Add("Manila", 120.9842);
            CITY_LATITUDE_MAP.Add("Manila", 14.5995);
            CITY_LONGITUDE_MAP.Add("Quezon City", 121.0437);
            CITY_LATITUDE_MAP.Add("Quezon City", 14.6760);
            CITY_LONGITUDE_MAP.Add("Caloocan", 121.0450);
            CITY_LATITUDE_MAP.Add("Caloocan", 14.7566);
            CITY_LONGITUDE_MAP.Add("Pasay", 121.0014);
            CITY_LATITUDE_MAP.Add("Pasay", 14.5378);
            CITY_LONGITUDE_MAP.Add("Pasig", 121.0851);
            CITY_LATITUDE_MAP.Add("Pasig", 14.5764);
            CITY_LONGITUDE_MAP.Add("Taguig", 121.0509);
            CITY_LATITUDE_MAP.Add("Taguig", 14.5176);
            CITY_LONGITUDE_MAP.Add("Navotas", 120.9350);
            CITY_LATITUDE_MAP.Add("Navotas", 14.6732);
            CITY_LONGITUDE_MAP.Add("Muntinlupa", 121.0415);
            CITY_LATITUDE_MAP.Add("Muntinlupa", 14.4081);
            CITY_LONGITUDE_MAP.Add("Mandaluyong", 121.0359);
            CITY_LATITUDE_MAP.Add("Mandaluyong", 14.5794);
            CITY_LONGITUDE_MAP.Add("Valenzuela", 120.986542);
            CITY_LATITUDE_MAP.Add("Valenzuela", 14.703580);
            CITY_LONGITUDE_MAP.Add("Malabon", 120.9658);
            CITY_LATITUDE_MAP.Add("Malabon", 14.6681);
            CITY_LONGITUDE_MAP.Add("San Juan", 121.0355);
            CITY_LATITUDE_MAP.Add("San Juan", 14.6019);
            CITY_LONGITUDE_MAP.Add("Marikina", 121.1029);
            CITY_LATITUDE_MAP.Add("Marikina", 14.6507);
            CITY_LONGITUDE_MAP.Add("Pateros", 121.0687);
            CITY_LATITUDE_MAP.Add("Pateros", 14.5454);
            CITY_LONGITUDE_MAP.Add("Paranaque", 121.0198);
            CITY_LATITUDE_MAP.Add("Paranaque", 14.4793);
            CITY_LONGITUDE_MAP.Add("Bonifacio Global City", 121.0503);
            CITY_LATITUDE_MAP.Add("Bonifacio Global City", 14.5409);

        }

        public static string GetCon()
        {
            return ConfigurationManager.ConnectionStrings["MyCon"].ConnectionString;
        }

        /// <summary>
        /// Returns a hash value using a secured hash algorithm (SHA-2)
        /// </summary>
        public static string Hash(string phrase)
        {
            SHA512Managed HashTool = new SHA512Managed();
            Byte[] PhraseAsByte = System.Text.Encoding.UTF8.GetBytes(string.Concat(phrase));
            Byte[] EncryptedBytes = HashTool.ComputeHash(PhraseAsByte);
            HashTool.Clear();
            return Convert.ToBase64String(EncryptedBytes);
        }

        public static double GetCityLongitude(String city)
        {
            return CITY_LONGITUDE_MAP[city];
        }

        public static double GetCityLatitude(String city)
        {
            return CITY_LATITUDE_MAP[city];
        }


        public static int RecordUserSessionLogin(int UserID, int TypeID)
        {
            Debug.WriteLine(UserID);
            Debug.WriteLine(TypeID);
            int AuditLogID = 0;
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"INSERT INTO UserSessionLog VALUES (@userID, @typeID, @dateOfLogin, @dateOfLogout); SELECT SCOPE_IDENTITY();";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", UserID);
                    sqlCmd.Parameters.AddWithValue("@typeID", TypeID);
                    sqlCmd.Parameters.AddWithValue("@dateOfLogin", DateTime.Now);
                    sqlCmd.Parameters.AddWithValue("@dateOfLogout", DBNull.Value);
                    AuditLogID = Convert.ToInt32(sqlCmd.ExecuteScalar());
                }
            }
            return AuditLogID;
        }

        public static void RecordUserSessionLogout(int UserID, int TypeID, int AuditLogID)
        {
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"UPDATE UserSessionLog SET userID=@userID, typeID=@typeID, dateOfLogout=@dateOfLogout 
                                     WHERE userID = @userID AND auditLogID = @auditLogID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@userID", UserID);
                    sqlCmd.Parameters.AddWithValue("@typeID", TypeID);
                    sqlCmd.Parameters.AddWithValue("@dateOfLogout", DateTime.Now);
                    sqlCmd.Parameters.AddWithValue("@auditLogID", AuditLogID);
                    sqlCmd.ExecuteNonQuery();
                }
            }
        }

        public static void SendEmail(string email, string subject, string message)
        {
            MailMessage emailMessage = new MailMessage();
            emailMessage.From = new MailAddress("benilde.web.development@gmail.com", "no-reply");
            emailMessage.To.Add(new MailAddress(email));
            emailMessage.Subject = subject;
            emailMessage.Body = message;
            emailMessage.IsBodyHtml = true;
            emailMessage.Priority = MailPriority.Normal;
            SmtpClient MailClient = new SmtpClient("smtp.gmail.com", 587);
            MailClient.EnableSsl = true;
            MailClient.Credentials = new System.Net.NetworkCredential("benilde.web.development@gmail.com", "!thisisalongpassword1234567890");
            MailClient.Send(emailMessage);
        }

        //favorite
        public static double GetPrice(string productID)
        {
            using (SqlConnection sqlCon = new SqlConnection(GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT price FROM products WHERE productID=@productID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@productID", productID);
                    return Convert.ToDouble((decimal)sqlCmd.ExecuteScalar());
                }
            }
        }

        public static int CountRecords(string table, string status)
        {
            using (SqlConnection sqlCon = new SqlConnection(GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT COUNT(*) FROM " + table + " WHERE Status=@Status";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@Status", status);
                    return (int)sqlCmd.ExecuteScalar(); // object typecasting
                }
            }
        }
        public static int CountRecord(string table)
        {
            using (SqlConnection sqlCon = new SqlConnection(GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT COUNT(*) FROM " + table;
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    return (int)sqlCmd.ExecuteScalar(); // object typecasting
                }
            }
        }

        public static bool IsExisting(string productID)
        {
            using (SqlConnection sqlCon = new SqlConnection(GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT productID FROM orderDetails WHERE orderNo=@orderNo AND userID=@userID AND productID=@productID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@orderNo", 0);
                    sqlCmd.Parameters.AddWithValue("@userID", 1); //HttpContext.Current.Session["userid"].ToString()
                    sqlCmd.Parameters.AddWithValue("@productID", productID);
                    return sqlCmd.ExecuteScalar() == null ? false : true;
                }
            }
        }

        public static void AddToCartRecord(string productID, int quantity)
        {
            using (SqlConnection sqlCon = new SqlConnection(GetCon()))
            {
                sqlCon.Open();
                string query = "";
                if (IsExisting(productID))
                {
                    query = @"UPDATE OrderDetails SET Quantity = Quantity + @Quantity, Amount = Amount + @Amount 
                                    WHERE orderNo=@orderNo AND userID=@userID AND productID=@productID";
                }
                else
                {
                    query = @"INSERT INTO OrderDetails VALUES (@orderNo, @userID, @productID, @quantity, @amount, @status)";
                }
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@orderNo", 0);
                    sqlCmd.Parameters.AddWithValue("@userID", 1); //HttpContext.Current.Session["userid"].ToString()
                    sqlCmd.Parameters.AddWithValue("@productID", productID);
                    sqlCmd.Parameters.AddWithValue("@quantity", quantity);
                    sqlCmd.Parameters.AddWithValue("@amount", GetPrice(productID) * quantity);
                    sqlCmd.Parameters.AddWithValue("@status", "In Cart");
                    sqlCmd.ExecuteNonQuery();
                }
            }
        }
        public static void ValidateUserForAdmin(string typeID)
        {
            if (HttpContext.Current.Session["typeid"] == null)
            {
                HttpContext.Current.Response.Redirect("~/Users/Index");
            }
            else
            {
                if (HttpContext.Current.Session["typeid"].ToString() != typeID)
                {
                    HttpContext.Current.Response.Redirect("~/Accounts/MyProfile");
                }
            }
        }

        public static string ToRelativeDate(DateTime input)
        {
            TimeSpan oSpan = DateTime.Now.Subtract(input);
            double TotalMinutes = oSpan.TotalMinutes;
            string Suffix = " ago";

            if (TotalMinutes < 0.0)
            {
                TotalMinutes = Math.Abs(TotalMinutes);
                Suffix = " from now";
            }

            var aValue = new SortedList<double, Func<string>>();
            aValue.Add(0.75, () => "few seconds");
            aValue.Add(1.5, () => "1 min");
            aValue.Add(45, () => string.Format("{0} min", Math.Round(TotalMinutes)));
            aValue.Add(90, () => "an hour");
            aValue.Add(1440, () => string.Format("{0} hours", Math.Round(Math.Abs(oSpan.TotalHours)))); // 60 * 24
            aValue.Add(2880, () => "a day"); // 60 * 48
            aValue.Add(43200, () => string.Format("{0} days", Math.Floor(Math.Abs(oSpan.TotalDays)))); // 60 * 24 * 30
            aValue.Add(86400, () => "a month"); // 60 * 24 * 60
            aValue.Add(525600, () => string.Format("{0} months", Math.Floor(Math.Abs(oSpan.TotalDays / 30)))); // 60 * 24 * 365 
            aValue.Add(1051200, () => "a year"); // 60 * 24 * 365 * 2
            aValue.Add(double.MaxValue, () => string.Format("{0} years", Math.Floor(Math.Abs(oSpan.TotalDays / 365))));

            return aValue.First(n => TotalMinutes < n.Key).Value.Invoke() + Suffix;
        }

    }
}