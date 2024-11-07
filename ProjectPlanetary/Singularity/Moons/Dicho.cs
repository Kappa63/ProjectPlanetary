namespace ProjectPlanetary.Forming;

public partial class ExplicitFormedDicho
{
    public override bool Equals(ExplicitFormation? other)
    {
        if (other!.Type == ExplicitType.DICHO)
            return State == (other as ExplicitFormedDicho)!.State;
        return false;
    }
}