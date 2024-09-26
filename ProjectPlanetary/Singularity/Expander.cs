using ProjectPlanetary.Forming;

namespace ProjectPlanetary.Singularity;
public class Expander
{
    public string? AlloyName { get; set; }
    public Dictionary<string, ExplicitFormation>? PlanetTrajectories { get; init; }
}