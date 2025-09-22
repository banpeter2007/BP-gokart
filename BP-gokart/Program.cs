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
        private string veznev;
        private string kernev;
        private DateTime szuldatum;
        private bool felnott;
        private string azon;
        private string email;

        public Versenyzok(string veznev, string kernev, DateTime szuldatum, bool felnott, string azon, string email)
        {
            this.veznev = veznev;
            this.kernev = kernev;
            this.szuldatum = szuldatum;
            this.felnott = felnott;
            this.azon = azon;
            this.email = email;
        }

        public string[] veznev_beolvasas()
        {
            StreamReader be_veznev = new StreamReader("vezeteknevek.txt");
            string sor = be_veznev.ReadLine();
            string[] reszek;
            string[] veznevek = new string[] { };
            while(sor != null)
            {
                if (sor != null)
                {
                    reszek = sor.Replace("'", "").Split(';');
                    veznevek = veznevek.Append(reszek[0]).ToArray();
                }
                sor = be_veznev.ReadLine();
            }

            return veznevek;
        }

        public string[] kernev_beolvasas()
        {
            StreamReader be_kernev = new StreamReader("keresztnevek.txt");
            string sor = be_kernev.ReadLine();
            string[] reszek;
            string[] kernevek = new string[] { };
            while (sor != null)
            {
                if (sor != null)
                {
                    reszek = sor.Replace("'", "").Split(';');
                    kernevek = kernevek.Append(reszek[0]).ToArray();
                }
                sor = be_kernev.ReadLine();
            }
            return kernevek;
        }

        public Dictionary<int, Versenyzok> versenyzok = new Dictionary<int, Versenyzok>();
        public Random rnd = new Random();
        public int versenyzokSzama()
        {
            int random_generalt_versenyzok_szama = rnd.Next(1, 151);
            return random_generalt_versenyzok_szama;
        }

        public void versenyzoHozzaadas(Versenyzok versenyzo)
        {
            int id = versenyzok.Count + 1;
            versenyzo.veznev = veznev_beolvasas()[rnd.Next(0, veznev_beolvasas().Length)];
            versenyzo.kernev = kernev_beolvasas()[rnd.Next(0, kernev_beolvasas().Length)];
            int year = rnd.Next(1953, 2014);
            int month = rnd.Next(1, 13);
            int day = rnd.Next(1, DateTime.DaysInMonth(year, month) + 1);
            versenyzo.szuldatum = new DateTime(year, month, day);
            if (DateTime.Now.Year - versenyzo.szuldatum.Year >= 18) versenyzo.felnott = true;
            else versenyzo.felnott = false;
            string teljesnev_egyben = versenyzo.veznev + versenyzo.kernev;
            string szuldatum_str = versenyzo.szuldatum.ToString("yyyyMMdd");
            versenyzo.azon = $"GO-{teljesnev_egyben}-{szuldatum_str}";
            versenyzo.email = $"{versenyzo.veznev.ToLower()}.{versenyzo.kernev.ToLower()}@gmail.com";

            if (id <= versenyzokSzama())
            {
                versenyzok.Add(id, versenyzo);
            }
            else
            {
                Console.WriteLine("A versenyzők generálása megtörtént!");
                Console.WriteLine($"{versenyzok.Count()} versenyző legenerálása valósult meg.");
            }
        }

        public void versenyzokKiirasa()
        {
            foreach (var item in versenyzok)
            {
                Console.WriteLine($"{item.Key}. {item.Value.veznev} {item.Value.kernev} | Születési dátum: {item.Value.szuldatum.ToString("yyyy.MM.dd.")} | Felnőtt: {(item.Value.felnott ? "Igen" : "Nem")} | Azon: {item.Value.azon} | Email: {item.Value.email}");
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

            Versenyzok versenyzok = new Versenyzok("", "", DateTime.Now, false, "", "");
            for (int i = 0; i < versenyzok.versenyzokSzama(); i++)
            {
                Versenyzok uj_versenyzo = new Versenyzok("", "", DateTime.Now, false, "", "");
                versenyzok.versenyzoHozzaadas(uj_versenyzo);
            }

            Console.WriteLine("A versenyzők, akik foglaltak:");
            versenyzok.versenyzokKiirasa();

            Console.WriteLine();
            Console.WriteLine("Kilépéshez nyomja meg az ENTER billentyűt.");
            Console.ReadLine();
        }
    }
}
