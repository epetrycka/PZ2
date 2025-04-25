using System.Security.Cryptography;
using System.Text;

class Program{
    public static void Main(string[] args){
        if (args.Length < 4){
            Console.WriteLine("4 Arguments are required:\n"
                                + "filename_a - (0) data to encode | (1) data to decode\n"
                                + "filename_b - file to write the programme output\n"
                                + "password - password to hash the data (at least 8 letter/numbers)\n"
                                + "command type - 0 (encoding) | 1 (decoding)");
            return;
        }
        string input_file = args[0];
        string output_file = args[1];
        string password = args[2];

        if (!File.Exists(input_file)){
            Console.WriteLine($"Input file {input_file} does not exist");
            return;
        }

        try{
            int type = int.Parse(args[3]);
            switch (type){
                case 0:
                    Encoding(input_file, output_file, password);
                    break;
                case 1:
                    Decoding(input_file, output_file, password);
                    break;
            }
        }
        catch{
            Console.WriteLine("Unexpected type of command. Please type one of following:\n"
                                + "0 - encoding mode\n"
                                + "1 - decoding mode");
            return;
        }
    }

    public static void Encoding(string input_file, string output_file, string password){
        try
        {
            using (FileStream fileStream = new(output_file, FileMode.OpenOrCreate))
            {
                using (Aes aes = Aes.Create())
                {
                    UnicodeEncoding byteConverter = new UnicodeEncoding();
                    byte[] key = byteConverter.GetBytes(password);
                    try{
                        aes.Key = key;
                    }
                    catch (CryptographicException ce){
                        Console.WriteLine("Password must be at least 8 letters long.\n", ce);
                        return;
                    }

                    byte[] iv = aes.IV;
                    fileStream.Write(iv, 0, iv.Length);

                    using (CryptoStream cryptoStream = new(
                        fileStream,
                        aes.CreateEncryptor(),
                        CryptoStreamMode.Write))
                    {
                        using (StreamWriter encryptWriter = new(cryptoStream))
                        {
                            encryptWriter.WriteLine(File.ReadAllText(input_file));
                        }
                    }
                }
            }
            Console.WriteLine("Encoded data has been written");
        }
        catch (Exception ex){
            Console.WriteLine(ex);
        }
    }

    public static void Decoding(string input_file, string output_file, string password){
        try
        {
            using (FileStream fileStream = new(input_file, FileMode.Open))
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] iv = new byte[aes.IV.Length];
                    int numBytesToRead = aes.IV.Length;
                    int numBytesRead = 0;

                    while (numBytesToRead > 0)
                    {
                        int n = fileStream.Read(iv, numBytesRead, numBytesToRead);
                        if (n == 0) break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                    UnicodeEncoding byteConverter = new UnicodeEncoding();
                    byte[] key = byteConverter.GetBytes(password);

                    using (CryptoStream cryptoStream = new(
                    fileStream,
                    aes.CreateDecryptor(key, iv),
                    CryptoStreamMode.Read))
                    {
                        using (StreamReader decryptReader = new(cryptoStream))
                        {
                            string decryptedMessage = decryptReader.ReadToEnd();
                            File.WriteAllText(output_file, decryptedMessage);
                        }
                    }
                }
            }
            Console.WriteLine("Decoded data has been written");
        }
        catch (Exception ex){
            Console.WriteLine(ex);
        }
    }
}