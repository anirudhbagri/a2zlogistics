//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FinalProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transaction
    {
        public string TransactionID { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public string Username { get; set; }
        public string CardID { get; set; }
        public decimal Amount { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }
        public string Type { get; set; }
        public string CardType { get; set; }
        public string VehicleNumber { get; set; }
    }
}
