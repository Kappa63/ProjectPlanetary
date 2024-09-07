namespace ProjectPlanetary;

public enum MoleculeType
{
    COMPOUND,
    ELEMENT_SYNTHESIS,
    ELEMENT_MODIFICATION,
    EXPLICIT_MAGNITUDE,
    EXPLICIT_DICHO,
    // EXPLICIT_VACUUM,
    ELEMENT,
    PROPERTY,
    EXPLICIT_ALLOY,
    MAGNITUDINAL_OPERATION,
    DICHOTOMIC_OPERATION,
}

public abstract class Molecule
{
    public abstract MoleculeType Type { get; }
}

public class Compound : Molecule
{
    public override MoleculeType Type { get; } = MoleculeType.COMPOUND;
    public List<Molecule> Molecules { get; set; } = new List<Molecule>(); 
}

public class ElementSynthesis : Molecule
{
    public override MoleculeType Type { get; } = MoleculeType.ELEMENT_SYNTHESIS;
    public bool? Stable { get; set; }
    public string? Symbol { get; set; }
    public Operation? Magnitude { get; set; } = null;
}

public abstract class Operation : Molecule { }

public class MagnitudinalOperation : Operation
{ 
    public override MoleculeType Type { get; } = MoleculeType.MAGNITUDINAL_OPERATION;
    public Operation? Pre { get; set; }
    public Operation? Post { get; set; }
    public string MagnitudeOperator { get; set; } = "+";
}

public class DichotomicOperation : Operation
{ 
    public override MoleculeType Type { get; } = MoleculeType.DICHOTOMIC_OPERATION;
    public Operation? Pre { get; set; }
    public Operation? Post { get; set; }
    public string DichoOperator { get; set; } = "==";
    public bool Negated { get; set; } = false;
}

public class Element : Operation
{
    public override MoleculeType Type { get; } = MoleculeType.ELEMENT;
    public string? Symbol { get; set; }
    public bool DichoNegated { get; set; } = false;
}

public class Property : Operation
{
    public override MoleculeType Type { get; } = MoleculeType.PROPERTY;
    public string? Symbol { get; set; }
    public Operation? Magnitude { get; set; } = null;
}

public class ExplicitAlloy : Operation
{
    public override MoleculeType Type { get; } = MoleculeType.EXPLICIT_ALLOY;
    public List<Property> Properties { get; set; } = new List<Property>();
}

public class ExplicitMagnitude : Operation
{
    public override MoleculeType Type { get; } = MoleculeType.EXPLICIT_MAGNITUDE;

    public double Magnitude { get; set; } = 0;
}

public class ExplicitDicho : Operation
{
    public override MoleculeType Type { get; } = MoleculeType.EXPLICIT_DICHO;

    public bool State { get; set; } = false;
}

public class ElementModification : Operation
{
    public override MoleculeType Type { get; } = MoleculeType.ELEMENT_MODIFICATION;
    public Operation? Element { get; set; }
    public Operation? Magnitude { get; set; }
}

// public class ExplicitVacuum : Operation
// {
//     public override MoleculeType Type { get; } = MoleculeType.EXPLICIT_VACUUM;
//
//     public string? Magnitude { get; } = "vacuum";
// }