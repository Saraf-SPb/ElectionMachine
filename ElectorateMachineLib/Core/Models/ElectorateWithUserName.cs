using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionMachine.Core.Models
{
    [NotMapped]
    public class ElectorateWithUserName
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public DateTime CreateDate { get; set; }
        public int UserId { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
    }
}
