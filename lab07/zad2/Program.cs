using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

class Program{
    public static void Main(string[] args){
        if (args.Length < 3){
            Console.WriteLine("3 arguments are required:\nfilename_a - data to hash\nfilename_b - hashed data\n(SHA256 | SHA512 | MD5) - hashing algorithm");
            return;
        }
        string file_to_hash = args[0];
        string file_hashed = args[1];
        string algorithm = args[2];
        
        switch (algorithm){
            case "MD5":
                var hash1 = MD5.Create();
                if (!File.Exists(file_hashed)){
                    File.WriteAllText(file_hashed, funMD5(file_to_hash));                    
                }
                else {
                    var hashed_data = File.ReadAllText(file_hashed);
                    var check_data = funMD5(file_to_hash);
                    if (hashed_data == check_data){
                        Console.WriteLine("Data is coherent");
                    }
                    else {
                        Console.WriteLine("Data is not coherent");
                    }
                }
                break;
            case "SHA512":
                var hash2 = SHA512.Create();
                if (!File.Exists(file_hashed)){
                    File.WriteAllText(file_hashed, funSHA512(file_to_hash));
                }
                else {
                    var hashed_data = File.ReadAllText(file_hashed);
                    var check_data = funSHA512(file_to_hash);
                    if (hashed_data == check_data){
                        Console.WriteLine("Data is coherent");
                    }
                    else {
                        Console.WriteLine("Data is not coherent");
                    }
                }
                break;
            case "SHA256":
                var hash3 = SHA256.Create();
                if (!File.Exists(file_hashed)){
                    File.WriteAllText(file_hashed, funSHA256(file_to_hash));                    
                }
                else {
                    var hashed_data = File.ReadAllText(file_hashed);
                    var check_data = funSHA256(file_to_hash);
                    if (hashed_data == check_data){
                        Console.WriteLine("Data is coherent");
                    }
                    else {
                        Console.WriteLine("Data is not coherent");
                    }
                }
                break;     
            default:
                Console.WriteLine("Unexpected algorithm provided. Select one from: SHA256 | SHA512 | MD5");
                return;
        }
    }

    public static string funSHA256(string file_to_hash)
    {
        Encoding enc = Encoding.UTF8;
        var hashBuilder = new StringBuilder();
        using var hash = SHA256.Create();
        byte[] result = hash.ComputeHash(enc.GetBytes(File.ReadAllText(file_to_hash)));
        foreach (var b in result)
            hashBuilder.Append(b.ToString("x2"));
        return hashBuilder.ToString();
    }

    public static string funSHA512(string file_to_hash)
    {
        Encoding enc = Encoding.UTF8;
        var hashBuilder = new StringBuilder();
        using var hash = SHA512.Create();
        byte[] result = hash.ComputeHash(enc.GetBytes(File.ReadAllText(file_to_hash)));
        foreach (var b in result)
            hashBuilder.Append(b.ToString("x2"));
        return hashBuilder.ToString();
    }

    public static string funMD5(string file_to_hash)
    {
        Encoding enc = Encoding.UTF8;
        var hashBuilder = new StringBuilder();
        using var hash = MD5.Create();
        byte[] result = hash.ComputeHash(enc.GetBytes(File.ReadAllText(file_to_hash)));
        foreach (var b in result)
            hashBuilder.Append(b.ToString("x2"));
        return hashBuilder.ToString();
    }
}