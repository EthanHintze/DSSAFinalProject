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
            while (userDungeon.EndDungeon != true)
            {
                Console.WriteLine("---What would you like to do---");
                Console.WriteLine("1) Continue");
                Console.WriteLine("2) Check Inventory");
                Console.WriteLine("3) Check Stats");
                Console.WriteLine("4) Give Up (loser)");

                int choice = int.Parse(Console.ReadLine());
                switch (choice - 1)
                {
                    case 0:
                        userDungeon.RunDungeon();
                        break;

                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        userDungeon.EndDungeon = true;
                        break;
                }
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