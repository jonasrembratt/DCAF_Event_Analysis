using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace DCAF.Inspection
{
    [DebuggerDisplay("{ToString()}")]
    public class RollCall : IEnumerable<RollCallEntry>
    {
        List<RollCallEntry> _entries;

        public int Count => _entries.Count;
        
        public string Name { get; set; }
        
        public DateTime DateTime { get; set; }

        public override string ToString() => $"{Name} {DateTime:u} ({Count})";

        public IEnumerator<RollCallEntry> GetEnumerator() => _entries.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_entries).GetEnumerator();

        public RollCall(string name, DateTime dateTime, List<RollCallEntry> entries)
        {
            Name = name;
            DateTime = dateTime;
            _entries = entries;
        }
    }
}