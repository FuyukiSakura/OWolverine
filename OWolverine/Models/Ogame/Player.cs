using CSharpUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using OWolverine.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OWolverine.Models.Ogame
{
    [XmlRoot("players")]
    public class PlayerList
    {
        [XmlElement("player")]
        public List<Player> Players { get; set; }
        [XmlAttribute("timestamp")]
        public double Timestamp { get; set; }
        [XmlIgnore]
        public DateTime LastUpdate => DateTimeHelper.UnixTimeStampToDateTime(Timestamp);
    }

    public class PlayerId
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }

    public class Player : IUpdatable
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("status")]
        [JsonIgnore]
        public string Status {
            get
            {
                string statusText = "";
                if (IsAdmin) statusText += "a ";
                if (IsFlee) statusText += "o ";
                if (IsVocation) statusText += "v ";
                if (IsBanned) statusText += "b ";
                if (IsInactive && !IsLeft) statusText += "i ";
                if (IsLeft) statusText += "I ";
                return statusText;
            }
            set
            {
                IsAdmin = value.Contains("a");
                IsBanned = value.Contains("b");
                IsVocation = value.Contains("u");
                IsFlee = value.Contains("o");
                IsInactive = value.Contains("i") || value.Contains("I");
                IsLeft = value.Contains("I");
            }
        }

        [XmlAttribute("alliance")]
        public int AllianceId { get; set; }
        [JsonIgnore]
        public Alliance Alliance { get; set; }
        [JsonIgnore]
        public Score Score { get; set; }

        //Status Property
        public bool IsAdmin { get; set; }
        public bool IsBanned { get; set; }
        public bool IsFlee { get; set; }
        public bool IsVocation { get; set; }
        public bool IsInactive { get; set; }
        public bool IsLeft { get; set; }
        [NotMapped]
        public bool IsActive
        {
            get
            {
                return !IsAdmin && !IsBanned && !IsInactive && !IsLeft && !IsVocation;
            }
        }

        public List<Planet> Planets { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// Update player with another player object
        /// </summary>
        /// <param name="obj"></param>
        public void Update(IUpdatable obj)
        {
            if(obj is Player player)
            {
                if (Name != player.Name)
                {
                    Name = player.Name;
                }

                if (Status != player.Status)
                {
                    Status = player.Status;
                }
            }
        }
    }

    public class PlayerViewModel
    {
        private Player _player { get; set; }
        public int Id => _player.Id;
        public string Name => _player.Name;
        public Alliance Alliance => _player.Alliance;
        public List<Planet> Planets => _player.Planets;
        //Status
        public string StatusText => HasStatus ? $"(Status: {_player.Status})":"";
        public bool HasStatus => _player.Status != "";
        //Score Total
        public int ScoreTotal => _player.Score.Total;
        public int ScoreTotalDiff
        {
            get
            {
                var historyTotal = _player.Score.UpdateHistory.FirstOrDefault(h => h.Type == ScoreType.Total.ToString());
                if (historyTotal == null)
                {
                    return 0;
                }
                return historyTotal.NewValue - historyTotal.OldValue;
            }
        }
        //Score Military
        public int ScoreMilitary => _player.Score.Military;
        public int ScoreMilitaryDiff
        {
            get
            {
                var historyMilitary = _player.Score.UpdateHistory.FirstOrDefault(h => h.Type == ScoreType.Military.ToString());
                if (historyMilitary == null)
                {
                    return 0;
                }
                return historyMilitary.NewValue - historyMilitary.OldValue;
            }
        }
        public int ScoreShip => _player.Score.Ship;
        public int ScoreShipDiff
        {
            get
            {
                var historyShip = _player.Score.UpdateHistory.FirstOrDefault(h => h.Type == "Ship");
                if (historyShip == null)
                {
                    return 0;
                }
                return historyShip.NewValue - historyShip.OldValue;
            }
        }
        public int ShipNumber => _player.Score.ShipNumber;
        public string SnapshotDiff
        {
            get
            {
                var historyTotal = _player.Score.UpdateHistory
                    .Where(h => h.Type == ScoreType.Total.ToString())
                    .OrderByDescending(h => h.UpdatedAt)
                    .ToArray();
                TimeSpan diff;
                if (historyTotal.Length <= 0)
                {
                    diff = DateTime.UtcNow - _player.LastUpdate;
                } else if (historyTotal.Length == 1)
                {
                    diff = _player.Score.LastUpdate - historyTotal[0].UpdatedAt;
                }
                else
                {
                    diff = historyTotal[0].UpdatedAt - historyTotal[1].UpdatedAt;
                }
                return String.Format("{0} {1}",
                    diff.Days == 0 ? "" : String.Format("{0} Day{1}", diff.Days, diff.Days > 1 ? "s" : ""),
                    String.Format("{0} Hour{1}", diff.Hours, diff.Hours > 1 ? "s" : ""));
            }
        }
        public string Style
        {
            get
            {
                if (ShipNumber == 0 || ScoreShip < 3000)
                {
                    return "N/A";
                }

                var avgShipScore = ScoreShip / ShipNumber;
                if (ScoreShip > ScoreMilitary * 0.75)
                {
                    if (avgShipScore > 10)
                    {
                        return "Wolf";
                    }
                    else
                    {
                        return "Sheep";
                    }
                }
                return "Turtle";
            }
        }
        public string LastUpdate => _player.LastUpdate.ToString("g");

        public PlayerViewModel(Player player)
        {
            _player = player;
        }
    }
}
