using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.ResponseClass
{
    public class CardDebitRequest
    {
        [Required]
        public string TransactionID { get; set; }
        [Required]
        public System.DateTime TimeStamp { get; set; }
        [Required]
        public string CardID { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Remarks { get; set; }
    }
}