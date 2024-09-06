using Microsoft.VisualBasic.FileIO;

namespace ProjectPlanetary;

public class Bonder
{
    private List<Atom> Atoms = new List<Atom>();

    public Compound createCompound(string system)
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

    private Atom getAtom(bool decay, AtomType? expectedAtom=null)
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
        return this.bondOperation();
    }

    private Operation bondOperation()
    {
        return this.bondSigmaOperation();
    }
    
    private Operation bondSigmaOperation()
    {
        Operation prePiOperation = this.bondPiOperation();

        while (this.getAtom(false).Type == AtomType.SIGMA_OPERATION)
        {
            string operation = this.getAtom(true).Value;
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

        while (this.getAtom(false).Type == AtomType.PI_OPERATION)
        {
            string operation = this.getAtom(true).Value;
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
        AtomType atomT = this.getAtom(false).Type;

        switch (atomT)
        {
            case AtomType.ELEMENT:
                return new Element() { Symbol = this.getAtom(true).Value };
            case AtomType.MAGNITUDE:
                return new ExplicitMagnitude()
                {
                    Magnitude = double.Parse(this.getAtom(true).Value)
                };
            case AtomType.VACUUM:
                this.getAtom(true);
                return new ExplicitVacuum();
            case AtomType.OPEN_ENCLOSURE:
                this.getAtom(true);
                Operation operation = this.bondOperation();
                this.getAtom(true, AtomType.CLOSE_ENCLOSURE);
                return operation;
            default:
                throw new MalformedLineException($"Unknown Atom type: {atomT}");
        }
    }
}
