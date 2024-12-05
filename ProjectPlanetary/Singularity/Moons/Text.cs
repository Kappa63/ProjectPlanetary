using System.Text.RegularExpressions;

namespace ProjectPlanetary.Forming;

public partial class ExplicitFormedText
{
    protected override void SynthMoons()
    {
        Moons.Add("count", new ExplicitFormedMoon() { Voyage = Count });
        Moons.Add("reverse", new ExplicitFormedMoon() { Voyage = Reverse });
        Moons.Add("toCluster", new ExplicitFormedMoon() { Voyage = ToCluster });
        Moons.Add("shrink", new ExplicitFormedMoon() { Voyage = Shrink });
        Moons.Add("swap", new ExplicitFormedMoon() { Voyage = Swap });
    }
    
    public override bool Equals(ExplicitFormation? other)
    {
        if (other!.Type == ExplicitType.TEXT)
            return Text == (other as ExplicitFormedText)!.Text;
        return false;
    }
    
    
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

    private ExplicitFormedText Swap(List<ExplicitFormation>? payload)
    {
        if (payload == null)
            throw new ArgumentException("Expected payload.");

        if (payload.Count < 2 || payload[0].Type != ExplicitType.TEXT || payload[1].Type != ExplicitType.TEXT)
            throw new ArgumentException("Expected payload of Texts.");

        string toSwap = (payload[0] as ExplicitFormedText)?.Text?.Trim('"') ??
                        throw new ArgumentException("Invalid pattern in payload.");
        string swapTo = (payload[1] as ExplicitFormedText)?.Text?.Trim('"') ??
                        throw new ArgumentException("Invalid replacement in payload.");

        Regex re = new Regex(toSwap);

        if (payload.Count == 3 && payload[2].Type == ExplicitType.MAGNITUDE)
        {
            int maxSwaps = (int)((payload[2] as ExplicitFormedMagnitude)?.Magnitude ??
                                 throw new ArgumentException("Invalid magnitude in payload."));
            return new ExplicitFormedText
            {
                Text = re.Replace(Text!, swapTo, maxSwaps)
            };
        }

        return new ExplicitFormedText
        {
            Text = re.Replace(Text!, swapTo)
        };
    }
}