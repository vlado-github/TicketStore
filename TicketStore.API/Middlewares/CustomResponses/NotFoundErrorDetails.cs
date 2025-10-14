using System.Net;
using TicketStore.Shared.Consts;

namespace TicketStore.API.Middlewares.CustomResponses
{
    public class NotFoundErrorDetails : ErrorDetailsBase
    {
        public NotFoundErrorDetails(string message)
            : base(Rfc.RfcNotFoundType, (int)HttpStatusCode.NotFound, message)
        {

        }
    }
}
