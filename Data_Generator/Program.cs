using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using System.IO;

namespace Data_Generator
{
    public static class DFSCLass
    {

        //Pare komentarzy searchPackage - dla ktorego szukamy, czy nie ma czasami cyklu,source, pakiet poczatkowy, list_of_dest - lista pakietow docelowych, listof_transformation lista transformacji
        public static bool DFS(PackageType searchpackage, PackageType source, List<PackageType> list_of_dest, List<PackageTransformations> list_of_transformation, List<PackageType> list_of_visited_nodes)
        {
            bool result = false;
            // przechodze po pakietach docelowych w liscie
            foreach(PackageType package in list_of_dest)
            {
                Console.WriteLine("Jestem w DFS {0}, a to poszukiwany pakiet {1}", package.id_of_package, searchpackage.id_of_package);
                // jesli pakiet docelowy == pakiet wyszukiwany zwracamy true
                if (searchpackage == package)
                {
                    result = true;
                    break;
                } 
                //Sprawdzamy, czy aby nie bylismy juz w tym wezle. Jesli bylismy, to pomijamy przechodzenie tego wezla ponownie.
                if(list_of_visited_nodes.FindAll(x => x.id_of_package == package.id_of_package).Count>0)
                {
                    continue;
                }

                //Jesli nie bylismy, to spoko
                list_of_visited_nodes.Add(package);
                // produkujemu liste pakietow docelowych z pakietu zrodlowego
                List<PackageType> list_of_dest_packages = new List<PackageType>();
                List<PackageTransformations> list_of_packages = list_of_transformation.FindAll(x => x.source_package.id_of_package == package.id_of_package);
                foreach(PackageTransformations pck_trnsf in list_of_packages)
                {
                    list_of_dest_packages.Add(pck_trnsf.destiny_package);
                }
                //wywolujemy dfs
                result = DFS(searchpackage, package, list_of_dest_packages, list_of_transformation,list_of_visited_nodes);
            }
            return result;
        }
    }
    public class PackageType
    {
        public int id_of_package { get; set; }

        public List<PackageType> list_of_incoming_packages;
        public PackageType()
        {
            list_of_incoming_packages = new List<PackageType>();
        }
        

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

        public ObjectType()
        {
            list_of_incoming_objects = new List<ObjectType>();
        }

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

        public FieldType()
        {
            list_of_incoming_fields = new List<FieldType>();
        }

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

            var csv_package_transformation = new StringBuilder();
            string filePath_pcg_tarnsformation = pathDesktop + "\\table_of_packages_transformation.csv";
            if (!File.Exists(filePath_pcg_tarnsformation))
            {
                File.Create(filePath_pcg_tarnsformation).Close();
            }
            var csv_package_transformation_header = string.Format("id_source_package,id_dest_package,ID_transformation");
            csv_package_transformation.AppendLine(csv_package_transformation_header);

            var csv_object_transformation = new StringBuilder();
            string filePath_obj_tarnsformation = pathDesktop + "\\table_of_objects_transformation.csv";
            if (!File.Exists(filePath_obj_tarnsformation))
            {
                File.Create(filePath_obj_tarnsformation).Close();
            }
            var csv_object_transformation_header = string.Format("id_source_package,id_source_object,id_dest_package,id_dest_object,ID_transformation");
            csv_object_transformation.AppendLine(csv_object_transformation_header);

            var csv_field_transformation = new StringBuilder();
            string filePath_fld_tarnsformation = pathDesktop + "\\table_of_fields_transformation.csv";
            if (!File.Exists(filePath_fld_tarnsformation))
            {
                File.Create(filePath_fld_tarnsformation).Close();
            }
            var csv_field_transformation_header = string.Format("id_source_package,id_source_object,id_source_field,id_dest_package,id_dest_object,id_dest_field,ID_transformation");
            csv_field_transformation.AppendLine(csv_field_transformation_header);

