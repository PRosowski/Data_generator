using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using System.IO;

namespace Data_Generator
{
    public class PackageType
    {
        public int id_of_package { get; set; }

        public List<PackageType> list_of_incoming_packages;

    }

    public class PackageTransformations
    {
        public PackageType source_package
        {
            get; set;
        }
        public PackageType destiny_package
        {
            get; set;
        }
        public int ID { get; set; }

    }

    public class ObjectType
    {
        public int id_of_object { get; set; }
        public int id_of_package { get; set; }
        public char type_of_object { get; set; }

        public List<ObjectType> list_of_incoming_objects;

    }

    public class ObjectTransformations
    {
        public ObjectType source_object
        {
            get; set;
        }
        public ObjectType destiny_object
        {
            get; set;
        }
        public int ID { get; set; }

    }

    public class FieldType
    {
        public int id_of_object { get; set; }
        public int id_of_package { get; set; }
        public int id_of_field { get; set; }

        public List<FieldType> list_of_incoming_fields;

    }

    public class FieldTransformations
    {
        public FieldType source_field
        {
            get; set;
        }
        public FieldType destiny_field
        {
            get; set;
        }
        public int ID { get; set; }

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
            double single_cycle_edge = 0.04;
            double double_cycle_edge = 0.02;
            //int[] table_of_packagess = new int[number_of_packages];
            List<ObjectType> list_of_objects = new List<ObjectType>();
            List<PackageType> list_of_packages = new List<PackageType>();
            List<FieldType> list_of_fields = new List<FieldType>();
            List<PackageTransformations> list_of_package_transformations = new List<PackageTransformations>();
            List<ObjectTransformations> list_of_object_transformations = new List<ObjectTransformations>();
            List<FieldTransformations> list_of_field_transformations = new List<FieldTransformations>();
            var csv = new StringBuilder();
            var csv2 = new StringBuilder();
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

            var new_line_header2= string.Format("idobject1,idobject2, idfield");
            var new_line_header1 = string.Format("idpackage, idobject, category_of_object");
            csv2.AppendLine(new_line_header2);
            csv.AppendLine(new_line_header1);

