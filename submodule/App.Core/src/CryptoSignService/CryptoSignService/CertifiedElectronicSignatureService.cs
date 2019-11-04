using System;
using System.Threading.Tasks;
using UACryptoSign;

namespace CryptoSignService
{
    public class CertifiedElectronicSignatureService
    {
        #region Members_Private
        // private string _serviceUrl = "http://95.67.116.179:8080/Server/UACryptoSign";
        private string _serviceUrl;
        #endregion

        #region Constructors
        public CertifiedElectronicSignatureService()
        {
        }

        public CertifiedElectronicSignatureService(string serviceUrl)
        {
            _serviceUrl = serviceUrl;
        }
        #endregion

        #region Methods_Public
        /// <summary>
        /// Signs data and returns a result object
        /// </summary>
        /// <param name="dataToSign">Data in String Base64 format</param>
        /// <param name="signAttach">Include original data to signature</param>
        /// <returns></returns>
        public async Task<CryptoSignResponse> SignDataAsync(string dataToSign, bool signAttach)
        {
            var response = ExecuteOperationAsync(OperationTypes.SignAsync, dataToSign, attach: signAttach);
            return await response;
        }

        /// <summary>
        /// Verifies data signed with data attached
        /// </summary>
        /// <param name="dataToVerify">Data in String Base64 format</param>
        /// <returns></returns>
        public async Task<CryptoSignResponse> VerifyDataAttachAsync(string dataToVerify)
        {
            var response = ExecuteOperationAsync(OperationTypes.VerifyAttachAsync, dataFromCES: dataToVerify);
            return await response;
        }

        /// <summary>
        /// Verifies data signed with data detached
        /// </summary>
        /// <param name="dataToVerify">Data in String Base64 format</param>
        /// <param name="originalData">Data in String Base64 format</param>
        /// <returns></returns>
        public async Task<CryptoSignResponse> VerifyDataDetachAsync(string dataToVerify, string originalData)
        {
            var response = ExecuteOperationAsync(OperationTypes.VerifyDetachAsync, originalData, dataToVerify);
            return await response;
        }

        /// <summary>
        /// Encryptes data
        /// </summary>
        /// <param name="data">Data in String Base64 format to encrypt</param>
        /// <returns></returns>
        public async Task<CryptoSignResponse> EncryptDataAsync(string data)
        {
            var response = ExecuteOperationAsync(OperationTypes.EncryptAsync, data);
            return await response;
        }

        /// <summary>
        /// Decryptes data
        /// </summary>
        /// <param name="data">Data in String Base64 format to decrypt</param>
        /// <returns></returns>
        public async Task<CryptoSignResponse> DecryptDataAsync(string data)
        {
            var response = ExecuteOperationAsync(OperationTypes.DecryptAsync, dataFromCES: data);
            return await response;
        }

        #endregion

        #region Methods_Private
        private async Task<CryptoSignResponse> ExecuteOperationAsync(OperationTypes opType, string originalData = "", string dataFromCES = "", bool attach = false)
        {
            ServiceClient serviceClient = null;
            if (string.IsNullOrEmpty(_serviceUrl)) {
                serviceClient = typeof(ServiceClient).GetConstructor(new Type[] { typeof(ServiceClient.EndpointConfiguration) })
                    .Invoke(new object[] { ServiceClient.EndpointConfiguration.ServicePort }) as ServiceClient;
            } else {
                serviceClient = typeof(ServiceClient).GetConstructor(new Type[] { typeof(ServiceClient.EndpointConfiguration), typeof(string) })
                    .Invoke(new object[] { ServiceClient.EndpointConfiguration.ServicePort, _serviceUrl }) as ServiceClient;
            }

            CryptoSignResponse response = null;
            switch (opType) {
                case OperationTypes.SignAsync:
                    var signResponse = await serviceClient.signAsync(originalData, attach);
                    response = signResponse.ConvertToCryptoSignResponse();
                    break;
                case OperationTypes.VerifyDetachAsync:
                    var verifyDetachResponse = await serviceClient.verifyDetachAsync(dataFromCES, originalData);
                    response = verifyDetachResponse.ConvertToCryptoSignResponse();
                    break;
                case OperationTypes.VerifyAttachAsync:
                    var verifyAttachResponse = await serviceClient.verifyAttachAsync(dataFromCES);
                    response = verifyAttachResponse.ConvertToCryptoSignResponse();
                    break;
                case OperationTypes.EncryptAsync:
                    var encryptResponse = await serviceClient.encryptAsync(originalData, null);
                    response = encryptResponse.ConvertToCryptoSignResponse();
                    break;
                case OperationTypes.DecryptAsync:
                    var decryptResponse = await serviceClient.decryptAsync(dataFromCES, null);
                    response = decryptResponse.ConvertToCryptoSignResponse();
                    break;
                default:
                    throw new Exception("Uknown operation type");
            }

            return response;
        }
        #endregion
    }
}
