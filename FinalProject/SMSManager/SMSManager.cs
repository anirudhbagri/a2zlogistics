using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
namespace FinalProject.SMSManager
{
    public class SMSManager
    {
        public static string sendSMS()
        {
            String message = HttpUtility.UrlEncode("Welcome to A2ZFamily");
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                {
                {"apikey" , "pNJVVCe4KwQ-kEnWCH70HWE8d4PUpiCBqU1jDLM00H"},
                {"numbers" , "8290430970"},
                {"message" , message},
                {"sender" , "AZLGST"}
                });
                string result = System.Text.Encoding.UTF8.GetString(response);
                return result;
            }
        }
    }
}