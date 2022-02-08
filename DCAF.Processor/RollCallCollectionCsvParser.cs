using System;
using System.Collections.Generic;
using System.Globalization;
using DCAF.Inspection._lib;

namespace DCAF.Inspection
{
    public class RollCallCollectionCsvParser
    {
        const char Separator = ',';
        const string StartIdent = "-- start --";
        const string EndIdent = "-- start --";
        const string HeaderColumns = "Name,Date,Time";
        const string RollCallColumns = "Role,Spec,Name,ID,Timestamp,Status";
        public Outcome<IEnumerable<RollCall>> ParseCsv(string[] lines)
        {
            string line = null!;
            var list = new List<RollCall>();
            for (var i = 0; i < lines.Length; i++)
            {
                skipToAfter(HeaderColumns);
                if (!parseRollCallMetadata(out var name, out var dateTime))
                    return Outcome<IEnumerable<RollCall>>.Fail(
                        new CsvFormatException("Expected roll call meta data (name, time etc.", i+1));
                
                skipToAfter(RollCallColumns);
                line = lines[i];
                if (line.StartsWith(EndIdent)) // there's always a small chance no one has roll called yet 
                    continue;

                if (!parseEntries(out List<RollCallEntry> entries))
                    return Outcome<IEnumerable<RollCall>>.Fail(
                        new CsvFormatException("Expected roll entries", i+1));
                
                list.Add(new RollCall(name!, dateTime!.Value, entries!));

                bool parseRollCallMetadata(out string? rcName, out DateTime? rcDateTime)
                {
                    const string DateFormat = "dd-MM-yyyy HH:mm";
                    var provider = CultureInfo.InvariantCulture;
                    
                    rcName = null;
                    rcDateTime = null;
                    var split = line.Split(Separator);
                    if (split.Length < 3)
                        return false;

                    rcName = split[0];
                    var sDataTime = $"{split[1]}T{split[2]}";
                    if (!DateTime.TryParseExact(sDataTime, DateFormat, provider, DateTimeStyles.AssumeUniversal,
                            out var dt))
                        return false;

                    rcDateTime = dt;
                    return true;
                }
                
                Outcome<IEnumerable<RollCallEntry>> parseEntries(out List<RollCallEntry>? rollCallEntries)
                {
                    rollCallEntries = new List<RollCallEntry>(); 
                    for (; i < lines.Length; i++)
                    {
                        line = lines[i];
                        var split = line.Split(Separator);
                        if (split.Length < 6)
                            return Outcome<IEnumerable<RollCallEntry>>.Fail(
                                new CsvFormatException("Expected a minimum of six columns for roll call entries", i+1));

                        if (!DateTime.TryParse(split[4], CultureInfo.InvariantCulture, DateTimeStyles.None, out var timeStamp))
                            return Outcome<IEnumerable<RollCallEntry>>.Fail(
                                new CsvFormatException($"Could not parse timestamp ({split[4]})", i+1));

                        if (!split[5].TryParseMemberStatus(out var status))
                            return Outcome<IEnumerable<RollCallEntry>>.Fail(
                                new CsvFormatException($"Could not parse member status ({split[5]})", i+1));

                        rollCallEntries.Add(
                            new RollCallEntry
                            {
                                Role = split[0],
                                Spec = split[1],
                                Name = split[2],
                                Id = split[3],
                                TimeStamp = timeStamp,
                                Status = status!.Value
                            });
                    }
                    return Outcome<IEnumerable<RollCallEntry>>.Success(rollCallEntries);
                }

                void skipToAfter(string pattern)
                {
                    for (; i < lines.Length; i++)
                    {
                        line = lines[i];
                        if (!line.Equals(pattern, StringComparison.InvariantCultureIgnoreCase)) 
                            continue;
                        
                        ++i;
                        return;
                    }
                }
            }
            
            return Outcome<IEnumerable<RollCall>>.Success(list);
        }
    }
}