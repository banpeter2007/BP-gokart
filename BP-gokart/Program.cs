using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BP_gokart
{
    #region 1. feladat: Helyszín
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
        static void Main(string[] args)
        {
            Helyszin helyszin = new Helyszin("BP - Gokart", "6969 Bivalybasznád, GYárkémény út 18.", "+36 1 209 09 09", "www.bpgokart.hu");

            Console.WriteLine();
            Console.WriteLine("Kilépéshez nyonja meg az ENTER billentyűt.");
            Console.ReadLine();
        }
    }
}
