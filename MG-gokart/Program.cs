using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG_gokart
{
    class gokart
    {
        // (vezetéknév, keresztnévnév, születési idő, 18+(i/N), versenyzo azonosito, emailcim)
        public string vnev;
        public string knev;
        public DateTime szido;
        public bool felnotte;
        public int vazonosito;
        public string emailcim;
        public gokart(string vnev, string knev, DateTime szido, bool felnotte, int vazonosito, string emailcim)
        {
            this.vnev = vnev;
            this.knev = knev;
            this.szido = szido;
            this.felnotte = felnotte;
            this.vazonosito = vazonosito;
            this.emailcim = emailcim;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {

            /*
            Bevezető
            MG 2025.09.15
            */
            string fejlec = "Bevezető";
            Console.WriteLine(fejlec);

            for (int i = 0; i < fejlec.Length; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();


            Console.WriteLine();
            Console.WriteLine("Nyomja meg az ENTER-t a kilépéshez");
            Console.ReadLine();

        }
    }
}
