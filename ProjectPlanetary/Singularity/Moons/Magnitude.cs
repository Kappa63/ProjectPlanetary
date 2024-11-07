namespace ProjectPlanetary.Forming;

public partial class ExplicitFormedMagnitude
{   
    public override bool Equals(ExplicitFormation? other)
    {
        if (other!.Type == ExplicitType.MAGNITUDE)
            return Math.Abs(Magnitude - (other as ExplicitFormedMagnitude)!.Magnitude) < 1e-6;
        return false;
    }
}