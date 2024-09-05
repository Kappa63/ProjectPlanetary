using System.Text.RegularExpressions;
namespace ProjectPlanetary;

public enum AtomType {
    MAGNITUDE,
    ELEMENT,
    EQUIVALENCE,
    
    OPEN_ENCLOSURE,
    CLOSE_ENCLOSURE,
    
    MAGNITUDE_OPERATIONS,
    SIGMA_OPERATION,
    PI_OPERATION,
    
    POLE,
    
    MANIFEST_ELEMENT,
    
    DARK_MATTER,
    HORIZON
}

public class Atom
{
    public string Value { get; set; }
    public AtomType Type { get; set; }

    public Atom(string value, AtomType type)
    {
        Value = value;
        Type = type;
    }
}

public static class Reactor 
{
    private static readonly Dictionary<string, AtomType> ObservedAtomType = new()
    {
        { "manifest", AtomType.MANIFEST_ELEMENT }
    };

    private const string Isotope = @"(?<MAGNITUDE>\d+)|"
                                   + @"(?<ELEMENT>[a-zA-Z]\w*)|"
                                   + @"(?<SIGMA_OPERATION>[+\-])|"
                                   + @"(?<PI_OPERATION>[\*/%])|"
                                   // + @"(?<MAGNITUDE_OPERATIONS>[+\-*/])|"
                                   + "(?<EQUIVALENCE>=)|"
                                   + @"(?<OPEN_ENCLOSURE>\()|"
                                   + @"(?<CLOSE_ENCLOSURE>\))|"
                                   + "(?<POLE>;)|"
                                   + @"\s+|"
                                   + "(?<DARK_MATTER>.)";


    public static List<Atom> Fission(string system)
    {
        List<Atom> atoms = new List<Atom>();
        MatchCollection matches = Regex.Matches(system, Isotope);
        int moleculeNumber = 1;
        int atomNumber = 0;
        foreach (Match match in matches)
        {
            atomNumber++;
            foreach (AtomType atomType in Enum.GetValues(typeof(AtomType)))
            {
                if (!match.Groups[atomType.ToString()].Success) continue;
                if (atomType == AtomType.DARK_MATTER)
                    throw new FormatException($"Dark Matter Detected at Molecule {moleculeNumber} Atom {atomNumber}. Unknown Atom Type.");
                if (atomType == AtomType.POLE)
                {
                    atomNumber = 0;
                    moleculeNumber++;
                }
                var value = match.Value;
                atoms.Add(ObservedAtomType.TryGetValue(value, out var subAtomType)
                    ? new Atom(value, subAtomType)
                    : new Atom(value, atomType));
                break;
            }
        }
        
        atoms.Add(new Atom("EndOfUniverse", AtomType.HORIZON));
        return atoms;
    }
    
}