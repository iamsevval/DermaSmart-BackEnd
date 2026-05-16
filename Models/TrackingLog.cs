using System;
using System.ComponentModel.DataAnnotations;

namespace DermaSmart.API.Models
{
    public class TrackingLog
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int RoutineStepId { get; set; }

        public DateTime CompletedDate { get; set; }

        public bool IsCompleted { get; set; }
    }
}
