using System.Net;
using TicketStore.Domain.Shared.Consts;

namespace TicketStore.API.Middlewares.CustomResponses
{
    public class InvalidOperationErrorDetails : ErrorDetailsBase
    {
        public InvalidOperationErrorDetails(string message)
            : base(Rfc.RfcBadRequestType, (int)HttpStatusCode.BadRequest, message)
        {

        }
    }
}
