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
    
    public partial class DiscountTransaction
    {
        public string DiscountID { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public string CardID { get; set; }
        public string Username { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
    }
}