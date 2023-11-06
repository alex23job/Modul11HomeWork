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
    /// Логика взаимодействия для DelDepWindow.xaml
    /// </summary>
    public partial class DelDepWindow : Window
    {
        public string NameDep { get; set; }
        public bool IsReserved { get; set; }

        public DelDepWindow()
        {
            InitializeComponent();
        }

        private void OnClickAddDepartment(object sender, RoutedEventArgs e)
        {
            if (newDepName.Text == "") return;
            else
            {
                NameDep = newDepName.Text;
                IsReserved = (bool)radioReserve.IsChecked;
            }
            this.DialogResult = true;
        }

        public void SetSelectedDepartment(string depName)
        {
            newDepName.Text = depName;
        }
    }
}
