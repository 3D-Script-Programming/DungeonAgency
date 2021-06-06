public class DungeonRoom
{
    Character[] monsters = new Character[6];
    Item item = Item.NONE;

    public void PlaceMonster(int index, Character monster)
    {
        monsters[index] = monster;
    }

    public void RemoveMonster(int index)
    {
        monsters[index] = null;
    }

    public void PlaceItem(Item item)
    {
        this.item = item;
    }

    public void RemoveItem()
    {
        item = Item.NONE;
    }
}
