using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace tlsh
{
    class Program
    {
        static void Main(string[] args)
        {
            BasicTest();

            TestUsingTextFile("sample.txt", "6FF02BEF718027B0160B4391212923ED7F1A463D563B1549B86CF62973B197AD2731F8");

            TestUsingBinaryFile(@"C:\Users\romcab\Desktop\GUEN-TEST\AutoIt\absetup.exe",
                "7d263307b8d95c22f85be634aee707f3b7abbe9cda05c983bf451e3244626427c51940");

            TestAgainstTlshGetter(@"C:\Users\romcab\Desktop\GUEN-TEST\AutoIt", "tlsh_values.csv");

            Console.WriteLine("Done");
            Console.ReadLine();
        }

        static void BasicTest()
        {
            var data = "The best documentation is the UNIX source. After all, this is what the "
                + "system uses for documentation when it decides what to do next! The "
                + "manuals paraphrase the source code, often having been written at "
                + "different times and by different people than who wrote the code. "
                + "Think of them as guidelines. Sometimes they are more like wishes... "
                + "Nonetheless, it is all too common to turn to the source and find "
                + "options and behaviors that are not documented in the manual. Sometimes "
                + "you find options described in the manual that are unimplemented "
                + "and ignored by the source.";

            var tlsh = new TlshWrapper();
            var hash = tlsh.Hash(Encoding.UTF8.GetBytes(data));

            Console.WriteLine(hash);
            if (hash.Equals("6FF02BEF718027B0160B4391212923ED7F1A463D563B1549B86CF62973B197AD2731F8"))
                Console.WriteLine("Equal");
        }

        /// <summary>
        /// Sample.txt contains the same text used in the readme
        /// </summary>
        static void TestUsingTextFile(string filename, string expected)
        {
            Console.WriteLine("Test using text file...");
            var data = ReadAllBytes(filename);
            var tlsh = new TlshWrapper();
            var hash = tlsh.Hash(data);
            Console.WriteLine(hash);

            if (hash.Equals(expected, StringComparison.CurrentCultureIgnoreCase))
                Console.WriteLine("Passed");
            else
            {
                Console.WriteLine("Failed");
            }

        }

        static void TestUsingBinaryFile(string filename, string expected)
        {
            Console.WriteLine("Test using binary file...");
            var data = ReadAllBytes(filename);
            var tlsh = new TlshWrapper();
            var hash = tlsh.Hash(data);
            Console.WriteLine(hash);

            if (hash.Equals(expected, StringComparison.CurrentCultureIgnoreCase))
                Console.WriteLine("Passed");
            else
            {
                Console.WriteLine("Failed");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir">dir to text</param>
        /// <param name="fileWithExpected">tlshGetter csv file output (c/o jaysonP)</param>
        static void TestAgainstTlshGetter(string dir, string fileWithExpected)
        {
            var dict = new Dictionary<string, string>();
            var tlsh = new TlshWrapper();

            using (var sw = new StreamWriter("tlsh.csv"))
            {
                foreach (var file in Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories))
                {
                    var sha1 = Hasher.ComputeSha1(file);
                    var data = ReadAllBytes(file);
                    var hash = tlsh.Hash(data);

                    if (!dict.ContainsKey(sha1))
                        dict.Add(sha1.ToLower(), hash.ToLower());

                    Console.WriteLine("sha1:{0} tlsh:{1}", sha1, hash);
                    sw.WriteLine("{0},{1},{2}", file, sha1, hash);
                }
            }
                
            
            // compare
            var expected = FileToDict(fileWithExpected);
            var result = dict.Count == expected.Count && !dict.Except(expected).Any();

            var result1 = dict.All(x => expected.Any(y => x.Value == y.Value));

            if (result && result1)Console.WriteLine("Passed");
           else Console.WriteLine("Failed");
        }


        /// <summary>
        /// D7267D9EC22E8E19D82878B4988444C13CADC6DD959985E876CFFD3FE64C8BC50BD670
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        static string ReadByChar(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var buffer = new char[fs.Length];
                using (var br = new BinaryReader(fs))
                {
                    br.Read(buffer, 0, (int)fs.Length);
                }
                return new string(buffer);
            }
        }

        static byte[] ReadAllBytes(string fileName)
        {
            byte[] buffer = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
            }


            return buffer;
        }


        static IDictionary<string, string> FileToDict(string filename)
        {
            var lines = File.ReadAllLines(filename);
            var dict = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                var values = line.Split(',');
                if (values.Length != 3) continue;

                // hash(1), tlsh(2)
                if(!dict.ContainsKey(values[1]))
                    dict.Add(values[1].ToLower(), values[2].ToLower());
            }
            return dict;
        }        
    }

    public static class Hasher
    {
        private static HashAlgorithm hasher = new SHA1Managed();       

        public static string ComputeSha1(string file)
        {
            using (var stream = File.OpenRead(file))
            {
                byte[] hash = hasher.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", String.Empty).ToLower();
            }
        }
    }
}
