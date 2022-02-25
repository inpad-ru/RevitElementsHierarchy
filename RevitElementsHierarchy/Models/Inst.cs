using System.Collections.ObjectModel;

namespace InpadPlugins.RevitElementsHierarchy.Models
{
    public class Inst
    {
        public string Name { get; set; }
        public ObservableCollection<Inst> InstItem { get; set; }
        public ObservableCollection<Parameter> Parameters { get; set; }
    }
}
