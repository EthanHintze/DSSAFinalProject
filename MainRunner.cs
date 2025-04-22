using System.Data;

namespace HerosQuest
{
    class Program
    {

        static void Main(string[] args)
        {
            Stack<Edge> visitedRooms = new Stack<Edge>();
            MapGeneration userDungeon = new MapGeneration();
            bool test = userDungeon.DungeonSetup();

            while (userDungeon.ExitFound != true)
            {
                userDungeon.RunDungeon();
            }

            if (test)
            {
                Console.WriteLine("Setup Successful");
                userDungeon.DisplayAllDungeonPaths();
                userDungeon.DFS(0);
            }
        }
    }
}