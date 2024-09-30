namespace ProjectPlanetary.Bonding;

public enum MoleculeType
{
    COMPOUND,
    ELEMENT_SYNTHESIS,
    PLANET_SYNTHESIS,
    LAW_SYNTHESIS,
    ELEMENT_MODIFICATION,
    
    ALLOY_TRAJECTORY_OPERATION,
    VOYAGE_OPERATION,
    
    EXPLICIT_MAGNITUDE,
    EXPLICIT_DICHO,
    EXPLICIT_TEXT,
    // EXPLICIT_VACUUM,
        
    ELEMENT,
    PROPERTY,
    EXPLICIT_ALLOY,
    EXPLICIT_CLUSTER,
    MAGNITUDINAL_OPERATION,
    DICHOTOMIC_OPERATION,
    TEXT_OPERATION
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

public class PlanetSynthesis : Molecule
{
    public override MoleculeType Type { get; } = MoleculeType.PLANET_SYNTHESIS;
    public string? Symbol {get; init; }
    public Compound? Compound { get; init; }
    public List<string> PayloadSymbols {get; init; } = new List<string>();
}

public class LawSynthesis : Molecule
{
    public override MoleculeType Type { get; } = MoleculeType.LAW_SYNTHESIS;
    public Operation? LawDicho  { get; init; }
    public bool Validator = true;
    public bool Cycler = false;
    public bool Orbiter = false;
    public Compound? LawCompound { get; init; }
}

public class ElementSynthesis : Molecule
{
    public override MoleculeType Type { get; } = MoleculeType.ELEMENT_SYNTHESIS;
    public bool? Stable { get; init; }
    public string? Symbol { get; init; }
    public Operation? Magnitude { get; init; }
}


public abstract class Operation : Molecule
{
    public bool Dichotomous { get; set; } = false;
    public bool TextOP { get; set; } = false;
}

public class MagnitudinalOperation : Operation
{ 
    public override MoleculeType Type { get; } = MoleculeType.MAGNITUDINAL_OPERATION;
    public Operation? Pre { get; init; }
    public Operation? Post { get; init; }
    public string MagnitudeOperator { get; init; } = "+";
}

public class DichotomicOperation : Operation
{ 
    public override MoleculeType Type { get; } = MoleculeType.DICHOTOMIC_OPERATION;
    public Operation? Pre { get; init; }
    public Operation? Post { get; init; }
    public string DichoOperator { get; init; } = "==";
    public bool Negated { get; init; }
}

public class TextOperation : Operation
{ 
    public override MoleculeType Type { get; } = MoleculeType.TEXT_OPERATION;
    public Operation? Pre { get; init; }
    public Operation? Post { get; init; }
    public string TextOperator { get; init; } = "..";
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
    public Operation? Planet { get; init; }
}

public class AlloyTrajectoryOperation : Operation
{
    public override MoleculeType Type { get; } = MoleculeType.ALLOY_TRAJECTORY_OPERATION;
    public Operation? Alloy { get; init; }
    public Operation? Property { get; init; }
    // public bool? Calculated { get; init; }
}

public class ExplicitAlloy : Operation
{
    public override MoleculeType Type { get; } = MoleculeType.EXPLICIT_ALLOY;
    public List<Property> Properties { get; init; } = new List<Property>();
}

public class ExplicitCluster : Operation
{
    public override MoleculeType Type { get; } = MoleculeType.EXPLICIT_CLUSTER;
    public List<Operation> Operations { get; init; } = new List<Operation>();
}

public class ExplicitMagnitude : Operation
{
    public override MoleculeType Type { get; } = MoleculeType.EXPLICIT_MAGNITUDE;

    public double Magnitude { get; init; }
}

public class ExplicitText : Operation
{
    public override MoleculeType Type { get; } = MoleculeType.EXPLICIT_TEXT;

    public string? Text { get; init; }
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