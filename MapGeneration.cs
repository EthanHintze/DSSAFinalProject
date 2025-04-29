using System.Runtime.CompilerServices;

namespace HerosQuest
{
    class MapGeneration
    {
        private Random random = new Random();
        private Challenge roomChallenge;
        public bool EndDungeon = false;
        public bool HasKey = false;
        public bool HasSword = false;
        public int startRoom = 0;
        public int nextRoom = 0;


        private Stack<Item> PossibleTreasure = new Stack<Item>();
        public HashSet<int> visited { get; set; }

        public Stack<Edge> visitedRooms = new Stack<Edge>();
        public Stack<Edge> notVisitedRooms = new Stack<Edge>();

        public Dictionary<int, List<Edge>> dungeon { get; set; }
        public ChallengeTree challengeTree = new ChallengeTree();
        public List<Edge> roomList { get; set; }

        public CharacterCreator playerCharacter { get; set; }

        public MapGeneration()
        {
            dungeon = new Dictionary<int, List<Edge>>();
            challengeTree = new ChallengeTree();
            roomList = new List<Edge>();
            visited = new HashSet<int>();
            playerCharacter = new CharacterCreator(5, 5, 5);
        }
        public bool DungeonSetup()
        {
            PossibleTreasure.Push(new Item("Gem", 0, 0, 0, 300, false));
            PossibleTreasure.Push(new Item("Smart Glasses", 0, 0, 5, 70, false));
            PossibleTreasure.Push(new Item("Heelys", 0, 5, 0, 50, false));
            PossibleTreasure.Push(new Item("Lockpick", 0, 0, 0, 20, true));
            PossibleTreasure.Push(new Item("Rock", 0, 0, 0, 1, false));
            PossibleTreasure.Push(new Item("Sword", 10, 0, 0, 30, false));

            for (int i = 0; i < 21; i++)
            {
                Challenge newChallenge = new Challenge(random.Next(3), i, random.Next(11), "");
                challengeTree.Insert(newChallenge);
            }
            Challenge LockChallenge = new Challenge(3, 21, random.Next(11), "Lockpick");
            challengeTree.Insert(LockChallenge);

            for (int i = 0; i < 16; i++)
            {
                Edge newRoom = new(i, i + 1, random.Next(6), random.Next(6), random.Next(6), random.Next(6));
                dungeon.Add(i, new List<Edge> { });
                notVisitedRooms.Push(newRoom);
                roomList.Add(newRoom);

                if (i == 1)
                {
                    dungeon[1].Add(roomList[0]);
                    dungeon[0].Add(roomList[1]);
                }
                else if (i != 0)
                {
                    dungeon[i].Add(roomList[i - 1]);
                    dungeon[i - 1].Add(roomList[i]);
                }
            }
            PickExit();

            for (int i = 16; i < 25; i++)
            {
                Edge newRoom = new(i, i + 1, random.Next(6), random.Next(6), random.Next(6), random.Next(6));
                dungeon.Add(i, new List<Edge> { });
                roomList.Add(newRoom);
                notVisitedRooms.Push(newRoom);

                AddConnection(newRoom);
            }
            AddDeadEnds();
            return true;
        }

        public void AddConnection(Edge newRoom)
        {
            int randomConnectingRoom = random.Next(dungeon.Count());
            Edge connectingRoom = roomList[randomConnectingRoom];

            while (dungeon[randomConnectingRoom].Count() == 4 && !connectingRoom.isExit)
            {
                randomConnectingRoom = random.Next(dungeon.Count());
            }

            dungeon[randomConnectingRoom].Add(newRoom);
            dungeon[newRoom.roomID].Add(connectingRoom);

        }
        public void AddDeadEnds()
        {
            for (int i = 25; i < 31; i++)
            {
                Edge newRoom = new(i, i + 1, random.Next(6), random.Next(6), random.Next(6), random.Next(6));
                dungeon.Add(i, new List<Edge> { });
                roomList.Add(newRoom);
                notVisitedRooms.Push(newRoom);

                AddConnection(newRoom);
            }
        }

