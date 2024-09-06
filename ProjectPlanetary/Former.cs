namespace ProjectPlanetary;

public class Former
{
    public ExplicitFormation formCompound(Compound compound, Space sp)
    {
        ExplicitFormation finalFormation = new ExplicitFormedVacuum();
        foreach (Molecule mol in compound.Molecules)
        {
            finalFormation = formMolecule(mol, sp);
        }
        return finalFormation;
    }

    private ExplicitFormation formMolecule(Molecule mol, Space sp)
    {
        return mol.Type switch
        {
            MoleculeType.EXPLICIT_MAGNITUDE => new ExplicitFormedMagnitude(){Magnitude = (mol as ExplicitMagnitude)!.Magnitude},
            // MoleculeType.EXPLICIT_VACUUM => new ExplicitFormedVacuum(),
            MoleculeType.MAGNITUDINAL_OPERATION => this.formOperation((mol as MagnitudinalOperation)!, sp),
            MoleculeType.ELEMENT => retrieveElement((mol as Element)!, sp),
            MoleculeType.ELEMENT_SYNTHESIS => formElement((mol as ElementSynthesis)!, sp),
            MoleculeType.ELEMENT_MODIFICATION => formElementModificationOperation((mol as ElementModification)!, sp),
            MoleculeType.EXPLICIT_ALLOY => formAlloy((mol as ExplicitAlloy)!, sp),
            MoleculeType.COMPOUND => this.formCompound((mol as Compound)!, sp),
            _ => throw new NotImplementedException("This Molecule type is not implemented yet.")
        };
    }

    private ExplicitFormation formOperation(MagnitudinalOperation operation, Space sp)
    {

        ExplicitFormation preFormation = formMolecule(operation.Pre!, sp);
        ExplicitFormation postFormation = formMolecule(operation.Post!, sp);

        if (preFormation.Type == ExplicitType.MAGNITUDE && postFormation.Type == ExplicitType.MAGNITUDE)
        {
            return formExplicitOperation((preFormation as ExplicitFormedMagnitude)!, (postFormation as ExplicitFormedMagnitude)!, operation.MagnitudeOperator);
        }
        return new ExplicitFormedVacuum();
    }

    private static ExplicitFormation formExplicitOperation(ExplicitFormedMagnitude pre, ExplicitFormedMagnitude post, string operation)
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

    private ExplicitFormation formElementModificationOperation(ElementModification elementModification, Space sp)
    {
        if (elementModification.Element!.Type != MoleculeType.ELEMENT)
            throw new NotImplementedException("Can only modify simple elements.");
        return sp.modifyElement((elementModification.Element as Element)!.Symbol!, this.formMolecule(elementModification.Magnitude!, sp));
    }

    private ExplicitFormation formAlloy(ExplicitAlloy alloy, Space sp)
    {
        ExplicitFormedAlloy formedAlloy = new ExplicitFormedAlloy();
        foreach (Property p in alloy.Properties)
            formedAlloy.properties.Add(p.Symbol!, p.Magnitude!=null?this.formMolecule(p.Magnitude, sp):new ExplicitFormedVacuum()); //sp.retrieveElement(p.Symbol!)
        return formedAlloy;
    }

    private static ExplicitFormation retrieveElement(Element element, Space sp)
    {
        return sp.retrieveElement(element.Symbol!);
    }
    
    private ExplicitFormation formElement(ElementSynthesis element, Space sp)
    {
        return sp.synthesizeElement(element.Symbol!, element.Magnitude!=null?this.formMolecule(element.Magnitude, sp):new ExplicitFormedVacuum(), element.Stable!.Value);
    }
}