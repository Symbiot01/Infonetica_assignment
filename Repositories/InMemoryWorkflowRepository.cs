using System.Collections.Concurrent;
using WorkflowService.Models;
using WorkflowService.Exceptions;

namespace WorkflowService.Repositories;

public class InMemoryWorkflowRepository : IWorkflowRepository
{
    private readonly ConcurrentDictionary<string, WorkflowDefinition> _defs = new();
    private readonly ConcurrentDictionary<string, WorkflowInstance> _instances = new();

    public void AddDefinition(WorkflowDefinition def)
    {
        if (!_defs.TryAdd(def.Id, def))
            throw new ValidationException($"Definition with Id '{def.Id}' already exists.");
    }

    public WorkflowDefinition? GetDefinition(string id) =>
        _defs.TryGetValue(id, out var def) ? def : null;

    public IEnumerable<WorkflowDefinition> ListDefinitions() => _defs.Values;

    public void AddInstance(WorkflowInstance inst)
    {
        if (!_instances.TryAdd(inst.Id, inst))
            throw new ValidationException($"Instance with Id '{inst.Id}' already exists.");
    }

    public WorkflowInstance? GetInstance(string id) =>
        _instances.TryGetValue(id, out var inst) ? inst : null;

    public void UpdateInstance(WorkflowInstance inst)
    {
        _instances[inst.Id] = inst;
    }
}