using System;
using System.Collections.Generic;
using System.Text;

namespace OgameApiBLL.Models
{
    public class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Alliance Alliance { get; set; } = new Alliance();
        public string Server { get; set; }
        public PlayerData Data { get; set; } = new PlayerData();
    }

    public class PlayerData
    {
        public int Ships { get; set; }
        public List<Planet> Planets { get; set; } = new List<Planet>();
    }
}
