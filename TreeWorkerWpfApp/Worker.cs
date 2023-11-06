using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeWorkerWpfApp
{
    public enum SortedCriterion
    {
        DateCreate,
        FullName,
        BirthDay,
        Position,
        Salary,
        Department,
        Parent
    }
    public class Worker
    {
        /// <summary>
        /// Идентификатор-
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Дата создания записи (приёма на работу)
        /// </summary>
        public string DateCreate { get; set; }

        /// <summary>
        /// Ф. И. О.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public string BirthDay { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public string Position { get; set; }

        public int Salary { get; set; }

        public string Department { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Worker()
        {
            ID = 0;
            DateTime dt = DateTime.Now;
            DateCreate = string.Format("{0:D04}.{1:D02}.{2:D02} {3:D02}:{4:D02}", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute);
            FullName = "";
            BirthDay = "";
            Position = "";
            Salary = 10000;
            Department = "Резерв";

        }

        /// <summary>
        /// Конструктор для получения записи из строки в CSV формате
        /// </summary>
        /// <param name="csvStr">строка в CSV формате</param>
        /// <param name="sep">раздклитель строки в CSV формате</param>
        public Worker(string csvStr, char sep = ';')
        {
            string[] zn = csvStr.Split(sep);
            if (zn.Length >= 7)
            {
                if (int.TryParse(zn[0], out int id))
                {
                    ID = id;
                }
                DateCreate = zn[1];
                FullName = zn[2];
                BirthDay = zn[3];
                Position = zn[4];
                if (int.TryParse(zn[5], out int sal))
                {
                    Salary = sal;
                }
                Department = zn[6];
            }
        }

        /// <summary>
        /// Конструктор для заполнения всех полей экземпляра класса
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <param name="fn">фамилия имя отчество</param>
        /// <param name="bd">дата рождения</param>
        /// <param name="pos">должность</param>
        /// <param name="sal">зарплата (число)</param>
        /// <param name="dep">департамент</param>
        public Worker(int id, string fn, string bd, string pos, int sal, string dep = "Резерв")
        {
            ID = id;
            DateTime dt = DateTime.Now;
            DateCreate = string.Format("{0:D04}.{1:D02}.{2:D02} {3:D02}:{4:D02}", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute);
            FullName = fn;
            BirthDay = bd;
            Position = pos;
            Salary = sal;
            Department = dep;
        }

        /// <summary>
        ///  Формирование строки в CSV формате
        /// </summary>
        /// <param name="sep">раздклитель строки в CSV формате</param>
        /// <returns>строка в CSV формате</returns>
        public string CsvRecord(char sep = ';')
        {
            StringBuilder res = new StringBuilder(ID.ToString());
            res.Append(sep + DateCreate);
            res.Append(sep + FullName);
            res.Append(sep + BirthDay);
            res.Append(sep + Position);
            res.Append(sep + Salary.ToString());
            res.Append(sep + Department);
            return res.ToString();
        }

        /// <summary>
        /// Формирование строки в задаваемом формате
        /// </summary>
        /// <param name="pattern">формат строки</param>
        /// <returns></returns>
        public string OutFormatString(string pattern)
        {
            return string.Format(pattern, ID, DateCreate, FullName, BirthDay, Position, Salary, Department);
        }

        /// <summary>
        /// Формирование строки из некоторых наиболее важных полей экхемпляра класса
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder res = new StringBuilder("Сотрудник № " + ID.ToString());
            res.Append(" " + FullName);
            res.Append(" дата рождения: " + BirthDay);
            res.Append(" департамент: " + Department);
            res.Append(" должность: " + Position);
            res.Append(" оклад: " + Salary.ToString());
            return res.ToString();
        }

        private class SortBySalary : IComparer<Worker>
        {
            public int Compare(Worker x, Worker y)
            {
                Worker X = (Worker)x;
                Worker Y = (Worker)y;

                if (X.Salary == Y.Salary) return 0;
                else if (X.Salary > Y.Salary) return 1;
                else return -1;
            }
        }
        private class SortByDateCreate : IComparer<Worker>
        {
            public int Compare(Worker x, Worker y)
            {
                return String.Compare(x.DateCreate, y.DateCreate);
            }
        }
        private class SortByName : IComparer<Worker>
        {
            public int Compare(Worker x, Worker y)
            {
                return String.Compare(x.FullName, y.FullName);
            }
        }
        private class SortByDepartment : IComparer<Worker>
        {
            public int Compare(Worker x, Worker y)
            {
                return String.Compare(x.Department, y.Department);
            }
        }
        private class SortByBirthDay : IComparer<Worker>
        {
            public int Compare(Worker x, Worker y)
            {
                return String.Compare(x.BirthDay, y.BirthDay);
            }
        }
        private class SortByPosition : IComparer<Worker>
        {
            public int Compare(Worker x, Worker y)
            {
                return String.Compare(x.Position, y.Position);
            }
        }

        public static IComparer<Worker> SortedBy(SortedCriterion Criterion)
        {
            IComparer<Worker> ic = null;
            switch(Criterion)
            {
                case SortedCriterion.DateCreate:
                    ic = new SortByDateCreate();
                    break;
                case SortedCriterion.FullName:
                    ic = new SortByName();
                    break;
                case SortedCriterion.BirthDay:
                    ic = new SortByBirthDay();
                    break;
                case SortedCriterion.Position:
                    ic = new SortByPosition();
                    break;
                case SortedCriterion.Salary:
                    ic = new SortBySalary();
                    break;
                case SortedCriterion.Department:
                    ic = new SortByDepartment();
                    break;
            }
            return ic;
        }
    }

}
