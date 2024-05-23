namespace InputFileGenerator
{
    public class Generator
    {
        static void Main(string[] args)
        {
            Generate(int.Parse(args[0]), args[1]);
        }

        public static void Generate(int n, string fileName)
        {
            int minSharing = 1;
            int maxSharing = 100;
            Random random = new Random();
            using (var fileStream = File.OpenWrite(fileName))
            using (var streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.Write("0 ");
                streamWriter.WriteLine(random.Next(minSharing ,maxSharing).ToString());

                for(int i=1; i<n; i++)
                {
                    streamWriter.Write(i.ToString());
                    streamWriter.Write(' ');
                    int followedIndex = random.Next(n);
                    while(followedIndex == i)
                    {
                        followedIndex = random.Next(n);
                    }
                    streamWriter.Write(followedIndex.ToString());
                    streamWriter.Write(' ');
                    streamWriter.WriteLine(random.Next(minSharing, maxSharing).ToString());
                }
            }
        }
    }
}