using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using MVVMAUbackup.ViewModels;


namespace MVVMAUbackup.Serialization
{
    class Serializer
    {
        private static BinaryFormatter binaryIO = new BinaryFormatter();

        public static void Serialize(FolderViewModel FM)
        {
            try
            {
                using (Stream stream = File.Create(@"./History.dat"))
                {
                    binaryIO.Serialize(stream, FM);
                }
            }
            catch (Exception e) { MessageBox.Show(e.Message); }

        }
        public static void DeSerialize(ref FolderViewModel FM)
        {
                try
                {
                    using (Stream stream = File.Open(@"./History.dat", FileMode.Open))
                    {
                        FM = (FolderViewModel)binaryIO.Deserialize(stream);
                    }
                }
                catch (Exception e) { MessageBox.Show(e.Message); }
                     
        }
    }
}
