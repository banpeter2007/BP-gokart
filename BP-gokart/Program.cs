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
            Console.WriteLine($"Név: {nev}");
            Console.WriteLine($"Cím: {cim}");
            Console.WriteLine($"Telefon: {telefon}");
            Console.WriteLine($"Domain: {domain}");
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
            Console.WriteLine("Kilépéshez nyomja meg az ENTER billentyűt.");
            Console.ReadLine();
        }
    }
}
