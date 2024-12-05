using ProjectPlanetary.Forming;

namespace ProjectPlanetary.Singularity.Alloys.Time;

public static class Planets
{
    internal static ExplicitFormation instant(List<ExplicitFormation> forms, Space sp)
    {
        return new ExplicitFormedText()
        {
            Text = DateTime.Now.ToString("h:mm:ss tt")
        };
    }
}