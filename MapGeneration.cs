using Microsoft.VisualBasic;

namespace HerosQuest
{
    class MapGeneration
    {
        private static Random random = new Random();
        //private static int roomID = 0;
        private int ExitRoom;
        private bool ExitExists = false;
        private static Dictionary<int, string> PossibleRequiredItems = new Dictionary<int, string> { { 1, "Lockpick" } };
        HashSet<int> visited = new HashSet<int>();
        List<int> path = new List<int>();
        private static Dictionary<int, List<Edge>> dungeon = new Dictionary<int, List<Edge>>();
        public void DungeonSetup()
        {
            for (int i = 0; i > 16; i++)
            {
                Edge newRoom = new(i, random.Next(6), random.Next(6), random.Next(6), random.Next(6));
                dungeon.Add(i, new List<Edge> {});
                AddConnection(newRoom);
            }
            //ExitRoom = random.Next(dungeon.Count());
            
        }
        public void AddConnection(Edge newRoom)
        {
            int connectingRoom = random.Next(dungeon.Count());

            while(dungeon[connectingRoom].Count() == 4)
            {
                connectingRoom = random.Next(dungeon.Count());
            }

            dungeon[connectingRoom].Add(newRoom);
        }
        public void DepthFirstSearch()
        {
           
        }
        
    }
}

// class Room
// {
//     public int RoomNumber;
//     public List<Edge> Paths;

//     public Room(int roomNumber, Edge path)
//     {
//         RoomNumber = roomNumber;
//         Paths.Add(path);
//     }

// }
class Edge
{
    public int To;
    public int Distance;
    public int StrengthReq;
    public int AgilityReq;
    public int IntelligenceReq;
    public Edge(int to, int distance, int strReq, int agiReq, int intelReq)
    {
        To = to;
        Distance = distance;
        StrengthReq = strReq;
        AgilityReq = agiReq;
        IntelligenceReq = intelReq;
    }
}