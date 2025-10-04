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

        static void kiiratas(Dictionary<DateTime, Dictionary<DateTime, List<gokart>>> idopontok)
        {
            for (int i = 0; i < idopontok.Keys.ToList()[0].ToString("yyyy.MM.dd").Length+1; i++)
            {
                Console.Write(" ");
            }
            Console.Write("|");
            for (int i = 0; i < idopontok.Values.ToList()[0].Keys.ToList().Count; i++)
            {
                Console.Write($"{$"   {idopontok.Values.ToList()[0].Keys.ToList()[i].Hour}-{idopontok.Values.ToList()[0].Keys.ToList()[i].Hour + 1} ", -10} |");
            }
            Console.WriteLine();
            for(int i = 0; i < (idopontok.Values.ToList()[0].Keys.ToList().Count+1)*10 + (idopontok.Keys.ToList()[0].ToString("yyyy.MM.dd").Length + 2)*2; i++)
                Console.Write("-");
            Console.WriteLine();
            //Console.Write($"{idopontok.Keys.ToList()}");
            for (int i = 0; i < idopontok.Keys.ToList().Count; i++)
            {
                Console.Write(idopontok.Keys.ToList()[i].ToString("yyyy.MM.dd") + " |");
                for (int j = 0; j < idopontok[idopontok.Keys.ToList()[i]].Count; j++)
                {
                    Console.Write(" ");
                    if (idopontok[idopontok.Keys.ToList()[i]].Values.ToList()[j].Count == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        for (int k = 0; k < 9; k++)
                        {
                            Console.Write(" ");
                        }
                        Console.ResetColor();
                        Console.Write(" |");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        for (int k = 0; k < 9; k++)
                        {
                            Console.Write(" ");
                        }
                        Console.ResetColor();
                        Console.Write(" |");
                    }
                }
                Console.WriteLine();
                for (int asd = 0; asd < (idopontok.Values.ToList()[0].Keys.ToList().Count + 1) * 10 + (idopontok.Keys.ToList()[0].ToString("yyyy.MM.dd").Length + 2) * 2; asd++)
                    Console.Write("-");
                Console.WriteLine();
            }
            
        }
        static Dictionary<DateTime, Dictionary<DateTime, List<gokart>>> modositas(Dictionary<DateTime, Dictionary<DateTime, List<gokart>>> idopontok)
        {
            Dictionary <DateTime, Dictionary<DateTime, List<gokart>>> modositott = idopontok;
            Dictionary<gokart, List<DateTime>> kiosztott_idopontok = new Dictionary<gokart, List<DateTime>>();
            foreach (var konyvtar in modositott.Values)
            {
                foreach (var innerkonyvtar in konyvtar)
                {
                    if(innerkonyvtar.Value.Count > 0)
                    {
                        if (!kiosztott_idopontok.ContainsKey(innerkonyvtar.Value[0]))
                        { 
                            kiosztott_idopontok.Add(innerkonyvtar.Value[0], new List<DateTime>() { innerkonyvtar.Key });
                        }
                        else
                        {
                            kiosztott_idopontok[innerkonyvtar.Value[0]].Add(innerkonyvtar.Key);
                        }
                    }
                }
            }
            foreach (var versenyzo in kiosztott_idopontok)
            {
                foreach (var idopont in versenyzo.Value)
                {
                    if (versenyzo.Value.Count > 1)
                    {
                        Console.WriteLine($"{versenyzo.Key.vazonosito} {idopont.ToString("yyyy.MM.dd")} {idopont.Hour}-{idopont.Hour + 2}");
                        break;
                    }
                    else
                        Console.WriteLine($"{versenyzo.Key.vazonosito} {idopont.ToString("yyyy.MM.dd")} {idopont.Hour}-{idopont.Hour + 1}");
                }
            }
            return modositott;
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
            Dictionary<DateTime, Dictionary<DateTime, List<gokart>>> idopontok = new Dictionary<DateTime, Dictionary<DateTime, List<gokart>>>();
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
                Dictionary<DateTime, List<gokart>> asd1 = new Dictionary<DateTime, List<gokart>>();
                while(idopont.Hour <= 18)
                {
                    DateTime asd = idopont;
                    asd1.Add(asd, new List<gokart>());
                    idopont = idopont.AddHours(1);                  
                }
                idopontok.Add(idopont, asd1);
                idopont = idopont.AddDays(1);
                idopont = new DateTime(idopont.Year, idopont.Month, idopont.Day, 08, 00, 00);   
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

            int hatralevo_foglalasok = idopontok.Keys.Count * idopontok.Values.ToList()[0].Values.Count;
            Console.WriteLine(hatralevo_foglalasok);
            Console.WriteLine(versenyzok.Count);
            while(hatralevo_foglalasok > 0 && versenyzok.Count > 0)
            {
                int foglalas = 1;
                List<gokart> ideiglenes = new List<gokart>();
                if (versenyzok.Count > 1)
                   foglalas = rnd.Next(1, 3);
                int v = rnd.Next(0, versenyzok.Count);
                Console.WriteLine($"{v}/{versenyzok.Count}");
                ideiglenes.Add(versenyzok[v]);
                versenyzok.RemoveAt(v);
                
                int randomdateint = rnd.Next(0, idopontok.Keys.ToList().Count);
                int randomtimeint = rnd.Next(0, idopontok[idopontok.Keys.ToList()[randomdateint]].Keys.ToList().Count);
                DateTime randomdate = idopontok.Keys.ToList()[randomdateint];
                DateTime randomtime = idopontok[idopontok.Keys.ToList()[randomdateint]].Keys.ToList()[randomtimeint];
                
                if(foglalas>1 && randomtime.Hour!=18)
                {
                    if (idopontok[randomdate][randomtime].Count == 0)
                    {
                        idopontok[randomdate][randomtime] = ideiglenes;
                        idopontok[randomdate][idopontok[idopontok.Keys.ToList()[randomdateint]].Keys.ToList()[randomtimeint + 1]] = ideiglenes;
                        hatralevo_foglalasok -= foglalas;
                    }
                    else
                        continue;
                }
                else
                {
                    if (idopontok[randomdate][randomtime].Count == 0)
                    {
                        idopontok[randomdate][randomtime] = ideiglenes;
                        hatralevo_foglalasok -= foglalas;
                    }
                    else
                        continue;
                }
            }

            kiiratas(idopontok);
            Console.Write("Szeretne adatokat módosítani?[i/N]: ");
            string input = Console.ReadLine();
            Console.Clear();
            Console.WriteLine();
            if (input.ToLower() == "i" || input.ToLower() == "igen")
            {
                idopontok = modositas(idopontok);
                Console.WriteLine();
                kiiratas(idopontok);
            }

            Console.WriteLine();
            Console.WriteLine("Nyomja meg az ENTER-t a kilépéshez");
            Console.ReadLine();

        }
    }
}