            for (int i = 1; i <= number_of_packages; i++)
            {
                //table_of_packagess[i] = i + 1;
                PackageType new_package = new PackageType();
                new_package.id_of_package = i;
                list_of_packages.Add(new_package);
                for (int j = 1; j <= av_num_of_obj_per_package.Sample(); j++)
                {

                    ObjectType new_object = new ObjectType();
                    new_object.id_of_object = j;
                    new_object.id_of_package = i;
                    if (randomNumber.Next(101) <= 70)
                    {
                        new_object.type_of_object = 'A';
                        list_of_objects.Add(new_object);
                        var newLineA = string.Format("{0},{1},{2}", i, j, 'A');
                        csv.AppendLine(newLineA);
                        for (int k = 1; k <= av_num_of_fields_of_obj_catA.Sample(); k++)
                        {
                            FieldType new_field = new FieldType();
                            new_field.id_of_package = i;
                            new_field.id_of_object = j;
                            new_field.id_of_field = k;
                            list_of_fields.Add(new_field);
                            var newLinefield_of_A_obj = string.Format("{0},{1},{2}", i, j,k);
                            csv2.AppendLine(newLinefield_of_A_obj); 
                        }
                    }
                    else
                    {
                        new_object.type_of_object = 'B';
                        list_of_objects.Add(new_object);
                        var newLineB = string.Format("{0},{1},{2}", i, j, 'B');
                        csv.AppendLine(newLineB);
                        for (int k = 1; k <= av_num_of_fields_of_obj_catA.Sample(); k++)
                        {
                            FieldType new_field = new FieldType();
                            new_field.id_of_package = i;
                            new_field.id_of_object = j;
                            new_field.id_of_field = k;
                            list_of_fields.Add(new_field);
                            var newLinefield_of_B_obj = string.Format("{0},{1},{2}", i, j, k);
                            csv2.AppendLine(newLinefield_of_B_obj);
                        }
                    }
                    
                    //
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

            // Generowanie krawędzi typu data_transformations
            //Pętla po wygenerowanych wczesniej pakietach
            int id_of_transformations = 1;
            foreach(PackageType package_element in list_of_packages)
            {
                //Petla do tworzenia krawedzi, ilosc wskazuje zmienna av_num_of_incoming_fields
                for (int i = 1; i <= av_num_of_incoming_fields.Sample(); i++)
                {
                    /*W petli najpierw losowo wybieramy pakiet z listy pakietow, ktory :
                        - ma rozne id, czyli czy aby to nie jest ten sam pakiet;
                        - nie ma jeszcze krawedzi, przy czym sprawdzam, czy ten moj obecny pakiet nie ma krawedzi 
                          przychodzacej od tego losowo wybranego pakietu
                        - i tu chyba powinienem sprawdzac, czy aby ten losowo wybrany pakiet nie ma juz krawedzi od tego 
                          mojego pakietu (package element), by cykle generowac pozniej
                    */
                    PackageType random_package;
                    do
                    {
                        int random_index_of_package = randomNumber.Next(list_of_packages.Count);
                        random_package = list_of_packages[random_index_of_package];
                    } while (package_element.id_of_package != random_package.id_of_package && !(package_element.list_of_incoming_packages.Contains(random_package)));
                    package_element.list_of_incoming_packages.Add(random_package);
                    PackageTransformations new_transformation = new PackageTransformations();
                    new_transformation.source_package = random_package;
                    new_transformation.destiny_package = package_element;
                    new_transformation.ID = id_of_transformations;
                    id_of_transformations++;
                    list_of_package_transformations.Add(new_transformation);
                    
                }

                foreach(PackageType incoming_package in package_element.list_of_incoming_packages)
                {
                    List<ObjectType> list_of_objects_from_incoming_package = list_of_objects.FindAll(x => x.id_of_package == incoming_package.id_of_package);
                    List<ObjectType> list_of_objects_from_package_element = list_of_objects.FindAll(y => y.id_of_package == package_element.id_of_package);
                    PackageTransformations temp_package = list_of_package_transformations.Find(x => x.source_package == incoming_package && x.destiny_package == package_element);
                    foreach (ObjectType object_element in list_of_objects_from_package_element)
                    {

                        for (int j = 1; j <= av_num_of_incoming_fields.Sample(); j++)
                        {
                            ObjectType random_object;
                            do
                            {

                                int random_index_of_object = randomNumber.Next(list_of_objects_from_incoming_package.Count);
                                random_object = list_of_objects_from_incoming_package[random_index_of_object];

                            } while (!(object_element.list_of_incoming_objects.Contains(random_object)));
                            object_element.list_of_incoming_objects.Add(random_object);
                            ObjectTransformations new_transformation = new ObjectTransformations();
                            new_transformation.source_object = random_object;
                            new_transformation.destiny_object = object_element;
                            new_transformation.ID = temp_package.ID;
                            list_of_object_transformations.Add(new_transformation);
                        }



                    }
                    List<FieldType> list_of_fields_from_incoming_package = list_of_fields.FindAll(x => x.id_of_package == incoming_package.id_of_package);
                    List<FieldType> list_of_fields_from_package_element = list_of_fields.FindAll(x => x.id_of_package == package_element.id_of_package);
                    foreach(FieldType field_element in list_of_fields_from_package_element)
                    {

                        for(int j=1; j<= av_num_of_incoming_fields.Sample(); j++)
                        {
                            FieldType random_field;
                            do
                            {
                                int random_index_of_field = randomNumber.Next(list_of_fields_from_incoming_package.Count);
                                random_field = list_of_fields_from_incoming_package[random_index_of_field];
                            } while (!(field_element.list_of_incoming_fields.Contains(random_field)));
                            field_element.list_of_incoming_fields.Add(random_field);
                            FieldTransformations new_transformation = new FieldTransformations();
                            new_transformation.source_field = random_field;
                            new_transformation.destiny_field = field_element;
                            new_transformation.ID = temp_package.ID;
                            list_of_field_transformations.Add(new_transformation);


                        }

                    }

                }

            }






















            foreach (ObjectType object_element in list_of_objects)
            {
                for (int i = 0; i < av_num_of_incoming_fields.Sample(); i++)
                {
                    ObjectType random_object;
                    do
                    {
                        int random_index_of_object = randomNumber.Next(list_of_objects.Count);
                        random_object = list_of_objects[random_index_of_object];
                    } while (object_element.id_of_object != random_object.id_of_object && object_element.id_of_package != random_object.id_of_package && object_element.list_of_incoming_objects.Contains(random_object));
                    object_element.list_of_incoming_objects.Add(random_object);


                }

            }



            File.WriteAllText(filePath_pcg_to_obj, csv.ToString());
            File.WriteAllText(filePath_pcg_to_obj_fields, csv2.ToString());
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
