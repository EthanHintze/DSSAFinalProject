namespace HerosQuest
{
    class CharacterCreator
    {
        private static int _strength { get; set; }
        private static int _agility { get; set; }
        private static int _intellegence { get; set; }
        private static int _health { get; set; }
        private static Queue<string> _inventory= new Queue<string>();

        public CharacterCreator(int strength, int agility, int intellegence)
        {
            _strength = strength;
            _agility = agility;
            _intellegence = intellegence;
            _health = 20;
            _inventory.Enqueue("Sword");
            _inventory.Enqueue("Health Potion");
        }

    }
}