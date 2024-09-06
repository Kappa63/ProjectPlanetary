namespace ProjectPlanetary;

public enum ExplicitType
{
    VACUUM,
    MAGNITUDE,
    DICHO,
    ALLOY
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
    public double Magnitude { get; set; } = 0;
}

public class ExplicitFormedDicho : ExplicitFormation
{
    public override ExplicitType Type { get; } = ExplicitType.DICHO;
    public bool state { get; set; } = false;
}

public class ExplicitFormedAlloy : ExplicitFormation
{
    public override ExplicitType Type { get; } = ExplicitType.ALLOY;
    public Dictionary<string, ExplicitFormation> properties { get; set; } = new Dictionary<string, ExplicitFormation>();
}