        public void PickExit()
        {
            int winningRoom = random.Next(5) + 10;
            roomList[winningRoom].isExit = true;
            Console.WriteLine($"Room {winningRoom} is the exit");

        }
        public bool DFS(int node)
        {
            if (roomList[node].isExit) return true;

            if (visited.Contains(node)) return false;

            visited.Add(node);
            Console.WriteLine($"Visiting node: {node}");

            if (dungeon.ContainsKey(node))
            {
                foreach (var edge in dungeon[node])
                {
                    if (DFS(edge.to)) return true;
                }
            }
            return false;
        }
        public void RunDungeon()
        {
            Console.Clear();
            if (!EndDungeon)
            {
                visitedRooms.Push(roomList[startRoom]);

                Console.WriteLine($"You are in Room {startRoom}");
                roomChallenge = challengeTree.FindClosest(startRoom);
                if (startRoom != 0)
                {
                    CheckForTreasure();
                    if (random.Next(2) == 0)
                    {

                        Console.WriteLine($"This room has a {roomChallenge.Type} encounter");
                        if (roomChallenge.Type != "Locked")
                        {
                            if (!CheckChallengeStats())
                            {
                                Console.WriteLine($"You failed the {roomChallenge.Type} encounter");
                                Console.WriteLine($"Your health is now: {playerCharacter._health}");
                            }
                            else
                            {
                                Console.WriteLine($"You passed the {roomChallenge.Type} encounter");
                            }
                        }
                        else
                        {
                            if (!CheckChallengeStats())
                            {
                                Console.WriteLine("This room is locked find a key");
                                Console.ReadKey();
                                RunDungeon();
                            }
                        }
                    }
                }


                if (!TraverseDungeon(startRoom))
                {
                    Console.Write("Where will you go?: ");
                    nextRoom = int.Parse(Console.ReadLine());

                    if (MoveForward(startRoom, nextRoom))
                    {
                        startRoom = nextRoom;
                    }
                    else
                    {
                        Console.WriteLine("That path doesn't exist, try again");
                        RunDungeon();
                    }
                }

            }
        }
        private bool MoveForward(int currentRoom, int nextRoom)
        {
            foreach (Edge path in dungeon[currentRoom])
            {
                if (path.roomID == nextRoom)
                {
                    return true;
                }
            }
            return false;
        }
        public void CheckForTreasure()
        {
            if (random.Next(4) == 0)
            {
                Console.WriteLine($"You found {PossibleTreasure.Peek()._name}");
                Console.WriteLine($"Pick it up?");
                if (playerCharacter.inventory.Count == 5)
                {
                    Console.WriteLine($" you will drop {playerCharacter.inventory.First()._name}");
                }
                Console.WriteLine("1) Yes");
                Console.WriteLine("2) No");
                int checkChoice = int.Parse(Console.ReadLine());
                if (checkChoice == 1)
                {
                    if (playerCharacter.inventory.Count == 5)
                    {
                        IncDecPlayerStats(0);
                        IncDecPlayerStats(1);
                        if (playerCharacter.inventory.First()._name == "Lockpick")
                        {
                            HasKey = false;
                        }
                        if (playerCharacter.inventory.First()._name == "Sword")
                        {
                            HasSword = false;
                        }

                        PossibleTreasure.Push(playerCharacter.inventory.Dequeue());
                        playerCharacter.inventory.Enqueue(PossibleTreasure.Peek());
                        PossibleTreasure.Pop();
                    }
                    else
                    {
                        IncDecPlayerStats(0);

                        playerCharacter.inventory.Enqueue(PossibleTreasure.Peek());
                        PossibleTreasure.Pop();
                        if (PossibleTreasure.Peek()._name == "Lockpick")
                        {
                            HasKey = true;
                        }
                        if (PossibleTreasure.Peek()._name == "Sword")
                        {
                            HasSword = true;
                        }
                    }
                    Console.WriteLine("Item picked up");

                }
                if (checkChoice != 1)
                {
                    Console.WriteLine("Item was lost to the dungeon");

                }

            }
        }

