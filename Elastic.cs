using MySql.Data.MySqlClient;
using System.IO;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1
{
    public class Elastic
    {
        public static List<string> basliklar = new List<string>();
        static ArrayList kisisel_sql = new ArrayList();
        static ArrayList cok_kisisel_sql = new ArrayList();
        static string[] sutunlar = new string[12];

        public enum sekmeler { kisisel, cok_kisisel, normal, tum };
        static void mysql1()
        {
            string query = "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA='elasticdb' AND TABLE_NAME='userinfodb' ORDER BY ORDINAL_POSITION";
            MySqlConnection conn = new MySqlConnection("Server=localhost;Database=elasticdb;user=root;Pwd=;");
            conn.Open();
            var cmd = new MySqlCommand(query, conn);
            var reader = cmd.ExecuteReader();
            int i = 0;
            while (reader.Read())
            {
                sutunlar[i] = (string)reader["COLUMN_NAME"];
                i++;
            }
        }
        static void mysql2(string tabloAdi)
        {
            string query = "SELECT veriler FROM "+ tabloAdi;

            MySqlConnection conn = new MySqlConnection("Server=localhost;Database=elasticdb;user=root;Pwd=;");
            conn.Open();

            var cmd = new MySqlCommand(query, conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (tabloAdi.Equals("kisisel"))
                {
                    kisisel_sql.Add((string)reader["veriler"]);
                }
                if (tabloAdi.Equals("cok_kisisel"))
                {
                    cok_kisisel_sql.Add((string)reader["veriler"]);
                }
            }
        }
        //static void textoku(string txtname)
        //{
        //    string line;
        //    StreamReader file = new StreamReader(txtname);

        //    while ((line = file.ReadLine()) != null)
        //    {
        //        if (txtname.Equals("kisisel.txt"))
        //        {
        //            kisisel_sql.Add(line);
        //        }
        //        if (txtname.Equals("cok_kisisel.txt"))
        //        {
        //            cok_kisisel_sql.Add(line);
        //        }
        //    }
        //}
        static string elastic(sekmeler secili)
        {
            mysql1();
            ArrayList indis = new ArrayList();

            if (secili == sekmeler.kisisel)
            {
                //textoku("kisisel.txt");
                mysql2("kisisel");
                for (int i = 0; i < sutunlar.Length; i++)
                {
                    for (int j = 0; j < kisisel_sql.Count; j++)
                    {
                        if (kisisel_sql[j].ToString().Contains(sutunlar[i]))
                        {
                            //Console.WriteLine((string)kisisel_sql[j] + " de "+ sutunlar[i]+" var");
                        }
                        if (kisisel_sql[j].Equals(sutunlar[i]))
                        {
                            //Console.WriteLine(i + (string)kisisel_sql[j] + " sütunu özel veri içerebilir.");
                            indis.Add(i);
                        }
                    }
                }
            }

            if (secili == sekmeler.cok_kisisel)
            {
                //textoku("cok_kisisel.txt");
                mysql2("cok_kisisel");
                for (int i = 0; i < sutunlar.Length; i++)
                {
                    for (int j = 0; j < cok_kisisel_sql.Count; j++)
                    {
                        if (cok_kisisel_sql[j].ToString().Contains(sutunlar[i]))
                        {
                            //Console.WriteLine((string)cok_kisisel_sql[j] + " de " + sutunlar[i] + " var");
                        }
                        if (cok_kisisel_sql[j].Equals(sutunlar[i]))
                        {
                            //Console.WriteLine(i + (string)cok_kisisel_sql[j] + " sütunu çok özel veri içerebilir.");
                            indis.Add(i);
                        }
                    }
                }
            }

            if (secili == sekmeler.normal)
            {
                for (int i = 0; i < sutunlar.Length; i++)
                {
                    indis.Add(i);
                }

                for (int i = 0; i < sutunlar.Length; i++)
                {
                    for (int j = 0; j < cok_kisisel_sql.Count; j++)
                    {
                        if (cok_kisisel_sql[j].Equals(sutunlar[i]))
                        {
                            //Console.WriteLine("1. normal elenmesi");
                            indis.Remove(i);
                        }
                    }
                }

                for (int i = 0; i < sutunlar.Length; i++)
                {
                    for (int j = 0; j < kisisel_sql.Count; j++)
                    {
                        if (kisisel_sql[j].Equals(sutunlar[i]))
                        {
                            //Console.WriteLine("2. normal elenmesi");
                            indis.Remove(i);
                        }
                    }
                }
            }

            if (secili == sekmeler.tum)
            {
                for (int i = 0; i < sutunlar.Length; i++)
                {
                    indis.Add(i);
                }
            }
            indis.Sort();

            //for (int i = 0; i < indis.Count; i++)
            //    Console.WriteLine("///" + indis[i]);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:9200/_sql?format=txt");
            httpWebRequest.ContentType = "application/json; charset=UTF-8";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                basliklar.Clear();
                string json = "{\"query\":\"SELECT \\\"";
                for (int i = 0; i < indis.Count; i++)
                {
                    json += sutunlar[(int)indis[i]].ToLower().Replace('ı', 'i');
                    basliklar.Add(sutunlar[(int)indis[i]].ToLower().Replace('ı', 'i'));
                    if (i < indis.Count - 1)
                        json += "\\\",\\\"";
                }
                json += "\\\" FROM \\\"elastic-db\\\" ORDER BY \\\"personid\\\" ASC \"}";
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }
        public static string[,] tabloYap(sekmeler secili)
        {
            string[,] veriler;
            string tablo = elastic(secili);
            tablo = tablo.Substring(tablo.IndexOf("\n", tablo.IndexOf("-")) + 1);

            List<string> t1 = tablo.Split("\n").ToList();
            List<string> t2 = t1[0].Split("|").ToList();

            veriler = new string[t1.Count - 1, t2.Count];

            for (int i = 0; i < t1.Count - 1; i++)
            {
                t2 = t1[i].Split("|").ToList();
                for (int j = 0; j < t2.Count; j++)
                {
                    veriler[i, j] = t2[j].Trim();
                }
            }
            // Console.WriteLine(veriler[250, 0] + veriler[250, 1]);
            return veriler;
        }
    }
}
