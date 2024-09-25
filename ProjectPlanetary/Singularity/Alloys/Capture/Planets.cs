using ProjectPlanetary.Forming;

namespace ProjectPlanetary.Singularity.Alloys.Capture;

public static class Planets
{
    public static readonly Dictionary<string, ExplicitFormation> planetTrajectories = new Dictionary<string, ExplicitFormation>()
    {
        {"text", new ExplicitFormedPrimePlanet(){Voyage = textCapture}},
        // {"magnitude", new ExplicitFormedPrimePlanet(){Voyage = output}},
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
    
    // private static ExplicitFormation output(List<ExplicitFormation> forms, Space sp)
    // {
    //     foreach (ExplicitFormation form in forms)
    //     {
    //         Console.Write(getOutText(form)); 
    //     }
    //     Console.WriteLine();
    //     return new ExplicitFormedVacuum();
    // }
}