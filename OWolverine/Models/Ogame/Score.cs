using CSharpUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OWolverine.Models.Ogame
{
    public enum ScoreCategory
    {
        Player = 1,
        Alliance = 2
    }

    public enum ScoreType
    {
        Total,
        Economy,
        Research,
        Military,
        MilitaryBuilt,
        MilitaryDestroyed,
        MilitaryLost,
        Honor
    }

    // ---------- For XML Parsing ---------- //
    [XmlRoot("highscore")]
    public class HighScore {
        [XmlElement("player")]
        public List<PlayerScore> Scores { get; set; }
        [XmlAttribute("timestamp")]
        public double Timestamp { get; set; }
        [XmlIgnore]
        public DateTime LastUpdate {
            get
            {
                return DateTimeHelper.UnixTimeStampToDateTime(Timestamp);
            }
        }
    }

    public class PlayerScore
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        [XmlAttribute("score")]
        public int Value { get; set; }
        [XmlAttribute("ships")]
        public int Ships { get; set; }
    }

    // ---------- High Score data ---------- //
    public class Score
    {
        public int Id { get; set; }
        public int Total { get; set; }
        public int Economy { get; set; }
        public int Research { get; set; }
        public int Military { get; set; }
        public int Ship { get; set; }
        public int ShipNumber { get; set; }
        public int MilitaryBuit { get; set; }
        public int MilitaryDestroyed { get; set; }
        public int MilitaryLost { get; set; }
        public int Honor { get; set; }
        public Player Player { get; set; }

        public List<ScoreHistory> UpdateHistory { get; set; } = new List<ScoreHistory>();
        public DateTime LastUpdate { get; set; }
    }
}
