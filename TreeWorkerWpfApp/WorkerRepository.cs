using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TreeWorkerWpfApp
{
    class WorkerRepository
    {
        List<Worker> workers = new List<Worker>();

        public WorkerRepository() { }
        public WorkerRepository(List<Worker> works)
        {
            workers = works;
        }

        /// <summary>
        /// Возвращает экземпляр Worker, или null если такого экземпляра нет
        /// </summary>
        /// <param name="index">Позиция в базе данных</param>
        /// <returns>Worker</returns>
        public Worker this[int index]
        {
            get { return this.workers[index]; }
        }

        public int Count => workers.Count;

        public void AddWorker(Worker w)
        {
            w.ID = Count;
            workers.Add(w);
        }

        public void DelWorker(Worker w)
        {
            workers.Remove(w);
        }

        public void DismissWorkersFromDep(Department fromDep)
        {
            for (int i = workers.Count - 1; i > 0; i--)
            {
                if (workers[i].Department == fromDep.Name)
                {
                    workers.RemoveAt(i);
                }
            }
        }

        public void TransferWorkers(Department fromDep, Department toDep)
        {
            for (int i = 0; i < workers.Count; i++)
            {
                if (workers[i].Department == fromDep.Name)
                {
                    workers[i].Department = toDep.Name;
                }
            }
        }

        public List<Worker> AllWorkers => workers;

        public List<Worker> GetWorkersFromDep(string nameDep)
        {
            List<Worker> res = new List<Worker>();
            foreach(Worker w in workers)
            {
                if (w.Department == nameDep)
                {
                    res.Add(w);
                }
            }
            return res;
        }

        public static WorkerRepository LoadRepositoryFromFile(string path)
        {
            try
            {
                List<Worker> tempCol = new List<Worker>();
                // Создаем сериализатор на основе указанного типа 
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Worker>));

                // Создаем поток для чтения данных
                Stream fStream = new FileStream(path, FileMode.Open, FileAccess.Read);

                // Запускаем процесс десериализации
                tempCol = xmlSerializer.Deserialize(fStream) as List<Worker>;

                // Закрываем поток
                fStream.Close();

                // Возвращаем результат
                return new WorkerRepository(tempCol);
            }
            catch (Exception e)
            {
                return new WorkerRepository();
            }
        }

        public void SaveRepositoryToFile(string Path)
        {
            // Создаем сериализатор на основе указанного типа 
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Worker>));

            // Создаем поток для сохранения данных
            Stream fStream = new FileStream(Path, FileMode.Create, FileAccess.Write);

            // Запускаем процесс сериализации
            xmlSerializer.Serialize(fStream, workers);

            // Закрываем поток
            fStream.Close();
        }

        /// <summary>
        /// Метод сериализации List<Worker>
        /// </summary>
        /// <param name="myList">Коллекция для сериализации</param>
        /// <param name="Path">Путь к файлу</param>
        public static void SerializeWorkerList(List<Worker> myList, string Path)
        {
            // Создаем сериализатор на основе указанного типа 
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Worker>));

            // Создаем поток для сохранения данных
            Stream fStream = new FileStream(Path, FileMode.Create, FileAccess.Write);

            // Запускаем процесс сериализации
            xmlSerializer.Serialize(fStream, myList);

            // Закрываем поток
            fStream.Close();
        }

        /// <summary>
        /// Метод десериализации WorkerList
        /// </summary>
        /// <param name="Path">Путь к файлу</param>
        public static List<Worker> DeserializeWorkerList(string Path)
        {
            List<Worker> tempCol = new List<Worker>();
            // Создаем сериализатор на основе указанного типа 
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Worker>));

            // Создаем поток для чтения данных
            Stream fStream = new FileStream(Path, FileMode.Open, FileAccess.Read);

            // Запускаем процесс десериализации
            tempCol = xmlSerializer.Deserialize(fStream) as List<Worker>;

            // Закрываем поток
            fStream.Close();

            // Возвращаем результат
            return tempCol;
        }
    }
}
