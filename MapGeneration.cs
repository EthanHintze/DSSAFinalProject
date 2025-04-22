using Microsoft.VisualBasic;

namespace HerosQuest
{
    class MapGeneration
    {
        private Random random = new Random();
        private Dictionary<int, string> PossibleRequiredItems = new Dictionary<int, string> { { 1, "Lockpick" } };
        HashSet<int> visited = new HashSet<int>();
        List<int> path = new List<int>();
        public Dictionary<int, List<Edge>> dungeon { get; set; }
        public List<Edge> roomList { get; set; }


        public MapGeneration()
        {
            dungeon = new Dictionary<int, List<Edge>>();
            roomList = new List<Edge>();
        }
        public bool DungeonSetup()
        {
            for (int i = 0; i < 16; i++)
            {
                Edge newRoom = new(i, random.Next(6), random.Next(6), random.Next(6), random.Next(6));
                dungeon.Add(i, new List<Edge> { });
                roomList.Add(newRoom);

                if (i == 1)
                {
                    dungeon[1].Add(roomList[0]);
                    dungeon[0].Add(roomList[1]);
                }
                else if (i != 0)
                {
                    dungeon[i].Add(roomList[i-1 ]);
                    dungeon[i-1].Add(roomList[i]);
                }
            }

            for (int i = 16; i < 25; i++)
            {
                Edge newRoom = new(i, random.Next(6), random.Next(6), random.Next(6), random.Next(6));
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

            while (dungeon[randomConnectingRoom].Count() == 4)
            {
                randomConnectingRoom = random.Next(dungeon.Count());
            }

            dungeon[randomConnectingRoom].Add(newRoom);
            dungeon[newRoom.roomID].Add(connectingRoom);
        }
        public void DepthFirstSearch()
        {

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
                Console.WriteLine();
            }
        }
    }

}

class Edge
{
    public int roomID;
    public int Distance;
    public int StrengthReq;
    public int AgilityReq;
    public int IntelligenceReq;
    public Edge(int to, int distance, int strReq, int agiReq, int intelReq)
    {
        roomID = to;
        Distance = distance;
        StrengthReq = strReq;
        AgilityReq = agiReq;
        IntelligenceReq = intelReq;
    }
}