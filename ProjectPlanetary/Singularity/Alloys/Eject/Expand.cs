using ProjectPlanetary.Forming;

namespace ProjectPlanetary.Singularity.Alloys.Eject;
public static class Expand
{
    public const string alloyName = "eject";

    public static readonly Dictionary<string, ExplicitFormation> planetTrajectories =
        new Dictionary<string, ExplicitFormation>()
        {
            { "out", new ExplicitFormedPrimePlanet() { Voyage = Planets.outputEject } },
            { "debug", new ExplicitFormedPrimePlanet() { Voyage = Planets.debugEject } },
            // {"dicho", new ExplicitFormedPrimePlanet(){Voyage = output}},
        };
        
    public static readonly Expander Expansion = new Expander()
    {
        AlloyName = alloyName,
        PlanetTrajectories = planetTrajectories
    };
}