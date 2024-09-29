using System.Globalization;
using Microsoft.VisualBasic.FileIO;

namespace ProjectPlanetary.Bonding;
public class Bonder2
{
    private List<Atom> Atoms = new List<Atom>();

    public Compound bondCompounds(string system)
    {
        this.Atoms = Reactor.Fission(system);

        Compound compound = new Compound();

        while (this.checkHorizon())
        {
            compound.Molecules.Add(this.bondMolecule());
            this.retrieveAtom(true, AtomType.POLE);
        }

        return compound;
    }
    
    private bool checkHorizon()
    {
        return this.Atoms.First().Type != AtomType.HORIZON;
    }

    private Atom retrieveAtom(bool decay, AtomType? expectedAtom=null)
    {
        Atom decayedAtom = this.Atoms.First();
        if (decay)
            this.Atoms.RemoveAt(0);
        if (expectedAtom.HasValue && expectedAtom != decayedAtom.Type)
            throw new MalformedLineException($"Expected {expectedAtom} but got {decayedAtom.Type}");
        return decayedAtom;
    }

    private Molecule bondMolecule()
    {
        return this.retrieveAtom(false).Type switch
        {
            AtomType.ELEMENT_SYNTHESIZER or AtomType.ELEMENT_STABILIZER => this.bondElementSynthesis(),
            AtomType.LAW_SYNTHESIZER => this.bondLawSynthesis(),
            _ => this.bondOperation()
        };
    }

    private Molecule bondElementSynthesis()
    {
        Atom tempAtom = this.retrieveAtom(true);
        if (tempAtom.Type == AtomType.ELEMENT_SYNTHESIZER && this.retrieveAtom(false).Type == AtomType.PLANET_SYNTHESIZER) return this.bondPlanetSynthesis();
        bool elementalStability = tempAtom.Type == AtomType.ELEMENT_STABILIZER;
        if (elementalStability) this.retrieveAtom(true, AtomType.ELEMENT_SYNTHESIZER);
        string elementalSymbol = this.retrieveAtom(true, AtomType.ELEMENT).Value;

        if (this.retrieveAtom(false, elementalStability?AtomType.EQUIVALENCE:null).Type == AtomType.POLE)
        {
            return new ElementSynthesis()
            {
                Stable = false,
                Symbol = elementalSymbol,
            };
        } 
        this.retrieveAtom(true, AtomType.EQUIVALENCE); 
        
        ElementSynthesis elementSynthesis =  new ElementSynthesis()
        {
            Stable = elementalStability,
            Symbol = elementalSymbol,
            Magnitude = this.bondOperation()
        };
        // this.retrieveAtom(true, AtomType.POLE);
        return elementSynthesis;
    }

    private Molecule bondLawSynthesis()
    {
        this.retrieveAtom(true);
        this.retrieveAtom(true, AtomType.DICHO_ENCLOSURE);
        Operation tempDichoOp = this.bondDisjunctionOperation();
        this.retrieveAtom(true, AtomType.DICHO_ENCLOSURE);
        Atom curAtom = this.retrieveAtom(false);
        bool validation = true;
        bool cyclic = false;
        bool orbital = false;
        while (checkHorizon() && curAtom.Type != AtomType.OPEN_CURLED_ENCLOSURE)
        {
            switch (this.retrieveAtom(true).Type)
            {
                case AtomType.LAW_VALIDATOR:
                    validation = true;
                    break;
                case AtomType.LAW_INVALIDATOR:
                    validation = false;
                    break;
                case AtomType.LAW_CYCLER:
                    cyclic = true;
                    break;
                case AtomType.LAW_ORBITER:
                    orbital = true;
                    break;
            }
            curAtom = this.retrieveAtom(false);
        }
        this.retrieveAtom(true, AtomType.OPEN_CURLED_ENCLOSURE);
        Compound planetCompound = new Compound();
        curAtom = this.retrieveAtom(false);
        while (this.checkHorizon() && curAtom.Type != AtomType.CLOSE_CURLED_ENCLOSURE)
        {
            planetCompound.Molecules.Add(this.bondMolecule());
            this.retrieveAtom(true, AtomType.POLE);
            curAtom = this.retrieveAtom(false);
        }
        this.retrieveAtom(true, AtomType.CLOSE_CURLED_ENCLOSURE);

        return new LawSynthesis()
        {
            LawDicho = tempDichoOp,
            Validator = validation,
            Cycler = cyclic,
            Orbiter = orbital,
            LawCompound = planetCompound
        };
    }

