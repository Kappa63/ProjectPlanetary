using System.Globalization;
using System.Text.Json;

namespace ProjectPlanetary.Singularity.Systems.Eject;

public static class Planets
{
    public static readonly Dictionary<string, ExplicitFormedPrimePlanet> planetTrajectories = new Dictionary<string, ExplicitFormedPrimePlanet>()
    {
        {"debug", new ExplicitFormedPrimePlanet(){Voyage = debug}},
        {"out", new ExplicitFormedPrimePlanet(){Voyage = output}}
    };
    
    private static ExplicitFormation debug(List<ExplicitFormation> forms, Space sp)
    {
        foreach (ExplicitFormation form in forms)
        {
            Console.Write(getDebugText(form)); 
        }
        Console.WriteLine();
        return new ExplicitFormedVacuum();
    }
    
    private static ExplicitFormation output(List<ExplicitFormation> forms, Space sp)
    {
        foreach (ExplicitFormation form in forms)
        {
            Console.Write(getOutText(form)); 
        }
        Console.WriteLine();
        return new ExplicitFormedVacuum();
    }

    private static string getDebugText(ExplicitFormation form)
    {
        return form.Type switch
        {
            ExplicitType.MAGNITUDE => JsonSerializer.Serialize(form as ExplicitFormedMagnitude),
            ExplicitType.DICHO => JsonSerializer.Serialize(form as ExplicitFormedDicho),
            ExplicitType.ALLOY => "{"+$"\"Type\":4, \"Properties\":"+"{"+string.Join(", ", (form as ExplicitFormedAlloy)!.Properties.Select(prop => $"{prop.Key}:"+getDebugText(prop.Value)))+"}}",
            ExplicitType.TEXT => JsonSerializer.Serialize(form as ExplicitFormedText),
            ExplicitType.PRIME_PLANET => "{\"Type\":5,\"Atom\":PRIME_PLANET}",
            _ => JsonSerializer.Serialize(form as ExplicitFormedVacuum)
        };
    }

    private static string getOutText(ExplicitFormation form)
    {
        return form.Type switch
        {
            ExplicitType.MAGNITUDE => (form as ExplicitFormedMagnitude)!.Magnitude.ToString(CultureInfo.CurrentCulture),
            ExplicitType.DICHO => (form as ExplicitFormedDicho)!.State ? "True" : "False",
            ExplicitType.ALLOY => "{" + string.Join(", ",
                (form as ExplicitFormedAlloy)!.Properties.Select(prop => $"{prop.Key}:{getOutText(prop.Value)}")) + "}",
            ExplicitType.TEXT => (form as ExplicitFormedText)!.Text!,
            ExplicitType.PRIME_PLANET => "{\"Type\":5,\"Atom\":PRIME_PLANET}",
            _ => "VACUUM"
        };
    }
}