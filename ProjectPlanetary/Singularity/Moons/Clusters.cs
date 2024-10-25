namespace ProjectPlanetary.Forming;

public partial class ExplicitFormedCluster
{
    private KeyValuePair<string, ExplicitFormedMoon> _count;
    
    private void SynthMoons()
    {
        _count = new KeyValuePair<string, ExplicitFormedMoon>( "count", new ExplicitFormedMoon() { Voyage = Count } );
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