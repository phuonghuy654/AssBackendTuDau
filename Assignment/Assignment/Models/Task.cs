using System;
using System.Collections.Generic;

namespace Assignment.Models;

public partial class Task
{
    public int TaskId { get; set; }

    public string TaskName { get; set; } = null!;

    public string? Description { get; set; }

    public int? RewardXp { get; set; }

    public virtual ICollection<CharacterTask> CharacterTasks { get; set; } = new List<CharacterTask>();
}
