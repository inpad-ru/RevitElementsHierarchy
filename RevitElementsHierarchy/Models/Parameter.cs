namespace InpadPlugins.RevitElementsHierarchy.Models
{
    public class Parameter
    {
        public string Name { get; set; }

        public Autodesk.Revit.DB.Parameter RevParameter { get; set; }
        public string Value { get; set; }

        public string OldValue { get; set; }

    }
}
