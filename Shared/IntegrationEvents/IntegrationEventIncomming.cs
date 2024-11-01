namespace Shared.IntegrationEvents;

public abstract record IntegrationEventIncomming
{
    public string CorrelationId { get; init; } = String.Empty;
    public ResultState State { get; init; } = ResultState.Failed;
}

public record PaymentProcessedResultEvent : IntegrationEventIncomming
{
    public decimal Amount { get; init; }
    
}

public enum ResultState
{
    Succeeded,
    Failed
}


public record ItemsReservedResultEvent : IntegrationEventIncomming
{
}



public record ItemsShippedResultEvent : IntegrationEventIncomming
{
}

public record OrderCompletedEvent : IntegrationEventIncomming
{
}


public abstract record FailedEvent : IntegrationEventIncomming
{
    public string Reason { get; init; } = string.Empty;
}

public record OrderFailedEvent : FailedEvent
{
}

public record PaymentFailedEvent : FailedEvent
{
}

public record ItemsReservationFailedEvent : FailedEvent
{
}

public record ItemsShipmentFailedEvent : FailedEvent
{
}