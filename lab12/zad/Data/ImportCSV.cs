using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
namespace zad.Data;

public class ImportCSV
{
    public string FilePath;
    public ImportCSV(string filePath)
    {
        this.FilePath = filePath;
        SQLiteConnection.CreateFile("MyDatabase.sqlite");
        string connectionString = "Data Source=MyDatabase.sqlite;Version=3;";
        SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString);

        using var reader = new StreamReader(FilePath);
        var header = reader.ReadLine();
        var listHeader = header.Split(";");

        m_dbConnection.Open();

        string columnsCreate = "id INTEGER PRIMARY KEY ASC AUTOINCREMENT, " + listHeader[0];
        
        for (int i = 1; i < 10; i++)
        {
            columnsCreate += ", " + listHeader[i] + " TEXT";
        }

        var sql = $"CREATE TABLE IF NOT EXISTS mySchema( {columnsCreate} );";
        SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
        command.ExecuteNonQuery();

        string columns = listHeader[0];
        for (int i = 1; i < 10; i++)
        {
            columns += ", " + listHeader[i];
        }

        for (int i = 0; i < 100; i++)
        {
            var line = reader.ReadLine();
            var listLine = line.Split(";");
            string values = "'" + listLine[0] + "'";
            for (int j = 1; j < 10; j++)
            {
                values += ", '" + listLine[j] + "'";
            }
            var sql_value = $"INSERT INTO mySchema ( {columns} ) VALUES ( {values} )";
            using SQLiteCommand command_value = new SQLiteCommand(sql_value, m_dbConnection);
            command_value.ExecuteNonQuery();
        }

        m_dbConnection.Close();
    }
}