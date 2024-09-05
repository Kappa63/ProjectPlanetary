using Microsoft.VisualBasic.FileIO;

namespace ProjectPlanetary;

public class Bonder
{
    private List<Atom> Atoms = new List<Atom>();

    public Universe createUniverse(string system)
    {
        this.Atoms = Reactor.Fission(system);
        Universe universe = new Universe()
        {
            Molecules = new List<Molecule>()
        };

        while (this.checkHorizon())
        {
            universe.Molecules.Add(this.bondMolecule());
        }

        return universe;
    }
    
    private bool checkHorizon()
    {
        return this.Atoms.First().Type != AtomType.HORIZON;
    }

    private Atom getAtom()
    {
        return this.Atoms.First();
    }

    private Atom decayAtom(AtomType? expectedAtom=null)
    {
        Atom decayedAtom = this.Atoms.First();
        this.Atoms.RemoveAt(0);
        if (expectedAtom.HasValue && expectedAtom != decayedAtom.Type)
            throw new MalformedLineException($"Expected {expectedAtom} but got {decayedAtom.Type}");
        return decayedAtom;
    }

    private Molecule bondMolecule()
    {
        return this.bondOperation();
    }

    private Operation bondOperation()
    {
        return this.bondSigmaOperation();
    }
    
    private Operation bondSigmaOperation()
    {
        Operation prePiOperation = this.bondPiOperation();

        while (this.getAtom().Type == AtomType.SIGMA_OPERATION)
        {
            string operation = this.decayAtom().Value;
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

        while (this.getAtom().Type == AtomType.PI_OPERATION)
        {
            string operation = this.decayAtom().Value;
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
        AtomType atomT = this.getAtom().Type;

        switch (atomT)
        {
            case AtomType.ELEMENT:
                return new Element() { Symbol = this.decayAtom().Value };
            case AtomType.MAGNITUDE:
                return new ExplicitMagnitude()
                {
                    Magnitude = double.Parse(this.decayAtom().Value)
                };
            case AtomType.OPEN_ENCLOSURE:
                this.decayAtom();
                Operation operation = this.bondOperation();
                this.decayAtom(AtomType.CLOSE_ENCLOSURE);
                return operation;
            default:
                throw new MalformedLineException($"Unknown Atom type: {atomT}");
        }
    }
}
