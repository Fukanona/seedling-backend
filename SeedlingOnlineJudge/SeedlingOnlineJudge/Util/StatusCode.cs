using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Util
{
    public static class SOJStatusCode
    {
        public enum SOJStatusCodes
        {
            Continue = 100,
            Processing = 102,
            OK = 200,
            Created = 201,
            Accepted = 202,
            NoContent = 204,
            BadRequest = 400,
            Unauthorized = 401,
            Forbidden = 403,
            NotFound = 404,
            InternalServerError = 500,
            BadGateway = 502,
            GatewayTimeout = 504
        };

        public static string HttpStatusCodesToString(SOJStatusCodes statusCode)
        {
            return statusCode.ToString();
        }

        public static string GetStatusCodeDescription(SOJStatusCodes statusCode)
        {
            switch (statusCode)
            {
                case SOJStatusCodes.Continue:
                    return "Continue";
                case SOJStatusCodes.Processing:
                    return "Processing";
                case SOJStatusCodes.OK:
                    return "Ok";
                case SOJStatusCodes.Accepted:
                    return "Accepted";
                case SOJStatusCodes.Created:
                    return "Created";
                case SOJStatusCodes.NoContent:
                    return "No Content";
                case SOJStatusCodes.BadRequest:
                    return "Bad Request";
                case SOJStatusCodes.Unauthorized:
                    return "Unauthorized";
                case SOJStatusCodes.Forbidden:
                    return "Forbidden";
                case SOJStatusCodes.NotFound:
                    return "Not Found";
                case SOJStatusCodes.InternalServerError:
                    return "Internal Server Error";
                case SOJStatusCodes.BadGateway:
                    return "Bad Gateway";
                case SOJStatusCodes.GatewayTimeout:
                    return "Gateway Timeout";
                default:
                    return "Status Code not mapped";
            }
        }

        public static bool GetStatusCodeSuccess(SOJStatusCodes statusCode)
        {
            switch (statusCode)
            {
                case SOJStatusCodes.Processing:
                case SOJStatusCodes.OK:
                case SOJStatusCodes.Created:
                case SOJStatusCodes.Accepted:
                case SOJStatusCodes.Continue:
                case SOJStatusCodes.NoContent:
                    return true;
                default:
                    return false;
            }
        }
    }
}