        public void IncDecPlayerStats(int IncDec)
        {
            if (IncDec == 0)
            {
                playerCharacter._strength += PossibleTreasure.Peek()._strengthBuff;
                playerCharacter._agility += PossibleTreasure.Peek()._agilityBuff;
                playerCharacter._intellegence += PossibleTreasure.Peek()._intellegenceBuff;
            }
            if (IncDec == 1)
            {
                playerCharacter._strength -= playerCharacter.inventory.First()._strengthBuff;
                playerCharacter._agility -= playerCharacter.inventory.First()._agilityBuff;
                playerCharacter._intellegence -= playerCharacter.inventory.First()._intellegenceBuff;
            }

        }

        public void ListPlayerInventory()
        {
            Console.Clear();
            Console.WriteLine("---Current Inventory---");
            foreach (Item item in playerCharacter.inventory)
            {
                Console.WriteLine($"{item._name}");
            }
        }

        public void ListPlayerStats()
        {
            Console.Clear();
            Console.WriteLine("---Current Stats---");
            Console.WriteLine($"Health: {playerCharacter._health} Strength: {playerCharacter._strength} Agility: {playerCharacter._agility} Intellegince: {playerCharacter._intellegence}");
        }
        public bool TraverseDungeon(int roomID)
        {
            if (roomList[roomID].isExit)
            {
                EndDungeon = true;
                return true;
            }
            if (playerCharacter._health <= 0)
            {
                EndDungeon = true;
                return true;
            }

            DisplayRoomPaths(roomID);
            return false;
        }
        public void DisplayRoomPaths(int roomID)
        {
            Console.WriteLine("-----Paths------");
            if (dungeon[roomID].Count() == 1)
            {
                roomList[roomID].isDeadEnd = true;
            }
            foreach (Edge edge in dungeon[roomID])
            {
                if (!(edge.isDeadEnd && visitedRooms.Contains(edge)))
                {
                    Console.WriteLine($"=> Room: {edge.roomID} ");
                }
            }
        }

        public void DisplayAllDungeonPaths()
        {
            for (int i = 0; i < dungeon.Count(); i++)
            {
                Console.Write($"Room #{i} connects too :");
                if (dungeon[i].Count() != 0)
                {
                    foreach (Edge edge in dungeon[i])
                    {
                        Console.Write($"Room: {edge.roomID} ");
                    }
                }

                if (roomList[i].isExit == true)
                {
                    Console.Write($"       |>");
                }
                Console.WriteLine();
            }
        }
        public bool CheckChallengeStats()
        {
            if (roomChallenge.Type == "Combat")
            {
                if (HasSword)
                {
                    return true;
                }
                if (roomChallenge.RequiredStat > playerCharacter._strength)
                {
                    playerCharacter._health -= roomChallenge.RequiredStat - playerCharacter._strength;
                    return false;
                }
            }

            if (roomChallenge.Type == "Puzzle")
            {
                if (roomChallenge.RequiredStat > playerCharacter._intellegence)
                {
                    playerCharacter._health -= roomChallenge.RequiredStat - playerCharacter._intellegence;
                    return false;
                }
            }

            if (roomChallenge.Type == "Trap")
            {
                if (roomChallenge.RequiredStat > playerCharacter._agility)
                {
                    playerCharacter._health -= roomChallenge.RequiredStat - playerCharacter._agility;
                    return false;
                }
            }
            if (roomChallenge.Type == "Locked")
            {

                if (!HasKey)
                {
                    return false;
                }
            }
            return true;
        }
    }

}

class Edge
{
    public int roomID;
    public int to;
    public int distance;
    public int strengthReq;
    public int agilityReq;
    public int intelligenceReq;
    public bool isExit;
    public bool isDeadEnd;
    public Edge(int RoomID, int To, int Distance, int strReq, int agiReq, int intelReq)
    {
        roomID = RoomID;
        to = To;
        distance = Distance;
        strengthReq = strReq;
        agilityReq = agiReq;
        intelligenceReq = intelReq;
        isExit = false;
        isDeadEnd = false;
    }
}