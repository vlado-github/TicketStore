using JasperFx.Events.Projections;
using Marten.Events.Projections;

namespace TicketStore.DAL.Projections;

public static class Registry
{
    public static void AddProjections(this ProjectionOptions options)
    {
        //options.Add<>(ProjectionLifecycle.Inline);
    }
}