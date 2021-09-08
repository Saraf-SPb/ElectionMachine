using ElectionMachine.Core.DB;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionMachine.Core.Models
{
    [NotMapped]
    public class ElectorateWithUserName : Electorate
    {
        public string UserName { get; set; }
    }
}
