using System.Globalization;
using System.IO.Enumeration;
using System.Runtime.ConstrainedExecution;
using CsvHelper;
using CsvHelper.Configuration;

namespace SimpleDB;

sealed class CsvDatabase<T> : IDatabaseRepository<T>
{
    private readonly string filename;

    public CsvDatabase(string filename)
    {
        this.filename = filename;
    }

    public IEnumerable<T> Read(int? limit = null)
    {
        using (var reader = new StreamReader(filename))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<T>();
            return limit.HasValue ? records.Take(limit.Value) : records;
        }
    }

    public void Store(T record)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false
        };
        using (var stream = File.Open(filename, FileMode.Append))
        using (var writer = new StreamWriter(stream))
        using (var csv = new CsvWriter(writer, config))
        {
            csv.WriteRecord(record);
        }
    }
}