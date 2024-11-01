namespace Shared.Queues;


public class WorkflowChannel
{
    // "workflowchannel" is the channel in the message broker where different events are published.
    // This is the name of the message broker channel
    public const string Channel = "workflowchannel";
    public class Topics
    {
        // This is the specific topic within the channel
        public const string PaymentResult = "paymentresult"; 

        public const string ItemsReserveResult = "itemsreserveresult";
        public const string ItemsShippedResult = "itemsshipresult";
    }
}