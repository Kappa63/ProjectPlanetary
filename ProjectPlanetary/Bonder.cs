using Microsoft.VisualBasic.FileIO;

namespace ProjectPlanetary;

public class Bonder
{
    private List<Atom> Atoms = new List<Atom>();

    public Compound bondCompound(string system)
    {
        this.Atoms = Reactor.Fission(system);

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
        switch (this.retrieveAtom(false).Type)
        {
            case AtomType.ELEMENT_SYNTHESIZER:
            case AtomType.ELEMENT_STABILIZER:
                return this.bondElementSynthesis();
            default:
                return this.bondOperation();
        }
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
        Operation preAssignmentOperation = this.bondSigmaOperation();

        if (this.retrieveAtom(false).Type != AtomType.EQUIVALENCE) return preAssignmentOperation;
        this.retrieveAtom(true);
        Operation postAssignmentOperation = this.bondModificationOperation();
        return new ElementModification()
        {
            Element = preAssignmentOperation,
            Magnitude = postAssignmentOperation
        };
    }
    
    private Operation bondSigmaOperation()
    {
        Operation prePiOperation = this.bondPiOperation();

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
        Operation preGeneralOperation = this.bondGeneralOperation();

        while (this.retrieveAtom(false).Type == AtomType.PI_OPERATOR)
        {
            string operation = this.retrieveAtom(true).Value;
            Operation postGeneralOperation = this.bondGeneralOperation();
            preGeneralOperation = new MagnitudinalOperation()
            {
                Pre = preGeneralOperation,
                Post = postGeneralOperation,
                MagnitudeOperator = operation
            };
        }
        return preGeneralOperation;
    }

    private Operation bondGeneralOperation()
    {
        AtomType atomT = this.retrieveAtom(false).Type;

        switch (atomT)
        {
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
            case AtomType.OPEN_ENCLOSURE:   
                this.retrieveAtom(true);
                Operation operation = this.bondOperation();
                this.retrieveAtom(true, AtomType.CLOSE_ENCLOSURE);
                return operation;
            default:
                throw new MalformedLineException($"Unknown Atom type: {atomT}");
        }
    }
}
