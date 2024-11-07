namespace ProjectPlanetary.Forming;

public partial class ExplicitFormedPrimePlanet
{
    public override bool Equals(ExplicitFormation? other)
    {
        return other!.GetHashCode() == GetHashCode();
    }
}