    private Molecule bondPlanetSynthesis()
    {
        this.retrieveAtom(true);
        string tempSymbol = this.retrieveAtom(true, AtomType.ELEMENT).Value;
        List<string> tempPayload = this.bondVoyagePayload().ConvertAll(load =>
            load.Type == MoleculeType.ELEMENT
                ? (load as Element)!.Symbol!
                : throw new Exception("Expected Elements for Payload"));
        this.retrieveAtom(true, AtomType.OPEN_CURLED_ENCLOSURE);
        Compound planetCompound = new Compound();
        Atom curAtom = this.retrieveAtom(false);
        while (this.checkHorizon() && curAtom.Type != AtomType.CLOSE_CURLED_ENCLOSURE)
        {
            planetCompound.Molecules.Add(this.bondMolecule());
            this.retrieveAtom(true, AtomType.POLE);
            curAtom = this.retrieveAtom(false);
        }
        this.retrieveAtom(true, AtomType.CLOSE_CURLED_ENCLOSURE);
        return new PlanetSynthesis()
        {
            Symbol = tempSymbol,
            PayloadSymbols = tempPayload,
            Compound = planetCompound
        };
    }

    private Operation bondOperation()
    {
        return this.bondModificationOperation();
    }

    private Operation bondModificationOperation()
    {
        Operation preAssignmentOperation = this.bondAlloyOperation();

        if (this.retrieveAtom(false).Type != AtomType.EQUIVALENCE) return preAssignmentOperation;
        this.retrieveAtom(true);
        Operation postAssignmentOperation = this.bondModificationOperation();
        return new ElementModification()
        {
            Element = preAssignmentOperation,
            Magnitude = postAssignmentOperation
        };
    }

    private Operation bondAlloyOperation()
    {
        if (this.retrieveAtom(false).Type != AtomType.OPEN_CURLED_ENCLOSURE)
        {
            return this.bondMagnitudinalOperation();
            // return this.retrieveAtom(false).Type == AtomType.DICHO_ENCLOSURE ? this.bondDichotomicOperation() : this.bondMagnitudinalOperation();
        }
        this.retrieveAtom(true);
        List<Property> properties = new List<Property>();
        while (this.checkHorizon() && this.retrieveAtom(false).Type != AtomType.CLOSE_CURLED_ENCLOSURE)
        {
            string symbol = this.retrieveAtom(true, AtomType.ELEMENT).Value;

            switch (this.retrieveAtom(false).Type)
            {
                case AtomType.SEPARATOR:
                    this.retrieveAtom(true);
                    goto case AtomType.CLOSE_CURLED_ENCLOSURE;
                case AtomType.CLOSE_CURLED_ENCLOSURE:
                    properties.Add(new Property()
                    {
                        Symbol = symbol,
                    });
                    continue;
            }
            this.retrieveAtom(true, AtomType.CONDUIT);
            
            properties.Add(new Property()
            {
                Symbol = symbol,
                Magnitude = this.bondOperation()
            });

            if (this.retrieveAtom(false).Type != AtomType.CLOSE_CURLED_ENCLOSURE)
                this.retrieveAtom(true, AtomType.SEPARATOR);
        }
        
        this.retrieveAtom(true, AtomType.CLOSE_CURLED_ENCLOSURE);

        return new ExplicitAlloy()
        {
            Properties = properties
        };
    }

    private Operation bondMagnitudinalOperation()
    {
        return this.bondSigmaOperation();
    }
    
