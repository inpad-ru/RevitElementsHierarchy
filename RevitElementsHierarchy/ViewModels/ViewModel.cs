using InpadPlugins.RevitElementsHierarchy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InpadPlugins.RevitElementsHierarchy.ViewModels
{
    internal class ViewModel:INotifyPropertyChanged
    {
        private Inst selectedInstance;
        public ObservableCollection<Parameter> Param { get; set; }
        public ObservableCollection<Inst> Insts { get; set; }
        public Inst SelectedInstance
        {
            get { return selectedInstance; }
            set
            {
                selectedInstance = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
