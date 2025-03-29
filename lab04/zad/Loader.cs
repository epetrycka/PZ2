using System;
using System.IO;
using System.Collections.Generic;

namespace lab4;

public class Loader<T>{
    public List<T> LoadData(String path, Func<string?[], T> generate)
    {
        var row = new List<T>();

        using (StreamReader sr = new StreamReader(path)){

            string? line = sr.ReadLine();

            while ((line = sr.ReadLine()) != null){
                string?[] values = line.Split(",");
                row.Add(generate(values));
            }
        };
        return row;
    }
}