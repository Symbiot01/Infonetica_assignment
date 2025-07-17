using WorkflowService.Models;
using WorkflowService.Exceptions;
using System.Linq;

namespace WorkflowService.Validators;

public static class TransitionValidator
{
    public static void Validate(WorkflowInstance inst, string actionId, WorkflowDefinition def)
    {
        var state = def.States.SingleOrDefault(s => s.Id == inst.CurrentStateId)
            ?? throw new ValidationException($"Current state '{inst.CurrentStateId}' not found in definition.");
        if (state.IsFinal)
            throw new ValidationException($"Cannot execute actions on final state '{state.Id}'.");

        var action = def.Actions.SingleOrDefault(a => a.Id == actionId)
            ?? throw new ValidationException($"Action '{actionId}' not found in definition.");
        if (!action.Enabled)
            throw new ValidationException($"Action '{actionId}' is disabled.");
        if (!action.FromStates.Contains(inst.CurrentStateId))
            throw new ValidationException($"Action '{actionId}' cannot be executed from state '{inst.CurrentStateId}'.");
    }
}