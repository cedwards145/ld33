using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;
using XNAGameLibrary.Pathfinding;
using XNAGameLibrary.Pathfinding.AStar;

namespace LD33
{
    public class Map
    {
        private TmxMap map;
        private Texture2D tileset;
        private int tileWidth, tileHeight;

        private int scale = 1;

        private int width, height;
        private TmxLayer passabilityLayer;

        private List<Minion> minions;
        private List<Tower> towers;

        private Point spawnPoint = new Point(9, 14);
        private Point destinationPoint = new Point(5, 0);
        private Path minionPath;

        // Used exclusively for pathfinding
        private Dictionary<Tuple<int, int>, Tile> tiles;

        public Map(string filename)
        {
            map = new TmxMap(filename);
            tileset = Game1.gameRef.Content.Load<Texture2D>("graphics\\" + map.Tilesets[0].Name);
            tileWidth = map.TileWidth;
            tileHeight = map.TileHeight;

            width = map.Width;
            height = map.Height;

            minions = new List<Minion>();
            towers = new List<Tower>();

            generateTiles();

            calculateMinionPath();
        }

        private void calculateMinionPath()
        {
            minionPath = findPath(spawnPoint, destinationPoint);
        }

        private void generateTiles()
        {
            tiles = new Dictionary<Tuple<int, int>, Tile>();

            // Create tile objects
            bool passLayer = map.Layers.Contains("pass");

            if (passLayer)
            {
                passabilityLayer = map.Layers["pass"];
                Tile tile;
                for (int index = 0; index < passabilityLayer.Tiles.Count; index++)
                {
                    int x = index % width;
                    int y = index / width;

                    tile = new Tile(new Point(x, y), (passabilityLayer.Tiles[index].Gid == 0));
                    tiles[new Tuple<int, int>(x, y)] = tile;
                }

                foreach (Tuple<int, int> key in tiles.Keys)
                {
                    generateNeighbours(key.Item1, key.Item2);
                }
            }
            else
            {
                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                        tiles[new Tuple<int, int>(x, y)] = new Tile(new Point(x, y), true);
            }
        }

        private void generateNeighbours(int x, int y)
        {
            Tile tile = tiles[new Tuple<int, int>(x, y)];

            Tuple<int, int> next;

            next = new Tuple<int, int>(x - 1, y);
            if (x - 1 >= 0 && tiles[next].passable)
                tile.neighbours.Add(tiles[next]);

            next = new Tuple<int, int>(x + 1, y);
            if (x + 1 < width && tiles[next].passable)
                tile.neighbours.Add(tiles[next]);

            next = new Tuple<int, int>(x, y - 1);
            if (y - 1 >= 0 && tiles[next].passable)
                tile.neighbours.Add(tiles[next]);

            next = new Tuple<int, int>(x, y + 1);
            if (y + 1 < height && tiles[next].passable)
                tile.neighbours.Add(tiles[next]);
        }

        public void update()
        {
            foreach (Minion minion in minions)
                minion.update();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            TmxLayer currentLayer;
            TmxLayerTile currentTile;
            for (int layer = 0; layer < map.Layers.Count; layer++)
            {
                currentLayer = map.Layers[layer];
                if (currentLayer.Visible)
                {
                    for (int tile = 0; tile < currentLayer.Tiles.Count; tile++)
                    {
                        currentTile = currentLayer.Tiles[tile];

                        if (currentTile.Gid != 0)
                        {
                            // All tile ids are offset by 1
                            int tileId = currentTile.Gid - 1;

                            int column = tileId % (tileset.Width / tileWidth);
                            int row = tileId / (tileset.Height / tileHeight);

                            int x = (int)(tile % width) * tileWidth;
                            int y = (int)(tile / width * tileHeight);

                            spriteBatch.Draw(tileset,
                                             new Rectangle(x * scale, y * scale, tileWidth * scale, tileHeight * scale),
                                             new Rectangle(column * tileWidth, row * tileHeight, tileWidth, tileHeight),
                                             Color.White);
                        }
                    }
                }
            }

            foreach (Minion minion in minions)
                minion.draw(spriteBatch);

            foreach (Tower tower in towers)
                tower.draw(spriteBatch);
        }

        public void addMinion(Minion minion)
        {
            minions.Add(minion);
            minion.setContainingMap(this);
        }

        public void addTower(Tower tower)
        {
            towers.Add(tower);
            tower.setContainingMap(this);

            Point pos = tower.getPosition();

            Tile tile = tiles[new Tuple<int, int>(pos.X, pos.Y)];
            tile.passable = false;
            foreach (Tile neighbour in tile.GetNeighbours())
            {
                neighbour.neighbours.Remove(tile);
            }
            calculateMinionPath();
        }

        public int getWidth()
        {
            return width;
        }

        public int getHeight()
        {
            return height;
        }

        public int getTileWidth()
        {
            return tileWidth;
        }

        public int getTileHeight()
        {
            return tileHeight;
        }

        public bool getTilePassability(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                return false;

            return tiles[new Tuple<int, int>(x, y)].passable;
        }

        public Path findPath(Point start, Point end)
        {
            return AStarPathfinder.findPath(tiles[new Tuple<int, int>(start.X, start.Y)],
                                            tiles[new Tuple<int, int>(end.X, end.Y)]);
        }

        public void spawnMinion()
        {
            Minion minion = new Minion();
            minion.setPosition(spawnPoint);
            addMinion(minion);
            minion.setPath(minionPath.Clone());
        }
    }
}
