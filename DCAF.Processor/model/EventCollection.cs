using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DCAF.Inspection._lib;

namespace DCAF.Inspection
{
    public class EventCollection : IEnumerable<Event>
    {
        readonly List<Event> _rollCalls;
        public IEnumerator<Event> GetEnumerator() => _rollCalls.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            var first = _rollCalls.FirstOrDefault();
            if (first is null)
                return "(none)";
            
            var last = _rollCalls.LastOrDefault();
            return first == last 
                ? $"{first.DateTime:u} (one roll call)" 
                : $"{first.DateTime:u} -- {last!.DateTime:u} ({_rollCalls.Count} events)";
        }

        public static async Task<Outcome<EventCollection>> LoadFromAsync(FileInfo file)
        {
            if (!file.Exists)
                return Outcome<EventCollection>.Fail(
                    new FileNotFoundException($"Could not find roll call file: {file.FullName}"));

            var csv = await File.ReadAllLinesAsync(file.FullName);
            var parser = new RollCallCollectionCsvParser();
            Outcome<EventCollection> outcome = parser.ParseCsv(csv);
            if (!outcome)
                return Outcome<EventCollection>.Fail(outcome.Exception!);

            var rollCalls = new EventCollection(new List<Event>(outcome.Value!));
            return Outcome<EventCollection>.Success(rollCalls);
        }

        public EventCollection(List<Event> rollCalls)
        {
            _rollCalls = rollCalls;
        }
        
    }
}