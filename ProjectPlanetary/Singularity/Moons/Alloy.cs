namespace ProjectPlanetary.Forming;

public partial class ExplicitFormedAlloy
{
    public override bool Equals(ExplicitFormation? other)
    {
        return other!.GetHashCode() == GetHashCode();
    }
}