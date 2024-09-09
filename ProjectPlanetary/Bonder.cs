// using System.Text.Json;
using Microsoft.VisualBasic.FileIO;

namespace ProjectPlanetary;

public class Bonder
{
    private List<Atom> Atoms = new List<Atom>();

    public Compound bondCompound(string system)
    {
        this.Atoms = Reactor.Fission(system);
        //
        // foreach (var atom in this.Atoms)
        // {
        //     Console.WriteLine(atom.Type);
        // }

        Compound compound = new Compound()
        {
            Molecules = new List<Molecule>()
        };

        while (this.checkHorizon())
        {
            compound.Molecules.Add(this.bondMolecule());
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
            _ => this.bondOperation()
        };
    }

    private Molecule bondElementSynthesis()
    {
        bool elementalStability = this.retrieveAtom(true).Type == AtomType.ELEMENT_STABILIZER;
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
        Operation preGeneralOperation = this.bondDichotomicOperation();

        while (this.retrieveAtom(false).Type == AtomType.PI_OPERATOR)
        {
            string operation = this.retrieveAtom(true).Value;
            Operation postGeneralOperation = this.bondDichotomicOperation();
            preGeneralOperation = new MagnitudinalOperation()
            {
                Pre = preGeneralOperation,
                Post = postGeneralOperation,
                MagnitudeOperator = operation
            };
        }
        return preGeneralOperation;
    }
    
    private Operation bondDichotomicOperation()
    {   
        if (this.retrieveAtom(false).Type != AtomType.DICHO_ENCLOSURE)
            return this.bondGeneralMagnitudinalOperation();
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

        // DichotomicOperation temp = (preJunctionOperation as DichotomicOperation)!;
        // temp.Negated = negateState;
        // Console.WriteLine(JsonSerializer.Serialize(temp));
        return preJunctionOperation;
    }

    private Operation bondJunctionOperation(bool negateState = false)
    {
        Operation preNegateOperation = this.dichotomizeMagnitude((this.bondMagnitudinalOperation() as MagnitudinalOperation)!);
        while (this.retrieveAtom(false).Type == AtomType.CONJUNCTOR)
        {
            string operation = this.retrieveAtom(true).Value;
            Operation postGeneralOperation = this.dichotomizeMagnitude((this.bondMagnitudinalOperation() as MagnitudinalOperation)!);
            preNegateOperation = new DichotomicOperation()
            {
                Pre = preNegateOperation,
                Post = postGeneralOperation,
                DichoOperator = operation,
                Negated = negateState
            };
            negateState = false;
        }
        return preNegateOperation;
    }

    private Operation dichotomizeMagnitude(MagnitudinalOperation magnitudeOperation)
    {
        magnitudeOperation.Dichotomous = true;
        return magnitudeOperation;
    }
    
    // private Operation bondNegateOperation()
    // {
    //     if (this.retrieveAtom(false).Type != AtomType.NEGATER)
    //         return this.bondDisjunctionOperation(false);
    //     this.retrieveAtom(true);
    //     return this.bondDisjunctionOperation(true);
    // }
    
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

        if (this.retrieveAtom(false).Type == AtomType.OPEN_ROUND_ENCLOSURE)
        {
            return this.bondVoyageOperation(trajectory);
        }

        return trajectory;
    }

    private Operation bondVoyageOperation(Operation trajectory)
    {
        Operation tempVoyage = new VoyageOperation()
        {
            Origin = trajectory,
            Payload = this.bondVoyagePayload()
        };
        if (this.retrieveAtom(false).Type == AtomType.OPEN_ROUND_ENCLOSURE)
            tempVoyage = this.bondVoyageOperation(tempVoyage);

        // do
        // {
        //     
        // } while (expression);
        return tempVoyage;
    }

    private Operation bondTrajectoryOperation()
    {   
        return new AlloyTrajectoryOperation();
    }

    private List<Operation> bondVoyagePayload()
    {
        List<Operation> payloads = new List<Operation>();
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
            // case AtomType.VACUUM:
            //     this.getAtom(true);
            //     return new ExplicitVacuum();
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
