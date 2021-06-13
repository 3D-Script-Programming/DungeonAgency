public class DungeonRoom
{
    private Character[] monsters = new Character[6]; //012 : 전열 345 후열
    private Item item = Item.NONE;

    public Character[] Monsters { get => monsters; set => monsters = value; }
    public Item Item { get => item; set => item = value; }

    public void PlaceMonster(int index, Character monster)
    {
        monsters[index] = monster;
    }

    public void RemoveMonster(int index)
    {
        monsters[index] = null;
    }

    public void RemoveMonster(Character monster) 
    {
        for (int i = 0; i < monsters.Length; i++)
        {
            if (monster == monsters[i]) {
                monsters[i] = null;
            }
        }
    }

    public void PlaceItem(Item item)
    {
        this.item = item;
    }

    public void RemoveItem()
    {
        item = Item.NONE; 
    }

    public Item GetItem() {
        return item;
    }
}
