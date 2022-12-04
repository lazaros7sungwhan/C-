using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;


namespace load_save_Test1
{
    // 추가 클래스 + 값이추가될때마다 배열공간 확보 
    public static class extenstion
    {
        public static T[] array_addition<T>(this T[] array, T item)
        {
            if(array==null)
            {
                return new T[] { item };
            }
            T[] result =new T[array.Length+1];
            array.CopyTo(result, 0);
            result[array.Length] = item;            
            return result;
        }
    }
    // 추가 클래스 + 값이추가될때마다 배열공간 확보 
    internal class Program
    {
        static string[] xstring, ystring;
        public static void load_save()
        {
            using(OpenFileDialog file_di = new OpenFileDialog())
            {
                file_di.Multiselect = true;
                file_di.FilterIndex = 1;
                file_di.Filter = "datfile.(*.dat)|*.dat|txtfile.(*.txt)|*.txt";
                if(file_di.ShowDialog() == DialogResult.OK)
                {
                    string[] seriesDirectory=file_di.FileNames;
                    foreach (string element in seriesDirectory)
                    {
                        string allocatedDirectory = null;
                        if (element.IndexOf(".dat") != -1)
                        {
                            allocatedDirectory=element.Remove(element.IndexOf(".dat"));
                            allocatedDirectory += "(R).txt";
                        }
                        else if (element.IndexOf(".txt") != -1)
                        {
                            allocatedDirectory = element.Remove(element.IndexOf(".txt"));
                            allocatedDirectory += "(R).txt";
                        }
                        else
                        {
                            MessageBox.Show("invalid file..");
                            break;
                        }
                        using (StreamReader fileRead=new StreamReader(element))
                        {
                            using (StreamWriter filewrite=new StreamWriter(allocatedDirectory))
                            {
                                string line_1 = null;
                                string[] line_1_array = null;
                                int numberofline = 0, writenumber_1 = 0 ;
                                while ((line_1 = fileRead.ReadLine()) != null)
                                {
                                    if (numberofline < 8) filewrite.WriteLine(line_1);
                                    else if ((line_1 != "") && (line_1.IndexOf("Station") == -1) && (line_1.IndexOf("Data") == -1) && 
                                    (line_1.IndexOf("Time") == -1) && (line_1.IndexOf("Test") == -1) && (line_1.IndexOf("s") == -1) && (line_1.IndexOf("mts") == -1))
                                    {
                                        //filewrite.WriteLine(line_1);
                                        line_1_array = line_1.Split('\t');
                                        xstring = xstring.array_addition(line_1_array[1]);
                                        ystring = ystring.array_addition(line_1_array[2]);
                                    }
                                    numberofline++;
                                }
                                foreach(string element2 in xstring)
                                {
                                    filewrite.Write(element2 + "\t" + ystring[writenumber_1] + "\n");
                                   writenumber_1++;
                                }
                            }
                        }
                    }      
                }  
            }
        }
       
        [STAThread]
        static void Main(string[] args)
        {
            char func_num = '0';
            do
            {
                Console.WriteLine("1");
                Console.WriteLine("2");
                Thread.Sleep(10);
                Console.Clear();
                if(Console.KeyAvailable==true)
                {
                    func_num = Console.ReadKey(true).KeyChar;

                    switch (func_num)
                    {
                        case '1':
                        {
                            load_save();
                            break;
                        }
                        case '2':
                        {
                            break;
                        }
                    }
                }
            } while (func_num!= '2');
        }
    }
}
