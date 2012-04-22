using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace AntOnslaught
{
    abstract class MovableObject : Drawable
    {
        public enum Direction
        {
            UP,
            DOWN,
            LEFT,
            RIGHT, 
            UPRIGHT,
            UPLEFT,
            DOWNRIGHT,
            DOWNLEFT
        }
        protected double direction;
        protected List<Cell> curPath = new List<Cell>();
        protected Vector2 position;
        protected Cell currentCell = null;
        protected Cell goalCell = null;
        protected Boolean isMoving = false;
        protected float speed = 0.5f;
        protected Cell foodCell = null;
        protected int amountOfFoodCarrying = 0;
        protected bool canCarryFood = true;
        public bool hasFood()
        {
            bool hasFood = false;
            if (amountOfFoodCarrying > 0)
            {
                hasFood = true;
            }
            return hasFood;
        }
        public void setFoodByGoal(Cell foodCell)
        {
            this.foodCell = foodCell;
        }
        public Cell getCurrentCell()
        {
            return currentCell;
        }
        public int getCurrentFood()
        {
            return amountOfFoodCarrying;
        }
        public void setCurrentFood(int foodAmount)
        {
            amountOfFoodCarrying = foodAmount;
        }
        public Cell getFoodCell()
        {
            return foodCell;
        }
        public void setCurrentCell(Cell c)
        {
            currentCell = c;
        }
        public Cell getGoalCell()
        {
            return goalCell;
        }
        public void setGoalCell(Cell c)
        {
            if (goalCell != null)
            {
                goalCell.occupied = false;
            }
            goalCell = c;
            goalCell.occupied = true;
        }
        public double getDirection()
        {
            return direction;
        }
        public Vector2 getPosition()
        {
            return position;
        }
        public bool updateMovement(GameTime timer)
        {
            bool canMove = true;
            if (foodCell != null && foodCell.food != null && canCarryFood)
            {
                if (foodCell.food.getAmountOfFoodLeft() <= 0)
                {
                    foodCell.food = null;
                }
            }
            if (curPath.Count > 0)
            {
                isMoving = true;
                if (curPath[0].passable)
                {
                    Vector2 normalVec = curPath[0].coord * 32 - position;
                    //Vector2 jumpAmount = normalVec * speed * timer.ElapsedGameTime.Milliseconds;
                    Vector2 normalVecNorm = normalVec;
                    if (normalVecNorm.Length() > 1)
                    {
                        normalVecNorm.Normalize();
                        Vector2 jumpAmount = normalVecNorm * speed * timer.ElapsedGameTime.Milliseconds;
                        if (normalVec.Length() < jumpAmount.Length())
                        {
                            position = curPath[0].coord * 32;
                            currentCell = curPath[0];
                            curPath.RemoveAt(0);
                            updateDirection();
                        }
                        else
                        {
                            position += jumpAmount;
                        }
                    }
                    else
                    {
                        position = curPath[0].coord * 32;
                        currentCell = curPath[0];
                        curPath.RemoveAt(0);
                        updateDirection();
                    }
                }
                else
                {
                    canMove = false;
                }
            }
            else
            {
                if (foodCell != null && foodCell.food != null && canCarryFood)
                {
                    if (foodCell.food.canPickUp())
                    {
                        foodCell.food.reduceFoodBy(1);
                        foodCell.food.resetTimer();
                        if (foodCell.food.getAmountOfFoodLeft() <= 0)
                        {
                            foodCell.food = null;
                        }
                        amountOfFoodCarrying = 1;
                    }
                }
                isMoving = false;
            }
            return canMove;
        }
        public void setPath(List<Cell> path)
        {
            curPath = path;
            curPath.Reverse();
            if (curPath.Count > 0)
            {
                curPath.RemoveAt(0);
            }
            updateDirection();
        }
        public bool hasPath()
        {
            bool hasAPath = false;
            if (curPath.Count > 0)
            {
                hasAPath = true;
            }
            return hasAPath;
        }
        private double Angle(Vector2 start, Vector2 end)
        {
            return Math.Atan2(start.Y - end.Y, end.X - start.X);
        }
        private void updateDirection()
        {
            if (curPath.Count > 0)
            {
                direction = Angle(position, curPath[0].coord * 32);
            }
        }
        public abstract Texture2D getTexture();
        public abstract void setTexture(Texture2D texture);
        public abstract Color getColor();
        public abstract void setColor(Color color);
        public abstract Rectangle getClip();
        public abstract void setClip(Rectangle clip);
    }
}
