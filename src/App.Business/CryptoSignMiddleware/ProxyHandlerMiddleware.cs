using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace App.Business.CryptoSignMiddleware
{
    public class ProxyHandlerMiddleware
    {
        private static string HttpRequestParameterAddress = "address";
        private static string HttpContentTypeBase64 = "X-user/base64-data";
        private static int HttpMaxContentSize = 10000000;
        private static int HttpBufferChunk = 0xFFFF;

        private static bool UseProxy = false;
        private static string ProxyAddress = "";
        private static int ProxyPort = 3128;
        private static string ProxyUser = "";
        private static string ProxyPassword = "";

        private static string[] KnownHosts = {
            "czo.gov.ua",
            "acskidd.gov.ua",
            "ca.informjust.ua",
            "csk.uz.gov.ua",
            "masterkey.ua",
            "ocsp.masterkey.ua",
            "tsp.masterkey.ua",
            "ca.ksystems.com.ua",
            "csk.uss.gov.ua",
            "csk.ukrsibbank.com",
            "acsk.privatbank.ua",
            "ca.mil.gov.ua",
            "acsk.dpsu.gov.ua",
            "acsk.er.gov.ua",
            "ca.mvs.gov.ua",
            "canbu.bank.gov.ua",
            "uakey.com.ua",
            "altersign.com.ua",
            "ca.altersign.com.ua",
            "ocsp.altersign.com.ua",
            "acsk.uipv.org",
            "ocsp.acsk.uipv.org",
            "acsk.treasury.gov.ua",
            "ocsp.treasury.gov.ua",
            "ca.oschadbank.ua",
            "ca.gp.gov.ua"
        };

        private bool IsKnownHost(string uriValue)
        {
            try
            {
                if (!uriValue.Contains("://"))
                    uriValue = "http://" + uriValue;

                Uri uri = new Uri(uriValue);
                string host = uri.Host;

                if (host == null || host == "")
                    host = uriValue;

                foreach (string knownHost in KnownHosts)
                    if (knownHost == host)
                        return true;
            }
            catch (Exception)
            {
            }

            return false;
        }

        private byte[] SafeReadDataStream(Stream stream)
        {
            byte[] buffer;
            int count;
            MemoryStream memoryStream;
            StreamReader streamReader;

            buffer = new byte[HttpBufferChunk];
            memoryStream = new MemoryStream();
            streamReader = new StreamReader(stream);

            while ((count = streamReader.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                memoryStream.Write(buffer, 0, count);

                if (memoryStream.Length > HttpMaxContentSize)
                    return null;
            }

            return memoryStream.ToArray();
        }

        private HttpClientHandler GetHttpClientHandler()
        {
            string proxyUri =
                string.Format("{0}:{1}", ProxyAddress, ProxyPort);

            WebProxy proxy = new WebProxy(proxyUri, false);
            proxy.UseDefaultCredentials = false;
            proxy.Credentials = new NetworkCredential(
                ProxyUser, ProxyPassword);

            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.Proxy = proxy;

            return httpClientHandler;

        }
        private async Task<HttpStatusCode> HandleRequest(HttpContext context)
        {
            string requestAddress;
            byte[] clientResponseData;
            HttpContent content;
            HttpResponseMessage reponse;

            requestAddress =
                context.Request.Query[HttpRequestParameterAddress];

            if (requestAddress == null || requestAddress == "" ||
                !IsKnownHost(requestAddress))
            {
                return HttpStatusCode.BadRequest;
            }

            HttpClient client = null;
            if (UseProxy)
                client = new HttpClient(GetHttpClientHandler());
            else
                client = new HttpClient();

            if (context.Request.Method == "POST")
            {
                byte[] requestData;
                string requestDataBase64String;
                byte[] serverRequestData;

                if (!context.Request.ContentType.Contains(HttpContentTypeBase64))
                    return HttpStatusCode.BadRequest;

                requestData = SafeReadDataStream(context.Request.Body);
                if (requestData == null)
                    return HttpStatusCode.RequestEntityTooLarge;

                requestDataBase64String =
                    System.Text.Encoding.UTF8.GetString(requestData);
                serverRequestData = Convert.FromBase64String(
                    requestDataBase64String);

                content = new ByteArrayContent(serverRequestData);
                reponse = await client.PostAsync(requestAddress, content);
            }
            else
            {
                reponse = await client.GetAsync(requestAddress);
            }

            clientResponseData = await reponse.Content.ReadAsByteArrayAsync();
            context.Response.ContentType = HttpContentTypeBase64;
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsync(Convert.ToBase64String(clientResponseData));

            return HttpStatusCode.OK;
        }

        public ProxyHandlerMiddleware(RequestDelegate next)
        {
        }

        public async Task Invoke(HttpContext context)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;

            try
            {
                string requestType = context.Request.Method;

                if (requestType == "GET" || requestType == "POST")
                    status = await HandleRequest(context);
                else
                    status = HttpStatusCode.BadRequest;
            }
            catch (Exception e)
            {
                await context.Response.WriteAsync("Виникла помилка при обробці запиту" + e);
                return;
            }
            finally
            {
                if (status != HttpStatusCode.OK)
                {
                    await context.Response.WriteAsync("Виникла помилка при обробці запиту");
                    context.Response.StatusCode = (int)status;
                }
            }
        }
    }
    public static class ProxyHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseProxyHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ProxyHandlerMiddleware>();
        }
    }

    public class ProxyHandlerMiddlewarePipeline
    {
        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseProxyHandlerMiddleware();
        }
    }
}
