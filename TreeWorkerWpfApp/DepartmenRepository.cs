using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeWorkerWpfApp
{
    class DepartmenRepository
    {
        List<Department> departments = new List<Department>();

        public DepartmenRepository() { }
        public DepartmenRepository(List<Department> dep)
        {
            departments = dep;
        }

        /// <summary>
        /// Возвращает экземпляр Department, или null если такого экземпляра нет
        /// </summary>
        /// <param name="index">Позиция в базе данных</param>
        /// <returns>Department</returns>
        public Department this[int index]
        {
            get { return this.departments[index]; }
        }

        public Department this[string nameDep]
        {
            get
            {
                foreach(Department dep in departments)
                {
                    if (dep.Name == nameDep)
                    {
                        return dep;
                    }
                }
                return null;
            }
        }

        public List<Department> GetSortedDepartments()
        {
            departments.Sort(Department.SortBy(SortedCriterion.Parent));
            return departments;
        }

        public void Generate()
        {
            Department mainDep = new Department("АгроТранс");
            AddDepartment(mainDep);
            AddDepartment(new Department("Руководство", mainDep));
            AddDepartment(new Department("Бухгалтерия", mainDep));
            Department dp = new Department("Департамент транспорта", mainDep);
            AddDepartment(new Department("Департамент пассажироперевозок", dp));
            AddDepartment(new Department("Департамент грузоперевозок", dp));
            AddDepartment(dp);
        }

        public void AddDepartment(Department dep)
        {
            departments.Add(dep);
        }

        public void DelDepartment(Department dep)
        {
            departments.Remove(dep);
        }

        public int Count => departments.Count;
        public List<Department> AllDepartments => departments;

        public static DepartmenRepository LoadRepositoryFromFile(string path)
        {
            try
            {
                List<Department> tempDepCol = new List<Department>();
                // Создаем сериализатор на основе указанного типа 
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Department>));

                // Создаем поток для чтения данных
                Stream fStream = new FileStream(path, FileMode.Open, FileAccess.Read);

                // Запускаем процесс десериализации
                tempDepCol = xmlSerializer.Deserialize(fStream) as List<Department>;

                // Закрываем поток
                fStream.Close();

                // Возвращаем результат
                return new DepartmenRepository(tempDepCol);
            }
            catch(Exception e)
            {
                return new DepartmenRepository();
            }
        }

        public void SaveRepositoryToFile(string Path)
        {
            // Создаем сериализатор на основе указанного типа 
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Department>));

            // Создаем поток для сохранения данных
            Stream fStream = new FileStream(Path, FileMode.Create, FileAccess.Write);

            // Запускаем процесс сериализации
            xmlSerializer.Serialize(fStream, departments);

            // Закрываем поток
            fStream.Close();
        }

        /// <summary>
        /// Метод сериализации List<Department>
        /// </summary>
        /// <param name="myList">Коллекция для сериализации</param>
        /// <param name="Path">Путь к файлу</param>
        public static void SerializeDepartmentList(List<Department> myList, string Path)
        {
            // Создаем сериализатор на основе указанного типа 
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Department>));

            // Создаем поток для сохранения данных
            Stream fStream = new FileStream(Path, FileMode.Create, FileAccess.Write);

            // Запускаем процесс сериализации
            xmlSerializer.Serialize(fStream, myList);

            // Закрываем поток
            fStream.Close();
        }

        /// <summary>
        /// Метод десериализации DepartmentList
        /// </summary>
        /// <param name="Path">Путь к файлу</param>
        public static List<Department> DeserializeDepartmentList(string Path)
        {
            List<Department> tempDepCol = new List<Department>();
            // Создаем сериализатор на основе указанного типа 
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Department>));

            // Создаем поток для чтения данных
            Stream fStream = new FileStream(Path, FileMode.Open, FileAccess.Read);

            // Запускаем процесс десериализации
            tempDepCol = xmlSerializer.Deserialize(fStream) as List<Department>;

            // Закрываем поток
            fStream.Close();

            // Возвращаем результат
            return tempDepCol;
        }
    }
}
