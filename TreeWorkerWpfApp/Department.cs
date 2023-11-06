using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeWorkerWpfApp
{
    public class Department
    {
        static int id = 0;
        static int NextID()
        {
            return ++id;
        }

        public int UID { get; set; }
        public string Name { get; set; }

        public Department Parent { get; set; }

        public Department()
        {
            UID = Department.NextID();
            Name = "NoName";
            Parent = null;
        }

        public Department(string nm, Department dep = null)
        {
            UID = Department.NextID();
            Name = nm;
            Parent = dep;
        }

        public override string ToString()
        {
            return $"{Name} {UID}";
        }

        private class SortByParent : IComparer<Department>
        {
            public int Compare(Department x, Department y)
            {
                if ((x.Parent == null) && (y.Parent == null)) return 0;
                else if ((x.Parent != null) && (y.Parent == null)) return 1;
                else if ((x.Parent == null) && (y.Parent != null)) return -1;
                else return String.Compare(x.Parent.ToString(), y.Parent.ToString());
            }
        }
        private class SortByName : IComparer<Department>
        {
            public int Compare(Department x, Department y)
            {
                return String.Compare(x.Name, y.Name);
            }
        }

        public static IComparer<Department> SortBy(SortedCriterion sortedCriterion)
        {
            IComparer<Department> ic = null;
            switch (sortedCriterion)
            {
                case SortedCriterion.Parent:
                    ic = new SortByParent();
                    break;
                case SortedCriterion.FullName:
                    ic = new SortByName();
                    break;
            }
            return ic;

        }
    }
}
