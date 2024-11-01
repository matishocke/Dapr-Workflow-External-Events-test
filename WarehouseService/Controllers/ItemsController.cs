using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Shared.IntegrationEvents;
using Shared.Queues;

namespace WarehouseService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemsController : ControllerBase
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<ItemsController> _logger;

    public ItemsController(DaprClient daprClient, ILogger<ItemsController> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    [Topic(WarehouseChannel.Channel, WarehouseChannel.Topics.Reservation)]
    [HttpPost]
    public async Task<IActionResult> DoReservation([FromBody] ReserveItemsEvent reserveItemsRequest)
    {
        _logger.LogInformation($"Payment request received: {reserveItemsRequest.CorrelationId}");

        var itemsReservedResponse = new ItemsReservedResultEvent
        {
            CorrelationId = reserveItemsRequest.CorrelationId,
            State = ResultState.Succeeded
        };

        await _daprClient.PublishEventAsync(WorkflowChannel.Channel, WorkflowChannel.Topics.ItemsReserveResult,
            itemsReservedResponse);

        _logger.LogInformation(
            $"Payment processed: {itemsReservedResponse.CorrelationId}, , {itemsReservedResponse.State}");

        return Ok();
    }

    [Topic(WarehouseChannel.Channel, WarehouseChannel.Topics.Shippment)]
    [HttpPost]
    public async Task<IActionResult> DoShippment([FromBody] ShipItemsEvent shipItemsEventRequest)
    {
        _logger.LogInformation($"Payment request received: {shipItemsEventRequest.CorrelationId}");

        var itemsShippedResponse = new ItemsShippedResultEvent
        {
            CorrelationId = shipItemsEventRequest.CorrelationId,
            State = ResultState.Succeeded
        };

        await _daprClient.PublishEventAsync(WorkflowChannel.Channel, WorkflowChannel.Topics.ItemsShippedResult,
            itemsShippedResponse);

        _logger.LogInformation(
            $"Payment processed: {itemsShippedResponse.CorrelationId}, , {itemsShippedResponse.State}");

        return Ok();
    }
}