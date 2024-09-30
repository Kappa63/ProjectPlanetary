using ProjectPlanetary.Forming;
using System.Globalization;
using System.Text.Json;

namespace ProjectPlanetary.Singularity.Alloys.Eject;
public static class Planets
{
    internal static ExplicitFormation debugEject(List<ExplicitFormation> forms, Space sp)
    {
        foreach (ExplicitFormation form in forms)
        {
            Console.Write(getDebugText(form)); 
        }
        Console.WriteLine();
        return new ExplicitFormedVacuum();
    }
    
    internal static ExplicitFormation outputEject(List<ExplicitFormation> forms, Space sp)
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
            ExplicitType.PRIME_PLANET => "{\"Type\":5, \"Atom\":PRIME_PLANET}",
            ExplicitType.PLANET => "{\"Type\":6, \"Atom\":PLANET}",
            ExplicitType.CLUSTER => "{\"Type\":7, \"Formations\":"+"["+string.Join(", ", (form as ExplicitFormedCluster)!.Forms.Select(getDebugText))+"]}",
            ExplicitType.EXO_PLANET => "{\"Type\":8, \"Atom\":EXO_PLANET}",
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
            ExplicitType.TEXT => (form as ExplicitFormedText)!.Text!.Trim('"'),
            ExplicitType.PRIME_PLANET => "PRIME_PLANET",
            ExplicitType.PLANET => "PLANET",
            ExplicitType.EXO_PLANET => "EXO_PLANET",
            ExplicitType.CLUSTER => "[" + string.Join(", ",
                (form as ExplicitFormedCluster)!.Forms.Select(getOutText)) + "]",
            _ => "Vacuum"
        };
    }
}