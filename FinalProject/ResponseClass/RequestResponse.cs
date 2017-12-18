using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.ResponseClass
{
    public class RequestResponse
    {
        public RequestResponse(Request request)
        {
            Amount = request.Amount;
            CardID = request.CardID;
            CardType = request.CardType;
            UserName = request.UserName;
            TimeStamp = request.TimeStamp;
            RequestID = request.RequestID;
            Balance = 0;
        }
        public string CardID { get; set; }
        public string CardType { get; set; }
        public decimal Amount { get; set; }
        public string UserName { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public string RequestID { get; set; }
        
        public decimal Balance { get; set;}

    }
}