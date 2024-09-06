namespace ProjectPlanetary;

public class Former
{
    public ExplicitFormation formCompound(Compound compound)
    {
        ExplicitFormation finalFormation = new ExplicitFormedVacuum();
        foreach (Molecule mol in compound.Molecules)
        {
            finalFormation = formMolecule(mol);
        }
        return finalFormation;
    }

    private ExplicitFormation formMolecule(Molecule mol)
    {
        return mol.Type switch
        {
            MoleculeType.EXPLICIT_MAGNITUDE => new ExplicitFormedMagnitude(){Magnitude = (mol as ExplicitMagnitude)!.Magnitude},
            MoleculeType.EXPLICIT_VACUUM => new ExplicitFormedVacuum(),
            MoleculeType.MAGNITUDINAL_OPERATION => formOperation((mol as MagnitudinalOperation)!),
            MoleculeType.COMPOUND => formCompound((mol as Compound)!),
            _ => throw new NotImplementedException("This Molecule type is not implemented yet.")
        };
    }

    private ExplicitFormation formOperation(MagnitudinalOperation operation)
    {
        ExplicitFormation preFormation = formMolecule(operation.Pre!);
        ExplicitFormation postFormation = formMolecule(operation.Post!);

        if (preFormation.Type == ExplicitType.MAGNITUDE && postFormation.Type == ExplicitType.MAGNITUDE)
        {
            return formExplicitOperation((preFormation as ExplicitFormedMagnitude)!, (postFormation as ExplicitFormedMagnitude)!, operation.MagnitudeOperator);
        }
        return new ExplicitFormedVacuum();
    }

    private ExplicitFormation formExplicitOperation(ExplicitFormedMagnitude pre, ExplicitFormedMagnitude post, string operation)
    {
        double magFormed = operation switch
        {
            "+" => pre.Magnitude + post.Magnitude,
            "-" => pre.Magnitude - post.Magnitude,
            "*" => pre.Magnitude * post.Magnitude,
            "/" => post.Magnitude != 0? pre.Magnitude/post.Magnitude : throw new DivideByZeroException(),
            "%" => pre.Magnitude % post.Magnitude,
            _ => throw new InvalidOperationException("Invalid operator")
        };
        return new ExplicitFormedMagnitude()
        {
            Magnitude = magFormed
        };
    }
}