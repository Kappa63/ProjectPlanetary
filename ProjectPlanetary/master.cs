using System.Text.Json;

namespace ProjectPlanetary;

internal static class ProjectPlanetary
{
    public static void Main()
    {
        // List<Atom> a = Reactor.Fission("manifest x = 90 -(8 * (3+2)/6);");
        //
        // Console.WriteLine("manifest x = 90 -(8 * (3+2)/6);");
        // foreach (var at in a)
        //     Console.WriteLine(at.Type);
        
        Bonder bonder = new Bonder();
        Former former = new Former();
        
        Console.WriteLine("\nPlanetary v1.0");

        while (true)
        {
            Console.Write(">> ");
            string? system = Console.ReadLine();
            if (system == null) continue;
            if (system.Contains("exit")) Environment.Exit(0);
            Compound comp = bonder.createCompound(system);
            ExplicitFormation frm = former.formCompound(comp);
            Console.WriteLine(frm.Type == ExplicitType.MAGNITUDE?JsonSerializer.Serialize(frm as ExplicitFormedMagnitude):"VACUUM");
        }
    }
}