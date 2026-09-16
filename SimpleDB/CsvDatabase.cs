using System.Globalization;
using System.IO.Enumeration;
using System.Runtime.ConstrainedExecution;
using CsvHelper;
using CsvHelper.Configuration;

namespace SimpleDB;

// singleton paddon: https://csharpindepth.com/Articles/Singleton

public sealed class CsvDatabase<T> : IDatabaseRepository<T>
{

    // Lazy object makes sure that only one instance of the class is created
    private static Lazy<CsvDatabase<T>>? _instance;
    private readonly string filename;

    private CsvDatabase(string filename)
    {
        this.filename = filename;
    }

    // Singleton pattern: checks if an instance already exists, if not, creates a new one.
    // Note: Multiple instances of the same class with different generic types can exist.
    // E.g., CsvDatabase<Observation> and CsvDatabase<Comment> will not return the same instance.
    public static CsvDatabase<T> GetInstance(string _filename)
    {
        _instance ??= new Lazy<CsvDatabase<T>>(() => new CsvDatabase<T>(_filename));

        return _instance.Value;
    }


    public IEnumerable<T> Read(int? limit = null)
    {
        using (var reader = new StreamReader(filename))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<T>();
            return limit.HasValue ? records.Take(limit.Value).ToList() : records.ToList();
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
            csv.NextRecord();
        }
    }
}