using System;
using System.IO;
using System.Security.Cryptography;  
using System.Text;

class Program{
    public static void Main(string[] args){
        if (args.Length < 1){
            Console.WriteLine("No mode provided. Choose one type:\n0 - create public and private key,\n1 - encode a file,\n2 - decode a file");
            return;
        }
        int type;
        try{
            type = int.Parse(args[0]);
        }
        catch{
            Console.WriteLine("Mode provided in unexpected format. Choose one type:\n0 - create public and private key,\n1 - encode a file,\n2 - decode a file");
            return;
        }
        var file_public_RSA = "public_key.dat";
        var file_private_RSA = "private_key.dat";
        switch (type){
            case 0:
                Task0(file_public_RSA, file_private_RSA);
                break;
            case 1:
                string file_to_encode;
                string file_output;
                if (args.Length < 3){
                    Console.WriteLine("Command 1 require two additional arguments:\n1 - command type,\nfilename_a - file that would be encoded,\nfilename_b - output file with encoded data");
                    return;
                }
                else{
                    file_to_encode = args[1];
                    file_output = args[2];
                }
                Task1(file_public_RSA, file_to_encode, file_output);
                break;
            case 2:
                string file_to_decode;
                if (args.Length < 3){
                    Console.WriteLine("Command 2 require two additional arguments:\n1 - command type,\nfilename_a - file that would be decoded,\nfilename_b - output file with decoded data");
                    return;
                }
                else{
                    file_to_decode = args[1];
                    file_output = args[2];
                }
                Task2(file_private_RSA, file_to_decode, file_output);
                break;
            default:
                Console.WriteLine($"Program does not have type '{type}'. Choose one of following type:\n0 - create public and private key,\n1 - encode a file,\n2 - decode a file");
                break;
        }
    }

    public static void Task0(string file_public_RSA, string file_private_RSA){
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        File.WriteAllText(file_public_RSA, rsa.ToXmlString(false));
        File.WriteAllText(file_private_RSA, rsa.ToXmlString(true));
    }

    public static void Task1(string file_public_RSA, string file_to_encode, string file_output){
        if (!File.Exists(file_to_encode)){
            Console.WriteLine("Provided file to encode does not exist");
            return;
        }
        if (!File.Exists(file_public_RSA)){
            string file_private_RSA = "private_key.dat";
            Task0(file_public_RSA, file_private_RSA);
        }
        var public_key = File.ReadAllText(file_public_RSA);
        var data_to_encode = File.ReadAllText(file_to_encode);
        try {
            EncryptText(public_key, data_to_encode, file_output);
        }
        catch (Exception ex){
            Console.WriteLine(ex);
            return;
        }
    }

    static void EncryptText(string public_key ,string data_to_encode, string file_output)  
    {  
        UTF8Encoding byteConverter = new UTF8Encoding();  
        byte[] data_to_encode_bytes = byteConverter.GetBytes(data_to_encode);  

        if (data_to_encode_bytes.Length > 245){
            throw new Exception("Data to encode is too long. Please provide data smaller then 245 bytes");
        }

        byte[] encoded_data;   
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())  
        {  
            rsa.FromXmlString(public_key);  
            encoded_data = rsa.Encrypt(data_to_encode_bytes, false);   
        }  
        File.WriteAllBytes(file_output, encoded_data);  
    } 

    public static void Task2(string file_private_RSA, string file_to_decode, string file_output){
        if (!File.Exists(file_to_decode)){
            Console.WriteLine("Provided file to decode does not exist");
            return;
        }
        if (!File.Exists(file_private_RSA)){
            string file_public_RSA = "public_key.dat";
            Task0(file_public_RSA, file_private_RSA);
        }
        var private_key = File.ReadAllText(file_private_RSA);
        try {
            DecryptData(private_key, file_to_decode, file_output);
        }
        catch (Exception ex){
            Console.WriteLine(ex);
            return;
        }
    }

    static void DecryptData(string privateKey, string file_data_to_decode, string file_output)  
    {  
        byte[] data_to_decode = File.ReadAllBytes(file_data_to_decode);  

        byte[] decoded_data;
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())  
        {  
            rsa.FromXmlString(privateKey);  
            decoded_data = rsa.Decrypt(data_to_decode, false);   
        }

        UTF8Encoding byteConverter = new UTF8Encoding();  
        File.WriteAllText(file_output, byteConverter.GetString(decoded_data));  
    } 
}