            for (int i = 0; i < number_of_packages; i++)
            {
                //table_of_packagess[i] = i + 1;
                PackageType new_package = new PackageType();
                new_package.id_of_package = i;
                list_of_packages.Add(new_package);
                for (int j = 0; j < av_num_of_obj_per_package.Sample(); j++)
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
                        for (int k = 0; k < av_num_of_fields_of_obj_catA.Sample(); k++)
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
                        for (int k = 0; k < av_num_of_fields_of_obj_catB.Sample(); k++)
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

            Console.WriteLine(list_of_packages.Count);
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
                    List<PackageType> tmp_list_pkg;
                    List<PackageType> temp_list_vst_pcg;
                    do
                    {
                        int random_index_of_package = randomNumber.Next(list_of_packages.Count);
                        Console.WriteLine(random_index_of_package);
                        random_package = list_of_packages[random_index_of_package];
                        Console.WriteLine("Id random_package: {0} ", random_package.id_of_package);
                        tmp_list_pkg = new List<PackageType>();
                        tmp_list_pkg.Add(package_element);
                        temp_list_vst_pcg = new List<PackageType>();
                    } while (package_element.id_of_package == random_package.id_of_package || package_element.list_of_incoming_packages.Contains(random_package) || DFSCLass.DFS(random_package,random_package,tmp_list_pkg,list_of_package_transformations, temp_list_vst_pcg));
                    Console.WriteLine("Numbrers of element in list {0}", package_element.list_of_incoming_packages.Count);
                    package_element.list_of_incoming_packages.Add(random_package);
                    PackageTransformations new_transformation = new PackageTransformations();
                    new_transformation.source_package = random_package;
                    new_transformation.destiny_package = package_element;
                    new_transformation.ID = id_of_transformations;
                    id_of_transformations++;
                    list_of_package_transformations.Add(new_transformation);
                    
                }
                Console.WriteLine("Koniec generowania data_transformations dla pakietow");
                Console.WriteLine(package_element.list_of_incoming_packages.Count);
                foreach(PackageType incoming_package in package_element.list_of_incoming_packages)
                {
                    Console.WriteLine("ID of incomig_package {0}, Id of package_element {1}", incoming_package.id_of_package, package_element.id_of_package);
                    Console.WriteLine("Poczatek generowania data transformation dla obiektow");
                    List<ObjectType> list_of_objects_from_incoming_package = list_of_objects.FindAll(x => x.id_of_package == incoming_package.id_of_package);
                    List<ObjectType> list_of_objects_from_package_element = list_of_objects.FindAll(y => y.id_of_package == package_element.id_of_package);
                    PackageTransformations temp_package = list_of_package_transformations.Find(x => x.source_package == incoming_package && x.destiny_package == package_element);
                    Console.WriteLine(temp_package.ID);
                    foreach (ObjectType object_element in list_of_objects_from_package_element)
                    {

                        for (int j = 0; j < av_num_of_incoming_fields.Sample(); j++)
                        {
                            ObjectType random_object;
                            do
                            {

                                int random_index_of_object = randomNumber.Next(list_of_objects_from_incoming_package.Count);
                                random_object = list_of_objects_from_incoming_package[random_index_of_object];
                                Console.WriteLine("Losowy obiekt id {0}.{1}", random_object.id_of_package,random_object.id_of_object);

                            } while (object_element.list_of_incoming_objects.Contains(random_object));
                            object_element.list_of_incoming_objects.Add(random_object);
                            ObjectTransformations new_transformation = new ObjectTransformations();
                            new_transformation.source_object = random_object;
                            new_transformation.destiny_object = object_element;
                            new_transformation.ID = temp_package.ID;
                            list_of_object_transformations.Add(new_transformation);
                            Console.WriteLine("Po generowaniu obiektu jednego");
                        }

                    }
                    List<FieldType> list_of_fields_from_incoming_package = list_of_fields.FindAll(x => x.id_of_package == incoming_package.id_of_package);
                    List<FieldType> list_of_fields_from_package_element = list_of_fields.FindAll(x => x.id_of_package == package_element.id_of_package);
                    foreach(FieldType field_element in list_of_fields_from_package_element)
                    {

                        for(int j=0; j< av_num_of_incoming_fields.Sample(); j++)
                        {
                            FieldType random_field;
                            do
                            {
                                int random_index_of_field = randomNumber.Next(list_of_fields_from_incoming_package.Count);
                                random_field = list_of_fields_from_incoming_package[random_index_of_field];
                                Console.WriteLine("Losowy obiekt id {0}.{1}.{2}", random_field.id_of_package, random_field.id_of_object, random_field.id_of_field);
                            } while (field_element.list_of_incoming_fields.Contains(random_field));
                            field_element.list_of_incoming_fields.Add(random_field);
                            FieldTransformations new_transformation = new FieldTransformations();
                            new_transformation.source_field = random_field;
                            new_transformation.destiny_field = field_element;
                            new_transformation.ID = temp_package.ID;
                            list_of_field_transformations.Add(new_transformation);
                            Console.WriteLine("po generowaniu pola jednego");


                        }

                    }

                }

            }
            Console.WriteLine("Generowanie cykli");
            //Generowanie cykli
            int number_of_edges = list_of_field_transformations.Count + list_of_object_transformations.Count + list_of_package_transformations.Count;
            int number_of_single_edge_cycles = Convert.ToInt32(number_of_edges * single_cycle_edge);
            int number_of_two_edge_cycles = Convert.ToInt32(number_of_edges * double_cycle_edge);
            //Generowanie cykli jednoelementowych
            for(int i=0; i<number_of_single_edge_cycles; i++)
            {
                //Wybieram losowy pakiet, który nie ma jeszcze cyklu pojedynczego
                PackageTransformations temp_transformation;
                int random_number;
                do
                {
                    random_number = randomNumber.Next(list_of_package_transformations.Count);
                    temp_transformation = list_of_package_transformations[random_number];
                    //  BŁAD - ZNALAZLEM DWA CYKLE JEDNOELEMENTOWE DO JEDNEGO PAKIETU - CZY JEST TO OK?? (MODYFIKACJA PO &&)
                } while (temp_transformation.destiny_package.id_of_package == temp_transformation.source_package.id_of_package && list_of_package_transformations.FindAll(x => x.source_package.id_of_package == temp_transformation.source_package.id_of_package && x.destiny_package.id_of_package == temp_transformation.source_package.id_of_package)!=null);
                
                //Tworze cykl, czyli wezel docelowy to wezel zrodlowy
                list_of_package_transformations[random_number].destiny_package = temp_transformation.source_package;
                //Pobieram informacje o ID transformacji, w celu pozniejszego pobrania transformacji na poziomie obiektu i pol o takim samym ID
                int ID_of_transformation = temp_transformation.ID;
                List<ObjectTransformations> temp_object_transformations = list_of_object_transformations.FindAll(x => x.ID == ID_of_transformation);
                List<FieldTransformations> temp_field_transformations = list_of_field_transformations.FindAll(x => x.ID == ID_of_transformation);
                Console.WriteLine("Cykl - pakiet");
                //Transformacje na poziomie obiektu
                foreach(ObjectTransformations obj_transformation in temp_object_transformations)
                {
                    if (i >= number_of_single_edge_cycles)
                        break;                    
                    Console.WriteLine("Cykl - obiekt");
                    //Jest cykl, modyfikuje wpis w liscie transformacji
                    obj_transformation.destiny_object = obj_transformation.source_object;
                    i++;
                    
                }
                //Transformacje na poziomie pola
                foreach(FieldTransformations fld_transformation in temp_field_transformations)
                {
                    if (i >= number_of_single_edge_cycles)
                        break;
                    
                    Console.WriteLine("Cykl - pole");
                    //Jest cykl na poziomie pola, modyfikuje wpis w liscie transformacji
                    fld_transformation.destiny_field = fld_transformation.source_field;
                    i++;
                    
                 
                }
            }


