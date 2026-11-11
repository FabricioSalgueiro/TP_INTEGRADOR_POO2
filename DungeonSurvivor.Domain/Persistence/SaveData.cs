using System.Collections.Generic;

namespace DungeonSurvivor.Domain.Persistence
{
    public class SaveData
    {
        public string PlayerName { get; set; } = string.Empty;
        public int Score { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public List<string> Inventory { get; set; } = new List<string>();
    }
}