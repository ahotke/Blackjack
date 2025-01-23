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
        Texture2D standTexture;
        Texture2D hitTexture;

        //Add previous mousestate

        int cardShuffle;
        int bet, money, dealerTotal, playerTotal, cardOffset = 0;

        bool lose = false, deal = true, stand = false, dealerDone = false, dealerDrawing = false, playing = false;

        Rectangle shuffleRect, tableRect, dealRect, bet10Rect, bet10NegRect, betMaxRect, betMinRect, hitRect, standRect;
        Rectangle sourceRect, logoRect, playRect, coinAnimRect, coinAnimRect1, coinPileRect, coinPileRect1;

        SpriteFont moneyFont, EndFont;

        MouseState mouseState, prevMouseState;
        enum Screen {intro, casino, end};

        Screen screen;

        List<Texture2D> cardTextures;
        List<Texture2D> playerCards;
        List<Texture2D> dealerCards;
        List<Texture2D> tempTextureHolder;
        List<int> deckValues;
        List<int> tempDeck;
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
            tempTextureHolder = new List<Texture2D>();

            deckValues = new List<int>();
            playerHand = new List<int>();
            dealerHand = new List<int>();
            tempDeck = new List<int>();

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
            hitRect = new Rectangle(525, 320, 100, 65);
            standRect = new Rectangle(525, 400, 100, 65);

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
                for (int o = 0; o < 13; o++)
                {
                    deckValues.Add(deckValues[o]);
                }
            }

            deckValues.Insert(0, 0);



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
            hitTexture = Content.Load<Texture2D>("hit_button");
            standTexture = Content.Load<Texture2D>("stand_button");

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            this.Window.Title = "Casino";

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
                        tempDeck.Add(deckValues[0]);
                        tempTextureHolder.Add(cardTextures[0]);

                        for (int i = 1; i < 302; i++)
                        {
                            cardShuffle = generator.Next(0, cardTextures.Count);
                            tempDeck.Add(deckValues[cardShuffle]);
                            tempTextureHolder.Add(cardTextures[cardShuffle]);
                        }

                        deckValues.Clear();
                        cardTextures.Clear();

                        deckValues.Add(tempDeck[0]);
                        cardTextures.Add(tempTextureHolder[0]);

                        for (int i = 1; i < 302; i++)
                        {
                            deckValues.Add(tempDeck[i]);
                            cardTextures.Add(tempTextureHolder[i]);
                        }

                        tempDeck.Clear();
                        tempTextureHolder.Clear();

                        deal = false;

                        playing = true;

                        playerHand.Add(deckValues[1]);
                        playerCards.Add(cardTextures[1]);
                        deckValues.Add(deckValues[1]);
                        deckValues.Remove(deckValues[1]);
                        cardTextures.Add(cardTextures[1]);
                        cardTextures.Remove(cardTextures[1]);
                        dealerHand.Add(deckValues[1]);
                        dealerCards.Add(cardTextures[0]);
                        deckValues.Add(deckValues[1]);
                        deckValues.Remove(deckValues[1]);
                        cardTextures.Add(cardTextures[1]);
                        cardTextures.Remove(cardTextures[1]);
                        playerHand.Add(deckValues[1]);
                        playerCards.Add(cardTextures[1]);
                        deckValues.Add(deckValues[1]);
                        deckValues.Remove(deckValues[1]);
                        cardTextures.Add(cardTextures[1]);
                        cardTextures.Remove(cardTextures[1]);
                        dealerHand.Add(deckValues[1]);
                        dealerCards.Add(cardTextures[1]);
                        deckValues.Add(deckValues[1]);
                        deckValues.Remove(deckValues[1]);
                        cardTextures.Add(cardTextures[1]);
                        cardTextures.Remove(cardTextures[1]);
                    }

                    if (hitRect.Contains(mouseState.Position) && playerTotal < 21)
                    {
                        playerHand.Add(deckValues[1]);
                        playerCards.Add(cardTextures[1]);
                        deckValues.Add(deckValues[1]);
                        deckValues.Remove(deckValues[1]);
                        cardTextures.Add(cardTextures[1]);
                        cardTextures.Remove(cardTextures[1]);
                    }

                    if (standRect.Contains(mouseState.Position) && dealerTotal !>= 17)
                    {
                        stand = true;
                        dealerDrawing = true;
                    }

                    if (dealerDone == true)
                    {
                        dealerDrawing = false;
                        playing = false;
                    }

                    if (dealerDrawing == true)
                    {
                        dealerHand.Add(deckValues[1]);
                        dealerCards.Add(cardTextures[1]);
                        deckValues.Add(deckValues[1]);
                        deckValues.Remove(deckValues[1]);
                        cardTextures.Add(cardTextures[1]);
                        cardTextures.Remove(cardTextures[1]);
                    }

                    if (dealerTotal >= 17)
                    {
                        dealerDone = true;
                    }

                    dealerTotal = dealerHand.Sum();
                    playerTotal = playerHand.Sum();

                    if (playerTotal == 21 && playerCards.Count == 2 && dealerTotal != 21)
                    {
                        money += (bet * 3);
                        bet = 0;
                        playerHand.Clear();
                        playerCards.Clear();
                        dealerHand.Clear();
                        dealerCards.Clear();
                    }

                    if (dealerTotal == 21 && dealerCards.Count == 2 && playerTotal != 21)
                    {
                        bet = 0;
                        playerHand.Clear();
                        playerCards.Clear();
                        dealerHand.Clear();
                        dealerCards.Clear();
                        dealerDone = true;
                    }

                    if (dealerTotal == 21 && dealerCards.Count == 2 && playerTotal == 21)
                    {
                        money += bet;
                        bet = 0;
                        dealerDone = true;
                    }

                    if (dealerTotal > 21)
                    {
                        for (int i = 0; i < dealerHand.Count; i++)
                        {
                            if (dealerHand[i] == 11)
                            {
                                dealerHand[i] = 1;
                            }
                        }

                        dealerTotal = dealerHand.Sum();
                    }

                    if (playerTotal > 21)
                    {
                        for (int i = 0; i < playerHand.Count; i++)
                        {
                            if (playerHand[i] == 11)
                            {
                                playerHand[i] = 1;
                            }
                        }

                        playerTotal = playerHand.Sum();

                        if (playerTotal > 21)
                        {
                            bet = 0;
                            playerHand.Clear();
                            playerCards.Clear();
                            dealerHand.Clear();
                            dealerCards.Clear();
                        }
                    }

                    if (stand == true && dealerDone == true)
                    {
                        if (dealerTotal > playerTotal)
                        {
                            bet = 0;
                            deal = true;
                        }

                        if (playerTotal > dealerTotal)
                        {
                            money += (bet * 2);
                            bet = 0;
                        }

                        if (dealerTotal == playerTotal)
                        {
                            money += bet;
                            bet = 0;
                        }

                        if (dealerTotal > 21)
                        {
                            money += (bet * 2);
                            bet = 0;
                        }
                    }

                    if (bet == 0 && money == 0)
                    {
                        screen = Screen.end;
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
                _spriteBatch.Draw(coinFallingTexture, coinAnimRect, Color.White);
                _spriteBatch.Draw(coinFallingTexture, coinAnimRect1, Color.White);
                _spriteBatch.Draw(coinPileTexture, coinPileRect, Color.White);
                _spriteBatch.Draw(coinPileTexture, coinPileRect1, Color.White);
            }

            if (screen == Screen.casino)
            {
                _spriteBatch.Draw(tableTexture, tableRect, Color.LightGray);
                _spriteBatch.Draw(cardTextures[0], new Rectangle(163, 40, 50, 75), Color.White);
                if (deal)
                {
                    _spriteBatch.Draw(dealBtnTexture, dealRect, Color.White);
                }
                _spriteBatch.DrawString(moneyFont, "Your bet: " + bet, new Vector2(5, 425), Color.White);
                _spriteBatch.DrawString(moneyFont, "Money: " + money, new Vector2(10, 450), Color.White);
                _spriteBatch.DrawString(moneyFont, "You have:" + playerTotal, new Vector2(335, 450), Color.White);
                _spriteBatch.DrawString(moneyFont, "Dealer Total: " + dealerTotal, new Vector2(100, 100), Color.White);
                _spriteBatch.Draw(bet10Texture, bet10Rect, Color.White);
                _spriteBatch.Draw(bet10NegTexture, bet10NegRect, Color.White);
                _spriteBatch.Draw(betMaxTexture, betMaxRect, Color.White);
                _spriteBatch.Draw(betMinTexture, betMinRect, Color.White);

                if (playing == true)
                {
                    _spriteBatch.Draw(hitTexture, hitRect, Color.White);
                    _spriteBatch.Draw(standTexture, standRect, Color.White);
                }
                
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