            //Generowanie cykli dwuelementowych
            //Ograniczam liste krawedzi( par wezlow zrodlo - cel ) do tych, ktore sa rozne( odrzucam cykle jednoelementowe )
           
            for(int i=0; i<number_of_two_edge_cycles; i++)
            {
                int random_number;
                PackageTransformations temp_transformations;
                int number_of_reverse_transformations;
                int number_of_source_packages_from_destiny_package;
                // Wybieramy transformacje, ktora nie posiada jeszcze cyklu;
                List<PackageType> tmp_list_pkg;
                List<PackageType> temp_list_vst_pcg;
                bool dfs_flag;
                do
                {
                    random_number = randomNumber.Next(list_of_package_transformations.Count);
                    temp_transformations = list_of_package_transformations[random_number];
                    //Sprawdzam, czy aby nie wybrałem transformacji będącej w cyklu           
                    number_of_reverse_transformations = list_of_package_transformations.FindAll(x => x.destiny_package.id_of_package == temp_transformations.source_package.id_of_package && x.source_package.id_of_package == temp_transformations.destiny_package.id_of_package).Count;
                    //Sprawdzam, czy mam z czegu usunac krawedz
                    number_of_source_packages_from_destiny_package = list_of_package_transformations.FindAll(x => x.destiny_package == temp_transformations.destiny_package ).Count;
                    
                    tmp_list_pkg = new List<PackageType>();
                    tmp_list_pkg.Add(temp_transformations.source_package);
                    dfs_flag = false;
                    
                    temp_list_vst_pcg = new List<PackageType>();
                    if (temp_transformations.source_package.id_of_package != temp_transformations.destiny_package.id_of_package)
                       dfs_flag = DFSCLass.DFS(temp_transformations.destiny_package, temp_transformations.destiny_package, tmp_list_pkg, list_of_package_transformations, temp_list_vst_pcg);
                    Console.WriteLine("PETLA W GENEROWANIU CYKLI PODWOJNYCH");
                    Console.WriteLine("dfs_flag {0}", dfs_flag);
                    Console.WriteLine("number_of_reverse_transformations {0}", number_of_reverse_transformations);
                    Console.WriteLine("number_of_source packages from dsetine {0}", number_of_source_packages_from_destiny_package);
                    Console.WriteLine(temp_transformations.source_package.id_of_package);
                    Console.WriteLine(temp_transformations.destiny_package.id_of_package);
                } while (number_of_reverse_transformations > 0 || number_of_source_packages_from_destiny_package < 2 || temp_transformations.source_package.id_of_package == temp_transformations.destiny_package.id_of_package || dfs_flag);
                //Jest wybrana transformacja. Bez cykli jednoelementowych oraz nie posiadajaca cyklu dwuelementowego

                PackageType source_package = temp_transformations.source_package;
                PackageType destiny_package = temp_transformations.destiny_package;
                // Wybieram transformację z której usunę krawędź
                List<PackageTransformations> list_of_package_transformations_from_destiny_package = list_of_package_transformations.FindAll(x => x.destiny_package == destiny_package && x.source_package != source_package);
                int random_nmb = randomNumber.Next(list_of_package_transformations_from_destiny_package.Count);
                PackageTransformations random_transformationn = list_of_package_transformations_from_destiny_package[random_nmb];
                destiny_package.list_of_incoming_packages.Remove(random_transformationn.source_package);
                int ID_of_transformation = random_transformationn.ID;
                //Usuwam transformacje na nizszych poziomach i na poziomie pakietu
                list_of_field_transformations.RemoveAll(x => x.destiny_field.id_of_package == destiny_package.id_of_package && x.source_field.id_of_package == random_transformationn.source_package.id_of_package);
                list_of_object_transformations.RemoveAll(x => x.destiny_object.id_of_package == destiny_package.id_of_package && x.source_object.id_of_package == random_transformationn.source_package.id_of_package);
                list_of_package_transformations.RemoveAll(x => x.destiny_package.id_of_package == destiny_package.id_of_package && x.source_package.id_of_package == random_transformationn.source_package.id_of_package);
                //Tworzenie nowej transformacji na poziomie pakietu
                source_package.list_of_incoming_packages.Add(destiny_package);
                PackageTransformations new_transformation = new PackageTransformations();
                new_transformation.destiny_package = source_package;
                new_transformation.source_package = destiny_package;
                new_transformation.ID = ID_of_transformation;
                //Tworzenie transformacji na poziomie obiektów
                List<ObjectTransformations> temp_object_transformations = list_of_object_transformations.FindAll(x => x.ID == temp_transformations.ID);
                List<FieldTransformations> temp_field_transformations = list_of_field_transformations.FindAll(x => x.ID == temp_transformations.ID);
                foreach (ObjectTransformations obj_transformation in temp_object_transformations)
                {
                    if (i >= number_of_two_edge_cycles)
                        break;
                    Console.WriteLine("Cykl dwuelementowy - obiekt");
                    ObjectTransformations new_object_transformation = new ObjectTransformations();
                    new_object_transformation.source_object = obj_transformation.destiny_object;
                    new_object_transformation.destiny_object = obj_transformation.source_object;
                    new_object_transformation.ID = ID_of_transformation;
                    list_of_object_transformations.Add(new_object_transformation);                
                    
                    i++;

                }
                //Tworzenie transformacji na poziomie pol
                foreach (FieldTransformations fld_transformation in temp_field_transformations)
                {
                    if (i >= number_of_two_edge_cycles)
                        break;
                    Console.WriteLine("Cykl dwuelementowy - obiekt");
                    FieldTransformations new_field_transformation = new FieldTransformations();
                    new_field_transformation.source_field = fld_transformation.destiny_field;
                    new_field_transformation.destiny_field = fld_transformation.source_field;
                    new_field_transformation.ID = ID_of_transformation;
                    list_of_field_transformations.Add(new_field_transformation);

                    i++;

                }
                Console.WriteLine("Koniec generowania cyklu dla danego pakietu");
            }


