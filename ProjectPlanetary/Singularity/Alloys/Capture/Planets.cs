using ProjectPlanetary.Forming;

namespace ProjectPlanetary.Singularity.Alloys.Capture;

public static class Planets
{
    public static readonly Dictionary<string, ExplicitFormation> planetTrajectories = new Dictionary<string, ExplicitFormation>()
    {
        {"text", new ExplicitFormedPrimePlanet(){Voyage = textCapture}},
        {"magnitude", new ExplicitFormedPrimePlanet(){Voyage = magCapture}},
        // {"dicho", new ExplicitFormedPrimePlanet(){Voyage = output}},
    };
    
    private static ExplicitFormation textCapture(List<ExplicitFormation> forms, Space sp)
    {
        string? tempText = Console.ReadLine();
        if (tempText == null) return new ExplicitFormedVacuum();
        return new ExplicitFormedText()
        {
            Text = forms.Count == 0
                ? tempText
                : tempText[..((int)(forms.First() as ExplicitFormedMagnitude)!.Magnitude)]
        };
    }
    
    private static ExplicitFormation magCapture(List<ExplicitFormation> forms, Space sp)
    {
        string? tempText = Console.ReadLine();
        if (tempText == null) return new ExplicitFormedVacuum();
        return new ExplicitFormedMagnitude()
        {
            Magnitude = forms.Count == 0
                ? double.Parse(tempText)
                : double.Parse(tempText[..((int)(forms.First() as ExplicitFormedMagnitude)!.Magnitude)])
        };
    }
}