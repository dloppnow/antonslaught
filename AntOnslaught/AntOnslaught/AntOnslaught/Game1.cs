using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AntOnslaught
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        AudioManager audioManager;
        Renderer rend;
        Map map;
        List<MovableObject> movableObjs;
        Menu menu;
        KeyboardState keyState;
        MouseState mouseState;
        KeyboardState prevKBState;
        MouseState prevMState;
        Vector2 sizeOfScreen;
        Vector2 currentMapLoc; //in pixels
        Vector2 mouseLeftPressed;
        bool leftReleased = true;
        bool leftPressed = false;
        bool gameOver = false;
        bool win = false;
        List<Ant> selectedAnts;
        //GUI members
        SpriteFont font;
        Rectangle background;
        Rectangle workerButton;
        Rectangle soldierButton;
        Rectangle resourceBox;
		Texture2D dummyTexture;	
	
        Cell foodDeliveryCell;
        Cell soldierWaypoint;
        Cell workerWaypoint;
        int amountOfFood = 0;
        int workerCost = 3;
        int soldierCost = 8;
        int numWorkers = 1;
        int numSoldiers = 1;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.ApplyChanges();
            sizeOfScreen.Y = graphics.PreferredBackBufferHeight;
            sizeOfScreen.X = graphics.PreferredBackBufferWidth;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            movableObjs = new List<MovableObject>();
            selectedAnts = new List<Ant>();
            //movableObjs.Add(new WorkerAnt(new Vector2(0, 0), new SpriteAnimation(Content.Load<Texture2D>("worker_sprite_sheet"), 32, 32, 100)));
            //movableObjs.Add(new WorkerAnt(new Vector2(3, 3), new SpriteAnimation(Content.Load<Texture2D>("worker_sprite_sheet"), 32, 32, 100)));
            //movableObjs.Add(new WorkerAnt(new Vector2(3, 1), new SpriteAnimation(Content.Load<Texture2D>("worker_sprite_sheet"), 32, 32, 100)));
            //movableObjs.Add(new WorkerAnt(new Vector2(1, 3), new SpriteAnimation(Content.Load<Texture2D>("worker_sprite_sheet"), 32, 32, 100)));
            base.Initialize();
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            map = new Map(Content);
            movableObjs.AddRange(map.getNewObjects());
            foreach (MovableObject obj in movableObjs)
            {
                if (obj is QueenAnt)
                {
                    foodDeliveryCell = map.getCell((int)obj.getPosition().X / 32, (int)obj.getPosition().Y / 32);
                    break;
                }
            }
            soldierWaypoint = map.getSoldierWaypoint();
            workerWaypoint = map.getWorkerWaypoint();
            map.setTexture(Content.Load<Texture2D>("tile_sheet"));
            rend = new Renderer(spriteBatch, GraphicsDevice.Viewport, new Vector2(20, 40), Content);
            audioManager = new AudioManager(Content);
			menu = new Menu(spriteBatch, Content);
            currentMapLoc = sizeOfScreen / 2 - rend.getViewCenter() * 32;
            currentMapLoc.X -= 16;
            currentMapLoc.Y -= 16;

            //Initialize GUI members
            font = Content.Load<SpriteFont>("Font");
            dummyTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });
            background = new Rectangle(0, GraphicsDevice.Viewport.Height - 50, GraphicsDevice.Viewport.Width, 50);
            workerButton = new Rectangle(12, GraphicsDevice.Viewport.Height - 37, 250, 25);
            soldierButton = new Rectangle(272, GraphicsDevice.Viewport.Height - 37, 250, 25);
            resourceBox = new Rectangle(534, GraphicsDevice.Viewport.Height - 37, 250, 25);
            // TODO: use this.Content to load your game content here
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            
            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            audioManager.startTheme();
            audioManager.update(gameTime);
            audioManager.playEffects();

            // TODO: Add your update logic here
            base.Update(gameTime);
            updateMenuState(gameTime);
            if (menu.shouldQuit())
                this.Exit();
            if (!menu.isPaused())
            {
                if (gameOver == false)
                {
                    updateGameState(gameTime);
                    updateGUI(gameTime);
                }
            }
            prevMState = mouseState;
            prevKBState = keyState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            if (menu.isPaused())
            {
                drawMenuState();
            }
            else
            {
                drawGameState();
                drawGUI();
                if (gameOver == true)
                {
                    drawGameOver();
                }
            }
            base.Draw(gameTime);
        }

        public void updateGameState(GameTime gameTime)
        {
            List<MovableObject> toKill = new List<MovableObject>();
            foreach (MovableObject obj in movableObjs)
            {
                if (obj.getCurrentCell() == null)
                {
                    obj.setCurrentCell(map.getCell((int)obj.getPosition().X / 32, (int)obj.getPosition().Y / 32));
                }
                if (!obj.updateMovement(gameTime))
                {
                    if (obj.getGoalCell().passable)
                    {
                        obj.setPath(map.getPath(obj.getCurrentCell(), map.findUnoccupiedClosestCell(obj.getGoalCell())));
                    }
                }
                if (obj is WorkerAnt)
                {
                    if (obj.hasFood() && map.getCell((int)obj.getPosition().X / 32, (int)obj.getPosition().Y / 32).Equals(foodDeliveryCell))
                    {
                        amountOfFood += obj.getCurrentFood();
                        obj.setCurrentFood(0);
                        if (obj.getFoodCell() == null)
                        {
                            obj.setGoalCell(map.findUnoccupiedClosestCell(workerWaypoint));
                            obj.setPath(map.getPath(obj.getCurrentCell(), obj.getGoalCell()));
                        }
                    }
                    if (!obj.hasPath() && obj.hasFood())
                    {
                        obj.setGoalCell(foodDeliveryCell);
                        obj.setPath(map.getPath(obj.getCurrentCell(), foodDeliveryCell));
                        audioManager.queueRandomEffectType(AudioManager.EffectType.munch);
                    }
                    if (!obj.hasPath() && obj.getFoodCell() != null)
                    {
                        if (obj.getFoodCell() != null && obj.getFoodCell().food != null && obj.getFoodCell().food.getAmountOfFoodLeft() >= 0)
                        {
                            obj.setGoalCell(obj.getFoodCell());
                            obj.setPath(map.getPath(obj.getCurrentCell(), obj.getFoodCell()));
                        }
                        else
                        {
                            obj.setFoodByGoal(null);
                            obj.setGoalCell(map.findUnoccupiedClosestCell(workerWaypoint));
                            obj.setPath(map.getPath(obj.getCurrentCell(), obj.getGoalCell()));
                        }
                    }
                    else
                    {
                        Ant ant = (Ant)obj;
                        ant.update(gameTime);
                    }
                }
                else if (obj is QueenAnt)
                {
                    QueenAnt ant = (QueenAnt)obj;
                    ant.update(gameTime);
                }
                else if (obj is SolderAnt)
                {
                    SolderAnt ant = (SolderAnt)obj;
                    if (ant.getTarget() == null && ant.hasPath() == false)
                    {
                        Enemy closeEnemy = null;
                        foreach (MovableObject mObj in movableObjs)
                        {
                            if ((mObj is Enemy) && Math.Abs(Vector2.Distance(mObj.getPosition(), ant.getPosition())) <= ant.getAggroRange())
                            {
                                if (checkIfPassablePath(mObj.getCurrentCell(), ant.getCurrentCell()))
                                {
                                    closeEnemy = (Enemy)mObj;
                                }
                                break;
                            }
                        }
                        if (closeEnemy != null)
                        {
                            ant.setTarget(closeEnemy);
                        }
                    }
                    else if (ant.getTarget() != null)
                    { //Soldier has soemthing to attack
                        Enemy e = ant.getTarget();
                        if (Math.Abs(Vector2.Distance(e.getPosition(), ant.getPosition())) <= ant.getAggroRange())
                        { //target is within aggro range
                            if (Math.Abs(Vector2.Distance(e.getPosition(), ant.getPosition())) <= ant.getAttackRange())
                            { //Soldier is within attack range;
                                if (ant.canAttack())
                                { //attack if it can
                                    ant.attacked();
                                    e.setHealth(e.getHealth() - ant.getDamage());
                                    if (e.getHealth() <= 0)
                                    { //target has died
                                        ant.setTarget(null);
                                        toKill.Add(e);
                                    }
                                }
                            }
                            else
                            { //Soldier is within aggroRange but not in AttackRange

                                obj.setGoalCell(map.findUnoccupiedClosestCell(e.getCurrentCell()));
                                obj.setPath(map.getPath(ant.getCurrentCell(), ant.getGoalCell()));
                            }
                        }
                        else
                        { //current target in range
                            ant.setTarget(null);
                        }
                    }
                    ant.update(gameTime);
                }
                if (obj is Enemy)
                {
                    Enemy enemyObj = (Enemy)obj;
                    if (!enemyObj.hasPath() && enemyObj.getTarget() == null)
                    {
                        Ant closeAnt = null;
                        foreach (MovableObject mObj in movableObjs)
                        {
                            if ((mObj is Ant) && Math.Abs(Vector2.Distance(mObj.getPosition(), enemyObj.getPosition())) <= enemyObj.getAggroRange())
                            {
                                if (checkIfPassablePath(mObj.getCurrentCell(), enemyObj.getCurrentCell()))
                                {
                                    closeAnt = (Ant)mObj;
                                    break;
                                }
                            }
                        }

                        if (closeAnt == null)
                        { //no close ants to attack
                            obj.setGoalCell(map.getRandomCell(map.getCell((int)enemyObj.getCenterOfMovementBox().X, (int)enemyObj.getCenterOfMovementBox().Y), 5));
                            obj.setPath(map.getPath(obj.getCurrentCell(), obj.getGoalCell()));
                        }
                        else
                        { //there is a close ant to attack
                            enemyObj.setTarget(closeAnt);
                        }
                    }
                    else if (enemyObj.getTarget() != null)
                    { //SPider has something to attack
                        Ant a = enemyObj.getTarget();
                        if (Math.Abs(Vector2.Distance(a.getPosition(), enemyObj.getPosition())) <= enemyObj.getAggroRange()) 
                        { //target is within aggro range
                            if (Math.Abs(Vector2.Distance(a.getPosition(), enemyObj.getPosition())) <= enemyObj.getAttackRange())
                            { //Spider is within attack range;
                                if (enemyObj.canAttack())
                                { //attack if it can
                                    enemyObj.attacked();
                                    a.setHealth(a.getHealth() - enemyObj.getDamage());
                                    if (a.getHealth() <= 0)
                                    { //target has died
                                        enemyObj.setTarget(null);
                                        audioManager.queueRandomEffectType(AudioManager.EffectType.death_ant);
                                        toKill.Add(a);
                                    }
                                }
                            }
                            else
                            { //Spider is within aggroRange but not in AttackRange
                                enemyObj.setGoalCell(a.getCurrentCell());
                                obj.setPath(map.getPath(enemyObj.getCurrentCell(), enemyObj.getGoalCell()));
                            }
                        }
                        else 
                        { //current target in range
                            enemyObj.setTarget(null);
                        }
                    }
                    enemyObj.update(gameTime);
                }
            }

            foreach (MovableObject obj in toKill)
            {
                if(obj is Enemy)
                {
                    Food newfood = new Food();
                    if (obj is Spider)
                    {
                        newfood.setTexture(Content.Load<Texture2D>("spider_dead"));
                        newfood.setAmountOfFoodLeft(20);
                    }
                    else if (obj is Roach)
                    {
                        newfood.setTexture(Content.Load<Texture2D>("cockroach_dead"));
                        newfood.setAmountOfFoodLeft(10);
                    }
                    else if (obj is Beetle)
                    {
                        newfood.setTexture(Content.Load<Texture2D>("beetle_dead"));
                        newfood.setAmountOfFoodLeft(3);
                    }
                    obj.getCurrentCell().food = newfood;
                }
                movableObjs.Remove(obj);
                obj.getCurrentCell().occupied = false;
                if (obj is Ant)
                    selectedAnts.Remove((Ant)obj);
                if (obj is QueenAnt)
                {
                    gameOver = true;
                    win = false;
                }
            }
            
            if ( mouseState.RightButton == ButtonState.Pressed )
            {
                Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y);
                if ((mousePos.X > currentMapLoc.X) && (mousePos.Y > currentMapLoc.Y) &&
                    (mousePos.X < currentMapLoc.X + map.getWidth() * 32) && (mousePos.Y < currentMapLoc.Y + map.getHeight() * 32))
                {
                    Vector2 mapMousePos = new Vector2(mouseState.X - currentMapLoc.X, mouseState.Y - currentMapLoc.Y);
                    foreach (Ant ant in selectedAnts)
                    {
                        //Cell c = map.getCell((int)mapMousePos.X / 32, (int)mapMousePos.Y / 32);
                        if (ant.getGoalCell() != null)
                        {
                            ant.getGoalCell().occupied = false;
                        }
                        Cell desCell = map.getCell((int)mapMousePos.X / 32, (int)mapMousePos.Y / 32);
                        if (desCell.food != null)
                        {
                            ant.setFoodByGoal(desCell);
                            desCell = map.findUnoccupiedClosestCell(desCell);
                        }
                        else if (desCell.occupied && desCell.passable)
                        {
                            ant.setFoodByGoal(null);
                            desCell = map.findUnoccupiedClosestCell(desCell);
                        }
                        else
                        {
                            ant.setFoodByGoal(null);
                        }
                        if (desCell != null && desCell.passable)
                        {
                            ant.setGoalCell(desCell);
                            if (ant.getCurrentCell() == null)
                            {
                                ant.setCurrentCell(map.getCell((int)ant.getPosition().X / 32, (int)ant.getPosition().Y / 32));
                            }
                            ant.setPath(map.getPath(ant.getCurrentCell(), ant.getGoalCell()));
                        }
                    }
                }
            }
            if (mouseState.LeftButton == ButtonState.Pressed && leftReleased == true)
            {
                mouseLeftPressed = new Vector2(mouseState.X, mouseState.Y);
                leftPressed = true;
                leftReleased = false;
            }
            if (mouseState.LeftButton == ButtonState.Released && leftPressed == true)
            {
                leftPressed = false;
                leftReleased = true;
                selectedAnts.Clear();
                Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y);

                Vector2 mapMousePos = new Vector2(mouseState.X - currentMapLoc.X, mouseState.Y - currentMapLoc.Y);
                Vector2 mapMouseOldPos = new Vector2(mouseLeftPressed.X - currentMapLoc.X, mouseLeftPressed.Y - currentMapLoc.Y);
                Rectangle mouseSelectBox;
                //if (mapMouseOldPos.X > mapMousePos.X && mapMouseOldPos.Y > mapMousePos.Y)
                //{
                int recX = 0;
                int recY = 0;
                int recWidth = 0;
                int recHeight = 0;
                if (mapMouseOldPos.X - (int)mapMousePos.X < 0)
                {
                    recX = (int)mapMouseOldPos.X;
                    recWidth = (int)mapMousePos.X - (int)mapMouseOldPos.X;

                }
                else
                {
                    recX = (int)mapMousePos.X;
                    recWidth = (int)mapMouseOldPos.X - (int)mapMousePos.X;
                }
                if (mapMouseOldPos.Y - (int)mapMousePos.Y < 0)
                {
                    recY = (int)mapMouseOldPos.Y;
                    recHeight = (int)mapMousePos.Y - (int)mapMouseOldPos.Y;

                }
                else
                {
                    recY = (int)mapMousePos.Y;
                    recHeight = (int)mapMouseOldPos.Y - (int)mapMousePos.Y;
                }
                mouseSelectBox = new Rectangle(recX, recY, recWidth, recHeight);
                foreach (MovableObject ant in movableObjs)
                {
                    if (ant is Ant)
                    {
                        Ant anAnt = (Ant)ant;
                        Rectangle bounding = new Rectangle((int)ant.getPosition().X, (int)ant.getPosition().Y, 32, 32);
                        if (mouseSelectBox.Width == 0 && mouseSelectBox.Height == 0)
                        {
                            if (bounding.Contains(mouseSelectBox.Center) && selectedAnts.Count < 13)
                            {
                                selectedAnts.Add(anAnt);
                            }
                        }
                        else if (mouseSelectBox.Contains(bounding.Center) && selectedAnts.Count < 13)
                        {
                            selectedAnts.Add(anAnt);
                        }
                    }
                }
            }
            updateFoodTimers(gameTime);
            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
            {
                Vector2 oldCenter = rend.getViewCenter();
                rend.setViewCenter(new Vector2(oldCenter.X - 1, oldCenter.Y));
                if (isGoodIndex(rend.getViewCenter()))
                {
                    currentMapLoc.X += 32;
                }
                else {
                    rend.setViewCenter(oldCenter);
                }
            }
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
            {
                Vector2 oldCenter = rend.getViewCenter();
                rend.setViewCenter(new Vector2(oldCenter.X + 1, oldCenter.Y));
                if (isGoodIndex(rend.getViewCenter()))
                {
                    currentMapLoc.X -= 32;
                }
                else
                {
                    rend.setViewCenter(oldCenter);
                }
            }
            if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
            {
                Vector2 oldCenter = rend.getViewCenter();
                rend.setViewCenter(new Vector2(oldCenter.X, oldCenter.Y - 1));
                if (isGoodIndex(rend.getViewCenter()))
                {
                    currentMapLoc.Y += 32;
                }
                else
                {
                    rend.setViewCenter(oldCenter);
                }
            }
            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
            {
                Vector2 oldCenter = rend.getViewCenter();
                rend.setViewCenter(new Vector2(oldCenter.X, oldCenter.Y + 1));
                if (isGoodIndex(rend.getViewCenter()))
                {
                    currentMapLoc.Y -= 32;
                }
                else
                {
                    rend.setViewCenter(oldCenter);
                }
            }
            if (prevKBState.IsKeyUp(Keys.D1) && keyState.IsKeyDown(Keys.D1))
            { //Press worker button
                workerButtonPressed();
            }
            if (prevKBState.IsKeyUp(Keys.D2) && keyState.IsKeyDown(Keys.D2))
            { //Press soldier button
                soldierButtonPressed();
            }

            if (rend.getFoodLeft() <= 0)
            {
                //Game Over
                gameOver = true;
                win = true;
            }
            if (numWorkers == 0)
            {
                if (amountOfFood < workerCost)
                {
                    gameOver = true;
                    win = false;
                }
            }
        }

        public bool isGoodIndex(Vector2 vec)
        {
            if (vec.X >= 0 && vec.X < map.getWidth())
            {
                if (vec.Y >= 0 && vec.Y < map.getHeight())
                {
                    return true;
                }
            }
            return false;
        }

        private bool checkIfPassablePath(Cell c, Cell c2)
        {
            bool passablePath = true;
            Vector2 currentCellPos = c.coord;
            while (currentCellPos != c2.coord)
            {
                if (Math.Abs(currentCellPos.X - c2.coord.X) > Math.Abs(currentCellPos.Y - c2.coord.Y))
                {
                    //move X
                    if (currentCellPos.X < c2.coord.X)
                    {
                        currentCellPos.X++;
                    }
                    else
                    {
                        currentCellPos.X--;
                    }
                }
                else
                {
                    if (currentCellPos.Y < c2.coord.Y)
                    {
                        currentCellPos.Y++;
                    }
                    else
                    {
                        currentCellPos.Y--;
                    }
                }
                if (!map.getGrid()[(int)currentCellPos.X, (int)currentCellPos.Y].passable)
                {
                    passablePath = false;
                    break;
                }
            }
            return passablePath;
        }
        public void updateFoodTimers(GameTime timer)
        {
            foreach (Cell c in map.getGrid())
            {
                if (c.food != null)
                {
                    c.food.update(timer);
                }
            }
        }
        public void drawGameState()
        {
            
            rend.Draw(map);
            rend.DrawSelectionCircles(selectedAnts);
            foreach (MovableObject obj in movableObjs)
            {
                rend.Draw(obj);
            }
        }

        public void updateMenuState(GameTime gameTime)
        {
            menu.update(gameTime, keyState, mouseState);
        }

        public void drawMenuState()
        {
            menu.draw();
        }

        public void updateGUI(GameTime gameTime)
        {
            if (prevMState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed) //button was just clicked
            {
                //Worker Ant Button
                if (mouseState.X >= workerButton.Left && mouseState.X <= workerButton.Right)
                {
                    if (mouseState.Y >= workerButton.Top && mouseState.Y <= workerButton.Bottom)
                    {
                        workerButtonPressed();
                    }
                }
                //Soldier Ant Button
                if (mouseState.X >= soldierButton.Left && mouseState.X <= soldierButton.Right)
                {
                    if (mouseState.Y >= soldierButton.Top && mouseState.Y <= soldierButton.Bottom)
                    {
                        soldierButtonPressed();
                    }
                }
            }
        }

        public void soldierButtonPressed()
        {
            Cell c = map.findUnoccupiedClosestCell(foodDeliveryCell);
            if (c != null && amountOfFood >= soldierCost)
            { //found good spot, make a new ant
                Ant a = new SolderAnt(new Vector2(c.coord.X, c.coord.Y), new SpriteAnimation(Content.Load<Texture2D>("soldier_sprite_sheet"), 32, 32, 100));
                Cell d = map.findUnoccupiedClosestCell(soldierWaypoint);
                if (d != null)
                    a.setGoalCell(d);
                else
                    a.setGoalCell(soldierWaypoint);
                a.setCurrentCell(c);
                a.setPath(map.getPath(a.getCurrentCell(), a.getGoalCell()));
                movableObjs.Add(a);

                amountOfFood -= soldierCost;
                audioManager.queueRandomEffectType(AudioManager.EffectType.pop);
            }
            else
            { //no good spot to spawn new ant.

            }
        }

        public void workerButtonPressed()
        {
            
            Cell c = map.findUnoccupiedClosestCell(foodDeliveryCell);
            if (c != null && amountOfFood >= workerCost)
            { //found good spot, make a new ant
                Ant a = new WorkerAnt(new Vector2(c.coord.X, c.coord.Y), new SpriteAnimation(Content.Load<Texture2D>("worker_sprite_sheet"), 32, 32, 100),
                                                                           new SpriteAnimation(Content.Load<Texture2D>("crumb_sprite_sheet"), 32, 32, 100));
                Cell d = map.findUnoccupiedClosestCell(workerWaypoint);
                if (d != null)
                    a.setGoalCell(d);
                else
                    a.setGoalCell(workerWaypoint);
                a.setCurrentCell(c);
                a.setPath(map.getPath(a.getCurrentCell(), a.getGoalCell()));
                movableObjs.Add(a);

                amountOfFood -= workerCost;
                audioManager.queueRandomEffectType(AudioManager.EffectType.pop);
            }
            else
            { //no good spot to spawn new ant.

            }
        }

        public void drawGameOver()
        {
            Viewport vp = spriteBatch.GraphicsDevice.Viewport;
            spriteBatch.Begin();
            spriteBatch.DrawString(font, (win) ? "YOU WON!" : "YOU LOST!", new Vector2(vp.Width / 2 - 60, vp.Height / 2 - 60), Color.White);
            spriteBatch.End();
        }

        public void drawGUI()
        {
            numWorkers = 0;
            numSoldiers = 0;
            foreach (MovableObject obj in movableObjs)
            {
                if (obj is WorkerAnt)
                {
                    numWorkers++;
                }
                else if (obj is SolderAnt)
                {
                    numSoldiers++;
                }
            }
            spriteBatch.Begin();
            spriteBatch.Draw(dummyTexture, background, Color.Black);
            spriteBatch.Draw(dummyTexture, workerButton, Color.Gray);
            spriteBatch.DrawString(font, "Worker(Cost=" + workerCost + "): " + numWorkers, new Vector2(workerButton.X, workerButton.Y), Color.White);
            spriteBatch.Draw(dummyTexture, soldierButton, Color.Gray);
            spriteBatch.DrawString(font, "Soldiers(Cost=" + soldierCost + "): " + numSoldiers, new Vector2(soldierButton.X, soldierButton.Y), Color.White);
            spriteBatch.DrawString(font, "Nutrients: " + amountOfFood, new Vector2(resourceBox.X, resourceBox.Y), Color.White);
            spriteBatch.End();
        }
    }
}
