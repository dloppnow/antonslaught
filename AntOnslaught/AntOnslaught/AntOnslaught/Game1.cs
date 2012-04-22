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
        MouseState prevMState;
        Vector2 sizeOfScreen;
        Vector2 currentMapLoc; //in pixels
        Vector2 mouseLeftPressed;
        bool leftReleased = true;
        bool leftPressed = false;
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
        int workerCost = 0;
        int soldierCost = 20;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
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
            rend = new Renderer(spriteBatch, GraphicsDevice.Viewport, new Vector2(0, 0), Content);
            audioManager = new AudioManager(Content);
			menu = new Menu(spriteBatch, Content);
            currentMapLoc = sizeOfScreen / 2 + rend.getViewCenter() * 32;
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

            // TODO: Add your update logic here
            base.Update(gameTime);
            updateMenuState(gameTime);
            if (menu.shouldQuit())
                this.Exit();
            if (!menu.isPaused())
            {
                updateGameState(gameTime);
                updateGUI(gameTime);
            }
            prevMState = mouseState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            if (menu.isPaused())
            {
                drawMenuState();
            }
            else
            {
                drawGameState();
                drawGUI();
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
                        obj.setPath(map.getPath(obj.getCurrentCell(), obj.getGoalCell()));
                    }
                }
                if (obj is Ant)
                {
                    if (obj.hasFood() && map.getCell((int)obj.getPosition().X / 32, (int)obj.getPosition().Y / 32).Equals(foodDeliveryCell))
                    {
                        amountOfFood += obj.getCurrentFood();
                        obj.setCurrentFood(0);
                        if (obj.getFoodCell() == null)
                        {
                            if (obj is SolderAnt)
                            {
                                obj.setPath(map.getPath(obj.getCurrentCell(), soldierWaypoint));
                            }
                            if (obj is WorkerAnt)
                            {
                                obj.setPath(map.getPath(obj.getCurrentCell(), workerWaypoint));
                            }
                        }
                    }
                    if (!obj.hasPath() && obj.hasFood() && obj is WorkerAnt)
                    {
                        obj.setGoalCell(foodDeliveryCell);
                        obj.setPath(map.getPath(obj.getCurrentCell(), foodDeliveryCell));
                    }
                    if (!obj.hasPath() && obj.getFoodCell() != null && obj is WorkerAnt)
                    {
                        if (obj.getFoodCell() != null && obj.getFoodCell().food != null && obj.getFoodCell().food.getAmountOfFoodLeft() >= 0)
                        {
                            obj.setGoalCell(obj.getFoodCell());
                            obj.setPath(map.getPath(obj.getCurrentCell(), obj.getFoodCell()));
                        }
                        else
                        {
                            if (!workerWaypoint.occupied)
                            {
                                obj.setGoalCell(workerWaypoint);
                            }
                            else
                            {
                                Cell c = map.findUnoccupiedClosestCell(workerWaypoint);
                                if (c != null)
                                {
                                    obj.setGoalCell(c);
                                }
                            }
                            obj.setPath(map.getPath(obj.getCurrentCell(), obj.getGoalCell()));
                        }
                    }
                    if (obj is QueenAnt)
                    {
                        QueenAnt ant = (QueenAnt)obj;
                        ant.update(gameTime);
                    }
                    else
                    {
                        Ant ant = (Ant)obj;
                        ant.update(gameTime);
                    }
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
                                closeAnt = (Ant)mObj;
                                break;
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
                                        toKill.Add(a);
                                    }
                                }
                            }
                            else
                            { //Spider is within aggroRange but not in AttackRange
                                obj.setGoalCell(a.getCurrentCell());
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
                movableObjs.Remove(obj);
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
                            if (bounding.Contains(mouseSelectBox.Center))
                            {
                                selectedAnts.Add(anAnt);
                            }
                        }
                        else if (mouseSelectBox.Contains(bounding.Center))
                        {
                            selectedAnts.Add(anAnt);
                        }
                    }
                }
            }
            updateFoodTimers(gameTime);
            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
            {
                Vector2 vec = rend.getViewCenter();
                rend.setViewCenter(new Vector2(vec.X - 1, vec.Y));
                currentMapLoc.X += 32;
            }
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
            {
                Vector2 vec = rend.getViewCenter();
                rend.setViewCenter(new Vector2(vec.X + 1, vec.Y));
                currentMapLoc.X -= 32;
            }
            if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
            {
                Vector2 vec = rend.getViewCenter();
                rend.setViewCenter(new Vector2(vec.X, vec.Y - 1));
                currentMapLoc.Y += 32;
            }
            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
            {
                Vector2 vec = rend.getViewCenter();
                rend.setViewCenter(new Vector2(vec.X, vec.Y + 1));
                currentMapLoc.Y -= 32;
            }
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
            audioManager.playEffects();
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
                        List<Cell> cells = map.getAdjacentCells(foodDeliveryCell);
                        int i = 0;
                        while (i < cells.Count && cells[i].passable == false)
                        {
                            i++;
                        }
                        if (i < cells.Count && amountOfFood >= workerCost)
                        { //found good spot, make a new ant
                            Ant a = new WorkerAnt(new Vector2(cells[i].coord.X, cells[i].coord.Y), new SpriteAnimation(Content.Load<Texture2D>("worker_sprite_sheet"), 32, 32, 100));
                            a.setGoalCell(workerWaypoint);
                            a.setCurrentCell(cells[i]);
                            a.setPath(map.getPath(a.getCurrentCell(), a.getGoalCell()));
                            movableObjs.Add(a);

                            amountOfFood -= workerCost;
                        }
                        else
                        { //no good spot to spawn new ant.

                        }
                    }
                }
                //Soldier Ant Button
                if (mouseState.X >= soldierButton.Left && mouseState.X <= soldierButton.Right)
                {
                    if (mouseState.Y >= soldierButton.Top && mouseState.Y <= soldierButton.Bottom)
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
                        }
                        else
                        { //no good spot to spawn new ant.

                        }
                    }
                }
            }
        }

        public void drawGUI()
        {
            int numWorkers = 0;
            int numSoldiers = 0;
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
            spriteBatch.Draw(dummyTexture, workerButton, Color.Orange);
            spriteBatch.DrawString(font, "Worker(Cost=" + workerCost + "): " + numWorkers, new Vector2(workerButton.X, workerButton.Y), Color.White);
            spriteBatch.Draw(dummyTexture, soldierButton, Color.Orange);
            spriteBatch.DrawString(font, "Soldiers(Cost=" + soldierCost + "): " + numSoldiers, new Vector2(soldierButton.X, soldierButton.Y), Color.White);
            spriteBatch.DrawString(font, "Nutrients: " + amountOfFood, new Vector2(resourceBox.X, resourceBox.Y), Color.White);
            spriteBatch.End();
        }
    }
}
