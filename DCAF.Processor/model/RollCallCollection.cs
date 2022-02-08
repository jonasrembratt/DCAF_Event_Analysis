using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DCAF.Inspection._lib;

namespace DCAF.Inspection
{
    public class RollCallCollection : IEnumerable<RollCall>
    {
        List<RollCall> _rollCalls;
        public IEnumerator<RollCall> GetEnumerator() => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static async Task<Outcome<RollCallCollection>> LoadFrom(FileInfo file)
        {
            if (!file.Exists)
                return Outcome<RollCallCollection>.Fail(
                    new FileNotFoundException($"Could not find roll call file: {file.FullName}"));

            var csv = await File.ReadAllLinesAsync(file.FullName);
            var parser = new RollCallCollectionCsvParser();
            Outcome<IEnumerable<RollCall>> outcome = parser.ParseCsv(csv);
            if (!outcome)
                return Outcome<RollCallCollection>.Fail(outcome.Exception!);

            var rollCalls = new RollCallCollection { _rollCalls = new List<RollCall>(outcome.Value!) };
            return Outcome<RollCallCollection>.Success(rollCalls);
        }
        
    }
}