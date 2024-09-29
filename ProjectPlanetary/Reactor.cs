﻿using System.Text.RegularExpressions;
namespace ProjectPlanetary;

public enum AtomType {
    // VACUUM,
    MAGNITUDE,
    ELEMENT,
    EQUIVALENCE,
    EXCEEDS,
    EQUATES,
    UNEQUATES,
    RECEDES, 
    NEGATER,
    CONJUNCTOR,
    DISJUNCTOR,
    TEXT_MERGER,
    
    OPEN_ROUND_ENCLOSURE,
    CLOSE_ROUND_ENCLOSURE,
    OPEN_SQUARE_ENCLOSURE,
    CLOSE_SQUARE_ENCLOSURE,
    OPEN_CURLED_ENCLOSURE,
    CLOSE_CURLED_ENCLOSURE,
    DICHO_ENCLOSURE,
    OPEN_ANGLED_ENCLOSURE,
    CLOSE_ANGLED_ENCLOSURE,
    
    TEXT,
    
    // MAGNITUDE_OPERATIONS,
    SIGMA_OPERATOR,
    PI_OPERATOR,
    
    CONDUIT,
    LINKER,
    SEPARATOR,
    POLE,
    
    ELEMENT_SYNTHESIZER,
    ELEMENT_STABILIZER,
    PLANET_SYNTHESIZER,
    LAW_SYNTHESIZER,
    LAW_VALIDATOR,
    LAW_INVALIDATOR,
    LAW_CYCLER,
    LAW_ORBITER,
    
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
        { "synth", AtomType.ELEMENT_SYNTHESIZER },
        { "stable", AtomType.ELEMENT_STABILIZER },
        { "planet", AtomType.PLANET_SYNTHESIZER},
        { "or", AtomType.DISJUNCTOR },
        { "and", AtomType.CONJUNCTOR },
        { "law", AtomType.LAW_SYNTHESIZER },
        { "valid", AtomType.LAW_VALIDATOR },
        { "invalid", AtomType.LAW_INVALIDATOR },
        { "cycle", AtomType.LAW_CYCLER },
        { "orbit", AtomType.LAW_ORBITER},
        // { "vacuum", AtomType.VACUUM }
    };

    private const string Isotope = "(?<TEXT>\\\"([^\\\"]*)\\\")|" // "(?<TEXT>(?<=\\\").*(?=\\\"))|"
                                   + @"(?<MAGNITUDE>\d+)|"
                                   + @"(?<TEXT_MERGER>\.\.)|"
                                   + @"(?<ELEMENT>[a-zA-Z]\w*)|"
                                   + @"(?<SIGMA_OPERATOR>[+\-])|"
                                   + @"(?<PI_OPERATOR>[\*/%])|"
                                   + @"(?<UNEQUATES>\!\=)|"
                                   + @"(?<EQUATES>\=\=)|"
                                   + @"(?<EQUIVALENCE>\=)|"
                                   + @"(?<EXCEEDS>\>\>)|"
                                   + @"(?<RECEDES>\<\<)|"
                                   + @"(?<NEGATER>\!)|"
                                   + @"(?<DICHO_ENCLOSURE>\|)|"
                                   + @"(?<OPEN_ROUND_ENCLOSURE>\()|"
                                   + @"(?<CLOSE_ROUND_ENCLOSURE>\))|"
                                   + @"(?<OPEN_SQUARE_ENCLOSURE>\[)|"
                                   + @"(?<CLOSE_SQUARE_ENCLOSURE>\])|"
                                   + @"(?<OPEN_CURLED_ENCLOSURE>\{)|"
                                   + @"(?<CLOSE_CURLED_ENCLOSURE>\})|"
                                   + @"(?<OPEN_ANGLED_ENCLOSURE>\<)|"
                                   + @"(?<CLOSE_ANGLED_ENCLOSURE>\>)|"
                                   + @"(?<POLE>\;)|"
                                   + @"(?<SEPARATOR>\,)|"
                                   + @"(?<CONDUIT>\:)|"
                                   + @"(?<LINKER>\.)|"
                                   + @"\s+|"
                                   + "(?<DARK_MATTER>.)";


    public static List<Atom> Fission(string system)
    {
        List<Atom> atoms = new List<Atom>();
        MatchCollection matches = Regex.Matches(Regex.Replace(system, "(?<NOTE>//.*\n)|", ""), Isotope);
        int compoundNumber = 1;
        int atomNumber = 0;
        foreach (Match match in matches)
        {
            atomNumber++;
            foreach (AtomType atomType in Enum.GetValues(typeof(AtomType)))
            {
                if (!match.Groups[atomType.ToString()].Success) continue;
                if (atomType == AtomType.DARK_MATTER)
                    throw new FormatException($"Dark Matter Detected at Compound {compoundNumber} Atom {atomNumber}. Unknown Atom Type.");
                if (atomType == AtomType.POLE)
                {
                    atomNumber = 0;
                    compoundNumber++;
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