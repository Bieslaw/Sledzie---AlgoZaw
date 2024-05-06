using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sledzie
{
    static class Program
    {
        static void Main(string[] args)
        {

            string workingDirectory = Environment.CurrentDirectory;
            var inFilePath = Path.Combine(workingDirectory, args[0]);
            var outFilePath = args[1];
            var result = Algorithms.MostSharesNoFollowing(inFilePath);
            writeResult(outFilePath, result);
        }

        static void writeResult(string path, List<Vertex> result)
        {
            int sum = 0;
            using (var fileStream = File.OpenWrite(path))
            using (var streamWriter = new StreamWriter(fileStream))
            {
                foreach (var item in result)
                {
                    streamWriter.Write(item.index);
                    streamWriter.Write(' ');
                    sum += item.shares;
                }
                streamWriter.Write('\n');
                streamWriter.WriteLine(sum);
            }
        }
    }
}
