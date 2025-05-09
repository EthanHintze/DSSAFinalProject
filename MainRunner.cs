﻿
namespace HerosQuest
{
    class Program
    {

        static void Main(string[] args)
        {
            MapGeneration userDungeon = new MapGeneration();
            bool test = userDungeon.DungeonSetup();
            while (userDungeon.EndDungeon != true)
            {
                Console.Clear();
                Console.WriteLine("---What would you like to do---");
                Console.WriteLine("1) Traverse Dungeon");
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
                        userDungeon.ListPlayerInventory();
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case 2:
                        userDungeon.ListPlayerStats();
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case 3:
                        userDungeon.EndDungeon = true;
                        break;
                }
            }
            if (userDungeon.playerCharacter._health <= 0)
            {
                Console.WriteLine("You Died");

            }
            else
            {
                int totalProfit = 0;
                Console.WriteLine("You found the exit");
                foreach (Item item in userDungeon.playerCharacter.inventory)
                {
                    totalProfit += item._price;
                }
            }

            // if (test)
            // {
            //     Console.WriteLine("Setup Successful");
            //     userDungeon.DisplayAllDungeonPaths();
            //     userDungeon.DFS(0);
            // }
        }
    }
}