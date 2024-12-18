using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Blackjack.Content
{
    public class Card
    {
        private Texture2D _cardSheetTexture;
        private Rectangle _location;
        private int _cardNumber;

        public Card(Texture2D texture, int x, int y, int cardNum)
        {
            _cardSheetTexture = texture;
            _location =  new Rectangle (x, y, 72, 96);
            _cardNumber = cardNum;
        }

        public Texture2D Texture
        {
            get { return _cardSheetTexture; }
        }
        public Rectangle Location
        {
            get { return _location; }
        }
        public int CardNumber
        {
            get { return _cardNumber; }
        }


        public Rectangle sourceRectangle = new Rectangle(1, 1, 72, 96);
        
        int row = 0, column = 0;

        //public cards[] cards;

        public void addcard()
        {
            for (int i = 0; i < 53; i++)
            {
                // needs to add each card coordinate to a list, and assign an integer to each card
                
                int xOffset = (column * 73), yOffset = (row * 98);

                if (xOffset == 950)
                {
                    row += 1;
                    column = 0;
                }

                if (row == 4)
                {
                    column = 0;
                    row = 0;
                } 

                sourceRectangle = new Rectangle(1 + xOffset, 1 + yOffset, 72, 96);

                

            }
        }
            
    }
}
