using ProjectPlanetary.Forming;

namespace ProjectPlanetary.Singularity.Alloys.Time;

public static class Expand
{
    private const string alloyName = "time";

    private static readonly Dictionary<string, ExplicitFormation> planetTrajectories =
        new Dictionary<string, ExplicitFormation>()
        { 
            { "instant", new ExplicitFormedPrimePlanet() { Voyage = Planets.instant } },
        };
    public static readonly Expander Expansion = new Expander()
    {
        AlloyName = alloyName,
        PlanetTrajectories = planetTrajectories
    };    
}