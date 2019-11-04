using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoSignService.Tests
{
    class TestHelper
    {
        static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
    }
}
