using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;

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

    #endregion

    #region 2. feladat: Versenyzők generálása
    // 2. feladat: Versenyzők generálása
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

    #region 3. feladat: Pályabérlés szabályai, foglalások időpontjai
    // 3. feladat: Pályabérlés szabályai, foglalások időpontjai
    public class Foglalasok
    {
        public Versenyzok versenyzo;
        public DateTime kezdes;
        public int idotartam;
        public DateTime vege => kezdes.AddHours(idotartam);

        private static Random rnd = new Random();

        public Foglalasok(Versenyzok v)
        {
            versenyzo = v;
            idotartam = rnd.Next(1, 2 + 1);
            
            DateTime kezdo_nap = DateTime.Today;
            DateTime zaro_nap = new DateTime(2025, 12, 31);
            int napok_szama = (zaro_nap - kezdo_nap).Days;
            DateTime randomnap = kezdo_nap.AddDays(rnd.Next(napok_szama + 1));

            int ora = rnd.Next(8, 19 + 1) - idotartam;  // 8 és 19 óra között kezdődhet, de figyelembe kell venni az időtartamot is

            kezdes = new DateTime(randomnap.Year, randomnap.Month, randomnap.Day, ora, 0, 0);
        }

        public void Kiir()
        {
            Console.WriteLine($"{versenyzo.veznev} {versenyzo.kernev}|{kezdes:yyyy.MM.dd. HH:mm} - {vege:HH:mm}|{idotartam} órás foglalás");
        }

    }
    #endregion

    #region 4. feladat: Hónap végéig fennmaradó napok foglalási szalagja
    // 4. feladat: Hónap végéig fennmaradó napok foglalási szalagja
    public class Idoszalag
    {
        private int ora_kezdet = 8;
        private int ora_vege = 19 - 1;  //mert 19 ig lehet foglalni, tehát 18-19-ig tart a legutolsó foglalás

        // Minden időpont szabad (false: szabad, true: foglalt)
        private bool[,] idopontok;

        private DateTime kezdodatum;
        private int napok_szama;

        public Idoszalag(DateTime kezdodatum)
        {
            this.kezdodatum = kezdodatum;
            int honap_napjai = DateTime.DaysInMonth(kezdodatum.Year, kezdodatum.Month);
            napok_szama = honap_napjai - kezdodatum.Day + 1;
            idopontok = new bool[napok_szama, ora_vege - ora_kezdet + 1];
        }

        public void FeltoltFoglalasokkal(List<Foglalasok> foglalasok)
        {
            foreach (var f in foglalasok)
            {
                // Csak az aktuális hónapban lévő foglalásokat vesszük figyelembe
                if (f.kezdes.Month == kezdodatum.Month && f.kezdes.Year == kezdodatum.Year && f.kezdes.Day >= kezdodatum.Day)
                {
                    int nap_index = f.kezdes.Day - kezdodatum.Day;
                    int ora_index_kezd = f.kezdes.Hour - ora_kezdet;
                    for (int i = 0; i < f.idotartam; i++)
                    {
                        int ora_index = ora_index_kezd + i;
                        if (nap_index >= 0 && nap_index < napok_szama && ora_index >= 0 && ora_index <= ora_vege - ora_kezdet)
                        {
                            idopontok[nap_index, ora_index] = true;
                        }
                    }
                }
            }
        }

        public void Megjelenit()
        {
            Console.Write("             ");
            for (int ora = ora_kezdet; ora <= ora_vege; ora++)
                Console.Write($"{ora}-{ora + 1} ");
            Console.WriteLine();

            for (int nap = 0; nap < napok_szama; nap++)
            {
                DateTime aktnap = kezdodatum.AddDays(nap);
                Console.Write($"{aktnap:yyyy.MM.dd} ");

                for (int ora = 0; ora <= ora_vege - ora_kezdet; ora++)
                {
                    if (idopontok[nap, ora])
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("   $  ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("   #  ");
                    }
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
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
             BP - 09.15.-10.05.
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

            foreach (var v in versenyzok)
            {
                v.Kiir();
            }
            
            Console.WriteLine();
            Console.WriteLine($"Eddig {darab} versenyző regisztrált be az összes adatával a foglalási naplóba.");

            List<Foglalasok> foglalasok = new List<Foglalasok>();

            foreach (var v in versenyzok)
            {
                foglalasok.Add(new Foglalasok(v));
            }

            Console.WriteLine();
            Console.WriteLine("Foglalásaik:");
            foreach (var f in foglalasok)
            {
                f.Kiir();
            }

            Console.WriteLine();
            Console.WriteLine("Időszalag a hónap végéig:");
            var idoszalag = new BP_gokart.Idoszalag(DateTime.Today);
            idoszalag.FeltoltFoglalasokkal(foglalasok);
            idoszalag.Megjelenit();

            Console.WriteLine();
            Console.WriteLine("Kilépéshez nyomja meg az ENTER billentyűt.");
            Console.ReadLine();
        }
    }
}
