using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
        public string vazonosito;
        public string emailcim;
        public gokart(string vnev, string knev, DateTime szido, bool felnotte, string vazonosito, string emailcim)
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
        public static string Ekezetmentesit(string input)
        {
            string normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char c in normalized)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            /*
            Bevezető
            MG 2025.09.15
            */
            string fejlec = "天井-GoKart";
            Console.WriteLine(fejlec);

            for (int i = 0; i < fejlec.Length; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
            List<gokart> versenyzok = new List<gokart>();
            Dictionary<DateTime, ArrayList> idopontok = new Dictionary<DateTime, ArrayList>();
            List<string> palya = new List<string>() { "天井-GoKart", "8879 Kerkateskánd, Csavargyár utca 56.",  "0692697752", "jovalasztas.hu"};
            StreamReader knev = new StreamReader("../../keresztnevek.txt");
            StreamReader vnev = new StreamReader("../../vezeteknevek.txt");
            List<string> keresztnev  = new List<string>();
            List<string> vezeteknev  = new List<string>();
            string knevline = knev.ReadLine();
            keresztnev = knevline.Split(',').ToList();
            string vnevline = vnev.ReadLine();
            vezeteknev = vnevline.Split(',').ToList();
            DateTime now = DateTime.Now;
            DateTime idopont = new DateTime(now.Year, now.Month, now.Day, 08, 00, 00);
            Random rnd = new Random();
            while (idopont.Day < DateTime.DaysInMonth(now.Year, now.Month))
            {
                Console.WriteLine(idopont);
                idopontok.Add(idopont, new ArrayList());
                idopont = idopont.AddHours(1);
                if (idopont.Hour >= 19)
                {
                    idopont.AddDays(1);
                    idopont = new DateTime(idopont.Year, idopont.Month, idopont.Day+1, 08, 00, 00);
                    continue;
                }
            }
            for (int i = 0; i < rnd.Next(1,151); i++)
            {
                int honap = rnd.Next(1, 13);
                DateTime ev = new DateTime(rnd.Next(1925, 2026), 01, 01);
                DateTime szulido = new DateTime(ev.Year, honap, rnd.Next(1, DateTime.DaysInMonth(ev.Year, honap)));
                Console.WriteLine(szulido);
                bool felnotte = (DateTime.Now - szulido).TotalDays >= 18;
                string mentesknev = Ekezetmentesit(keresztnev[rnd.Next(0, keresztnev.Count)]).Replace("'", "").Trim();
                string mentesvnev = Ekezetmentesit(vezeteknev[rnd.Next(0, vezeteknev.Count)]).Replace("'", "").Trim();
                versenyzok.Add(new gokart(mentesvnev, mentesknev, szulido, felnotte, $"GO-{mentesvnev}{mentesknev}-{Convert.ToString(szulido.Year)}{Convert.ToString(szulido.Month)}{Convert.ToString(szulido.Day)}", $"{(mentesvnev+"."+mentesknev).ToLower()}@gmail.com"));
            }

            foreach (var item in versenyzok)
            {
                Console.WriteLine(item.vazonosito);
            }
            
            Console.WriteLine();
            Console.WriteLine("Nyomja meg az ENTER-t a kilépéshez");
            Console.ReadLine();

        }
    }
}
