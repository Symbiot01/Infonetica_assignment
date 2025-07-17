using WorkflowService.Models;

namespace WorkflowService.Repositories;

public interface IWorkflowRepository
{
    void AddDefinition(WorkflowDefinition def);
    WorkflowDefinition? GetDefinition(string id);
    IEnumerable<WorkflowDefinition> ListDefinitions();
    void AddInstance(WorkflowInstance inst);
    WorkflowInstance? GetInstance(string id);
    void UpdateInstance(WorkflowInstance inst);
}