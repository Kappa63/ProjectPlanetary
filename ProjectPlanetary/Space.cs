namespace ProjectPlanetary;

public class Space
{
    private readonly Space? rootSpace;
    private readonly Dictionary<string, ExplicitFormation> elements;
    private readonly List<string> stabilized;

    public Space(Space? rootSpace=null)
    {
        this.rootSpace = rootSpace;
        this.elements = new Dictionary<string, ExplicitFormation>();
        this.stabilized = new List<string>();
    }

    public ExplicitFormation synthesizeElement(string elementName, ExplicitFormation exForm, bool stability)
    {
        if (!elements.TryAdd(elementName, exForm))
            throw new InvalidOperationException($"Element {elementName} is already synthesized. Cannot synthesize an already synthesized element.");
        if (stability) 
            stabilized.Add(elementName);
        return exForm;
    }

    public ExplicitFormation modifyElement(string elementName, ExplicitFormation exForm)
    {
        Space elementSpace = locateElementalSpace(elementName);
        if (elementSpace.stabilized.Contains(elementName))
            throw new InvalidOperationException("Cannot modify a stabilized element.");
        elementSpace.elements[elementName] = exForm;
        return exForm;
    }

    public ExplicitFormation retrieveElement(string elementName)
    {
        return locateElementalSpace(elementName).elements[elementName];
    }

    private Space locateElementalSpace(string elementName)
    {
        if (this.elements.ContainsKey(elementName))
            return this;
        if(this.rootSpace == null)
            throw new InvalidOperationException($"Element {elementName} has not been synthesized. Cannot retrieve non-synthesized element.");
        return this.rootSpace.locateElementalSpace(elementName);
    }
}