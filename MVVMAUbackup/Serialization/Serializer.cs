using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using MVVMAUbackup.Models;


namespace MVVMAUbackup.Serialization
{
    class Serializer
    {
        private static BinaryFormatter binaryIO = new BinaryFormatter();

        public static void Serialize(FolderModel FM, StatusModel SM)
        {
            try
            {
                using (Stream stream = File.Create(@"./History.dat"))
                {

                }
            }
            catch (Exception e) { MessageBox.Show(e.Message); }

        }
        public static void DeSerialize()
        {
            if (File.Exists(@"./History.dat"))
            {
                try
                {
                    using (Stream stream = File.Open(@"./History.dat", FileMode.Open))
                    {

                    }
                }
                catch (Exception e) { }
            }          
        }
    }
}