    private Operation bondSigmaOperation()
    {
        Operation prePiOperation = (this.retrieveAtom(false).Type != AtomType.SIGMA_OPERATOR)?this.bondPiOperation():new ExplicitMagnitude();

        while (this.retrieveAtom(false).Type == AtomType.SIGMA_OPERATOR)
        {
            string operation = this.retrieveAtom(true).Value;
            Operation postPiOperation = this.bondPiOperation();
            prePiOperation = new MagnitudinalOperation()
            {
                Pre = prePiOperation,
                Post = postPiOperation,
                MagnitudeOperator = operation
            };
        }
        
        return prePiOperation;
    }

    private Operation bondPiOperation()
    {
        Operation preGeneralOperation = this.findNextBondedOperation();

        while (this.retrieveAtom(false).Type == AtomType.PI_OPERATOR)
        {
            string operation = this.retrieveAtom(true).Value;
            Operation postGeneralOperation = this.findNextBondedOperation();
            preGeneralOperation = new MagnitudinalOperation()
            {
                Pre = preGeneralOperation,
                Post = postGeneralOperation,
                MagnitudeOperator = operation
            };
        }
        return preGeneralOperation;
    }

    private Operation findNextBondedOperation()
    {
        // Atom curAtom = this.retrieveAtom(false);
        return this.retrieveAtom(false).Type switch
        {
            AtomType.DICHO_ENCLOSURE => this.bondDichotomicOperation(),
            AtomType.OPEN_ANGLED_ENCLOSURE => this.bondTextOperation(),
            AtomType.TEXT => new ExplicitText(){Text = this.retrieveAtom(true).Value,},
            _ => this.bondVoyageTrajectoryOperation()
        };
    }

    private Operation bondTextOperation()
    {
        this.retrieveAtom(true);
        Operation tempTextOp = this.bondMergeTextOperation();
        this.retrieveAtom(true, AtomType.CLOSE_ANGLED_ENCLOSURE);
        return tempTextOp;
    }

    private Operation bondMergeTextOperation()
    {
        Operation preJunctionOperation = textOpOperation(bondMagnitudinalOperation());
        while (this.retrieveAtom(false).Type == AtomType.TEXT_MERGER)
        {
            string operation = this.retrieveAtom(true).Value;
            Operation postJunctionOperation = textOpOperation(bondMagnitudinalOperation());
            preJunctionOperation = new TextOperation()
            {
                Pre = preJunctionOperation,
                Post = postJunctionOperation,
                TextOperator = operation,
            };
        }

        return preJunctionOperation;
    }
    
    private static Operation textOpOperation(Operation generalOperation)
    {
        if (generalOperation.Type == MoleculeType.EXPLICIT_MAGNITUDE)
        {
            ExplicitMagnitude tempMag = (generalOperation as ExplicitMagnitude)!;
            tempMag.TextOP = true;
            return tempMag;
            // return new ExplicitText()
            // {
            //     Text = (generalOperation as ExplicitMagnitude)!.Magnitude.ToString(CultureInfo.CurrentCulture),
            // };
        }
        if (generalOperation.Type == MoleculeType.MAGNITUDINAL_OPERATION)
        {
            MagnitudinalOperation tempMag = (generalOperation as MagnitudinalOperation)!;
            tempMag.TextOP = true;
            return tempMag;
        }
        if (generalOperation.Type == MoleculeType.ELEMENT)
        {
            Element tempElement = (generalOperation as Element)!;
            tempElement.TextOP = true;
            return tempElement;
        }
        if (generalOperation.Type == MoleculeType.EXPLICIT_DICHO)
        {
            ExplicitDicho tempDicho = (generalOperation as ExplicitDicho)!;
            tempDicho.TextOP = true;
            return tempDicho;
            // return new ExplicitText()
            // {
            //     Text = (generalOperation as ExplicitDicho)!.State.ToString(CultureInfo.CurrentCulture)
            // };
        }
        if (generalOperation.Type == MoleculeType.DICHOTOMIC_OPERATION)
        {
            DichotomicOperation tempDicho = (generalOperation as DichotomicOperation)!;
            tempDicho.TextOP = true;
            return tempDicho;
        }

        return generalOperation;
    }
    
