namespace HerosQuest
{
    class MapGeneration
    {
        public enum Danger
        {
            Low = 1,
            Medium = 3,
            High = 5
        }

        public class Edge
        {
            public string Destination { get; set; }
            public int Distance { get; set; }
            public int EngergyCost { get; set; }
            public Danger DangerLevel { get; set; }

            public Edge(string to, int distance, int engergyCost, Danger dangerLevel)
            {
                Destination = to;
                Distance = distance;
                EngergyCost = engergyCost;
                DangerLevel = dangerLevel;
            }
        }
            public Dictionary<string, List<Edge>> Graph { get; set; }

            public MapGeneration()
            {
                Graph = new Dictionary<string, List<Edge>>();
            }
        }
    }