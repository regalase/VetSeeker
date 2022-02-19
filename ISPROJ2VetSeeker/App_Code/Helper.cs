using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;

namespace ISPROJ2VetSeeker.App_Code
{
    public class Helper
    {
        public const string USER_ID_KEY = "userID";
        public const string TYPE_ID_KEY = "typeID";

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