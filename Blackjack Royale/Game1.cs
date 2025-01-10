using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Blackjack_Royale
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D cardSpritesheet;
        Texture2D cropTexture;
        Texture2D logoTexture;
        Texture2D playTexture;
        Texture2D coinFallingTexture;
        Texture2D coinPileTexture;


        Rectangle sourceRect, logoRect, playRect, coinAnimRect, coinAnimRect1, coinPileRect, coinPileRect1;

        MouseState mouseState;
        enum Screen {intro, casino, end};

        Screen screen;

        List<Texture2D> cardTextures;
        List<int> deck;
        List<int> playerHand;
        List<int> dealerHand;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            base.Initialize();

            screen = Screen.intro;
            cardTextures = new List<Texture2D>();

            deck = new List<int>();

            logoRect = new Rectangle(240, 0, 325, 325);
            playRect = new Rectangle(272, 325, 250, 100);
            coinAnimRect = new Rectangle(0, 0, 200, 450);
            coinAnimRect1 = new Rectangle();
            coinPileRect = new Rectangle(-40, 310, 350, 200);
            coinPileRect1 = new Rectangle(485, 310, 350, 200);

            int width = cardSpritesheet.Width / 13;
            int height = cardSpritesheet.Height / 5;
            for (int y = 0; y < 5; y++)
                for (int x = 0; x < 13; x++)
                {
                    sourceRect = new Rectangle(x * width, y * height, width, height);
                    Texture2D cropTexture = new Texture2D(GraphicsDevice, width, height);
                    Color[] data = new Color[width * height];
                    cardSpritesheet.GetData(0, sourceRect, data, 0, data.Length);
                    cropTexture.SetData(data);
                    if (cardTextures.Count < 55)
                        cardTextures.Add(cropTexture);
                }


            for (int i = 0; i < 5; i++)
            {
                for (int c = 0; c < 52; c++)
                    cardTextures.Add(cardTextures[c]);
            }

            for (int i = 0; i < 312; i++)
            {
                deck.Add(i);
            }

        }

        protected override void LoadContent()
        {
            cardSpritesheet = Content.Load<Texture2D>("card_deck");
            logoTexture = Content.Load<Texture2D>("blackjack_symbol");
            playTexture = Content.Load<Texture2D>("play_button");
            coinFallingTexture = Content.Load<Texture2D>("coins_falling");
            coinPileTexture = Content.Load<Texture2D>("coin_pile");
           
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouseState = Mouse.GetState();

            if (screen == Screen.intro)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (playRect.Contains(mouseState.Position))
                    {
                        screen = Screen.casino;
                    }

                }
            }

            

            



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkRed);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            if (screen == Screen.intro) 
            {
                _spriteBatch.Draw(logoTexture, logoRect, Color.White);
                _spriteBatch.Draw(playTexture, playRect, Color.White);
                //_spriteBatch.Draw(coinFallingTexture, coinAnimRect, Color.White);
                _spriteBatch.Draw(coinPileTexture, coinPileRect, Color.White);
                _spriteBatch.Draw(coinPileTexture, coinPileRect1, Color.White);
            }

            if (screen == Screen.casino)
            {

                //for (int i = 0; i < cardTextures.Count; i++)

                //_spriteBatch.Draw(cardTextures[deck[i]], new Rectangle((0 + 60 * i), 0, 60, 100), Color.White);

                // positions 52, 53 + 54 are all useless, should cycle 54 to the start of the deck and not recyle. Remove 52 + 53 
                _spriteBatch.Draw(cardTextures[deck[55]], new Rectangle(0, 0, 60, 100), Color.White);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
