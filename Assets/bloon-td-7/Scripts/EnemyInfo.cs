public static class EnemyStuff
{
    public class EnemyInfo
    {
        protected float moveSpeed; 
        protected int dmg; 
        protected int health; 
        protected int id; 
        protected int moneyWorth;
        protected bool isCamo;
        protected float size;

        /// <summary>
        /// Declare a new enemy type with the specified parameters.
        /// </summary>
        /// <param name="moveSpeed">How fast the enemy moves along the track</param>
        /// <param name="dmg">How much damage the enemy deals upon reaching the end of the track</param>
        /// <param name="health">How much health the enemy has before it gets destroyed</param>
        /// <param name="id">unique identifier number for each enemy</param>
        /// <param name="moneyWorth">How much money is gained upon destroying the enemy</param>
        /// <param name="isCamo">Whether the enemy is camo or not</param>
        /// <param name="size">How much to scale the enemy size by (1 is default) </param>
        public EnemyInfo (float moveSpeed, int dmg, int health, int moneyWorth, float size = 5f, bool isCamo = false)
        {
            this.moveSpeed = moveSpeed;
            this.size = size;
            this.dmg = dmg; 
            this.health = health;
            this.moneyWorth = moneyWorth;
            this.isCamo = isCamo;
        }
    }

    public static EnemyInfo[] enemies = {
        new EnemyInfo(10f, 1, 5, 10),
        new EnemyInfo(30f, 4, 30, 50)
    };

}