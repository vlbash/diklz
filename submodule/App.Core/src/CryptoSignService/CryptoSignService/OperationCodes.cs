namespace CryptoSignService
{
    public enum OperationCodes
    {
        SUCCESS = 0,
        SERVER_INTERNAL_ERROR = 1,
        CRYPTO_ERROR = 2,
        INVALID_PARAM = 3,
        DATA_NOT_FOUND = 4,
        DATA_DONT_MATCH = 5,
        CERTIFICATE_NOT_FOUND = 6,
        CERTIFICATE_DONT_MATCH = 7,
        CERTIFICATE_REVOKED = 8,
        WRONG_RECIPIENT = 9,
        TS_ERROR = 10,
        UKNOWN = 100
    }
}
