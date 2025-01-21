using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Blackjack_Royale
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Random generator = new Random();

        Texture2D cardSpritesheet;
        Texture2D cropTexture;
        Texture2D logoTexture;
        Texture2D playTexture;
        Texture2D coinFallingTexture;
        Texture2D coinPileTexture;
        Texture2D shuffleBtnTexture;
        Texture2D tableTexture;
        Texture2D dealBtnTexture;
        Texture2D bet10Texture;
        Texture2D bet10NegTexture;
        Texture2D betMaxTexture;
        Texture2D betMinTexture;

        //Add previous mousestate

        int cardShuffle;
        int bet, money, dealerTotal, playerTotal, cardOffset = 0;

        bool lose = false;

        Rectangle shuffleRect, tableRect, dealRect, bet10Rect, bet10NegRect, betMaxRect, betMinRect;
        Rectangle sourceRect, logoRect, playRect, coinAnimRect, coinAnimRect1, coinPileRect, coinPileRect1;

        SpriteFont moneyFont, EndFont;

        MouseState mouseState, prevMouseState;
        enum Screen {intro, casino, end};

        Screen screen;

        List<Texture2D> cardTextures;
        List<Texture2D> playerCards;
        List<Texture2D> dealerCards;
        List<int> deck;
        List<int> tempDeck;
        List<int> deckValues;
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
            playerCards = new List<Texture2D>();
            dealerCards = new List<Texture2D>();

            deck = new List<int>();
            tempDeck = new List<int>();
            deckValues = new List<int>();
            playerHand = new List<int>();
            dealerHand = new List<int>();

            logoRect = new Rectangle(240, 0, 325, 325);
            playRect = new Rectangle(272, 325, 250, 100);
            coinAnimRect = new Rectangle(0, 0, 200, 450);
            coinAnimRect1 = new Rectangle(550, 0, 200, 450);
            coinPileRect = new Rectangle(-40, 310, 350, 200);
            coinPileRect1 = new Rectangle(485, 310, 350, 200);
            shuffleRect = new Rectangle(250, 250, 150, 75);
            tableRect = new Rectangle(100, 0, 600, 400);
            dealRect = new Rectangle(535, 35, 80, 40);
            bet10Rect = new Rectangle(650, 340, 60, 60);
            bet10NegRect = new Rectangle(720, 340, 60, 60);
            betMaxRect = new Rectangle(650, 410, 60, 60);
            betMinRect = new Rectangle(720, 410, 60, 60);

            moneyFont = Content.Load<SpriteFont>("moneyFont");
            EndFont = Content.Load<SpriteFont>("EndFont");

            int width = cardSpritesheet.Width / 13;
            int height = cardSpritesheet.Height / 5;
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 13; x++)
                {
                    
                    sourceRect = new Rectangle(x * width, y * height, width, height);
                    Texture2D cropTexture = new Texture2D(GraphicsDevice, width, height);
                    Color[] data = new Color[width * height];
                    cardSpritesheet.GetData(0, sourceRect, data, 0, data.Length);
                    cropTexture.SetData(data);
                    if (cardTextures.Count < 52)
                        cardTextures.Add(cropTexture);
                }
            }

            sourceRect = new Rectangle(2 * width, 4 * height, width, height);
            Texture2D cropTextureBack = new Texture2D(GraphicsDevice, width, height);
            Color[] dataBack = new Color[width * height];
            cardSpritesheet.GetData(0, sourceRect, dataBack, 0, dataBack.Length);
            cropTextureBack.SetData(dataBack);
            cardTextures.Insert(0, cropTextureBack);




            for (int i = 0; i < 5; i++)
            {
                for (int c = 1; c < 52; c++)
                    cardTextures.Add(cardTextures[c]);
            }

            for (int i = 0; i < 312; i++)
            {
                deck.Add(i);
            }

            deckValues.Add(0);
            deckValues.Add(11);
            deckValues.Add(2);
            deckValues.Add(3);
            deckValues.Add(4);
            deckValues.Add(5);
            deckValues.Add(6);
            deckValues.Add(7);
            deckValues.Add(8);
            deckValues.Add(9);
            deckValues.Add(10);
            deckValues.Add(10);
            deckValues.Add(10);
            deckValues.Add(10);
            
            for (int i = 0; i < 24; i++)
            {
                for (int o = 1; o < 13; o++)
                {
                    deckValues.Add(deckValues[o]);
                }
            }

            
            
            //0, 11, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10

            bet = 0;
            money = 1000;
        }

        protected override void LoadContent()
        {
            cardSpritesheet = Content.Load<Texture2D>("card_deck");
            logoTexture = Content.Load<Texture2D>("blackjack_symbol");
            playTexture = Content.Load<Texture2D>("play_button");
            coinFallingTexture = Content.Load<Texture2D>("coins_falling");
            coinPileTexture = Content.Load<Texture2D>("coin_pile");
            shuffleBtnTexture = Content.Load<Texture2D>("ShuffleButton");
            tableTexture = Content.Load<Texture2D>("table");
            dealBtnTexture = Content.Load<Texture2D>("deal_button");
            bet10Texture = Content.Load<Texture2D>("bet10_button");
            bet10NegTexture = Content.Load<Texture2D>("bet10neg_button");
            betMaxTexture = Content.Load<Texture2D>("betmax_button");
            betMinTexture = Content.Load<Texture2D>("betmin_button");

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            this.Window.Title = $"x = {mouseState.X}, y = {mouseState.Y}";

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

            if (screen == Screen.casino)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    if (bet10Rect.Contains(mouseState.Position) && money >= 10)
                    {
                        money -= 10;
                        bet += 10;
                    }

                    if (bet10NegRect.Contains(mouseState.Position) && bet >= 10)
                    {
                        money += 10;
                        bet -= 10;
                    }

                    if (betMaxRect.Contains(mouseState.Position) && money > 10)
                    {
                        bet += money;
                        money = 0;
                    }

                    if (betMinRect.Contains(mouseState.Position))
                    {
                        money += bet - 10;
                        bet = 10;
                    }

                    if (dealRect.Contains(mouseState.Position))
                    {
                        playerHand.Add(deckValues[1]);
                        playerCards.Add(cardTextures[1]);
                        deckValues.Insert(deckValues.Count, deckValues[1]);
                        deckValues.Remove(deckValues[1]);
                        cardTextures.Add(cardTextures[1]);
                        cardTextures.Remove(cardTextures[1]);
                        dealerHand.Add(deckValues[1]);
                        dealerCards.Add(cardTextures[0]);
                        deckValues.Insert(deckValues.Count, deckValues[1]);
                        deckValues.Remove(deckValues[1]);
                        playerHand.Add(deckValues[1]);
                        playerCards.Add(cardTextures[1]);
                        deckValues.Insert(deckValues.Count, deckValues[1]);
                        deckValues.Remove(deckValues[1]);
                        cardTextures.Add(cardTextures[1]);
                        cardTextures.Remove(cardTextures[1]);
                        dealerHand.Add(deckValues[1]);
                        dealerCards.Add(cardTextures[1]);
                        deckValues.Insert(deckValues.Count, deckValues[1]);
                        deckValues.Remove(deckValues[1]);
                        cardTextures.Add(cardTextures[1]);
                        cardTextures.Remove(cardTextures[1]);
                    }

                    //if (shuffleRect.Contains(mouseState.Position))
                    //{
                    //    //tempDeck.Add(deck[0]);
                    //    tempTextureHolder.Add(cardTextures[0]);

                    //    //for (int i = 1; 1 < 312; i++)
                    //    //{
                    //    //    cardShuffle = generator.Next(cardTextures.Count);
                    //    //    tempDeck.Add(deckValues[cardShuffle]);
                    //    //    tempTextureHolder.Add(cardTextures[cardShuffle+1]);
                    //    //}

                    //    //for (int i = 1; i < 312; i++)
                    //    //{
                    //    //    deck.Add(tempDeck[i]);
                    //    //    cardTextures.Add(tempTextureHolder[i]);
                    //    //}

                    //    tempDeck.Clear();
                    //    tempTextureHolder.Clear();

                        
                    //}

                    dealerTotal = dealerHand.Sum();
                    playerTotal = playerHand.Sum();

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
                _spriteBatch.Draw(coinFallingTexture, coinAnimRect, Color.White);
                _spriteBatch.Draw(coinFallingTexture, coinAnimRect1, Color.White);
                _spriteBatch.Draw(coinPileTexture, coinPileRect, Color.White);
                _spriteBatch.Draw(coinPileTexture, coinPileRect1, Color.White);
            }

            if (screen == Screen.casino)
            {
                _spriteBatch.Draw(tableTexture, tableRect, Color.LightGray);
                _spriteBatch.Draw(cardTextures[deck[0]], new Rectangle(163, 40, 50, 75), Color.White);
                _spriteBatch.Draw(dealBtnTexture, dealRect, Color.White);
                _spriteBatch.DrawString(moneyFont, "Your bet: " + bet, new Vector2(5, 425), Color.White);
                _spriteBatch.DrawString(moneyFont, "Money: " + money, new Vector2(10, 450), Color.White);
                _spriteBatch.DrawString(moneyFont, "You have:" + playerTotal, new Vector2(335, 450), Color.White);
                // _spriteBatch.Draw(shuffleBtnTexture, shuffleRect, Color.White); 
                _spriteBatch.Draw(bet10Texture, bet10Rect, Color.White);
                _spriteBatch.Draw(bet10NegTexture, bet10NegRect, Color.White);
                _spriteBatch.Draw(betMaxTexture, betMaxRect, Color.White);
                _spriteBatch.Draw(betMinTexture, betMinRect, Color.White);
                for (int i = 0; i < playerHand.Count; i++)
                {
                    cardOffset = i * 60;
                    _spriteBatch.Draw(playerCards[i], new Rectangle(410 - cardOffset, 365, 50, 75), Color.White);
                }

                for (int i = 0; i < dealerHand.Count; i++)
                {
                    cardOffset = i * 60;
                    _spriteBatch.Draw(dealerCards[i], new Rectangle(410 - cardOffset, 80, 50, 75), Color.White);
                }




            }

            if (screen == Screen.end && lose)
            {
                GraphicsDevice.Clear(Color.Black);
                _spriteBatch.DrawString(EndFont, "You Lose", new Vector2(200, 250), Color.Red);
                
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
