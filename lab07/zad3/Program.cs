using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

class Program{
    public static void Main(string[] args){
        if (args.Length < 2){
            Console.WriteLine("2 arguments are required:\nfilename_a - file with data to sign,\nfilename_b - file with signed data");
            return;
        }
        var file_public_RSA = "public_key.dat";
        var file_private_RSA = "private_key.dat";
        string file_to_sign = args[0];
        string file_signed = args[1];
        using SHA256 algorithm  = SHA256.Create();
        
        if (!File.Exists(file_to_sign)){
            Console.WriteLine($"File {file_to_sign} does not exist");
            return;
        }

        byte[] dane = Encoding.ASCII.GetBytes(File.ReadAllText(file_to_sign));
        byte[] hash = algorithm.ComputeHash(dane);

        if (!File.Exists(file_signed)){    
            string privateKeyXml = File.ReadAllText(file_private_RSA);
            byte[] signed_data;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKeyXml);

                RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm(nameof(SHA256));

                signed_data = rsaFormatter.CreateSignature(hash);
            }
            File.WriteAllBytes(file_signed, signed_data);
        }
        else {
            string publicKeyXml = File.ReadAllText(file_public_RSA);
            using (RSA rsa = RSA.Create())
            {
                byte[] signed_data = File.ReadAllBytes(file_signed);
                rsa.FromXmlString(publicKeyXml);
                RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm(nameof(SHA256));
                if (rsaDeformatter.VerifySignature(hash, signed_data))
                {
                    Console.WriteLine("Sign is coherent");
                }
                else
                {
                    Console.WriteLine("Sign is not coherent");
                }
            }
        }
    }
}