namespace WorkflowService.Models;

public class WorkflowDefinition
{
    public string Id { get; set; } = null!;
    public List<State> States { get; set; } = new();
    public List<ActionTransition> Actions { get; set; } = new();
}