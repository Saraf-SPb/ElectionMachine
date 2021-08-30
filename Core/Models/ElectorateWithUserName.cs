using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionMachine.Core.Models
{
    [NotMapped]
    public class ElectorateWithUserName : DB.Electorate
    {        
        public string UserName { get; set; }
    }
}
