using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Configuration;
using Formulyar.ViewModel;

namespace Formulyar
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString;
        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            DataContext = new AppViewModel();//TODO: можно убрать
             
        }

        private void otiGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
        private ObjectDataProvider MyObjectDataProvider
        {
            get
            {
                return this.TryFindResource("EmployeeData") as ObjectDataProvider;
            }
        }

        private void checkBox_Click(object sender, EventArgs e)
        {
            CheckBox chbx = sender as CheckBox;
            if (chbx.IsChecked == true)
                chbx.IsChecked = false;
            else
                chbx.IsChecked = true;
        }
        private void checkBox1_Click(object sender, EventArgs e)
        {
          
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void otiGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (otiGrid.SelectedItem!=null)
                otiGrid.ScrollIntoView(otiGrid.SelectedItem);
        }

        private void Button_Click_Insert_New_Position(object sender, RoutedEventArgs e)
        {


        }

        private void checkBoxKosmos_Click(object sender, RoutedEventArgs e)
        {

        }

        private void smtnAopoGrid_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action.ToString()=="Added")
                ButtonSave.IsEnabled = false;            
            else
                ButtonSave.IsEnabled = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
  
}
