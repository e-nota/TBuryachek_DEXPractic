using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Service
{
    public class FileExportService
    {
        public void FileExport<T>(T person)
        {
            var myType = person.GetType();
            var properties = myType.GetProperties();
            foreach(var property in properties)
            {
                var val = property.GetValue(person);
                var name = property.Name;
                string text = name + " - " + val + " ";

                string path = Path.Combine("D:", "Courses", "TBuryachek_DEXPractic", "BankSystem", "Files");
                using (FileStream fileStream = new FileStream($"{path}\\PropertiesOfPerson.txt", FileMode.Append))
                {
                    byte[] array = System.Text.Encoding.Default.GetBytes(text);
                    fileStream.Write(array, 0, array.Length);
                }
            }

        }
    }
}
