namespace ProjectPlanetary.Forming;

public partial class ExplicitFormedCluster
{
    protected override void SynthMoons()
    {
        Moons.Add("at", new ExplicitFormedMoon() { Voyage = At });
        Moons.Add("formAt", new ExplicitFormedMoon() { Voyage = TransformTo });
        Moons.Add("has", new ExplicitFormedMoon() { Voyage = Has });
        Moons.Add("count", new ExplicitFormedMoon() { Voyage = Count });
        Moons.Add("add", new ExplicitFormedMoon() { Voyage = Add });
        Moons.Add("addAt", new ExplicitFormedMoon() { Voyage = AddAt });
        Moons.Add("rem", new ExplicitFormedMoon() { Voyage = Remove });
        Moons.Add("remAt", new ExplicitFormedMoon() { Voyage = RemoveAt });
        Moons.Add("annihilate", new ExplicitFormedMoon() { Voyage = Annihilate });
    }
    
    public override bool Equals(ExplicitFormation? other)
    {
        if (other!.Type == ExplicitType.CLUSTER)
            return Forms.SequenceEqual((other as ExplicitFormedCluster)!.Forms);
        return false;
    }
    
    
    private ExplicitFormation At(List<ExplicitFormation>? payload)
    {
        if (payload is { Count: 1 })
        {
            if (payload[0].Type == ExplicitType.MAGNITUDE)
                return Forms[(int)(payload[0] as ExplicitFormedMagnitude)!.Magnitude];
            throw new ArgumentException("Expected payload to contain a valid position.");
        }
        return new ExplicitFormedVacuum();
    }
    
    private ExplicitFormation TransformTo(List<ExplicitFormation>? payload)
    {
        if (payload is { Count: 2 })
        {
            if (payload[0].Type == ExplicitType.MAGNITUDE)
                Forms[(int)(payload[0] as ExplicitFormedMagnitude)!.Magnitude] = payload[1];
            else 
                throw new ArgumentException("Expected payload to contain a valid position.");
        }
        return new ExplicitFormedVacuum();
    }
    
    private ExplicitFormedDicho Has(List<ExplicitFormation>? payload)
    {
        if (payload is { Count: 1 })
            return new ExplicitFormedDicho() { State = Forms.Contains(payload[0]) };
        throw new ArgumentException("Expected payload to contain a formation.");
    }
    
    private ExplicitFormedMagnitude Count(List<ExplicitFormation>? _)
    {
        return new ExplicitFormedMagnitude()
        {
            Magnitude = Forms.Count
        };
    }
    
    private ExplicitFormedVacuum Add(List<ExplicitFormation>? payload)
    {
        if (payload is not {Count:0})
            Forms.AddRange(payload!);
        return new ExplicitFormedVacuum();
    }

    private ExplicitFormedVacuum AddAt(List<ExplicitFormation>? payload)
    {
        if (payload is not { Count: 0 })
        {
            if (payload![0].Type == ExplicitType.MAGNITUDE)
            {
                int i = (int)(payload[0] as ExplicitFormedMagnitude)!.Magnitude;
                payload.RemoveAt(0);
                Forms.InsertRange(i, payload);
            }
            else
                throw new ArgumentException("Expected payload to contain a valid position.");
        }
        return new ExplicitFormedVacuum();
    }
    
    private ExplicitFormedVacuum Remove(List<ExplicitFormation>? payload)
    {
        if (payload is not { Count: 0 })
            foreach (ExplicitFormation p in payload!)
                Forms.Remove(p);
        return new ExplicitFormedVacuum();
    }
    
    private ExplicitFormedVacuum RemoveAt(List<ExplicitFormation>? payload)
    {
        if (payload is { Count: 1 })
        {
            if (payload[0].Type == ExplicitType.MAGNITUDE)
                Forms.RemoveAt((int)(payload[0] as ExplicitFormedMagnitude)!.Magnitude);
            else
                throw new ArgumentException("Expected payload to contain a valid position.");
        }
        return new ExplicitFormedVacuum();
    }

    private ExplicitFormedVacuum Annihilate(List<ExplicitFormation>? _)
    {
        Forms.Clear();
        return new ExplicitFormedVacuum();
    }
}