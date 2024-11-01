using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using OrderService.DaprWorkflow.Workflows;
using OrderService.Domain;

namespace OrderService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly DaprClient _daprClient;

    public OrderController(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Order order)
    {
        var instanceId = Guid.NewGuid().ToString();


        // alternatively, this could be the name of a workflow component defined in yaml
        var workflowComponentName = "dapr";

        //"MyWorkflowDefinition";
        var workflowName = nameof(OrderWorkflow);


        //This calls the StartWorkflowAsync method from the DaprClient class. This is what starts the workflow within Dapr.
        var startResponse = await _daprClient.StartWorkflowAsync 
            (
            workflowComponentName, workflowName, instanceId, order
            );


        return Ok(startResponse);
    }
}