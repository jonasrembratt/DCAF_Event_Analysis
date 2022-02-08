using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DCAF.Inspection._lib;

namespace DCAF.Inspection
{
    public class RollCallCollection : IEnumerable<RollCall>
    {
        readonly List<RollCall> _rollCalls;
        public IEnumerator<RollCall> GetEnumerator() => _rollCalls.GetEnumerator();

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

        public static async Task<Outcome<RollCallCollection>> LoadFromAsync(FileInfo file)
        {
            if (!file.Exists)
                return Outcome<RollCallCollection>.Fail(
                    new FileNotFoundException($"Could not find roll call file: {file.FullName}"));

            var csv = await File.ReadAllLinesAsync(file.FullName);
            var parser = new RollCallCollectionCsvParser();
            Outcome<RollCallCollection> outcome = parser.ParseCsv(csv);
            if (!outcome)
                return Outcome<RollCallCollection>.Fail(outcome.Exception!);

            var rollCalls = new RollCallCollection(new List<RollCall>(outcome.Value!));
            return Outcome<RollCallCollection>.Success(rollCalls);
        }

        public RollCallCollection(List<RollCall> rollCalls)
        {
            _rollCalls = rollCalls;
        }
        
    }
}