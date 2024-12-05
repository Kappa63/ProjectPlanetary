using ProjectPlanetary.Forming;

namespace ProjectPlanetary.Singularity.Alloys.Eject;
public static class Expand
{
    private const string alloyName = "eject";

    private static readonly Dictionary<string, ExplicitFormation> planetTrajectories =
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