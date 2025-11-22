using System;
using System.Collections.Generic;

namespace Assignment.Models;

public partial class Vehicle
{
    public int VehicleId { get; set; }

    public string? VehicleName { get; set; }

    public int? Speed { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();
}
