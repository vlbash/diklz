using System;
using UACryptoSign;

namespace CryptoSignService
{
    public class CryptoSignResponse
    {
        public CryptoSignCertificate Certificate { get; set; }
        public OperationCodes Code { get; private set; }
        public string Data { get; private set; }
        public string SigningTime { get; private set; }
        public bool TimeStamp { get; private set; }
        public bool TimeStampSpecified { get; private set; }
        /// <summary>
        /// Returns true if operation is successfull
        /// </summary>
        public bool Success
        {
            get
            {
                return Code == OperationCodes.SUCCESS;
            }
        }

        #region Constructors
        public CryptoSignResponse()
        {
            Certificate = new CryptoSignCertificate();
        }

        public CryptoSignResponse(elementCert cert, int code, string data, string signingTime, bool timeStamp, bool timeStampSpecified)
        {
            Certificate = new CryptoSignCertificate(cert);
            Code = OperationCodeFromIntCode(code);
            Data = data;
            SigningTime = signingTime;
            TimeStamp = timeStamp;
            TimeStampSpecified = timeStampSpecified;
        }
        #endregion

        public static OperationCodes OperationCodeFromIntCode(int code)
        {
            OperationCodes operationCode;
            try {
                operationCode = (OperationCodes)code;
            }
            catch {
                operationCode = OperationCodes.UKNOWN;
            }

            return operationCode;
        }

    }
}
