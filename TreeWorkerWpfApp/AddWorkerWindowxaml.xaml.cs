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
    /// Логика взаимодействия для AddWorkerWindowxaml.xaml
    /// </summary>
    public partial class AddWorkerWindowxaml : Window
    {
        public bool IsEdit { get; set; } = false;
        public Worker curWorker = null;
        string strBirthDay = "";
        public AddWorkerWindowxaml()
        {
            InitializeComponent();
            datePickerBirthday.IsDropDownOpen = false;
        }

        public void FillComboDepartments(List<Department> departments)
        {
            parentDepComboBox.Items.Clear();
            foreach (Department dep in departments)
            {
                parentDepComboBox.Items.Add(dep.Name);
            }
            if (parentDepComboBox.Items.Count == 0)
            {
                parentDepComboBox.Items.Add("Резерв");
            }
            parentDepComboBox.SelectedItem = "Резерв";
        }

        public void SetEditWorker(Worker w)
        {
            curWorker = w;
            wrkWin.Title = "Редактирование данных сотрудника";
            btnOK.Content = "Сохранить";
            labelID.Content = $"ID : {w.ID}";
            dataCreate.Content = $"Запись создана : {w.DateCreate}";
            fullName.Text = w.FullName;
            position.Text = w.Position;
            salary.Text = w.Salary.ToString();
            if (parentDepComboBox.Items.Count > 0)
            {
                parentDepComboBox.SelectedItem = w.Department;
            }
            string[] sdt = w.BirthDay.Split('.');
            if (sdt.Length >= 2)
            {
                if (int.TryParse(sdt[0], out int year) && int.TryParse(sdt[1], out int month) && int.TryParse(sdt[2], out int day))
                {
                    DateTime dt = new DateTime(year, month, day);
                    //datePickerBirthday.DisplayDate = dt;
                    datePickerBirthday.SelectedDate = dt;
                    datePickerBirthday.IsDropDownOpen = false;
                }
            }
            IsEdit = true;
        }

        private void OnSelectedDataChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? dt = datePickerBirthday.SelectedDate;
            if (dt != null)
            {
                DateTime sdt = (DateTime)dt;
                //strBirthDay = string.Format("{0:D02}.{1:D02}.{2:D04}", sdt.Day, sdt.Month, sdt.Year);
                strBirthDay = string.Format("{0:D04}.{1:D02}.{2:D02}", sdt.Year, sdt.Month, sdt.Day);
            }
        }

        private void OnClickAddWorker(object sender, RoutedEventArgs e)
        {
            if (fullName.Text == "") return;
            if (position.Text == "") return;
            if (salary.Text == "") return;
            if (strBirthDay == "") return;
            if (IsEdit == false)
            {
                int.TryParse(salary.Text, out int sal);
                curWorker = new Worker(0, fullName.Text, strBirthDay, position.Text, sal, parentDepComboBox.SelectedItem.ToString());
            }
            else
            {
                curWorker.FullName = fullName.Text;
                curWorker.Department = parentDepComboBox.SelectedItem.ToString();
                curWorker.Position = position.Text;
                if (int.TryParse(salary.Text, out int sal))
                {
                    curWorker.Salary = sal;
                }
                curWorker.BirthDay = strBirthDay;
            }
            DialogResult = true;
        }
    }
}
