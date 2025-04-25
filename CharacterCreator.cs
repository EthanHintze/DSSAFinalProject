namespace HerosQuest
{
    class CharacterCreator
    {
        public  int _strength { get; set; }
        public  int _agility { get; set; }
        public  int _intellegence { get; set; }
        public  int _health { get; set; }
        public  Queue<Item> inventory= new Queue<Item>();

        public CharacterCreator(int strength, int agility, int intellegence)
        {
            _strength = strength;
            _agility = agility;
            _intellegence = intellegence;
            _health = 3;
        }

    }
}