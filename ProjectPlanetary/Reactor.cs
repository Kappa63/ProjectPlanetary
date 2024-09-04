using System.Text.RegularExpressions;
namespace ProjectPlanetary;

public enum AtomType {
    MAGNITUDE,
    ELEMENT,
    EQUIVALENCE,
    
    OPEN_ENCLOSURE,
    CLOSE_ENCLOSURE,
    
    MAGNITUDE_OPERATIONS,
    
    MANIFEST_ELEMENT,
    
    END
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

    private const string Neutron = @"(?<MAGNITUDE>\d+)|"
                                   + @"(?<ELEMENT>[a-zA-Z]\w*)|"
                                   + @"(?<MAGNITUDE_OPERATIONS>[+\-*/])|"
                                   + @"(?<EQUIVALENCE>=)|"
                                   + @"(?<OPEN_ENCLOSURE>\()|"
                                   + @"(?<CLOSE_ENCLOSURE>\))|"
                                   + @"\s+";

    public static List<Atom> Fission(string sourceAtom)
    {
        var atoms = new List<Atom>();
        var matches = Regex.Matches(sourceAtom, Neutron);

        foreach (Match match in matches)
        {
            foreach (AtomType atomType in Enum.GetValues(typeof(AtomType)))
            {
                if (!match.Groups[atomType.ToString()].Success) continue;
                var value = match.Value;
                atoms.Add(ObservedAtomType.TryGetValue(value, out var subAtomType)
                    ? new Atom(value, subAtomType)
                    : new Atom(value, atomType));
                break;
            }
        }
        
        atoms.Add(new Atom("EndOfUniverse", AtomType.END));
        return atoms;
    }
    
}