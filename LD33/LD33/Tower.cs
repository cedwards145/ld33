using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAGameLibrary.Pathfinding;

namespace LD33
{
    public class Tower
    {
        protected Point position;
        protected Texture2D graphic;
        protected Map containingMap;
        protected int tileWidth = 0, tileHeight = 0;

        public Tower()
        {
            graphic = Game1.gameRef.Content.Load<Texture2D>(@"graphics\tower");
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (containingMap != null)
                spriteBatch.Draw(graphic,
                                 new Rectangle(position.X * tileWidth - ((graphic.Width - tileWidth) / 2),
                                               position.Y * containingMap.getTileHeight() - (graphic.Height - tileHeight),
                                               graphic.Width, graphic.Height),
                                 Color.White);
        }

        public void update()
        {

        }

        public void setPosition(Point newPos)
        {
            position = newPos;
        }

        public void setPosition(int x, int y)
        {
            position = new Point(x, y);
        }

        public Point getPosition()
        {
            return position;
        }

        public void setContainingMap(Map value)
        {
            containingMap = value;
            tileHeight = containingMap.getTileHeight();
            tileWidth = containingMap.getTileWidth();
        }
    }
}
