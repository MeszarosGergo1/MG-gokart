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

        static void kiiratas(Dictionary<List<DateTime>, List<gokart>> idopontok)
        {
            for (int i = 0; i < idopontok.Keys.ToList()[0][idopontok.Keys.ToList()[0].Count - 1].ToString("yyyy.MM.dd").Length; i++)
            {
                Console.Write(" ");
            }
            Console.Write(" |");
            for (int i = 0; i < idopontok.Keys.ToList()[0].Count; i++)
            {
                Console.Write($" {idopontok.Keys.ToList()[0][i].Hour}-{idopontok.Keys.ToList()[0][i].Hour + 1} |");
            }
            Console.WriteLine();
            for (int i = 0; i < idopontok.Keys.Count; i++)
            {
                Console.WriteLine(idopontok.Keys.ToList()[i][0].ToString("yyyy.MM.dd"));
            }
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
            Dictionary<List<DateTime>, List<gokart>> idopontok = new Dictionary<List<DateTime>, List<gokart>>();
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
            while (idopont.Day <= DateTime.DaysInMonth(now.Year, now.Month) && idopont.Month == now.Month)
            {
                List<DateTime> ideig = new List<DateTime>();
                while(idopont.Hour <= 18)
                {
                    ideig.Add(idopont);
                    idopont = idopont.AddHours(1);  
                    continue;
                }
                idopont = idopont.AddDays(1);
                idopont = new DateTime(idopont.Year, idopont.Month, idopont.Day, 08, 00, 00);   
                idopontok.Add(ideig, new List<gokart>());
            }
            for (int i = 0; i < rnd.Next(1,151); i++)
            {
                int honap = rnd.Next(1, 13);
                DateTime ev = new DateTime(rnd.Next(1925, 2026), 01, 01);
                DateTime szulido = new DateTime(ev.Year, honap, rnd.Next(1, DateTime.DaysInMonth(ev.Year, honap)));
                bool felnotte = (DateTime.Now - szulido).TotalDays >= 18;
                string mentesknev = Ekezetmentesit(keresztnev[rnd.Next(0, keresztnev.Count)]).Replace("'", "").Trim();
                string mentesvnev = Ekezetmentesit(vezeteknev[rnd.Next(0, vezeteknev.Count)]).Replace("'", "").Trim();
                versenyzok.Add(new gokart(mentesvnev, mentesknev, szulido, felnotte, $"GO-{mentesvnev}{mentesknev}-{Convert.ToString(szulido.Year)}{Convert.ToString(szulido.Month)}{Convert.ToString(szulido.Day)}", $"{(mentesvnev+"."+mentesknev).ToLower()}@gmail.com"));
            }

            int hatralevo_foglalasok = idopontok.Count;

            while(hatralevo_foglalasok > 0)
            {
                List<gokart> ideig = new List<gokart>();
                if (hatralevo_foglalasok >= 20)
                {
                    int foglalas = rnd.Next(8, hatralevo_foglalasok + 1);
                    for (int i = 0; i < foglalas; i++)
                    {
                        ideig.Add(versenyzok[rnd.Next(0, versenyzok.Count)]);
                    }
                    idopontok[idopontok.Keys.ToList()[rnd.Next(0, idopontok.Keys.ToList().Count)]] = ideig;
                    hatralevo_foglalasok -= foglalas;
                    continue;
                }
                if (hatralevo_foglalasok >= 8 && hatralevo_foglalasok <20) 
                {
                    int foglalas = rnd.Next(8,hatralevo_foglalasok+1);
                    for (int i = 0; i < foglalas; i++)
                    {
                        ideig.Add(versenyzok[rnd.Next(0, versenyzok.Count)]);
                    }
                    idopontok[idopontok.Keys.ToList()[rnd.Next(0,   idopontok.Keys.ToList().Count)]] = ideig;
                    hatralevo_foglalasok -= foglalas;
                    continue;
                }
                else
                {
                    int foglalas = rnd.Next(1, hatralevo_foglalasok + 1);
                    for (int i = 0; i < foglalas; i++)
                    {
                        ideig.Add(versenyzok[rnd.Next(0, versenyzok.Count)]);
                    }
                    idopontok[idopontok.Keys.ToList()[rnd.Next(0, idopontok.Keys.ToList().Count)]] = ideig;
                    hatralevo_foglalasok -= foglalas;
                    continue;
                }
            }
            /*
            while (hatralevo_foglalasok >= 8)
            {
                int foglalas = rnd.Next(8, 21);

            }
            */
            kiiratas(idopontok);

            Console.WriteLine();
            Console.WriteLine("Nyomja meg az ENTER-t a kilépéshez");
            Console.ReadLine();

        }
    }
}
