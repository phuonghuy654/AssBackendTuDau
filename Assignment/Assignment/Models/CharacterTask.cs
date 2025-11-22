using System;
using System.Collections.Generic;

namespace Assignment.Models;

public partial class CharacterTask
{
    public int CharacterId { get; set; }

    public int TaskId { get; set; }

    public DateTime? DateCompleted { get; set; }

    public virtual Character Character { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;
}
