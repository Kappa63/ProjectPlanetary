using System.Text.Json;

namespace ProjectPlanetary;

internal static class ProjectPlanetary
{
    public static void Main()
    {
        Bonder bonder = new Bonder();
        Space space = new Space();
        Former former = new Former();
        Console.WriteLine("\nPlanetary v0.05");
        
        // var Atoms = Reactor.Fission("synth x = |(True..0)++!True|"); // law ( a >> 20) valid cycle {x= {kol, <a:s>}}
        //
        // foreach (var atom in Atoms)
        // {
        //     Console.WriteLine(atom.Type);
        // }   
        
        while (true)
        {
            Console.Write(">> ");
            string? system = Console.ReadLine();
            if (system == null) continue;
            if (system.Contains("exit")) Environment.Exit(0);
            Compound comp = bonder.bondCompound(system);
            ExplicitFormation frm = former.formCompound(comp, space);
            Console.WriteLine(frm.Type switch
            {
                ExplicitType.MAGNITUDE=>JsonSerializer.Serialize(frm as ExplicitFormedMagnitude),
                ExplicitType.DICHO=>JsonSerializer.Serialize(frm as ExplicitFormedDicho),
                ExplicitType.ALLOY=>JsonSerializer.Serialize(frm as ExplicitFormedAlloy),
                _=>JsonSerializer.Serialize(frm as ExplicitFormedVacuum)
            });
        }
    }
}