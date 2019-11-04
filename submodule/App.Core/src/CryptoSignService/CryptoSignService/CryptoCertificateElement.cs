using System;
using UACryptoSign;

namespace CryptoSignService
{
    public class CryptoCertificateElement
    {
        public string SerialNumber { get; private set; }
        public string CommonName { get; private set; }
        public string Title { get; private set; }
        public string OrganizationName { get; private set; }
        public string OrganizationUnitName { get; private set; }
        public string StateOrProvinceName { get; private set; }
        public string LocalityName { get; private set; }
        public string Surname { get; private set; }
        public string GivenName { get; private set; }
        public string Edrpou { get; private set; }
        public string Drfo { get; private set; }

        public CryptoCertificateElement()
        {

        }

        public CryptoCertificateElement(elementName element)
        {
            SerialNumber = element?.serialNumber;
            CommonName = element?.commonName;
            Title = element?.title;
            OrganizationName = element?.organizationName;
            OrganizationUnitName = element?.organizationalUnitName;
            StateOrProvinceName = element?.stateOrProvinceName;
            LocalityName = element?.localityName;
            Surname = element?.surname;
            GivenName = element?.givenName;
            Edrpou = element?.edrpou;
            Drfo = element?.drfo;
        }
    }
}
