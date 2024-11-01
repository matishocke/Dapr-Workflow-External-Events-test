using Dapr.Workflow;
using OrderService.DaprWorkflow.Workflows.Activities;
using OrderService.DaprWorkflow.Workflows.External;
using OrderService.Domain;
using Shared.IntegrationEvents;

namespace OrderService.DaprWorkflow.Workflows;

public class OrderWorkflow : Workflow<Order, OrderResult>
{
    public override async Task<OrderResult> RunAsync(WorkflowContext context, Order order)
    {
        #region Create order

        var newOrder = order with { Status = OrderStatus.Received, OrderId = context.InstanceId };


        // Notify someone that an order was received
        await context.CallActivityAsync(nameof(NotifyActivity),
            new Notification($"Received order {order.ShortId} from {order.CustomerDto.Name}.", newOrder));
        //..


        //await context.CallActivityAsync(
        //    nameof(CreateOrderActivity),
        //    newOrder);

        #endregion


        #region Process Payment

        newOrder = newOrder with { Status = OrderStatus.CheckingPayment };


        // Check Payment Process next
        var paymentDto = new PaymentDto(newOrder.TotalAmount);
        await context.CallActivityAsync(nameof(ProcessPaymentActivity), paymentDto);
        //..



        await context.CallActivityAsync(
            nameof(NotifyActivity),
            new Notification($"Wating for Payment: Order {order.ShortId} from {order.CustomerDto.Name}.", newOrder));

        var paymentResult = await context.WaitForExternalEventAsync<PaymentProcessedResultEvent>(
            ExternalEvents.PaymentEvent,
            TimeSpan.FromDays(3));

        if (paymentResult.State == ResultState.Failed)
        {
            newOrder = newOrder with { Status = OrderStatus.PaymentFailing };
            await context.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Failed: Order {order.ShortId} from {order.CustomerDto.Name}. Payment failed.",
                    newOrder));

            return new OrderResult(newOrder.Status, newOrder, "Payment failed.");
        }

        newOrder = newOrder with { Status = OrderStatus.PaymentSuccess };
        await context.CallActivityAsync(
            nameof(NotifyActivity),
            new Notification($"Payment Completed: Order {order.ShortId} from {order.CustomerDto.Name}.", newOrder));

        #endregion

        await context.CallActivityAsync(
            nameof(NotifyActivity),
            new Notification($"Completed: Order {order.ShortId} from {order.CustomerDto.Name}.", newOrder));

        return new OrderResult(newOrder.Status, newOrder);
    }
}