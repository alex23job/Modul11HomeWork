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
    /// Логика взаимодействия для EditDepWindow.xaml
    /// </summary>
    public partial class EditDepWindow : Window
    {
        public string NameDep { get; set; }
        public string NameParentDep { get; set; }

        public EditDepWindow()
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
            Department curDep = null;
            foreach (Department dep in departments)
            {
                if (depName == dep.Name)
                {
                    curDep = dep;
                }    
                parentDepComboBox.Items.Add(dep.Name);
            }
            if (curDep != null && curDep.Parent != null)
            {
                parentDepComboBox.SelectedItem = curDep.Parent.Name;
            }
            newDepName.Text = depName;
        }
    }
}
