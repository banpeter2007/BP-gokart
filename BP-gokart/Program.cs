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

        private static string ekezet_eltavolitas(string szoveg)
        {
            string ekezetes = "áéíóöőúüűÁÉÍÓÖŐÚÜŰ";
            string normal = "aeiooouuuAEIOOOUUU";
            string eredmeny = "";

            foreach (char c in szoveg)
            {
                int index = ekezetes.IndexOf(c);
                if (index >= 0)
                    eredmeny += normal[index];
                else
                    eredmeny += c;
            }

            return eredmeny;
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
            string teljesnev_ekezet_nelkul = ekezet_eltavolitas(teljesnev);
            string szuldatum_str = szuldatum.ToString("yyyyMMdd");
            azon = $"GO-{teljesnev_ekezet_nelkul}-{szuldatum_str}";

            email = $"{ekezet_eltavolitas(veznev).ToLower()}.{ekezet_eltavolitas(kernev).ToLower()}@gmail.com";
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

        #region 5. feladat: Manuális időpontfoglalás átállító
        // 5. feladat: Manuális időpontfoglalás átállító
        static Versenyzok versenyzo_kereso(List<Versenyzok> versenyzok, string azon)
        {
            return versenyzok.FirstOrDefault(v => v.azon == azon);
        }

        static void foglalas_atallito(List<Foglalasok> foglalasok, Versenyzok versenyzo)
        {
            var foglalas = foglalasok.FirstOrDefault(f => f.versenyzo == versenyzo);
            if (foglalas == null)
            {
                Console.WriteLine("Ehhez a versenyzőhöz nincs foglalás!");
                return;
            }

            Console.Write("Adja meg az új dátumot: ");
            string datum = Console.ReadLine();
            Console.Write("Adja meg az új kezdési órát: ");
            int ora = int.Parse(Console.ReadLine());
            Console.Write("Adja meg az időtartamot: ");
            int idotartam = int.Parse(Console.ReadLine());
            int honap = int.Parse(datum.Split('.')[0]);
            int nap = int.Parse(datum.Split('.')[1]);

            DateTime uj_kezdes = new DateTime(DateTime.Today.Year, honap, nap, ora, 0, 0);

            foglalas.kezdes = uj_kezdes;
            foglalas.idotartam = idotartam;

            Console.WriteLine("Foglalás sikeresen módosítva!");
        }
        #endregion

        #region Menü
        // + Menü
        static void menu(List<Versenyzok> versenyzok, List<Foglalasok> foglalasok, Helyszin ceg)
        {
            bool fut = true;

            while (fut)
            {
                Console.WriteLine();
                Console.WriteLine("=========== Menü ===========");
                Console.WriteLine("1 - Helyszín/Céginfók");
                Console.WriteLine("2 - Versenyzők listája");
                Console.WriteLine("3 - Foglalások listája");
                Console.WriteLine("4 - Időszalag megjelenítése");
                Console.WriteLine("5 - Foglalás átállítása");
                Console.WriteLine("0 - Kilépés");
                Console.Write("Választás: ");

                int valasztas = int.Parse(Console.ReadLine());
                Console.WriteLine();

                switch (valasztas)
                {
                    case 1:
                        ceg.kiiratas();
                        break;
                    case 2:
                        foreach (var v in versenyzok)
                            v.Kiir();
                        Console.WriteLine($"Összesen {versenyzok.Count} versenyző.");
                        break;
                    case 3:
                        foreach (var f in foglalasok)
                            f.Kiir();
                        break;
                    case 4:
                        Console.WriteLine("Ebben a hónapban hátralevő foglalások időszalagja:");
                        var idoszalag = new Idoszalag(DateTime.Today);
                        idoszalag.FeltoltFoglalasokkal(foglalasok);
                        idoszalag.Megjelenit();
                        break;
                    case 5:
                        Console.Write("Adja meg a módosítandó versenyző azonosítóját: ");
                        string azon = Console.ReadLine();
                        Versenyzok keresett = versenyzo_kereso(versenyzok, azon);
                        if (keresett != null)
                            foglalas_atallito(foglalasok, keresett);
                        else
                            Console.WriteLine("Nincs ilyen azonosítóval versenyző!");
                        break;
                    case 0:
                        Console.WriteLine("Köszönjük, hogy igénybe vette szolgáltatásainkat!");
                        fut = false;
                        break;
                    default:
                        Console.WriteLine("Érvénytelen választás!");
                        Console.WriteLine("Válasszon a felsorolt menüpontok sorszámai közül!");
                        break;
                }
            }
        }
        #endregion

        public static void Main(string[] args)
        {
            fejlec();

            Helyszin ceg = new Helyszin("BP - Gokart", "6969 Bivalybasznád, Gyárkémény út 18.", "+36 1 209 09 09", "www.bpgokart.hu");

            ceg.kiiratas();
            Console.WriteLine();
            Console.WriteLine("Üdvözöljük a BP - Gokart foglalási naplójában!");
            Console.WriteLine();
            Console.WriteLine($"A program a {DateTime.Today.Year} év hátralevő versenyzőit és foglalásiat tartalmazza!");
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

            Console.WriteLine($"{darab} versenyző regisztrált be az összes adatával a foglalási naplóba.");

            List<Foglalasok> foglalasok = new List<Foglalasok>();

            foreach (var v in versenyzok)
            {
                foglalasok.Add(new Foglalasok(v));
            }

            menu(versenyzok, foglalasok, ceg);

            Console.WriteLine();
            Console.WriteLine("Kilépéshez nyomja meg az ENTER billentyűt.");
            Console.ReadLine();
        }
    }
}