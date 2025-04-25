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

        public Challenge(int type, int difficulty, int requiredStat, string requiredItem = null)
        {
            Type = ChallengeType[type];
            Difficulty = difficulty;
            RequiredStat = requiredStat;
            RequiredItem = requiredItem;
        }

    }

    public class ChallengeNode
    {
        public Challenge Challenge;
        public ChallengeNode Left, Right;
        public int Height;

        public ChallengeNode(Challenge challenge)
        {
            Challenge = challenge;
            Height = 1;
            
        }
    }


    public class ChallengeTree
    {

        public ChallengeNode root;

        // Public Insert
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

        // Remove function with AVL rebalancing
        public void Remove(int difficulty)
        {
            root = Remove(root, difficulty);
        }

        private ChallengeNode Remove(ChallengeNode node, int difficulty)
        {
            if (node == null) return null;

            if (difficulty < node.Challenge.Difficulty)
                node.Left = Remove(node.Left, difficulty);
            else if (difficulty > node.Challenge.Difficulty)
                node.Right = Remove(node.Right, difficulty);
            else
            {
                // Node found
                if (node.Left == null || node.Right == null)
                {
                    node = (node.Left != null) ? node.Left : node.Right;
                }
                else
                {
                    ChallengeNode successor = GetMin(node.Right);
                    node.Challenge = successor.Challenge;
                    node.Right = Remove(node.Right, successor.Challenge.Difficulty);
                }
            }

            if (node == null) return null;

            UpdateHeight(node);
            return Balance(node);
        }

        // Get minimum node (in-order successor)
        private ChallengeNode GetMin(ChallengeNode node)
        {
            while (node.Left != null) node = node.Left;
            return node;
        }

        // Balance a node if necessary
        private ChallengeNode Balance(ChallengeNode node)
        {
            int balance = GetBalance(node);

            // Left heavy
            if (balance > 1)
            {
                if (GetBalance(node.Left) >= 0)
                    return RotateRight(node);           // LL
                else
                {
                    node.Left = RotateLeft(node.Left);  // LR
                    return RotateRight(node);
                }
            }

            // Right heavy
            if (balance < -1)
            {
                if (GetBalance(node.Right) <= 0)
                    return RotateLeft(node);            // RR
                else
                {
                    node.Right = RotateRight(node.Right); // RL
                    return RotateLeft(node);
                }
            }

            return node; // Already balanced
        }

        // Rotate right (LL case)
        private ChallengeNode RotateRight(ChallengeNode y)
        {
            ChallengeNode x = y.Left;
            ChallengeNode T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            UpdateHeight(y);
            UpdateHeight(x);

            return x;
        }

        // Rotate left (RR case)
        private ChallengeNode RotateLeft(ChallengeNode x)
        {
            ChallengeNode y = x.Right;
            ChallengeNode T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            UpdateHeight(x);
            UpdateHeight(y);

            return y;
        }

        // Update height of a node
        private void UpdateHeight(ChallengeNode node)
        {
            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        }

        private int GetHeight(ChallengeNode node) => node?.Height ?? 0;
        private int GetBalance(ChallengeNode node) => GetHeight(node.Left) - GetHeight(node.Right);

        // Find closest challenge
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