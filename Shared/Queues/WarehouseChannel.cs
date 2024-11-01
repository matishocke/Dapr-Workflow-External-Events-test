namespace Shared.Queues;

public class WarehouseChannel
{
    public const string Channel = "warehousechannel";
    public class Topics
    {
        public const string Reservation = "reservation";
        public const string Shippment = "shippment";
    }
}