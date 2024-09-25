using ProjectPlanetary.Forming;

namespace ProjectPlanetary.Singularity.Alloys;

public static class AlloyBuilder
{
    public static void expandPrimeAlloys(Space sp)
    {
        sp.synthesizeElement("eject", new ExplicitFormedAlloy()
        {
            Properties = Eject.Planets.planetTrajectories
        }, true);
        
        sp.synthesizeElement("capture", new ExplicitFormedAlloy()
        {
            Properties = Capture.Planets.planetTrajectories
        }, true);
    }
}