namespace ProjectPlanetary.Forming;

public abstract partial class ExplicitFormation : IEquatable<ExplicitFormation>
{
    public abstract bool Equals(ExplicitFormation? other);
    protected virtual void SynthMoons(){}
    
    public bool TryGetMoon(string planetSymbol, out ExplicitFormation? planet)
    {
        return Moons.TryGetValue(planetSymbol, out planet);
    }

    protected ExplicitFormation()
    {
        SynthMoons();
    }
}