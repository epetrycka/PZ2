using System.Data.SQLite;
using System.Globalization;

namespace lab8
{
    class ReadCSV
    {
        public List<string>? Headers { get; private set; }
        public List<List<string>> Data { get; private set; }

        public ReadCSV(string fileName, char separator = ',', bool hasHeader = true)
        {
            using var reader = new StreamReader(fileName);

            if (hasHeader && !reader.EndOfStream)
            {
                var headerLine = reader.ReadLine();
                if (!string.IsNullOrWhiteSpace(headerLine))
                    Headers = headerLine.Split(separator).ToList();
            }

            Data = new List<List<string>>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var cols = line.Split(separator);
                while (Data.Count < cols.Length)
                    Data.Add(new List<string>());

                for (int i = 0; i < cols.Length; i++)
                    Data[i].Add(cols[i]);
            }
        }

        public List<(string SqlType, bool IsNullable)> InferColumnSchema()
        {
            var schema = new List<(string, bool)>();
            if (Data == null || Data.Count == 0)
                return schema;

            foreach (var column in Data)
            {
                bool isNullable = column.Any(x => string.IsNullOrWhiteSpace(x));
                bool allInt = true;
                bool allDouble = true;

                foreach (var cell in column)
                {
                    if (string.IsNullOrWhiteSpace(cell))
                        continue;

                    if (!int.TryParse(cell, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
                        allInt = false;
                    if (!double.TryParse(cell, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out _))
                        allDouble = false;
                }

                string type = allInt ? "INTEGER"
                                : allDouble ? "REAL"
                                : "TEXT";
                schema.Add((type, isNullable));
            }

            return schema;
        }
    }

    static class SqliteUtils
    {
        public static void CreateTable(string tableName,
                                       List<string> headers,
                                       List<(string SqlType, bool IsNullable)> schema,
                                       SQLiteConnection conn)
        {
            if (headers.Count != schema.Count)
                throw new ArgumentException("Headers i schema muszą być tej samej długości.");

            var cols = headers.Select((h, i) =>
            {
                var (type, nullable) = schema[i];
                return $"\"{h}\" {type}{(nullable ? "" : " NOT NULL")}";
            });

            string ddl = $"CREATE TABLE IF NOT EXISTS \"{tableName}\" (\n  {string.Join(",\n  ", cols)}\n);";
            using var cmd = new SQLiteCommand(ddl, conn);
            cmd.ExecuteNonQuery();
        }

        public static void InsertRows(string tableName,
                                      List<string> headers,
                                      ReadCSV csv,
                                      SQLiteConnection conn)
        {
            string cols = string.Join(", ", headers.Select(h => $"\"{h}\""));
            string vals = string.Join(", ", headers.Select((h, i) => $"@p{i}"));
            string sql = $"INSERT INTO \"{tableName}\" ({cols}) VALUES ({vals});";

            using var tran = conn.BeginTransaction();
            using var cmd = new SQLiteCommand(sql, conn, tran);

            for (int i = 0; i < headers.Count; i++)
                cmd.Parameters.Add(new SQLiteParameter($"@p{i}", System.Data.DbType.String));

            int rowCount = csv.Data[0].Count;
            for (int r = 0; r < rowCount; r++)
            {
                for (int c = 0; c < headers.Count; c++)
                {
                    var val = csv.Data[c][r];
                    cmd.Parameters[c].Value = string.IsNullOrWhiteSpace(val) ? DBNull.Value : (object)val;
                }
                cmd.ExecuteNonQuery();
            }

            tran.Commit();
        }

        public static void PrintTable(string tableName, SQLiteConnection conn)
        {
            using var cmd = new SQLiteCommand($"SELECT * FROM \"{tableName}\";", conn);
            using var rdr = cmd.ExecuteReader();

            var names = Enumerable.Range(0, rdr.FieldCount).Select(rdr.GetName).ToArray();
            Console.WriteLine(string.Join(" | ", names));
            Console.WriteLine(new string('-', names.Sum(n => n.Length + 3)));

            while (rdr.Read())
            {
                var vals = new string[rdr.FieldCount];
                for (int i = 0; i < rdr.FieldCount; i++)
                    vals[i] = rdr.IsDBNull(i) ? "NULL" : rdr.GetValue(i).ToString()!;
                Console.WriteLine(string.Join(" | ", vals));
            }
        }
    }
}