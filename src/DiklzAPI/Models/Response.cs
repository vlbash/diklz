using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace App.WebApi.Models
{
    internal interface IResponse
    {
        string Message { get; set; }

        bool DidError { get; set; }

        string ErrorMessage { get; set; }
    }

    internal interface ISingleResponse<TModel>: IResponse
    {
        TModel Model { get; set; }
    }

    internal interface IListResponse<TModel>: IResponse
    {
        IEnumerable<TModel> Model { get; set; }
    }

    internal class SingleResponse<TModel>: ISingleResponse<TModel>
    {
        public bool DidError { get; set; }

        public string ErrorMessage { get; set; }

        public string Message { get; set; }

        public TModel Model { get; set; }
    }

    internal class ListResponse<TModel>: IListResponse<TModel>
    {
        public bool DidError { get; set; }

        public string ErrorMessage { get; set; }

        public string Message { get; set; }

        public IEnumerable<TModel> Model { get; set; }
    }

    internal static class ResponseExtensions
    {
        public static IActionResult ToHttpResponse(this IResponse response)
        {
            var status = response.DidError ? HttpStatusCode.InternalServerError : HttpStatusCode.OK;

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
        }

        public static IActionResult ToHttpResponse<TModel>(this ISingleResponse<TModel> response)
        {
            var status = HttpStatusCode.OK;

            if (response.DidError)
                status = HttpStatusCode.InternalServerError;
            else if (response.Model == null)
                status = HttpStatusCode.NotFound;

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
        }

        public static IActionResult ToHttpResponse<TModel>(this IListResponse<TModel> response)
        {
            var status = HttpStatusCode.OK;

            if (response.DidError)
                status = HttpStatusCode.InternalServerError;
            else if (response.Model == null)
                status = HttpStatusCode.NoContent;

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
        }
    }
}
