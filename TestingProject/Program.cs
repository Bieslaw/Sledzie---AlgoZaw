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
            //GenerateTests(50, 10);
            int n = 10;
            for (int i = 0; i < n; i++)
            {
                string file = "test50_" + i.ToString() + ".txt";
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Program.Main_withoutWrite(file);
                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
            }
        }


        static void GenerateTests(int size, int n)
        {
            string s = "test" + size.ToString() + "_";
            for (int i = 0; i < n; i++)
            {
                Generator.Generate(20, s + i.ToString() + ".txt");
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