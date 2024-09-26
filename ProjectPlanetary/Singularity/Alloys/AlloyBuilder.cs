using ProjectPlanetary.Forming;

namespace ProjectPlanetary.Singularity.Alloys;

public static class AlloyBuilder
{
    private static readonly List<Expander> primeAlloyExpanders = new List<Expander>()
    {
        Capture.Expand.Expansion,
        Eject.Expand.Expansion
    };
    
    public static void expandPrimeAlloys(Space sp)
    {
        foreach (Expander alloyExpander in primeAlloyExpanders)
            sp.synthesizeElement(alloyExpander.AlloyName!, new ExplicitFormedAlloy()
            {
                Properties = alloyExpander.PlanetTrajectories!
            }, true);
    }
}