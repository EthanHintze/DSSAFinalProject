using System.Data;

namespace HerosQuest
{
    class Program
    {

        static void Main(string[] args)
        {
            bool test = false;
            MapGeneration userDungeon = new MapGeneration();
            test = userDungeon.DungeonSetup();
            if (test)
            {
                Console.WriteLine("Setup Successful");
                userDungeon.DisplayDungeonPaths();
                userDungeon.DFS(0);
            }
        }
    }
}