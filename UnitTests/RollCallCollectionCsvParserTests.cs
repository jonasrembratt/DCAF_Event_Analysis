using System.IO;
using System.Threading.Tasks;
using DCAF.Inspection;
using Xunit;

namespace UnitTests
{
    public class RollCallCollectionCsvParserTests
    {
        [Fact]
        public async Task Test_parse_CSV_file()
        {
            var file = new FileInfo("./_files/test1.csv");
            var outcome = await EventCollection.LoadFromAsync(file);
            Assert.True(outcome);
        }
    }
}