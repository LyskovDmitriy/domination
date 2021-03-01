namespace Domination.LevelLogic
{
    public class Tile
    {
        public TileType Type { get; private set; }


        public Tile(TileType type)
        {
            Type = type;
        }
    }
}
