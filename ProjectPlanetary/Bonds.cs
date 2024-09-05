namespace ProjectPlanetary;

public enum MoleculeType
{
    UNIVERSE,
    EXPLICIT_MAGNITUDE,
    ELEMENT,
    MAGNITUDINAL_OPERATION
}

public abstract class Molecule
{
    public abstract MoleculeType Type { get; }
}

public class Universe : Molecule
{
    public override MoleculeType Type { get; } = MoleculeType.UNIVERSE;
    public List<Molecule> Molecules { get; set; } = new List<Molecule>(); 
}

public abstract class Operation : Molecule { }

public class MagnitudinalOperation : Operation
{ 
    public override MoleculeType Type { get; } = MoleculeType.MAGNITUDINAL_OPERATION;
    public Operation? Pre { get; set; }
    public Operation? Post { get; set; }
    public string? MagnitudeOperator { get; set; }
}

public class Element : Operation
{
    public override MoleculeType Type { get; } = MoleculeType.ELEMENT;
    public string? Symbol { get; set; }
}

public class ExplicitMagnitude : Operation
{
    public override MoleculeType Type { get; } = MoleculeType.EXPLICIT_MAGNITUDE;

    public double? Magnitude { get; set; }
}