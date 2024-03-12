using System.Collections.Generic;
using System.IO;

namespace ZdravoCorp.Utils.Serializer
{
    public class Serializer<T> where T : Serializable, new()
    {
        private static char DELIMITER = '|';
        private static readonly object csvLock = new object();

        public void toCSV(string fileName, List<T> objects)
        {
            lock (csvLock)
            {
                using StreamWriter streamWriter = new StreamWriter(fileName);

                foreach (Serializable obj in objects)
                {
                    string line = string.Join(DELIMITER.ToString(), obj.ToCSV());
                    streamWriter.WriteLine(line);
                }
            }
        }

        public List<T> fromCSV(string fileName)
        {
            lock (csvLock)
            {
                List<T> objects = new List<T>();

                foreach (string line in File.ReadLines(fileName))
                {
                    string[] csvValues = line.Split(DELIMITER);
                    T obj = new T();
                    obj.FromCSV(csvValues);
                    objects.Add(obj);
                }

                return objects;
            }
        }
    }

}