    private Operation bondDichotomicOperation()
    {
        this.retrieveAtom(true);
        Operation tempDichoOp = this.bondDisjunctionOperation();
        this.retrieveAtom(true, AtomType.DICHO_ENCLOSURE);
        return tempDichoOp;
    }

    private Operation bondDisjunctionOperation(bool negateState = false)
    {
        Operation preJunctionOperation = this.bondJunctionOperation(negateState);
        if (preJunctionOperation.Type == MoleculeType.DICHOTOMIC_OPERATION) negateState = false;
        while (this.retrieveAtom(false).Type == AtomType.DISJUNCTOR)
        {
            string operation = this.retrieveAtom(true).Value;
            Operation postJunctionOperation = this.bondJunctionOperation();
            preJunctionOperation = new DichotomicOperation()
            {
                Pre = preJunctionOperation,
                Post = postJunctionOperation,
                DichoOperator = operation,
                Negated = negateState
            };
        }

        return preJunctionOperation;
    }

    private Operation bondJunctionOperation(bool negateState = false)
    {
        // if (this.retrieveAtom(false).Type == AtomType.OPEN_ROUND_ENCLOSURE)
        //     return this.bondGeneralDichotomicOperation();
        Operation preCompOperation = this.bondComparisonOperation(negateState);
        while (this.retrieveAtom(false).Type == AtomType.CONJUNCTOR)
        {
            string operation = this.retrieveAtom(true).Value;
            Operation postCompOperation = this.bondComparisonOperation();
            preCompOperation = new DichotomicOperation()
            {
                Pre = preCompOperation,
                Post = postCompOperation,
                DichoOperator = operation,
                Negated = negateState
            };
            negateState = false;
        }
        return preCompOperation;
    }

    private Operation bondComparisonOperation(bool negateState = false)
    {
        Operation preNegateOperation = this.dichotomizeMagnitude(this.bondMagnitudinalOperation());
        while (this.retrieveAtom(false).Type == AtomType.COMP_OPERATOR)
        {
            string operation = this.retrieveAtom(true).Value;
            Operation postNegateOperation = this.dichotomizeMagnitude(this.bondMagnitudinalOperation());
            preNegateOperation = new DichotomicOperation()
            {
                Pre = preNegateOperation,
                Post = postNegateOperation,
                DichoOperator = operation,
                Negated = negateState
            };
            negateState = false;
        }
        return preNegateOperation;
    }

    private Operation dichotomizeMagnitude(Operation generalOperation)
    {
        if (generalOperation.Type == MoleculeType.EXPLICIT_MAGNITUDE)
        {
            ExplicitMagnitude tempMag = (generalOperation as ExplicitMagnitude)!;
            tempMag.Dichotomous = true;
            return tempMag;
            // return new ExplicitDicho()
            // {
            //     State = (generalOperation as ExplicitMagnitude)!.Magnitude != 0,
            // };
        }
        if (generalOperation.Type == MoleculeType.MAGNITUDINAL_OPERATION)
        {
            MagnitudinalOperation tempMag = (generalOperation as MagnitudinalOperation)!;
            tempMag.Dichotomous = true;
            return tempMag;
        }
        if (generalOperation.Type == MoleculeType.EXPLICIT_TEXT)
        {
            ExplicitText tempText = (generalOperation as ExplicitText)!;
            tempText.Dichotomous = true;
            return tempText;
            // return new ExplicitDicho()
            // {
            //     State = (generalOperation as ExplicitText)!.Text!.Length != 0,
            // };
        }
        if (generalOperation.Type == MoleculeType.TEXT_OPERATION)
        {
            TextOperation tempText = (generalOperation as TextOperation)!;
            tempText.Dichotomous = true;
            return tempText;
        }
        if (generalOperation.Type == MoleculeType.ELEMENT)
        {
            Element tempElement = (generalOperation as Element)!;
            tempElement.Dichotomous = true;
            return tempElement;
        }

        return generalOperation;
    }
    
