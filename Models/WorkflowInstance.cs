namespace WorkflowService.Models;

public class ActionHistoryEntry
{
    public string ActionId { get; set; } = null!;
    public DateTime Timestamp { get; set; }
}

public class WorkflowInstance
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string DefinitionId { get; set; } = null!;
    public string CurrentStateId { get; set; } = null!;
    public List<ActionHistoryEntry> History { get; set; } = new();
}