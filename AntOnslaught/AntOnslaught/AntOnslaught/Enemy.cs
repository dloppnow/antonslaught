using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AntOnslaught
{
    abstract class Enemy : MovableObject
    {
        protected SpriteAnimation sAnimation;
        protected Vector2 initialPosition;
        protected Ant target;
        protected float aggroRange;
        protected float attackRange;
        protected int damage;
        protected float attackTimer;
        protected float attackInterval;
        int health;
        bool ableToAttack;

        public Enemy(Vector2 position, SpriteAnimation sAnimation)
        {
            initialPosition = position;
            this.position = position * 32;
            this.sAnimation = sAnimation;
            this.speed = speed;
            aggroRange = 250;
            attackRange = 50;
            damage = 5;
            target = null;
            attackTimer = 2000;
            attackInterval = 250;
            ableToAttack = true;
        }
        public void update(GameTime gameTime)
        {
            attackTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (!ableToAttack)
            {
                if (attackTimer >= attackInterval)
                {
                    ableToAttack = true;
                }
            }
            if (isMoving)
            {
                sAnimation.setRepeatable(true);
            }
            else
            {
                sAnimation.setRepeatable(false);
            }
            sAnimation.update(gameTime);
        }

        public void attacked()
        {
            ableToAttack = false;
            attackTimer = 0;
        }

        public bool canAttack()
        {
            return ableToAttack;
        }

        public float getAggroRange()
        {
            return aggroRange;
        }

        public void setAggroRange(float aggroRange)
        {
            this.aggroRange = aggroRange;
        }

        public float getAttackRange()
        {
            return attackRange;
        }

        public void setAttackRange(float attackRange)
        {
            this.attackRange = attackRange;
        }

        public int getDamage()
        {
            return damage;
        }

        public void setDamage(int damage)
        {
            this.damage = damage;
        }

        public Ant getTarget()
        {
            return target;
        }

        public void setTarget(Ant target)
        {
            this.target = target;
        }

       
        public override Texture2D getTexture()
        {
            return sAnimation.getTexture();
        }

        public override void setTexture(Texture2D texture)
        {
            sAnimation.setTexture(texture);
        }

        public Vector2 getCenterOfMovementBox()
        {
            return initialPosition;
        }
        public override Color getColor()
        {
            return sAnimation.getColor();
        }

        public override void setColor(Color color)
        {
            sAnimation.setColor(color); ;
        }

        public override Rectangle getClip()
        {
            return sAnimation.getClip();
        }

        public override void setClip(Rectangle clip)
        {
            sAnimation.setClip(clip);
        }
    }
}
