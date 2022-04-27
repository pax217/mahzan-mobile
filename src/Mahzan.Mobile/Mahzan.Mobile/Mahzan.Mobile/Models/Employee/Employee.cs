using System;

namespace Mahzan.Mobile.Models.Employee
{
    public class Employee
    {
        public Guid EmployeeId { get; set; }
    
        public string Code { get; set; }
    
        public string FirstName { get; set; }
    
        public string SecondName { get; set; }
    
        public string LastName { get; set; }
    
        public string SureName { get; set; }
    
        public string Email { get; set; }
    
        public string Phone { get; set; }
    
        public Guid MemberId { get; set; }
    }
}