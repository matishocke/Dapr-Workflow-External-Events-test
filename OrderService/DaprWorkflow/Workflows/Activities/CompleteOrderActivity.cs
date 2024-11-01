using Dapr.Workflow;
using OrderService.Domain;
using OrderService.Repository;

namespace OrderService.DaprWorkflow.Workflows.Activities;

//public class CompleteOrderActivity : WorkflowActivity<Order, object?>
//{
//    readonly IStateManagementRepository _stateManagement;

//    public CompleteOrderActivity(IStateManagementRepository stateManagement)
//    {
//        _stateManagement = stateManagement;
//    }

//    public override async Task<object?> RunAsync(WorkflowActivityContext context, Order order)
//    {
//        await _stateManagement.SaveOrderAsync(order);

//        return null;
//    }
//}