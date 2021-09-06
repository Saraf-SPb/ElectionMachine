using System;

namespace ElectionMachine.Core.DB
{
    public class Electorate
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public DateTime CreateDate { get; set; }
        public int UserId { get; set; }
        public string Phone { get; set; }
    }
}
