using System.Text.Json;

namespace ProjectPlanetary;

public class Former
{
    public ExplicitFormation formCompound(Compound compound, Space sp)
    {
        ExplicitFormation finalFormation = new ExplicitFormedVacuum();
        
        // foreach (Molecule mol in compound.Molecules)
        //     Console.WriteLine(JsonSerializer.Serialize(mol));

        foreach (Molecule mol in compound.Molecules)
            finalFormation = formMolecule(mol, sp);
        return finalFormation;
    }

    private ExplicitFormation formMolecule(Molecule mol, Space sp)
    {
        return mol.Type switch
        {
            MoleculeType.EXPLICIT_MAGNITUDE => new ExplicitFormedMagnitude(){Magnitude = (mol as ExplicitMagnitude)!.Magnitude},
            MoleculeType.EXPLICIT_DICHO => new ExplicitFormedDicho(){State = (mol as ExplicitDicho)!.State},
            // MoleculeType.EXPLICIT_VACUUM => new ExplicitFormedVacuum(),
            MoleculeType.MAGNITUDINAL_OPERATION => this.formMagnitudinalOperation((mol as MagnitudinalOperation)!, sp),
            MoleculeType.ELEMENT => retrieveElement((mol as Element)!, sp),
            MoleculeType.ELEMENT_SYNTHESIS => formElement((mol as ElementSynthesis)!, sp),
            MoleculeType.ELEMENT_MODIFICATION => formElementModificationOperation((mol as ElementModification)!, sp),
            MoleculeType.DICHOTOMIC_OPERATION => this.formDichotomicOperation((mol as DichotomicOperation)!, sp),
            MoleculeType.EXPLICIT_ALLOY => formAlloy((mol as ExplicitAlloy)!, sp),
            MoleculeType.COMPOUND => this.formCompound((mol as Compound)!, sp),
            MoleculeType.ALLOY_TRAJECTORY_OPERATION => this.retrieveAlloyTrajectory((mol as AlloyTrajectoryOperation)!, sp),
            _ => throw new NotImplementedException("This Molecule type is not implemented yet.")
        };
    }
    
    private ExplicitFormation formDichotomicOperation(DichotomicOperation operation, Space sp)
    {
        ExplicitFormation preFormation = formMolecule(operation.Pre!, sp);
        ExplicitFormation postFormation = formMolecule(operation.Post!, sp);
        if (preFormation.Type != ExplicitType.VACUUM && postFormation.Type != ExplicitType.VACUUM)
            return RetrieveExplicitDicho(formExplicitDichotomicOperation(RetrieveExplicitDicho(preFormation), RetrieveExplicitDicho(postFormation), operation.DichoOperator, operation.Negated));
        return new ExplicitFormedVacuum();
    }
    
    private ExplicitFormedDicho RetrieveExplicitDicho(ExplicitFormation form, bool negation=false)
    {
        ExplicitFormedDicho x = form.Type switch
        {
            ExplicitType.MAGNITUDE => new ExplicitFormedDicho()
            {
                State = negation?(form as ExplicitFormedMagnitude)!.Magnitude==0:(form as ExplicitFormedMagnitude)!.Magnitude!=0 
            },
            ExplicitType.DICHO => negation?new ExplicitFormedDicho()
            {
                State = !((form as ExplicitFormedDicho)!.State)
            }:(form as ExplicitFormedDicho)!
        };

        return x;
    }
    
    private static ExplicitFormation formExplicitDichotomicOperation(ExplicitFormedDicho pre, ExplicitFormedDicho post, string operation, bool negation)
    {
        bool dichoFormed = operation switch
        {
            "and" => pre.State && post.State,
            "or" => pre.State || post.State,
            _ => throw new InvalidOperationException("Invalid operator")
        };
        return new ExplicitFormedDicho()
        {
            State = negation?!dichoFormed:dichoFormed
        };
    }

    private ExplicitFormation formMagnitudinalOperation(MagnitudinalOperation operation, Space sp)
    {
        ExplicitFormation preFormation = formMolecule(operation.Pre!, sp);
        ExplicitFormation postFormation = formMolecule(operation.Post!, sp);

        if (preFormation.Type == ExplicitType.VACUUM || postFormation.Type == ExplicitType.VACUUM)
            return new ExplicitFormedVacuum();
        ExplicitFormation tempMag = formExplicitMagnitudinalOperation(retrieveExplicitMagnitude(preFormation), retrieveExplicitMagnitude(postFormation), operation.MagnitudeOperator);
        return operation.Dichotomous?this.RetrieveExplicitDicho(tempMag):tempMag;
    }

    private static ExplicitFormedMagnitude retrieveExplicitMagnitude(ExplicitFormation form)
    {
        return form.Type switch
        {
            ExplicitType.MAGNITUDE => (form as ExplicitFormedMagnitude)!,
            ExplicitType.DICHO => new ExplicitFormedMagnitude()
            {
                Magnitude = (form as ExplicitFormedDicho)!.State ? 1 : 0
            }
        };
    }

    private static ExplicitFormation formExplicitMagnitudinalOperation(ExplicitFormedMagnitude pre, ExplicitFormedMagnitude post, string operation)
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
            formedAlloy.Properties.Add(p.Symbol!, p.Magnitude!=null?this.formMolecule(p.Magnitude, sp):new ExplicitFormedVacuum()); //sp.retrieveElement(p.Symbol!)
        return formedAlloy;
    }
    
    private ExplicitFormation retrieveElement(Element element, Space sp)
    {
        ExplicitFormation tempFormation = sp.retrieveElement(element.Symbol!);
        return element.DichoNegated?this.RetrieveExplicitDicho(tempFormation, true):tempFormation;
    }
    
    private ExplicitFormation formElement(ElementSynthesis element, Space sp)
    {
        return sp.synthesizeElement(element.Symbol!, element.Magnitude!=null?this.formMolecule(element.Magnitude, sp):new ExplicitFormedVacuum(), element.Stable!.Value);
    }

    private ExplicitFormation retrieveAlloyTrajectory(AlloyTrajectoryOperation alloy, Space sp)
    {
        if (alloy.Alloy!.Type != MoleculeType.ELEMENT) throw new NotImplementedException("Can only modify simple elements.");
        ExplicitFormedAlloy tempAlloy = (this.retrieveElement((alloy.Alloy as Element)!, sp) as ExplicitFormedAlloy)!;
        string propName = (alloy.Property as Element)!.Symbol!;
        if (tempAlloy.Properties.TryGetValue(propName, out var trajectory))
            return trajectory;
        throw new ArgumentException($"Property {propName} not found in alloy");
    }
}