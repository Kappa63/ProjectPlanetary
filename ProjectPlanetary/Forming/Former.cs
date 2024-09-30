using System.Globalization;
using ProjectPlanetary.Singularity;
using ProjectPlanetary.Bonding;

namespace ProjectPlanetary.Forming;

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
            MoleculeType.EXPLICIT_MAGNITUDE => formProperFromExplicitMagnitude((mol as ExplicitMagnitude)!),
            MoleculeType.EXPLICIT_DICHO => formProperFromExplicitDicho((mol as ExplicitDicho)!),
            MoleculeType.EXPLICIT_TEXT => formProperFromExplicitText((mol as ExplicitText)!),
            // MoleculeType.EXPLICIT_VACUUM => new ExplicitFormedVacuum(),
            MoleculeType.VOYAGE_OPERATION => this.formPlanetaryVoyage((mol as VoyageOperation)!, sp),
            MoleculeType.MAGNITUDINAL_OPERATION => this.formMagnitudinalOperation((mol as MagnitudinalOperation)!, sp),
            MoleculeType.ELEMENT => retrieveElement((mol as Element)!, sp),
            MoleculeType.ELEMENT_SYNTHESIS => formElement((mol as ElementSynthesis)!, sp),
            MoleculeType.PLANET_SYNTHESIS => formPlanet((mol as PlanetSynthesis)!, sp),
            MoleculeType.LAW_SYNTHESIS => formLaw((mol as LawSynthesis)!, sp),
            MoleculeType.ELEMENT_MODIFICATION => formElementModificationOperation((mol as ElementModification)!, sp),
            MoleculeType.DICHOTOMIC_OPERATION => this.formDichotomicOperation((mol as DichotomicOperation)!, sp),
            MoleculeType.TEXT_OPERATION => this.formTextOperation((mol as TextOperation)!, sp),
            MoleculeType.EXPLICIT_ALLOY => formAlloy((mol as ExplicitAlloy)!, sp),
            MoleculeType.EXPLICIT_CLUSTER => formCluster((mol as ExplicitCluster)!, sp),
            MoleculeType.COMPOUND => this.formCompound((mol as Compound)!, sp),
            MoleculeType.ALLOY_TRAJECTORY_OPERATION => this.retrieveAlloyTrajectory((mol as AlloyTrajectoryOperation)!, sp),
            _ => throw new NotImplementedException("This Molecule type is not implemented yet.")
        };
    }
    
    private ExplicitFormation formProperFromExplicitMagnitude(ExplicitMagnitude mol)
    {
        ExplicitFormedMagnitude tempMag = new ExplicitFormedMagnitude() { Magnitude = mol.Magnitude };
        // if (mol.Dichotomous)
        //     return RetrieveExplicitDicho(tempMag);
        // if (mol.TextOP)
        //     return RetrieveExplicitText(tempMag);
        return tempMag;
    }
    
    private static ExplicitFormation formProperFromExplicitDicho(ExplicitDicho mol)
    {
        ExplicitFormedDicho tempMag = new ExplicitFormedDicho() { State = mol.State };
        // if (mol.TextOP)
        //     return RetrieveExplicitText(tempMag);
        return tempMag;
    }

    private ExplicitFormation formProperFromExplicitText(ExplicitText mol)
    {
        ExplicitFormedText tempMag = new ExplicitFormedText() { Text = mol.Text };
        // if (mol.Dichotomous)
        //     return RetrieveExplicitDicho(tempMag);
        // if (mol.TextOP)
        //     return RetrieveExplicitText(tempMag);
        return tempMag;
    }
    
    private ExplicitFormation formDichotomicOperation(DichotomicOperation operation, Space sp)
    {
        ExplicitFormation preFormation = formMolecule(operation.Pre!, sp);
        ExplicitFormation postFormation = formMolecule(operation.Post!, sp);
        // if (preFormation.Type == ExplicitType.VACUUM || postFormation.Type == ExplicitType.VACUUM)
        //     return new ExplicitFormedVacuum();
        ExplicitFormation tempMag = RetrieveExplicitDicho(formExplicitDichotomicOperation(preFormation, postFormation, operation.DichoOperator, operation.Negated));
        if (operation.TextOP)
            return RetrieveExplicitText(tempMag);
        return tempMag;        
    }
    
    private ExplicitFormation formTextOperation(TextOperation operation, Space sp)
    {
        ExplicitFormation preFormation = formMolecule(operation.Pre!, sp);
        ExplicitFormation postFormation = formMolecule(operation.Post!, sp);
        if (preFormation.Type == ExplicitType.VACUUM || postFormation.Type == ExplicitType.VACUUM)
            return new ExplicitFormedVacuum();
        ExplicitFormation tempMag = RetrieveExplicitText(formExplicitTextOperation(RetrieveExplicitText(preFormation), RetrieveExplicitText(postFormation), operation.TextOperator));
        if (operation.Dichotomous)
            return RetrieveExplicitDicho(tempMag);
        return tempMag;     
    }
    
    private static ExplicitFormedText RetrieveExplicitText(ExplicitFormation form)
    {
        return form.Type switch
        {
            ExplicitType.MAGNITUDE => new ExplicitFormedText()
            {
                Text = (form as ExplicitFormedMagnitude)!.Magnitude.ToString(CultureInfo.CurrentCulture) 
            },
            ExplicitType.DICHO => new ExplicitFormedText()
            {
                Text = (form as ExplicitFormedDicho)!.State.ToString(CultureInfo.CurrentCulture)
            },
            ExplicitType.TEXT => (form as ExplicitFormedText)!,
            _ => throw new ArgumentException("Can't Retrieve Text from non Dicho or Mag Expression")
        };
    }
    
    private static ExplicitFormation formExplicitTextOperation(ExplicitFormedText pre, ExplicitFormedText post, string operation)
    {
        return new ExplicitFormedText()
        {
            Text = operation switch
            {
                ".." => pre.Text!.TrimEnd('\"') + post.Text!.TrimStart('\"'),
                _ => throw new InvalidOperationException("Invalid operator")
            }
        };
    }
    
    private static ExplicitFormedDicho RetrieveExplicitDicho(ExplicitFormation form, bool negation=false)
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
            }:(form as ExplicitFormedDicho)!,
            ExplicitType.TEXT => new ExplicitFormedDicho()
            {
                State = negation?(form as ExplicitFormedText)!.Text!.Length==0:(form as ExplicitFormedText)!.Text!.Length!=0 
            },
            _ => throw new ArgumentException("Can't Retrieve Dicho from non Dicho or Mag Expression")
        };

        return x;
    }
    
    private ExplicitFormation formExplicitDichotomicOperation(ExplicitFormation pre, ExplicitFormation post, string operation, bool negation)
    {
        // Console.WriteLine(retrieveExplicitMagnitude(pre).Magnitude);
        bool dichoFormed = operation switch
        {
            "and" => RetrieveExplicitDicho(pre).State && RetrieveExplicitDicho(post).State,
            "or" => RetrieveExplicitDicho(pre).State || RetrieveExplicitDicho(post).State,
            "==" => performOptimalEquivalenceComp(pre, post),
            "!=" => !performOptimalEquivalenceComp(pre, post),
            ">>" => retrieveExplicitMagnitude(pre).Magnitude > retrieveExplicitMagnitude(post).Magnitude,
            "<<" => retrieveExplicitMagnitude(pre).Magnitude < retrieveExplicitMagnitude(post).Magnitude,
            ">=" => retrieveExplicitMagnitude(pre).Magnitude >= retrieveExplicitMagnitude(post).Magnitude,
            "<=" => retrieveExplicitMagnitude(pre).Magnitude <= retrieveExplicitMagnitude(post).Magnitude,
            _ => throw new InvalidOperationException("Invalid operator")
        };
        return new ExplicitFormedDicho()
        {
            State = negation?!dichoFormed:dichoFormed
        };
    }

    private static bool performOptimalEquivalenceComp(ExplicitFormation preForm, ExplicitFormation postForm)
    {
        const double TOLERANCE = 1e-6;
        return (preForm.Type) switch
        {
            ExplicitType.MAGNITUDE => Math.Abs((preForm as ExplicitFormedMagnitude)!.Magnitude - retrieveExplicitMagnitude(postForm).Magnitude) < TOLERANCE,
            ExplicitType.TEXT => (preForm as ExplicitFormedText)!.Text!.Trim('\u0022') == RetrieveExplicitText(postForm).Text!.Trim('\u0022'),
            ExplicitType.DICHO => (preForm as ExplicitFormedDicho)!.State == RetrieveExplicitDicho(postForm).State,
            ExplicitType.VACUUM => postForm.Type == ExplicitType.VACUUM
        };
    }

    private ExplicitFormation formMagnitudinalOperation(MagnitudinalOperation operation, Space sp)
    {
        ExplicitFormation preFormation = formMolecule(operation.Pre!, sp);
        ExplicitFormation postFormation = formMolecule(operation.Post!, sp);

        if (preFormation.Type == ExplicitType.VACUUM || postFormation.Type == ExplicitType.VACUUM)
            return new ExplicitFormedVacuum();
        ExplicitFormation tempMag = formExplicitMagnitudinalOperation(retrieveExplicitMagnitude(preFormation), retrieveExplicitMagnitude(postFormation), operation.MagnitudeOperator);
        if (operation.Dichotomous)
            return RetrieveExplicitDicho(tempMag);
        if (operation.TextOP)
            return RetrieveExplicitText(tempMag);
        return tempMag;
    }

    private static ExplicitFormedMagnitude retrieveExplicitMagnitude(ExplicitFormation form)
    {
        return form.Type switch
        {
            ExplicitType.MAGNITUDE => (form as ExplicitFormedMagnitude)!,
            ExplicitType.DICHO => new ExplicitFormedMagnitude()
            {
                Magnitude = (form as ExplicitFormedDicho)!.State ? 1 : 0
            },
            ExplicitType.TEXT => new ExplicitFormedMagnitude()
            {
                Magnitude = (form as ExplicitFormedText)!.Text!.Length==0 ? 0 : 1
            },
            _ => throw new ArgumentException("Can't Retrieve Magnitude from non Dicho or Mag Expression")
        };
    }

    private static ExplicitFormation formExplicitMagnitudinalOperation(ExplicitFormedMagnitude pre, ExplicitFormedMagnitude post, string operation)
    {
        return new ExplicitFormedMagnitude()
        {
            Magnitude = operation switch
            {
                "+" => pre.Magnitude + post.Magnitude,
                "-" => pre.Magnitude - post.Magnitude,
                "*" => pre.Magnitude * post.Magnitude,
                "/" => post.Magnitude != 0? pre.Magnitude/post.Magnitude : throw new DivideByZeroException(),
                "%" => pre.Magnitude % post.Magnitude,
                _ => throw new InvalidOperationException("Invalid operator")
            }
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
    
    private ExplicitFormation formCluster(ExplicitCluster cluster, Space sp)
    {
        ExplicitFormedCluster formedCluster = new ExplicitFormedCluster();
        foreach (Operation o in cluster.Operations)
            formedCluster.Forms.Add(this.formMolecule(o, sp)); //sp.retrieveElement(p.Symbol!)
        return formedCluster;
    }
    
    private ExplicitFormation retrieveElement(Element element, Space sp)
    {
        return  sp.retrieveElement(element.Symbol!);
        // ExplicitFormation tempFormation = sp.retrieveElement(element.Symbol!);
        // return element.DichoNegated?RetrieveExplicitDicho(tempFormation, true):element.Dichotomous?RetrieveExplicitDicho(tempFormation):tempFormation;
    }
    
    private ExplicitFormation formElement(ElementSynthesis element, Space sp)
    {
        return sp.synthesizeElement(element.Symbol!, element.Magnitude!=null?this.formMolecule(element.Magnitude, sp):new ExplicitFormedVacuum(), element.Stable!.Value);
    }

    private static ExplicitFormation formPlanet(PlanetSynthesis planet, Space sp)
    {
        return sp.synthesizeElement(planet.Symbol!, new ExplicitFormedPlanet()
        {
            Symbol = planet.Symbol,
            PayloadSymbols = planet.PayloadSymbols,
            PlanetCompound = planet.Compound,
            PlanetSpace = sp
        }, true);
    } 

    private ExplicitFormation retrieveAlloyTrajectory(AlloyTrajectoryOperation alloy, Space sp)
    {
        ExplicitFormedAlloy tempAlloy = (this.formMolecule(alloy.Alloy!, sp) as ExplicitFormedAlloy)!;
        string propName = (alloy.Property as Element)!.Symbol!;
        if (tempAlloy.Properties.TryGetValue(propName, out var trajectory))
            return trajectory;
        throw new ArgumentException($"Property {propName} not found in alloy");
    }

    private ExplicitFormation formPlanetaryVoyage(VoyageOperation voyage, Space sp)
    {
        List<ExplicitFormation> formedPayload  = voyage.Payload.ConvertAll(load => this.formMolecule(load, sp));
        ExplicitFormation planet = this.formMolecule(voyage.Planet!, sp);

        if (planet.Type == ExplicitType.PRIME_PLANET)
            return (planet as ExplicitFormedPrimePlanet)!.Voyage!(formedPayload, sp);
        if (planet.Type == ExplicitType.PLANET)
        {
            ExplicitFormedPlanet tempPlanet = (planet as ExplicitFormedPlanet)!;
            Space planetSpace = new Space(tempPlanet.PlanetSpace);
            if (formedPayload.Count != tempPlanet.PayloadSymbols.Count)
                throw new ArgumentException("Payload is lacking elements.");
            for (int i = 0; i < tempPlanet.PayloadSymbols.Count; i++)
                planetSpace.synthesizeElement(tempPlanet.PayloadSymbols[i], formedPayload[i], false);
            return this.formCompound(tempPlanet.PlanetCompound!, planetSpace);
        }
        throw new Exception("Can't call non-planet.");
    }

    private ExplicitFormation formLaw(LawSynthesis law, Space sp)
    {
        Space lawSpace = new Space(sp);
        ExplicitFormation tempForm = new ExplicitFormedVacuum();
        
        do
            if (RetrieveExplicitDicho(this.formMolecule(law.LawDicho!, lawSpace), !law.Validator).State)
                tempForm = this.formCompound(law.LawCompound!, lawSpace);
            else break;
        while (law.Orbiter);
        
        return tempForm;
    }
}