            foreach(PackageTransformations packg_transformation in list_of_package_transformations)
            {
                var newLineA = string.Format("{0},{1},{2}", packg_transformation.source_package.id_of_package, packg_transformation.destiny_package.id_of_package, packg_transformation.ID);
                csv_package_transformation.AppendLine(newLineA);
            }

            foreach (ObjectTransformations objc_transformation in list_of_object_transformations)
            {
                var newLineA = string.Format("{0},{1},{2},{3},{4}", objc_transformation.source_object.id_of_package, objc_transformation.source_object.id_of_object, objc_transformation.destiny_object.id_of_package, objc_transformation.destiny_object.id_of_object, objc_transformation.ID);
                csv_object_transformation.AppendLine(newLineA);
            }

            foreach(FieldTransformations flds_transformation in list_of_field_transformations)
            {
                var newlineA = string.Format("{0},{1},{2},{3},{4},{5},{6}", flds_transformation.source_field.id_of_package, flds_transformation.source_field.id_of_object, flds_transformation.source_field.id_of_field, flds_transformation.destiny_field.id_of_package,flds_transformation.destiny_field.id_of_object,flds_transformation.destiny_field.id_of_field,flds_transformation.ID);
                csv_field_transformation.AppendLine(newlineA);
            }


            File.WriteAllText(filePath_pcg_to_obj, csv.ToString());
            File.WriteAllText(filePath_pcg_to_obj_fields, csv2.ToString());
            File.WriteAllText(filePath_pcg_tarnsformation, csv_package_transformation.ToString());
            File.WriteAllText(filePath_obj_tarnsformation, csv_object_transformation.ToString());
            File.WriteAllText(filePath_fld_tarnsformation, csv_field_transformation.ToString());
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
