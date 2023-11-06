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
using System.Windows.Shapes;

namespace TreeWorkerWpfApp
{
    /// <summary>
    /// Логика взаимодействия для AddDepWindow.xaml
    /// </summary>
    public partial class AddDepWindow : Window
    {
        public string NameDep { get; set; }
        public string NameParentDep { get; set; }

        public AddDepWindow()
        {
            InitializeComponent();
        }

        private void OnClickAddDepartment(object sender, RoutedEventArgs e)
        {
            if (newDepName.Text == "") return;
            else
            {
                NameDep = newDepName.Text;
                NameParentDep = (string)parentDepComboBox.SelectedItem;
            }
            this.DialogResult = true;
        }

        public void SetSelectedDepartment(string depName, List<Department> departments)
        {
            parentDepComboBox.Items.Clear();
            //parentDepComboBox.Items.Add("АгроТранс");
            foreach (Department dep in departments)
            {
                parentDepComboBox.Items.Add(dep.Name);
            }
            parentDepComboBox.SelectedItem = depName;
        }
    }
}
