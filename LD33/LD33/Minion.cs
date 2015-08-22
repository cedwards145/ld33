using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAGameLibrary.Pathfinding;

namespace LD33
{
    public class Minion
    {
        protected Point position;
        protected Texture2D graphic;
        protected Map containingMap;

        protected Vector2 offset;
        protected Point moveDirection;
        protected bool moving = false;
        protected float speed = 0.25f;
        protected Path path;

        public Minion()
        {
            graphic = Game1.gameRef.Content.Load<Texture2D>(@"graphics\mob");
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (containingMap != null)
                spriteBatch.Draw(graphic,
                                 new Rectangle((int)(position.X * containingMap.getTileWidth() + offset.X),
                                               (int)(position.Y * containingMap.getTileHeight() + offset.Y),
                                               graphic.Width, graphic.Height),
                                 Color.White);
        }

        public void update()
        {
            // If moving, update movement
            if (moving)
            {
                offset.X += (moveDirection.X * speed);
                offset.Y += (moveDirection.Y * speed);

                // Moved a whole tile, stop and change position
                if (Math.Abs(offset.X) > containingMap.getTileWidth() || Math.Abs(offset.Y) > containingMap.getTileHeight())
                {
                    offset = new Vector2(0, 0);
                    moving = false;
                    position.X += moveDirection.X;
                    position.Y += moveDirection.Y;
                }
            }
            // If not moving, check path for moves
            else if (path != null && path.Count > 0)
            {
                Tile tile;
                do {
                    tile = (Tile)path.Dequeue();
                } while (tile.position == position);

                if (tile.position != position)
                    move(new Point(tile.position.X - position.X,
                                   tile.position.Y - position.Y));
            }
        }

        public void move(Point direction)
        {
            if (!moving)
            {
                Point destination = new Point(position.X + direction.X, position.Y + direction.Y);

                if (containingMap.getTilePassability(destination.X, destination.Y))
                {
                    moveDirection = direction;
                    moving = true;
                }
            }
        }

        public void setPath(Path newPath)
        {
            path = newPath;
        }

        public void setPosition(Point newPos)
        {
            position = newPos;
        }

        public void setPosition(int x, int y)
        {
            position = new Point(x, y);
        }

        public void setContainingMap(Map value)
        {
            containingMap = value;
        }
    }
}
