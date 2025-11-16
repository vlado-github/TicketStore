using System.Net;
using Caravan.Domain.Shared.Consts;

namespace Caravan.API.Middlewares.CustomResponses
{
    public class NotFoundErrorDetails : ErrorDetailsBase
    {
        public NotFoundErrorDetails(string message)
            : base(Rfc.RfcNotFoundType, (int)HttpStatusCode.NotFound, message)
        {

        }
    }
}
