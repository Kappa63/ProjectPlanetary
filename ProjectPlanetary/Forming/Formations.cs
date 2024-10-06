using ProjectPlanetary.Bonding;
using ProjectPlanetary.Singularity;

namespace ProjectPlanetary.Forming;

public enum ExplicitType
{
    VACUUM,
    MAGNITUDE,
    DICHO,
    TEXT,
    ALLOY,
    PRIME_PLANET,
    PLANET,
    CLUSTER,
    MOON
}

public abstract class ExplicitFormation
{
    public abstract ExplicitType Type { get; }
    protected Dictionary<string, ExplicitFormation> Moons { get; init; } = new Dictionary<string, ExplicitFormation>();

    public bool TryGetMoon(string planetSymbol, out ExplicitFormation? planet)
    {
        return Moons.TryGetValue(planetSymbol, out planet);
    }
}

public class ExplicitFormedVacuum : ExplicitFormation
{
    public override ExplicitType Type { get; } = ExplicitType.VACUUM;
}

public class ExplicitFormedMagnitude : ExplicitFormation
{
    public override ExplicitType Type { get; } = ExplicitType.MAGNITUDE;
    public double Magnitude { get; init; }
}

public class ExplicitFormedText : ExplicitFormation
{
    public override ExplicitType Type { get; } = ExplicitType.TEXT;
    public string? Text { get; init; }
    
    public ExplicitFormedText()
    {
        Moons = new Dictionary<string, ExplicitFormation>
        {
            { "count", new ExplicitFormedMoon()
            {
                Voyage = Count
            }  },
            { "reverse", new ExplicitFormedMoon()
            {
                Voyage = Reverse
            } },
            { "toCluster", new ExplicitFormedMoon()
            {
                Voyage = ToCluster
            } },
            { "shrink", new ExplicitFormedMoon()
            {
                Voyage = Shrink
            } }
        };
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
}

public class ExplicitFormedDicho : ExplicitFormation
{
    public override ExplicitType Type { get; } = ExplicitType.DICHO;
    public bool State { get; init; }
}

public class ExplicitFormedAlloy : ExplicitFormation
{
    public override ExplicitType Type { get; } = ExplicitType.ALLOY;
    public Dictionary<string, ExplicitFormation> Properties { get; init; } = new Dictionary<string, ExplicitFormation>();
}

public class ExplicitFormedCluster : ExplicitFormation
{
    public override ExplicitType Type { get; } = ExplicitType.CLUSTER;
    public List<ExplicitFormation> Forms { get; init; } = new List<ExplicitFormation>();
    
    public ExplicitFormedCluster()
    {
        Moons = new Dictionary<string, ExplicitFormation>
        {
            { "count", new ExplicitFormedMoon()
            {
                Voyage = Count
            } },
            { "add", new ExplicitFormedMoon()
            {
                Voyage = Add
            } },
        };
    }

    private ExplicitFormedMagnitude Count(List<ExplicitFormation>? _)
    {
        return new ExplicitFormedMagnitude()
        {
            Magnitude = Forms.Count
        };
    }
    
    private ExplicitFormedVacuum Add(List<ExplicitFormation>? forms)
    {
        if (forms is not {Count:0})
            Forms.AddRange(forms!);
        return new ExplicitFormedVacuum();
    }
}

public class ExplicitFormedPrimePlanet : ExplicitFormation
{
    public override ExplicitType Type { get; } = ExplicitType.PRIME_PLANET;
    public Func<List<ExplicitFormation>, Space, ExplicitFormation>? Voyage { get; init; }    
}

public class ExplicitFormedMoon : ExplicitFormation
{
    public override ExplicitType Type { get; } = ExplicitType.MOON;
    // public List<object?>? Payload { get; init; }
    public Func<List<ExplicitFormation>?, ExplicitFormation>? Voyage { get; init; }    
}

public class ExplicitFormedPlanet : ExplicitFormation
{
    public override ExplicitType Type { get; } = ExplicitType.PLANET;
    public string? Symbol { get; init; }
    public List<string> PayloadSymbols { get; init; } = new List<string>();
    public Space? PlanetSpace { get; init; }
    public Compound? PlanetCompound { get; init; }
}