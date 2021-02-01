﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Utilities
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private readonly string email;

        public ValidEmailDomainAttribute(string email)
        {
            this.email = email;
        }

        public override bool IsValid(object value)
        {
            string emailDomain =  value.ToString().Split("@")[1];
            return emailDomain.ToLower() == email.ToLower(); 
        }
    }
}
