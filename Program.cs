using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using WorkflowService.Repositories;
using WorkflowService.Exceptions;
using WorkflowService.Models;
using WfSvc = WorkflowService.Services.WorkflowService;

var builder = WebApplication.CreateBuilder(args);

// Register services & Swagger
builder.Services.AddSingleton<IWorkflowRepository, InMemoryWorkflowRepository>();
builder.Services.AddSingleton<WfSvc>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WorkflowService v1"));

// Health check / welcome endpoint
//app.MapGet("/", () => Results.Ok("WorkflowService is running"));

// Create a new workflow definition
app.MapPost("/workflows", (WfSvc svc, WorkflowDefinition def) =>
{
    try
    {
        svc.AddDefinition(def);
        return Results.Ok(def);
    }
    catch (ValidationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("CreateWorkflowDefinition");

// Get a workflow definition by id
app.MapGet("/workflows/{id}", (WfSvc svc, string id) =>
{
    var def = svc.GetDefinition(id);
    return def is not null ? Results.Ok(def) : Results.NotFound();
})
.WithName("GetWorkflowDefinition");

// List all workflow definitions
//app.MapGet("/workflows", (WfSvc svc) => Results.Ok(svc.ListDefinitions()))
//.WithName("ListWorkflowDefinitions");

// Start a new workflow instance
app.MapPost("/workflows/{id}/instances", (WfSvc svc, string id) =>
{
    try
    {
        var inst = svc.StartInstance(id);
        return Results.Ok(inst);
    }
    catch (ValidationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("StartWorkflowInstance");

// Get a workflow instance
app.MapGet("/instances/{id}", (WfSvc svc, string id) =>
{
    var inst = svc.GetInstance(id);
    return inst is not null ? Results.Ok(inst) : Results.NotFound();
})
.WithName("GetWorkflowInstance");

// Execute an action on an instance
app.MapPost("/instances/{id}/actions/{actionId}", (WfSvc svc, string id, string actionId) =>
{
    try
    {
        svc.ExecuteAction(id, actionId);
        var inst = svc.GetInstance(id);
        return Results.Ok(inst);
    }
    catch (ValidationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("ExecuteWorkflowAction");



app.Run();