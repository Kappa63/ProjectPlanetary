using ProjectPlanetary.Singularity;

namespace ProjectPlanetary;
static class ProjectPlanetary
{
    public static readonly Bonding.Bonder bonder = new Bonding.Bonder();
    public static readonly Space space = new Space();
    public static readonly Forming.Former former = new Forming.Former();
    
    public static void Main()
    {
        Console.WriteLine("\nPlanetary v0.11");
        
        // REPL
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
        
        // string[] testFiles = Directory.GetFiles("../../../PlanetaryTests/ExplicitTests/", "*.ps");
        // foreach (string testFile in testFiles)
        // {
        //     Console.WriteLine(testFile);
        //     string system = File.ReadAllText(testFile);
        //     former.formCompound(bonder.bondCompounds(system), new Space());
        //     Console.WriteLine("===========================");
        // }
        
        string system = File.ReadAllText("../../../PlanetaryTests/src.ps");
        former.formCompound(bonder.bondCompounds(system), space);
    }
}