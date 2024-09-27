using ProjectPlanetary.Singularity;

namespace ProjectPlanetary;
internal static class ProjectPlanetary
{
    public static void Main()
    {
        Bonding.Bonder bonder = new Bonding.Bonder();
        Bonding.Bonder3 bonder3 = new Bonding.Bonder3();
        Space space = new Space();
        Forming.Former former = new Forming.Former();
        Console.WriteLine("\nPlanetary v0.1");
        
        // var Atoms = Reactor.Fission("synth x = |(True..0)++!True|"); // law ( a >> 20) valid cycle {x= {kol, <a:s>}}
        //
        // foreach (var atom in Atoms)
        // {
        //     Console.WriteLine(atom.Type);
        // }   
        
        // while (true)
        // {
        //     Console.Write(">> ");
        //     string? system = Console.ReadLine();
        //     if (system == null) continue;
        //     if (system.Contains("exit")) Environment.Exit(0);
        //     Compound comp = bonder.bondCompound(system);
        //     ExplicitFormation frm = former.formCompound(comp, space);
        //     Console.WriteLine(frm.Type switch
        //     {
        //         ExplicitType.MAGNITUDE=>JsonSerializer.Serialize(frm as ExplicitFormedMagnitude),
        //         ExplicitType.DICHO=>JsonSerializer.Serialize(frm as ExplicitFormedDicho),
        //         ExplicitType.ALLOY=>JsonSerializer.Serialize(frm as ExplicitFormedAlloy),
        //         _=>JsonSerializer.Serialize(frm as ExplicitFormedVacuum)
        //     });
        // }

        string system = File.ReadAllText("/home/karim/Desktop/Prog/RiderProjects/ProjectPlanetary/ProjectPlanetary/src.ps");
        former.formCompound(bonder3.bondCompounds(system), space);
    }
}