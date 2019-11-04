using UACryptoSign;

namespace CryptoSignService
{
    internal static class CryptoConverter
    {
        internal static CryptoSignResponse ConvertToCryptoSignResponse(this signResponse response)
        {
            if (response == null || response.@return == null) {
                return new CryptoSignResponse();
            }

            return new CryptoSignResponse(response.@return.cert,
                response.@return.code,
                response.@return.data,
                response.@return.signingTime,
                response.@return.timestamp,
                response.@return.timestampSpecified);
        }

        internal static CryptoSignResponse ConvertToCryptoSignResponse(this verifyAttachResponse response)
        {
            if (response == null || response.@return == null) {
                return new CryptoSignResponse();
            }

            return new CryptoSignResponse(response.@return.cert,
                response.@return.code,
                response.@return.data,
                response.@return.signingTime,
                response.@return.timestamp,
                response.@return.timestampSpecified);
        }

        internal static CryptoSignResponse ConvertToCryptoSignResponse(this verifyDetachResponse response)
        {
            if (response == null || response.@return == null) {
                return new CryptoSignResponse();
            }

            return new CryptoSignResponse(response.@return.cert,
                response.@return.code,
                response.@return.data,
                response.@return.signingTime,
                response.@return.timestamp,
                response.@return.timestampSpecified);
        }

        internal static CryptoSignResponse ConvertToCryptoSignResponse(this encryptResponse response)
        {
            if (response == null || response.@return == null) {
                return new CryptoSignResponse();
            }

            return new CryptoSignResponse(response.@return.cert,
                response.@return.code,
                response.@return.data,
                response.@return.signingTime,
                response.@return.timestamp,
                response.@return.timestampSpecified);
        }

        internal static CryptoSignResponse ConvertToCryptoSignResponse(this decryptResponse response)
        {
            if (response == null || response.@return == null) {
                return new CryptoSignResponse();
            }

            return new CryptoSignResponse(response.@return.cert,
                response.@return.code,
                response.@return.data,
                response.@return.signingTime,
                response.@return.timestamp,
                response.@return.timestampSpecified);
        }
    }
}
