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
    PLANET
}

public abstract class ExplicitFormation
{
    public abstract ExplicitType Type { get; }
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
}

public class ExplicitFormedDicho : ExplicitFormation
{
    public override ExplicitType Type { get; } = ExplicitType.DICHO;
    public bool State { get; init; }
}

public class ExplicitFormedAlloy : ExplicitFormation
{
    public override ExplicitType Type { get; } = ExplicitType.ALLOY;
    public Dictionary<string, ExplicitFormation> Properties { get; set; } = new Dictionary<string, ExplicitFormation>();
}

// public delegate ExplicitFormation PlanetTrajectory(ExplicitFormation[] args, Space sp);

public class ExplicitFormedPrimePlanet : ExplicitFormation
{
    public override ExplicitType Type { get; } = ExplicitType.PRIME_PLANET;
    public Func<List<ExplicitFormation>, Space, ExplicitFormation>? Voyage { get; init; }    
}

public class ExplicitFormedPlanet : ExplicitFormation
{
    public override ExplicitType Type { get; } = ExplicitType.PLANET;
    public string? Symbol { get; init; }
    public List<string> PayloadSymbols { get; init; } = new List<string>();
    public Space? PlanetSpace { get; init; }
    public Compound? PlanetCompound { get; init; }
}