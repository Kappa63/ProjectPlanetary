namespace ProjectPlanetary;

public enum MoleculeType
{
    COMPOUND,
    ELEMENT_SYNTHESIS,
    ELEMENT_MODIFICATION,
    
    ALLOY_TRAJECTORY_OPERATION,
    VOYAGE_OPERATION,
    
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
    public List<Molecule> Molecules { get; init; } = new List<Molecule>(); 
}

public class ElementSynthesis : Molecule
{
    public override MoleculeType Type { get; } = MoleculeType.ELEMENT_SYNTHESIS;
    public bool? Stable { get; init; }
    public string? Symbol { get; init; }
    public Operation? Magnitude { get; init; }
}

public abstract class Operation : Molecule { }

public class MagnitudinalOperation : Operation
{ 
    public override MoleculeType Type { get; } = MoleculeType.MAGNITUDINAL_OPERATION;
    public Operation? Pre { get; init; }
    public Operation? Post { get; init; }
    public string MagnitudeOperator { get; init; } = "+";
    public bool Dichotomous { get; set; } = false;
}

public class DichotomicOperation : Operation
{ 
    public override MoleculeType Type { get; } = MoleculeType.DICHOTOMIC_OPERATION;
    public Operation? Pre { get; init; }
    public Operation? Post { get; init; }
    public string DichoOperator { get; init; } = "==";
    public bool Negated { get; init; }
    
}

public class Element : Operation
{
    public override MoleculeType Type { get; } = MoleculeType.ELEMENT;
    public string? Symbol { get; init; }
    public bool DichoNegated { get; init; }
}

public class Property : Operation
{
    public override MoleculeType Type { get; } = MoleculeType.PROPERTY;
    public string? Symbol { get; init; }
    public Operation? Magnitude { get; init; }
}

public class VoyageOperation : Operation
{
    public override MoleculeType Type { get; } = MoleculeType.VOYAGE_OPERATION;
    public List<Operation> Payload { get; init; } = new List<Operation>();
    public Operation? Origin { get; init; }
}

public class AlloyTrajectoryOperation : Operation
{
    public override MoleculeType Type { get; } = MoleculeType.ALLOY_TRAJECTORY_OPERATION;
    public Operation? Alloy { get; init; }
    public Operation? Property { get; init; }
    public bool? Calculated { get; init; }
}

public class ExplicitAlloy : Operation
{
    public override MoleculeType Type { get; } = MoleculeType.EXPLICIT_ALLOY;
    public List<Property> Properties { get; init; } = new List<Property>();
}

public class ExplicitMagnitude : Operation
{
    public override MoleculeType Type { get; } = MoleculeType.EXPLICIT_MAGNITUDE;

    public double Magnitude { get; init; }
}

public class ExplicitDicho : Operation
{
    public override MoleculeType Type { get; } = MoleculeType.EXPLICIT_DICHO;

    public bool State { get; init; }
}

public class ElementModification : Operation
{
    public override MoleculeType Type { get; } = MoleculeType.ELEMENT_MODIFICATION;
    public Operation? Element { get; init; }
    public Operation? Magnitude { get; init; }
}

// public class ExplicitVacuum : Operation
// {
//     public override MoleculeType Type { get; } = MoleculeType.EXPLICIT_VACUUM;
//
//     public string? Magnitude { get; } = "vacuum";
// }