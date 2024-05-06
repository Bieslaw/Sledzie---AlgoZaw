using Sledzie;

namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var fileName = "Examples/ExampleGraph1.txt";
            var expectedIndices = new HashSet<int>{ 1, 3, 5, 6, 8, 10 };
            var expectedShares = 41;
            RunAndCheckEquality(fileName, expectedIndices, expectedShares);
        }
        [Fact]
        public void Test2()
        {
            var fileName = "Examples/ExampleGraph2.txt";
            var expectedIndices = new HashSet<int> { 0, 8, 7, 10, 5, 6 };
            var expectedShares = 25;
            RunAndCheckEquality(fileName, expectedIndices, expectedShares);
        }
        [Fact]
        public void Test3()
        {
            var fileName = "Examples/ExampleGraph3.txt";
            var expectedIndices = new HashSet<int> { 0, 1, 5, 7, 8, 9 };
            var expectedShares = 26;
            RunAndCheckEquality(fileName, expectedIndices, expectedShares);
        }
        private void RunAndCheckEquality(string fileName, HashSet<int> expectedIndices, int expectedShares)
        {
            
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory)!.Parent!.Parent!.FullName;
            var filePath = Path.Combine(projectDirectory, fileName);
            var result = Algorithms.MostSharesNoFollowing(filePath);
            
            var hs1 = result.Select(v => v.index).ToHashSet();
            Assert.Equal(hs1, expectedIndices);
            Assert.Equal(result.Sum(v => v.shares), expectedShares);

        }
    }
}