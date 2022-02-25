using InpadPlugins.RevitElementsHierarchy.Models;
using System;
using System.Collections.Generic;
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
        private ViewModels.ViewModel viewModel { get; }
        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = new ViewModels.ViewModel();

            viewModel = DataContext as ViewModels.ViewModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void instList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Inst p = (Inst)instList.SelectedItem;
            //MessageBox.Show(p.Name);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            viewModel.SelectedInstance = (Inst)treeCat.SelectedItem;
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            viewModel.SelectedInstance = (Inst)e.NewValue;
        }
    }
}
