using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net; 
using System.Text;
namespace FinalProject.SMSManager
{
    public class SMSSender
    {
        public static string sendSMS(string receiver,string username)
        {
            string sUserID = "anirudhbagri";
            string sApikey = "fyJNNNTECJ87fllWDAZo";
            string sNumber = receiver;
            string sSenderid = "MYTEXT";
            string sMessage = "Your account was created successfully at a2zlogistics. LoginID: "+username+". Please contact support team to know your password.";
            string sType = "txt";
            string sURL = "http://smshorizon.co.in/api/sendsms.php?user=" + sUserID + "&apikey=" + sApikey + "&mobile=" + sNumber + "&message=" + sMessage + "&senderid=" + sSenderid + "&type=" + sType + "";
            return GetResponse(sURL);
        }
        public static string GetResponse(string sURL) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sURL); request.MaximumAutomaticRedirections = 4;
            request.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse (); Stream receiveStream = response.GetResponseStream ();
                StreamReader readStream = new StreamReader (receiveStream, Encoding.UTF8); string sResponse = readStream.ReadToEnd();
                response.Close ();
                readStream.Close ();
                return sResponse;
            }
            catch(Exception)
            {
                return "";
            }
        }
    }
}
