using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
namespace Data_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            int number_of_packages = 500;
            Binomial av_num_of_obj_per_package = new Binomial(0.0125, 8000);
            Binomial av_num_of_fields_of_obj_catA = new Binomial(0.02, 1500);
            Binomial av_num_of_fields_of_obj_catB = new Binomial(0.02, 400);
            Binomial av_num_of_incoming_fields = new Binomial(0.03125, 80);
            int sum_av_num_of_obj_per_package=0;
            int sum_av_num_of_fields_of_obj_catA=0;
            int sum_av_num_of_fields_of_obj_catB=0;
            int sum_av_num_of_incoming_fields=0;
            for (int i=0; i<number_of_packages; i++)
            {
                sum_av_num_of_obj_per_package += av_num_of_obj_per_package.Sample();
                sum_av_num_of_fields_of_obj_catA += av_num_of_fields_of_obj_catA.Sample();
                sum_av_num_of_fields_of_obj_catB += av_num_of_fields_of_obj_catB.Sample();
                sum_av_num_of_incoming_fields += av_num_of_incoming_fields.Sample();
                 
            }
            Console.WriteLine("Srednia liczba obiektów w pakiecie:" + sum_av_num_of_obj_per_package / number_of_packages);
            Console.WriteLine("Srednia liczba obiektów kategoria A:" + sum_av_num_of_fields_of_obj_catA / number_of_packages);
            Console.WriteLine("Srednia liczba obiektów kategoria B:" + sum_av_num_of_fields_of_obj_catB / number_of_packages);
            Console.WriteLine("Srednia liczba przychodzących pól:" + sum_av_num_of_incoming_fields / number_of_packages);

            Console.ReadLine();
        }
    }
}
