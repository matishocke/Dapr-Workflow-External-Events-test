using OrderService.Domain;

namespace OrderService.DaprWorkflow.Workflows.Activities
{
    public class ReserveItemsActivity
    {
    }

    public record ItemsDto(OrderItem[] OrderItems)
    {

    }
}
