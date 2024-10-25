namespace ProjectPlanetary.Forming;

public partial class ExplicitFormedText
{
    private ExplicitFormedMagnitude Count(List<ExplicitFormation>? _)
    {
        return new ExplicitFormedMagnitude()
        {
            Magnitude = Text!.Length-2
        };
    }

    private ExplicitFormedText Reverse(List<ExplicitFormation>? _)
    {
        return new ExplicitFormedText()
        {
            Text =  new string(Text!.Reverse().ToArray())
        };
    }
    
    private ExplicitFormedCluster ToCluster(List<ExplicitFormation>? _)
    {
        return new ExplicitFormedCluster()
        {
            Forms =  Text!.Trim('"').ToArray().Select(ExplicitFormation (c) => new ExplicitFormedText(){Text = "'"+c+"'"}).ToList()
        };
    }
    
    private ExplicitFormedText Shrink(List<ExplicitFormation>? payload)
    {
        if (payload is { Count: 2 } && payload[0].Type == ExplicitType.MAGNITUDE &&
            payload[1].Type == ExplicitType.MAGNITUDE)
        {
            int start = (int)(payload[0] as ExplicitFormedMagnitude)!.Magnitude + 1;
            return new ExplicitFormedText()
            {
                Text = string.Concat("\"",
                    Text!.Substring(start, ((int)(payload[1] as ExplicitFormedMagnitude)!.Magnitude + 1)-start), "\"")
            };
        }

        throw new ArgumentException("Expected payload to consist of magnitudes.");
    }
}