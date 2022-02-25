using System.Collections.ObjectModel;
using Autodesk.Revit.DB;

namespace InpadPlugins.RevitElementsHierarchy.Models
{
    public class Inst
    {
        public string Name { get; set; }

        public Element Element { get; set; }
        public ObservableCollection<Inst> InstItem { get; set; }
        public ObservableCollection<Parameter> Parameters { get; set; }
    }
}
