using System.Net;
using Caravan.Domain.Shared.Consts;

namespace Caravan.API.Middlewares.CustomResponses
{
    public class InvalidOperationErrorDetails : ErrorDetailsBase
    {
        public InvalidOperationErrorDetails(string message)
            : base(Rfc.RfcBadRequestType, (int)HttpStatusCode.BadRequest, message)
        {

        }
    }
}
