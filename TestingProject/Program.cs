using Sledzie;
using InputFileGenerator;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace TestingProject
{
    internal class TestingProgram
    {
        static void Main()
        {
            //GenerateTests(1000000, 10);
            int n = 10;
            List<int> sizes = new List<int>() {100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000, 100000, 200000, 500000, 1000000 };
            Stopwatch sw = new Stopwatch();
            foreach (int size in sizes)
            {
                TimeSpan averageTime = new TimeSpan();
                for (int i = 0; i < n; i++)
                {
                    string file = "test" + size.ToString() + "_" + i.ToString() + ".txt";
                    sw.Restart();
                    Program.Main_withoutWrite(file);
                    sw.Stop();
                    //Console.WriteLine("Elapsed={0}", sw.Elapsed);
                    averageTime += sw.Elapsed;
                }
                averageTime /= 10;
                Console.WriteLine("Size: " + size.ToString() + " Time: " + averageTime.TotalMilliseconds.ToString());
            }
        }


        static void GenerateTests(int size, int n)
        {
            string s = "test" + size.ToString() + "_";
            for (int i = 0; i < n; i++)
            {
                Generator.Generate(size, s + i.ToString() + ".txt", 100);
            }
        }

        static int BruteForce(string filePath) //Użytę by potwierdzić, że wyniki są prawidłowe
        {
            var G = Algorithms.ConstructGraphFromFile(filePath);
            List<int> list = new List<int>();
            return BruteForceRek(G, list, 0);
        }

        static int BruteForceRek(Graph G, List<int> list, int index)
        {
            int result = 0;
            if(index == G.vertices.Count)
            {
                return BruteForceStop(G, list);
            }
            list.Add(index);
            result = BruteForceRek(G, list, index + 1);
            list.Remove(index);
            result = Math.Max(result, BruteForceRek(G, list, index + 1));
            return result;

        }
        static int BruteForceStop(Graph G, List<int> list)
        {
            int sum = 0;
            foreach(int i in list)
            {
                sum += G.vertices[i].shares;
                foreach(var child in G.vertices[i].children)
                {
                    if(list.Contains(child.index))
                    {
                        return 0;
                    }
                }
            }
            return sum;
        }
    }
}