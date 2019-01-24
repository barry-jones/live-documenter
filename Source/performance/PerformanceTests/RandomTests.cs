
namespace PerformanceTests
{
    using BenchmarkDotNet.Attributes;

    public class RandomTests
    {
        private const string ConstantString = "testing2";

        public RandomTests()
        {
            // create some other interned strings so there is more strings in the interned list
            string a = "first string";
            string b = "second string";
            string c = "third string";
            string d = "fourth string";
            string e = "fifth string";
            string f = "sixth string";
            string g = "seventh string";
            string h = "eigth string";
            string i = "ninth string";
            string j = "tenth string";
            string k = "eleventh string";
            string l = "twelth string";
            string m = "thirteenth string";
            string n = "fourteenth string";
            string o = "fifthteenth string";
            string p = "sixteenth string";

            if((a == b) || (c == d) || (e == f) || (g == h) || (i == j) || (k == l) || (m == n) || (o == p))
            {
                string z = p;
                string y = a;
                p = y;
                a = z;
            }
        }

        [Benchmark]
        public bool InternedString()
        {
            string test = "testing1";
            return test == "testing1";
        }

        [Benchmark]
        public bool ConstantStrings()
        {
            string test = ConstantString;
            return test == ConstantString;
        }

        private void CreateMoreStrings1()
        {
            // create some other interned strings so there is more strings in the interned list
            string a = "first string1";
            string b = "second string1";
            string c = "third string1";
            string d = "fourth string1";
            string e = "fifth string1";
            string f = "sixth string1";
            string g = "seventh string1";
            string h = "eigth string1";
            string i = "ninth string1";
            string j = "tenth string1";
            string k = "eleventh string1";
            string l = "twelth string1";
            string m = "thirteenth string1";
            string n = "fourteenth string1";
            string o = "fifthteenth string1";
            string p = "sixteenth string1";

            if((a == b) || (c == d) || (e == f) || (g == h) || (i == j) || (k == l) || (m == n) || (o == p))
            {
                string z = p;
                string y = a;
                p = y;
                a = z;
            }
        }

        private void CreateMoreStrings2()
        {
            // create some other interned strings so there is more strings in the interned list
            string a = "first string2";
            string b = "second string2";
            string c = "third string2";
            string d = "fourth string2";
            string e = "fifth string2";
            string f = "sixth string2";
            string g = "seventh string2";
            string h = "eigth string2";
            string i = "ninth string2";
            string j = "tenth string2";
            string k = "eleventh string2";
            string l = "twelth string2";
            string m = "thirteenth string2";
            string n = "fourteenth string2";
            string o = "fifthteenth string2";
            string p = "sixteenth string2";

            if((a == b) || (c == d) || (e == f) || (g == h) || (i == j) || (k == l) || (m == n) || (o == p))
            {
                string z = p;
                string y = a;
                p = y;
                a = z;
            }
        }

        private void CreateMoreStrings3()
        {
            // create some other interned strings so there is more strings in the interned list
            string a = "first string3";
            string b = "second string3";
            string c = "third string3";
            string d = "fourth string3";
            string e = "fifth string3";
            string f = "sixth string3";
            string g = "seventh string3";
            string h = "eigth string3";
            string i = "ninth string3";
            string j = "tenth string3";
            string k = "eleventh string3";
            string l = "twelth string3";
            string m = "thirteenth string3";
            string n = "fourteenth string3";
            string o = "fifthteenth string3";
            string p = "sixteenth string3";

            if((a == b) || (c == d) || (e == f) || (g == h) || (i == j) || (k == l) || (m == n) || (o == p))
            {
                string z = p;
                string y = a;
                p = y;
                a = z;
            }
        }
    }
}
