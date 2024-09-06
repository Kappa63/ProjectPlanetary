namespace ProjectPlanetary;

public class Space
{
    private Space? rootSpace;
    private Dictionary<string, ExplicitFormation> elements;

    public Space(Space? rootSpace=null)
    {
        this.rootSpace = rootSpace;
        this.elements = new Dictionary<string, ExplicitFormation>();
    }

    public ExplicitFormation synthesizeElement(string elementName, ExplicitFormation exForm)
    {
        if (elements.TryAdd(elementName, exForm))
            return exForm;
        throw new InvalidOperationException($"Element {elementName} is already synthesized. Cannot synthesize an already synthesized element.");
    }

    public ExplicitFormation modifyElement(string elementName, ExplicitFormation exForm)
    {
        locateElementalSpace(elementName).elements[elementName] = exForm;
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