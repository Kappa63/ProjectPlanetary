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
        Space space = new Space();
        space.synthesizeElement("True", new ExplicitFormedDicho() { state = true }, true);
        space.synthesizeElement("False", new ExplicitFormedDicho() { state = false }, true);
        space.synthesizeElement("vacuum", new ExplicitFormedVacuum(), true);
        Former former = new Former();
        
        Console.WriteLine("\nPlanetary v0.03");

        while (true)
        {
            Console.Write(">> ");
            string? system = Console.ReadLine();
            if (system == null) continue;
            if (system.Contains("exit")) Environment.Exit(0);
            Compound comp = bonder.bondCompound(system);
            ExplicitFormation frm = former.formCompound(comp, space);
            Console.WriteLine(frm.Type == ExplicitType.MAGNITUDE?JsonSerializer.Serialize(frm as ExplicitFormedMagnitude):JsonSerializer.Serialize(frm as ExplicitFormedVacuum));
        }
    }
}