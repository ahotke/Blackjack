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
        Rectangle sourceRect, logoRect, playRect;

        enum Screen {intro, casino, end};

        Screen screen;

        List<Texture2D> cardTextures;
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

            screen = Screen.casino;
            cardTextures = new List<Texture2D>();

            logoRect = new Rectangle(240, 0, 325, 325);
            playRect = new Rectangle(272, 325, 250, 100);

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
        }

        protected override void LoadContent()
        {
            cardSpritesheet = Content.Load<Texture2D>("card_deck");
            logoTexture = Content.Load<Texture2D>("blackjack_symbol");
            playTexture = Content.Load<Texture2D>("play_button");
           
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            



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
            }

            if (screen == Screen.casino)
            {
                for (int i = 0; i < cardTextures.Count; i++)
                    _spriteBatch.Draw(cardTextures[i], new Rectangle((0 + 60 * i), 0, 60, 100), Color.White);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
