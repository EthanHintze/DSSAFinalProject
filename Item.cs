namespace HerosQuest
{
    class Item
    {
        public string _name { get; set; }
        public  int _strengthBuff { get; set; }
        public  int _agilityBuff { get; set; }
        public  int _intellegenceBuff { get; set; }
        public  int _price { get; set; }
        bool _isLP { get; set; }

        public Item(string name, int strength, int agility, int intellegence, int price, bool isLP)
        {
            _name = name;
            _strengthBuff = strength;
            _agilityBuff = agility;
            _intellegenceBuff = intellegence;
            _price = price;
            _isLP = isLP;
        }

    }
}