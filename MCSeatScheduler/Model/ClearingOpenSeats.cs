using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MCSeatScheduler.Model
{
    public partial class OpenSeats
    {
		[Required]
		[DisplayName("Date")]
		public DateTime Date { get; set; }
		
		[DisplayName("Employee ID")]
		public string EmployeeId { get; set; }
    }
}
