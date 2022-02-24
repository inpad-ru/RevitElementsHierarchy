using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InpadPlugins.RevitElementsHierarchy.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window, INotifyPropertyChanged
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

        public MainWindow()
        {
            InitializeComponent();
            Insts = new ObservableCollection<Inst>
            {
                new Inst
                {
                    Name ="Category 1",
                    InstItem = new ObservableCollection<Inst>
                    {
                        new Inst {Name ="Type 1"},
                        new Inst {Name="Type 2" },
                        new Inst
                        {
                            Name ="Type 3",
                            InstItem = new ObservableCollection<Inst>
                            {
                                new Inst {Name="Inst 1" },
                                new Inst {Name="Inst 2" },
                                new Inst {Name="Inst 3" },
                                new Inst {Name="Inst 4" },
                            }
                        }
                    }
                },
                new Inst
                {
                    Name="Category 2",
                    InstItem = new ObservableCollection<Inst>
                    {
                        new Inst
                        {
                            Name ="Type 1",
                            InstItem = new ObservableCollection<Inst>
                            {
                                new Inst {Name="Inst 1" },
                                new Inst {Name="Inst 2" }
                            }
                        },
                        new Inst {Name="Type 2" },
                        new Inst
                        {
                            Name ="Type 3",
                           InstItem = new ObservableCollection<Inst>
                            {
                                new Inst {Name="Instance 1",
                                Parameters = new ObservableCollection<Parameter>
                                {
                                    new Parameter {Name="Param1"},
                                    new Parameter {Name="Param2"},
                                    new Parameter {Name="Param3"}
                                }
                                },
                                new Inst {Name="Instance 2" },
                                new Inst {Name="Instance 3" }
                            }
                        }
                    }
                },
                new Inst {Name="Category 3"}
            };
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void instList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Inst p = (Inst)instList.SelectedItem;
            MessageBox.Show(p.Name);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedInstance = (Inst)treeCat.SelectedItem;
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedInstance = (Inst)e.NewValue;
        }
    }

    public class Inst
    {
        public string Name { get; set; }
        public ObservableCollection<Inst> InstItem { get; set; }
        public ObservableCollection<Parameter> Parameters { get; set; }
    }

    public class Parameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
