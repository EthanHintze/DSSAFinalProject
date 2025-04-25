namespace HerosQuest
{
    public class Challenge
    {
        public List<string> ChallengeType = ["Combat", "Puzzle", "Trap", "Locked" ];
        private Random random = new Random();
        public string Type { get; set; }
        public int Difficulty { get; set; }
        public string RequiredItem { get; set; }
        public int RequiredStat { get; set; }

        public Challenge(int type, int difficulty, int requiredStat, string requiredItem)
        {
            Type = ChallengeType[type];
            Difficulty = difficulty;
            RequiredStat = requiredStat;
            RequiredItem = requiredItem;
        }

    }

    public class ChallengeNode
    {
        public Challenge Challenge {get; set;}
        public ChallengeNode Left, Right;
        public int Height {get; set;}

        public ChallengeNode(Challenge challenge)
        {
            Challenge = challenge;
            Height = 1;
            
        }
    }


    public class ChallengeTree
    {

        public ChallengeNode root;

        public void Insert(Challenge challenge)
        {
            root = privInsert(root, challenge);
        }

        private ChallengeNode privInsert(ChallengeNode node, Challenge challenge)
        {
            if (node == null)
                return new ChallengeNode(challenge);

            if (challenge.Difficulty < node.Challenge.Difficulty)
                node.Left = privInsert(node.Left, challenge);
            else
                node.Right = privInsert(node.Right, challenge);

            UpdateHeight(node);
            return Balance(node);
        }

        private ChallengeNode Balance(ChallengeNode node)
        {
            int balance = GetBalance(node);

            if (balance > 1)
            {
                if (GetBalance(node.Left) >= 0)
                    return RotateRight(node);           
                else
                {
                    node.Left = RotateLeft(node.Left);  
                    return RotateRight(node);
                }
            }

            if (balance < -1)
            {
                if (GetBalance(node.Right) <= 0)
                    return RotateLeft(node);            
                else
                {
                    node.Right = RotateRight(node.Right); 
                    return RotateLeft(node);
                }
            }

            return node; 
        }

        private ChallengeNode RotateRight(ChallengeNode y)
        {
            ChallengeNode left = y.Left;
            ChallengeNode right = left.Right;

            left.Right = y;
            y.Left = right;

            UpdateHeight(y);
            UpdateHeight(left);

            return left;
        }

        private ChallengeNode RotateLeft(ChallengeNode x)
        {
            ChallengeNode right = x.Right;
            ChallengeNode left = right.Left;

            right.Left = x;
            x.Right = left;

            UpdateHeight(x);
            UpdateHeight(right);

            return right;
        }

        private void UpdateHeight(ChallengeNode node)
        {
            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        }

        private int GetHeight(ChallengeNode node) => node?.Height ?? 0;
        private int GetBalance(ChallengeNode node) => GetHeight(node.Left) - GetHeight(node.Right);

        public Challenge FindClosest(int roomNumber)
        {
            return FindClosest(root, roomNumber, root.Challenge);
        }

        private Challenge FindClosest(ChallengeNode node, int roomNumber, Challenge closest)
        {
            if (node == null) return closest;

            if (Math.Abs(node.Challenge.Difficulty - roomNumber) < Math.Abs(closest.Difficulty - roomNumber))
            {
                closest = node.Challenge;
            }

            if (roomNumber < node.Challenge.Difficulty)
            {   
                return FindClosest(node.Left, roomNumber, closest);
            }
            else
            {   
                return FindClosest(node.Right, roomNumber, closest);
            }
        }
    }


}