    private Operation bondGeneralDichotomicOperation()
    {
        AtomType atomT = this.retrieveAtom(false).Type;
        bool negated = false;
        if (atomT == AtomType.NEGATER)
        {
            negated = true;
            this.retrieveAtom(true);
            atomT = this.retrieveAtom(false).Type;
        }

        switch (atomT)
        {
            case AtomType.ELEMENT:
                return new Element() { Symbol = this.retrieveAtom(true).Value, DichoNegated = negated};
            case AtomType.MAGNITUDE:
                return new ExplicitDicho()
                {
                    State = negated?double.Parse(this.retrieveAtom(true).Value)==0:double.Parse(this.retrieveAtom(true).Value)!=0,
                };
            // case AtomType.VACUUM:
            //     this.getAtom(true);
            //     return new ExplicitVacuum();
            case AtomType.OPEN_ROUND_ENCLOSURE:   
                this.retrieveAtom(true);
                Operation operation = this.bondDisjunctionOperation(negated);
                this.retrieveAtom(true, AtomType.CLOSE_ROUND_ENCLOSURE);
                return operation;
            default:
                throw new MalformedLineException($"Unknown Atom type: {atomT}");
        }
    }

    private Operation bondVoyageTrajectoryOperation()
    {
        Operation trajectory = this.bondTrajectoryOperation();

        return this.retrieveAtom(false).Type == AtomType.OPEN_ROUND_ENCLOSURE ? this.bondVoyageOperation(trajectory) : trajectory;
    }

    private Operation bondVoyageOperation(Operation trajectory)
    {
        Operation tempVoyage = new VoyageOperation()
        {
            Planet = trajectory,
            Payload = this.bondVoyagePayload()
        };
        
        while (this.retrieveAtom(false).Type == AtomType.OPEN_ROUND_ENCLOSURE)
            tempVoyage = new VoyageOperation()
            {
                Planet = tempVoyage,
                Payload = this.bondVoyagePayload()
            };

        return tempVoyage;
    }

    private Operation bondTrajectoryOperation()
    {
        Operation alloy = this.bondGeneralMagnitudinalOperation();
        while (this.retrieveAtom(false).Type == AtomType.LINKER) // or this.retrieveAtom(false).Type == AtomType.OPEN_SQUARE_ENCLOSURE
        {
            // Atom curAtom =
            this.retrieveAtom(true);
            
            Operation alloyProperty = this.bondGeneralMagnitudinalOperation();

            if (alloyProperty.Type != MoleculeType.ELEMENT)
                throw new MalformedLineException("Expected Element");
            alloy = new AlloyTrajectoryOperation()
            {
                Alloy = alloy,
                Property = alloyProperty,
            };
        }
        return alloy;
    }

    private List<Operation> bondVoyagePayload()
    {
        this.retrieveAtom(true, AtomType.OPEN_ROUND_ENCLOSURE);
        List<Operation> payloads = new List<Operation>();
        if (this.retrieveAtom(false).Type != AtomType.CLOSE_ROUND_ENCLOSURE)
        {
            bool skipSep = true;
            do
            {
                if (!skipSep) this.retrieveAtom(true);
                payloads.Add(this.bondModificationOperation());
                skipSep = false;
            } while (this.retrieveAtom(false).Type == AtomType.SEPARATOR);
        }
        this.retrieveAtom(true, AtomType.CLOSE_ROUND_ENCLOSURE);
        return payloads;
    }
    
    private Operation bondGeneralMagnitudinalOperation()
    {
        AtomType atomT = this.retrieveAtom(false).Type;

        switch (atomT)
        {
            case AtomType.NEGATER:
                return this.bondGeneralDichotomicOperation();
            case AtomType.ELEMENT:
                return new Element() { Symbol = this.retrieveAtom(true).Value };
            case AtomType.MAGNITUDE:
                return new ExplicitMagnitude()
                {
                    Magnitude = double.Parse(this.retrieveAtom(true).Value)
                };
            case AtomType.OPEN_ROUND_ENCLOSURE:   
                this.retrieveAtom(true);
                Operation operation = this.bondOperation();
                this.retrieveAtom(true, AtomType.CLOSE_ROUND_ENCLOSURE);
                return operation;
            default:
                throw new MalformedLineException($"Unknown Atom type: {atomT}");
        }
    }
}
