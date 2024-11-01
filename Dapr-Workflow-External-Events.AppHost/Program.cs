var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.OrderService>("orderservice");

builder.AddProject<Projects.PaymentService>("paymentservice");

builder.AddProject<Projects.WarehouseService>("warehouseservice");

builder.Build().Run();
