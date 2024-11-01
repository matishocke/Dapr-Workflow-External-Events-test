using System.Text.Json.Serialization;

namespace Shared.Dtos;

public record OrderItemDto(ItemTypeDto PizzaType, int Quantity)
{
    public OrderItemDto() : this(default, 1)
    {
    }
}

public record OrderDto(
    string OrderId,
    OrderItemDto[] OrderItems,
    DateTime OrderDate,
    CustomerDto CustomerDto,
    OrderStatusDto Status = OrderStatusDto.Received)
{
    public string ShortId => OrderId.Substring(0, 8);
}

public record CustomerDto(string Name, string Email);

public record InventoryRequestDto(OrderItemDto[] PizzasRequested);

public record InventoryResultDto(bool IsSufficientInventory, OrderItemDto[] PizzasInStock);


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ItemTypeDto
{
    Computer = 1,
    Monitor = 2,
    Keyboard = 3,
    Mouse = 4
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatusDto
{
    Received = 0,
    CheckingInventory = 1,
    SufficientInventory = 2,
    InsufficientInventory = 3,
    CheckingPayment = 4,
    PaymentFailing = 5,
    ShipingItems = 6,
    ShipingItemsFailing = 7,
    Error = 8
}