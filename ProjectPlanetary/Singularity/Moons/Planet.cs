namespace ProjectPlanetary.Forming;

public partial class ExplicitFormedPlanet
{
    public override bool Equals(ExplicitFormation? other)
    {
        return other!.GetHashCode() == GetHashCode();
    }
}