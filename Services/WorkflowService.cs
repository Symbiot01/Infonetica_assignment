using System.Linq;
using WorkflowService.Models;
using WorkflowService.Repositories;
using WorkflowService.Exceptions;
using WorkflowService.Validators;

namespace WorkflowService.Services;

public class WorkflowService
{
    private readonly IWorkflowRepository _repo;

    public WorkflowService(IWorkflowRepository repo) => _repo = repo;

    public void AddDefinition(WorkflowDefinition def)
    {
        DefinitionValidator.Validate(def);
        _repo.AddDefinition(def);
    }

    public WorkflowDefinition? GetDefinition(string id) =>
        _repo.GetDefinition(id);

    public IEnumerable<WorkflowDefinition> ListDefinitions() =>
        _repo.ListDefinitions();

    public WorkflowInstance StartInstance(string definitionId)
    {
        var def = _repo.GetDefinition(definitionId)
            ?? throw new ValidationException($"Definition '{definitionId}' not found.");

        var initial = def.States.SingleOrDefault(s => s.IsInitial && s.Enabled)
            ?? throw new ValidationException($"Definition '{definitionId}' has no enabled initial state.");

        var inst = new WorkflowInstance
        {
            DefinitionId = definitionId,
            CurrentStateId = initial.Id
        };
        _repo.AddInstance(inst);
        return inst;
    }

    public void ExecuteAction(string instanceId, string actionId)
    {
        var inst = _repo.GetInstance(instanceId)
            ?? throw new ValidationException($"Instance '{instanceId}' not found.");
        var def = _repo.GetDefinition(inst.DefinitionId)
            ?? throw new ValidationException($"Definition '{inst.DefinitionId}' not found.");

        TransitionValidator.Validate(inst, actionId, def);

        var action = def.Actions.Single(a => a.Id == actionId);
        inst.CurrentStateId = action.ToState;
        inst.History.Add(new ActionHistoryEntry { ActionId = actionId, Timestamp = DateTime.UtcNow });
        _repo.UpdateInstance(inst);
    }

    public WorkflowInstance? GetInstance(string id) =>
        _repo.GetInstance(id);
}