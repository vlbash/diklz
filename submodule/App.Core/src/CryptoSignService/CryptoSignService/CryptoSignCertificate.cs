using System;
using System.Globalization;
using UACryptoSign;

namespace CryptoSignService
{
    public class CryptoSignCertificate
    {
        public string SerialNumber { get; private set; }
        public DateTime NotBefore { get; private set; }
        public DateTime NotAfter { get; private set; }
        public CryptoCertificateElement Subject { get; private set; }
        public CryptoCertificateElement Issuer { get; private set; }
        public bool IsUpToDate {
            get {
                DateTime now = DateTime.Now;
                return (NotBefore < now && NotAfter > now);
            }
        }

        public CryptoSignCertificate()
        {

        }

        public CryptoSignCertificate(elementCert cert)
        {
            SerialNumber = cert?.serialNumber;
            Subject = new CryptoCertificateElement(cert?.subject);
            Issuer = new CryptoCertificateElement(cert?.issuer);

            var provider = new CultureInfo("ru-RU");
            try {
                NotBefore = DateTime.Parse(cert?.validity?.notBefore, provider);
                NotAfter = DateTime.Parse(cert?.validity?.notAfter, provider);
            }
            catch {
                // OK, there is nothing we can do here
            }
        }

        
    }
}
