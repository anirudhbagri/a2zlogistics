﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.ResponseClass
{
    public class UserResponse
    {
        public UserResponse()
        {
            
        }
    
        public string UserName { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public string Mobile_Number { get; set; }
        public decimal Discount { get; set; }
    }
}