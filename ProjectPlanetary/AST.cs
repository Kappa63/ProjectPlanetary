namespace ProjectPlanetary;

public enum MoleculeType
{
    Universe,
    NumericLiteral,
    Identifier,
    BinaryExpr
}

public interface IMolecule
{
    MoleculeType Type { get; }
}

public interface IUniverse : IMolecule
{
    new MoleculeType Type { get; }
    List<IMolecule> Atoms { get; }
}

public interface IOperational : IMolecule { }

public interface IMagnitudinalCompound : IOperational
{ 
    new MoleculeType Type { get; }
    IOperational Left { get; }
    IOperational Right { get; }
    string MagnitudeOperator { get; }
}

public interface IElement : IOperational
{
    new MoleculeType Type { get; }
    string Symbol { get; }
}

public interface IMagnitudeLit : IOperational
{
    new MoleculeType Type { get; }
    double Value { get; }
}