using System.Data.SQLite;

using lab8;

public class Program{

    static void Main(string[] args)
        {
            var csv = new ReadCSV("zwierzaki_200.csv", ',');
            var headers = csv.Headers ?? throw new InvalidOperationException("Brak headera");

            var schema = csv.InferColumnSchema();

            using var conn = new SQLiteConnection("Data Source=zwierzatka.db;");
            conn.Open();

            SqliteUtils.CreateTable("zwierzatka", headers, schema, conn);

            SqliteUtils.InsertRows("zwierzatka", headers, csv, conn);

            SqliteUtils.PrintTable("zwierzatka", conn);
        }
}