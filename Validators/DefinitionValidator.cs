
using WorkflowService.Models;

using WorkflowService.Exceptions;

using System.Linq;

using System.Collections.Generic;



namespace WorkflowService.Validators;



public static class DefinitionValidator

{

    public static void Validate(WorkflowDefinition def)

    {

        if (string.IsNullOrWhiteSpace(def.Id))

            throw new ValidationException("Definition Id is required.");



        if (def.States.Select(s => s.Id).Distinct().Count() != def.States.Count)

            throw new ValidationException("Duplicate state Ids found.");



        if (def.Actions.Select(a => a.Id).Distinct().Count() != def.Actions.Count)

            throw new ValidationException("Duplicate action Ids found.");



        var initialCount = def.States.Count(s => s.IsInitial);

        if (initialCount != 1)

            throw new ValidationException($"Definition must have exactly one initial state; found {initialCount}.");



        var stateIds = new HashSet<string>(def.States.Select(s => s.Id));

        foreach (var action in def.Actions)

        {

            if (!stateIds.IsSupersetOf(action.FromStates))

                throw new ValidationException($"Action '{action.Id}' has invalid fromStates.");

            if (!stateIds.Contains(action.ToState))

                throw new ValidationException($"Action '{action.Id}' has invalid toState '{action.ToState}'.");

        }

    }

}