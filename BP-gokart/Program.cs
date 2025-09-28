using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BP_gokart
{
    #region 1. feladat: Helyszín/Céginfók
    // 1. feladat: Helyszín
    public class Helyszin
    {
        private string nev;
        private string cim;
        private string telefon;
        private string domain;

        public Helyszin(string nev, string cim, string telefon, string domain)
        {
            this.nev = nev;
            this.cim = cim;
            this.telefon = telefon;
            this.domain = domain;
        }

        public void kiiratas()
        {
            Console.WriteLine($"Név: {nev}|");
            Console.WriteLine($"Cím: {cim}|");
            Console.WriteLine($"Telefon: {telefon}|");
            Console.WriteLine($"Domain: {domain}|");
            Console.WriteLine("-------------------------------------------");
        }
    }

    public class Versenyzok
    {
        private static List<string> veznevek;   // Statikus listák – fájlokból csak egyszer olvassuk be
        private static List<string> kernevek;

        public string veznev;
        public string kernev;
        public DateTime szuldatum;
        public bool felnott;
        public string azon;
        public string email;

        private static Random rnd = new Random();

        public static void nev_beolvasas()
        {
            veznevek = File.ReadAllText("vezeteknevek.txt").Replace("'", "").Split(',').Select(v => v.Trim()).ToList();     // Select kell, hogy a szóközöket is eltávolítsa

            kernevek = File.ReadAllText("keresztnevek.txt").Replace("'", "").Split(',').Select(k => k.Trim()).ToList();     // Select kell, hogy a szóközöket is eltávolítsa
        }

        // Versenyző példány konstruktora
        public Versenyzok(int id)
        {
            veznev = veznevek[rnd.Next(veznevek.Count)];
            kernev = kernevek[rnd.Next(kernevek.Count)];

            int year = rnd.Next(1953, 2014);
            int month = rnd.Next(1, 13);
            int day = rnd.Next(1, DateTime.DaysInMonth(year, month) + 1);
            szuldatum = new DateTime(year, month, day);

            int age = DateTime.Now.Year - szuldatum.Year;
            felnott = szuldatum.AddYears(18) <= DateTime.Now;

            string teljesnev = $"{veznev}{kernev}";
            string szuldatum_str = szuldatum.ToString("yyyyMMdd");
            azon = $"GO-{teljesnev}-{szuldatum_str}";

            email = $"{veznev.ToLower()}.{kernev.ToLower()}@gmail.com";
        }

        public void Kiir()
        {
            Console.WriteLine($"{veznev} {kernev}|Születési dátum:{szuldatum:yyyy.MM.dd.}|Felnőtt:{(felnott ? "Igen" : "Nem")}|Azonosító:{azon}|Email:{email}");
        }
    }
        #endregion
        internal class Program
    {
        #region fejlec
        static void fejlec()
        {
            /*
             BP - Gokart
             BP - 09.15.-09.28.
            */

            Type type = typeof(Program);
            string namespaceName = type.Namespace;
            Console.WriteLine(namespaceName);
            for (int i = 0; i < namespaceName.Length; i++) Console.Write('-');
            Console.WriteLine();
        }
        #endregion
        public static void Main(string[] args)
        {
            Helyszin ceg = new Helyszin("BP - Gokart", "6969 Bivalybasznád, Gyárkémény út 18.", "+36 1 209 09 09", "www.bpgokart.hu");

            ceg.kiiratas();
            Console.WriteLine();
            Console.WriteLine("Üdvözöljük a Bp - Gokart foglalási naplójában!");
            Console.WriteLine();

            Versenyzok.nev_beolvasas();

            List<Versenyzok> versenyzok = new List<Versenyzok>();
            Random rnd = new Random();
            int darab = rnd.Next(1, 151);

            for (int i = 1; i <= darab; i++)
            {
                versenyzok.Add(new Versenyzok(i));
            }

            /*
            foreach (var v in versenyzok)
            {
                v.Kiir();
            }
            */

            Console.WriteLine($"Eddig {darab} versenyző regisztrált be az összes adatával a foglalási naplóba.");

            Console.WriteLine();
            Console.WriteLine("Kilépéshez nyomja meg az ENTER billentyűt.");
            Console.ReadLine();
        }
    }
}
