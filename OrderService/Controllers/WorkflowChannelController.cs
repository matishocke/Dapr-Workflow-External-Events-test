using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using OrderService.DaprWorkflow.Workflows;
using OrderService.DaprWorkflow.Workflows.External;
using Shared.IntegrationEvents;
using Shared.Queues;

namespace WorkflowSample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WorkflowChannelController : ControllerBase
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<WorkflowChannelController> _logger;
    private readonly string _workflowComponentName = "dapr";
    // private readonly string _workflowName = nameof(OrderWorkflow);

    public WorkflowChannelController(DaprClient daprClient, ILogger<WorkflowChannelController> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }





    //This attribute tells Dapr that this method is listening to a specific topic in the WorkflowChannel.
    //WorkflowChannel.Channel: Refers to the name of the messaging channel (defined elsewhere).
    //WorkflowChannel.Topics.PaymentResult: Refers to the specific topic in the channel that contains payment results.
    /////// WorkflowChannelController is subscribed to the WorkflowChannel 
    //httpPost not httpGet we sending payment data to be processed not just feting that 
    [Topic(WorkflowChannel.Channel, WorkflowChannel.Topics.PaymentResult)]
    [HttpPost]
    public async Task<IActionResult> PaymentResult([FromBody] PaymentProcessedResultEvent paymentResponse)
    {
        // Logging the incoming data
        _logger.LogInformation(
            $"Payment response received: Id: {paymentResponse.CorrelationId}, Amount: {paymentResponse.Amount}, State: {paymentResponse.State}");


        // Passing the data to a workflow
        await _daprClient.RaiseWorkflowEventAsync(paymentResponse.CorrelationId, _workflowComponentName,
            ExternalEvents.PaymentEvent,
            paymentResponse);


        _logger.LogInformation("Payment response send to workflow");
        return Ok();
    }






    //[Topic(WorkflowChannel.Channel, WorkflowChannel.Topics.ItemsReserveResult)]
    //[HttpPost]
    //public async Task<IActionResult> ItemsReservedResult([FromBody] ItemsReservedResultEvent itemsReservedResponse)
    //{
    //    _logger.LogInformation(
    //        $"Payment response received: Id: {itemsReservedResponse.CorrelationId}, State: {itemsReservedResponse.State}");

    //    await _daprClient.RaiseWorkflowEventAsync(itemsReservedResponse.CorrelationId, _workflowComponentName,
    //        ExternalEvents.ItemReservedEvent,
    //        itemsReservedResponse);

    //    _logger.LogInformation("Payment response send to workflow");
    //    return Ok();
    //}

    //[Topic(WorkflowChannel.Channel, WorkflowChannel.Topics.ItemsReserveResult)]
    //[HttpPost]
    //public async Task<IActionResult> ItemsShippedResult([FromBody] ItemsShippedResultEvent itemsShippedResponse)
    //{
    //    _logger.LogInformation(
    //        $"Payment response received: Id: {itemsShippedResponse.CorrelationId}, State: {itemsShippedResponse.State}");

    //    await _daprClient.RaiseWorkflowEventAsync(itemsShippedResponse.CorrelationId, _workflowComponentName,
    //        ExternalEvents.ItemShippedEvent,
    //        itemsShippedResponse);

    //    _logger.LogInformation("Payment response send to workflow");
    //    return Ok();
    //}
}