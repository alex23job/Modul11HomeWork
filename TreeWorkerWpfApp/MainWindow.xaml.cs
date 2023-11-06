using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace TreeWorkerWpfApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DepartmenRepository depRep = null;
        WorkerRepository wrkRep = null;


        string pathDepartments = "Departments.xml";
        string pathWorkers = "Workers.xml";

        string selectDepName = "";
        public MainWindow()
        {
            InitializeComponent();
            wrkRep = WorkerRepository.LoadRepositoryFromFile(pathWorkers);
            depRep = DepartmenRepository.LoadRepositoryFromFile(pathDepartments);
            if (depRep.Count == 0) depRep.Generate();
            CreateTreeFromList(depRep.GetSortedDepartments());
            if (wrkRep.Count > 0)
            {
                dataGrid.ItemsSource = wrkRep.AllWorkers;
            }
        }

        private void CreateTreeFromList(List<Department> list)
        {
            //treeView.Items.Clear();
            int i;
            if (list.Count == 0)
            {

            }
            else
            {
                Node rootNode = null;
                for (i = 0; i < list.Count; i++)
                {
                    if (list[i].Parent == null)
                    {
                        rootNode = new Node() { Name = list[i].Name };
                    }
                    else
                    {
                        if (rootNode != null)
                        {
                            Node curNode = rootNode[list[i].Parent.Name];
                            if (curNode != null)
                            {
                                curNode.Children.Add(new Node() { Name = list[i].Name });
                            }
                        }
                    }

                }
                if (rootNode != null)
                {
                    treeView.ItemsSource = new ObservableCollection<Node>() { rootNode };
                }
            }
        }

        private void OnAddChildDepartment(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("In OnAddChildDepartment !!!");
        }

        private void OnRenameDepartment(object sender, RoutedEventArgs e)
        {
            //var data = (sender as FrameworkElement).DataContext as Department;
            var data = e.OriginalSource as TreeViewItem;
            Department dep = data.Header as Department;
            if (dep != null)
            MessageBox.Show($"In OnRenameDepartment sender = {dep.Name}");
        }

        private void OnDelDepartment(object sender, RoutedEventArgs e)
        {
            //MenuItem menuItem = (sender as ContextMenu).Items[0] as MenuItem; // Получите выбранный пункт меню
            MessageBox.Show($"In OnDelDepartment menuItem.DataContext={(sender as MenuItem).DataContext.ToString()}");
            //TreeNode selectedTreeNode = treeView.ItemContainerGenerator.ContainerFromItem(menuItem.DataContext) as TreeNode;
        }

        private void AppClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            depRep.SaveRepositoryToFile(pathDepartments);
            wrkRep.SaveRepositoryToFile(pathWorkers);
        }

        private void AddWorkerClick(object sender, RoutedEventArgs e)
        {
            AddWorkerWindowxaml aww = new AddWorkerWindowxaml();
            aww.FillComboDepartments(depRep.AllDepartments);
            if (aww.ShowDialog() == true)
            {
                wrkRep.AddWorker(aww.curWorker);
                dataGrid.ItemsSource = wrkRep.AllWorkers;
            }
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //MessageBox.Show("OnTreeViewSelectedItemChanged");
            Node nNode = e.NewValue as Node;
            if (nNode != null)
            {
                selectDepName = nNode.Name;
                if (wrkRep.Count > 0)
                {
                    Department dep = depRep[nNode.Name];
                    if (dep != null && dep.Parent == null)
                    {
                        selectDepName = "";
                    }
                    dataGrid.ItemsSource = wrkRep.GetWorkersFromDep(selectDepName);
                }
            }
        }

        private void OnMouseRightButtonClick(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = e.OriginalSource as TextBlock;
            if (tb != null)
            {
                Node curNode = tb.DataContext as Node;
                if (curNode != null)
                {
                    treeView.Items.MoveCurrentTo(curNode);
                    OnTreeViewSelectedItemChanged(sender, new RoutedPropertyChangedEventArgs<object>(treeView.SelectedItem, curNode));

                    ContextMenu contextMenu = new ContextMenu();
                    MenuItem menuItemAdd = new MenuItem();
                    menuItemAdd.Header = "Добавить подчинённый департамент";
                    menuItemAdd.Click += (send, args) =>
                    {
                        AddDepWindow adw = new AddDepWindow();
                        adw.SetSelectedDepartment(curNode.Name, depRep.AllDepartments);
                        if (adw.ShowDialog() == true)
                        {
                            depRep.AddDepartment(new Department(adw.NameDep, depRep[adw.NameParentDep]));
                            CreateTreeFromList(depRep.GetSortedDepartments());
                        }
                    };
                    contextMenu.Items.Add(menuItemAdd);
                    MenuItem menuItemEdit = new MenuItem();
                    menuItemEdit.Header = "Изменить назвагние и/или подчинённость департамента";
                    menuItemEdit.Click += (send, args) => {
                        // Здесь ваш код при клике по пункту меню
                        EditDepWindow edw = new EditDepWindow();
                        edw.SetSelectedDepartment(curNode.Name, depRep.AllDepartments);
                        if (edw.ShowDialog() == true)
                        {
                            Department editDep = depRep[curNode.Name];
                            editDep.Name = edw.NameDep;
                            editDep.Parent = depRep[edw.NameParentDep];
                            CreateTreeFromList(depRep.GetSortedDepartments());
                        }
                    };
                    contextMenu.Items.Add(menuItemEdit);
                    MenuItem menuItemDel = new MenuItem();
                    menuItemDel.Header = "Удалить департамент";
                    menuItemDel.Click += (send, args) => {
                        //if (MessageBox.Show($"Выбран департамент : {curNode.Name}\n\nУдалить департамент ?", "Удаление департамента", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        //{
                        //    Department dep = depRep[curNode.Name];
                        //    depRep.DelDepartment(dep);
                        //    CreateTreeFromList(depRep.GetSortedDepartments());
                        //}
                        DelDepWindow ddw = new DelDepWindow();
                        ddw.SetSelectedDepartment(curNode.Name);
                        if (ddw.ShowDialog() == true)
                        {
                            if (ddw.IsReserved)
                            {
                                wrkRep.TransferWorkers(depRep[curNode.Name], depRep["Резерв"]);
                            }
                            else
                            {
                                wrkRep.DismissWorkersFromDep(depRep[curNode.Name]);
                            }
                            Department dep = depRep[curNode.Name];
                            depRep.DelDepartment(dep);
                            CreateTreeFromList(depRep.GetSortedDepartments());
                        }
                    };
                    contextMenu.Items.Add(menuItemDel);
                    contextMenu.IsOpen = true;
                }
            }
        }

        private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = e.OriginalSource as TextBlock;
            if (tb != null)
            {
                Node curNode = tb.DataContext as Node;
                if (curNode != null)
                {
                    treeView.Items.MoveCurrentTo(curNode);
                    OnTreeViewSelectedItemChanged(sender, new RoutedPropertyChangedEventArgs<object>(treeView.SelectedItem, curNode));
                }
            } 
        }

        private void OnDataGridMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show($"sender={sender}  e={e}");
            TextBlock tb = e.OriginalSource as TextBlock;
            if (tb != null)
            {
                Worker w = tb.DataContext as Worker;
                if (w != null)
                {
                    //MessageBox.Show(w.ToString());
                    dataGrid.SelectedItem = w;

                    ContextMenu contextMenu = new ContextMenu();
                    MenuItem menuItemEdit = new MenuItem();
                    menuItemEdit.Header = "Редактировать данные сотрудника";
                    menuItemEdit.Click += (send, args) => {
                        AddWorkerWindowxaml aww = new AddWorkerWindowxaml();
                        aww.FillComboDepartments(depRep.AllDepartments);
                        aww.SetEditWorker(w);
                        if (aww.ShowDialog() == true)
                        {   
                            dataGrid.InputScope = null;
                            w = aww.curWorker;
                            //wrkRep.AddWorker(aww.curWorker);
                            dataGrid.ItemsSource = wrkRep.GetWorkersFromDep(selectDepName);
                            dataGrid.Items.Refresh();
                        }
                    };
                    contextMenu.Items.Add(menuItemEdit);
                    MenuItem menuItemDel = new MenuItem();
                    menuItemDel.Header = "Уволить сотрудника";
                    menuItemDel.Click += (send, args) => {
                        if (MessageBox.Show($"Выбран сотрудник : {w.ToString()}\n\nУволить сотрудника ?", "Уволить сотрудника", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            wrkRep.DelWorker(w);
                            dataGrid.ItemsSource = wrkRep.GetWorkersFromDep(selectDepName);
                        }
                    };
                    contextMenu.Items.Add(menuItemDel);
                    contextMenu.IsOpen = true;
                }
            }
            //
        }
    }

    public class WindowCommands
    {
        static WindowCommands()
        {
            AddDep = new RoutedCommand("AddDep", typeof(MainWindow));
            EditDep = new RoutedCommand("EditDep", typeof(MainWindow));
            DelDep = new RoutedCommand("DelDep", typeof(MainWindow));
        }
        public static RoutedCommand AddDep { get; set; }
        public static RoutedCommand EditDep { get; set; }
        public static RoutedCommand DelDep { get; set; }
    }

    public class Node
    {
        public string Name { get; set; }
        public ObservableCollection<Node> Children { get; } = new ObservableCollection<Node>();

        public Node this[string name]
        {
            get
            {
                if (name == Name) return this;
                else if (Children.Count > 0)
                {
                    Node res = null;
                    for (int i = 0; i < Children.Count; i++)
                    {
                        res = Children[i][name];
                        if (res != null) return res;
                    }
                }
                return null;
            }
        }
    }
}
