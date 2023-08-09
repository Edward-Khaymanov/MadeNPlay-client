using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MadeNPlayShared
{
    [Serializable]
    public class LocalGameData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public List<GameTeam> Teams { get; set; }

        public int MaxPlayers => Teams.Sum(x => x.MaxPlayers);
        [JsonIgnore] public Sprite Icon { get; set; }
        [JsonIgnore] public string Path { get; set; }
    }
}