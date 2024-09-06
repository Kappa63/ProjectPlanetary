namespace ProjectPlanetary;

public enum ExplicitType
{
    MAGNITUDE,
    DICHO,
    VACUUM,
}

public abstract class ExplicitFormation
{
    public abstract ExplicitType Type { get; }
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

public class ExplicitFormedVacuum : ExplicitFormation
{
    public override ExplicitType Type { get; } = ExplicitType.VACUUM;
}