namespace App.Domain
{
    public class BattleSamurai
    {
        public int SamuraiId { get; set; }
        public int BattleId { get; set; }

        // Additional data in a join is refered to as "payload"
        public DateTime DateJoined { get; set; }

    }
}