using SeedlingOnlineJudge.Model.Front;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SeedlingOnlineJudge.Util;
using static SeedlingOnlineJudge.Util.SOJStatusCode;

namespace SeedlingOnlineJudge.BO
{
    public class ContractBO
    {
        public ContractBO()
        {
        }

        public BackendResponse<T> BuildBackendResponse<T>(string message, SOJStatusCodes statusCode, T content = null) where T : class
        {
            var backendResponse = new BackendResponse<T>
            {
                Content = content,
                Message = message,
                StatusCode = SOJStatusCode.HttpStatusCodesToString(statusCode),
                IsSuccess = SOJStatusCode.GetStatusCodeSuccess(statusCode),
                ResponseTime = Helper.GetDateTimeNowBrazil(),
                StatusCodeDescription = SOJStatusCode.GetStatusCodeDescription(statusCode)
            };

            return backendResponse;
        }
    }
}
