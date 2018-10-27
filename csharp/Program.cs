using System;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace CertCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            getCertThumbprint("https://github.com");
        }

        public static void getCertThumbprint(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ServerCertificateValidationCallback += (sender, validatedCert, chain, errors) => {
                var cert = new X509Certificate2(validatedCert);
                foreach (var chainElement in chain.ChainElements)
                {
                    var cn = chainElement.Certificate.Subject.Split(',').First().Replace("CN=", "");
                    Console.WriteLine("Cert Name: {0}", cn);
                    Console.WriteLine("Thumbprint: {0}\n", chainElement.Certificate.Thumbprint);
                }
                return errors == SslPolicyErrors.None;
            };
            request.GetResponse();
            }
    }
}
