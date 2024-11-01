﻿using Dapr.Workflow;
using OrderService.Domain;
using OrderService.Repository;

namespace OrderService.DaprWorkflow.Workflows.Activities.CompensatingActivities;

public class UnReserveItemsActivity : WorkflowActivity<Order, OrderResult>
{
    private readonly IStateManagementRepository _stateManagement;

    public UnReserveItemsActivity(IStateManagementRepository stateManagement)
    {
        _stateManagement = stateManagement;
    }

    public override async Task<OrderResult> RunAsync(WorkflowActivityContext context, Order order)
    {
        await _stateManagement.SaveOrderAsync(order);

        return new OrderResult(OrderStatus.Received, order);
    }
}