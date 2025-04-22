using Microsoft.VisualBasic;

namespace HerosQuest
{
    class MapGeneration
    {
        private Random random = new Random();
        private Dictionary<int, string> PossibleRequiredItems = new Dictionary<int, string> { { 1, "Lockpick" } };
        public HashSet<int> visited { get; set; }
        List<int> path = new List<int>();
        public Dictionary<int, List<Edge>> dungeon { get; set; }
        public List<Edge> roomList { get; set; }


        public MapGeneration()
        {
            dungeon = new Dictionary<int, List<Edge>>();
            roomList = new List<Edge>();
            visited = new HashSet<int>();
        }
        public bool DungeonSetup()
        {
            for (int i = 0; i < 16; i++)
            {
                Edge newRoom = new(i, i + 1, random.Next(6), random.Next(6), random.Next(6), random.Next(6));
                dungeon.Add(i, new List<Edge> { });
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

                AddConnection(newRoom);
            }
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
        public void DisplayDungeonPaths()
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
    public Edge(int RoomID, int To, int Distance, int strReq, int agiReq, int intelReq)
    {
        roomID = RoomID;
        to = To;
        distance = Distance;
        strengthReq = strReq;
        agilityReq = agiReq;
        intelligenceReq = intelReq;
        isExit = false;
    }
}