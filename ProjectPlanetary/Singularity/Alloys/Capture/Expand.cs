using ProjectPlanetary.Forming;

namespace ProjectPlanetary.Singularity.Alloys.Capture;
public static class Expand
{
    private const string alloyName = "capture";

    private static readonly Dictionary<string, ExplicitFormation> planetTrajectories =
        new Dictionary<string, ExplicitFormation>()
        {
            { "text", new ExplicitFormedPrimePlanet() { Voyage = Planets.textCapture } },
            { "magnitude", new ExplicitFormedPrimePlanet() { Voyage = Planets.magCapture } },
            // {"dicho", new ExplicitFormedPrimePlanet(){Voyage = output}},
        };
    public static readonly Expander Expansion = new Expander()
    {
        AlloyName = alloyName,
        PlanetTrajectories = planetTrajectories
    };
}