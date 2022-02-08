using System;

namespace DCAF.Inspection
{
    public class RollCallEntry
    {
        public string Role { get; set; }

        public string Spec { get; set; }

        public string Name { get; set; }

        public string Id { get; set; }

        public DateTime TimeStamp { get; set; }

        public MemberStatus Status { get; set; }
    }
}