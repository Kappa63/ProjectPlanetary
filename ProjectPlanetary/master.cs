namespace ProjectPlanetary;

internal static class ProjectPlanetary
{
    public static void Main()
    {
        var a = Reactor.Fission("manifest x = 90 -(8 * 6)");

        foreach (var at in a)
            Console.WriteLine(at.Type);
    } 
}