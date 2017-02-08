using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using System.IO;

namespace Data_Generator
{

    public class PackageToObject
    {
        public int id_of_package { get; set; }
        public int id_of_object { get; set; }

    }

    public class ObjectType
    {
        public int id_of_object { get; set; }
        public char type_of_object { get; set; }

    }

    public class ObjectToFields
    {
        public int id_of_object { get; set; }
        public int id_of_field { get; set; }

    }
    class Program
    {
        static void Main(string[] args)
        {
            int number_of_packages = 500;
            Random randomNumber = new Random(5);
            Binomial av_num_of_obj_per_package = new Binomial(0.0125, 8000);
            Binomial av_num_of_fields_of_obj_catA = new Binomial(0.02, 1500);
            Binomial av_num_of_fields_of_obj_catB = new Binomial(0.02, 400);
            Binomial av_num_of_incoming_fields = new Binomial(0.03125, 80);
            //int[] table_of_packagess = new int[number_of_packages];
            List<PackageToObject> table_of_edges_pcg_obj = new List<PackageToObject>();
            List<ObjectToFields> table_of_edges_obj_fields = new List<ObjectToFields>();
            List<ObjectType> list_of_objects = new List<ObjectType>();
           

            string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath_pcg_to_obj = pathDesktop + "\\table_of_edges_pcg_obj.csv";
            if (!File.Exists(filePath_pcg_to_obj))
            {
                File.Create(filePath_pcg_to_obj).Close();
            }
            string filePath_pcg_to_obj_fields = pathDesktop + "\\table_of_edges_obj_fields.csv";
            if (!File.Exists(filePath_pcg_to_obj_fields))
            {
                File.Create(filePath_pcg_to_obj_fields).Close();
            }
            string filePath_list_of_objects = pathDesktop + "\\objects.csv";
            if (!File.Exists(filePath_list_of_objects))
            {
                File.Create(filePath_list_of_objects).Close();
            }
            string delimter = ",";




            for (int i = 0; i < number_of_packages; i++)
            {
                //table_of_packagess[i] = i + 1;
                for (int j = 0; j < av_num_of_obj_per_package.Sample(); j++)
                {
                    ObjectType new_object = new ObjectType();
                    new_object.id_of_object = j;
                    if (randomNumber.Next(101) <= 70)
                    {
                        new_object.type_of_object = 'A';
                        for (int k = 0; k < av_num_of_fields_of_obj_catA.Sample(); k++)
                        {
                            ObjectToFields new_edge = new ObjectToFields();
                            new_edge.id_of_object = j;
                            new_edge.id_of_field = k;
                            table_of_edges_obj_fields.Add(new_edge);
                        }
                    }
                    else
                    {
                        new_object.type_of_object = 'B';
                        for (int k = 0; k < av_num_of_fields_of_obj_catA.Sample(); k++)
                        {
                            ObjectToFields new_edge = new ObjectToFields();
                            new_edge.id_of_object = j;
                            new_edge.id_of_field = k;
                            table_of_edges_obj_fields.Add(new_edge);
                        }
                    }
                    list_of_objects.Add(new_object);
                    PackageToObject temporary = new PackageToObject();
                    temporary.id_of_object = j;
                    temporary.id_of_package = i;
                    table_of_edges_pcg_obj.Add(temporary);
                }
                /*
                       int dodatnia=0;
            int ujemna=0;
            int sum_av_num_of_obj_per_package=0;
            int sum_av_num_of_fields_of_obj_catA=0;
            int sum_av_num_of_fields_of_obj_catB=0;
            double sum_av_num_of_incoming_fields=0;
                if (randomowa.Next(101) <= 70)
                    dodatnia++;
                else
                    ujemna--;
                sum_av_num_of_obj_per_package += av_num_of_obj_per_package.Sample();
                sum_av_num_of_fields_of_obj_catA += av_num_of_fields_of_obj_catA.Sample();
                sum_av_num_of_fields_of_obj_catB += av_num_of_fields_of_obj_catB.Sample();
                sum_av_num_of_incoming_fields += av_num_of_incoming_fields.Sample();
                 */
            }
            /*
            Console.WriteLine("Srednia liczba obiektów w pakiecie:" + sum_av_num_of_obj_per_package / number_of_packages);
            Console.WriteLine("Srednia liczba obiektów kategoria A:" + sum_av_num_of_fields_of_obj_catA / number_of_packages);
            Console.WriteLine("Srednia liczba obiektów kategoria B:" + sum_av_num_of_fields_of_obj_catB / number_of_packages);
            Console.WriteLine("Srednia liczba przychodzących pól:" + sum_av_num_of_incoming_fields / number_of_packages);
            Console.WriteLine(dodatnia);
            Console.WriteLine(ujemna);
            */
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
