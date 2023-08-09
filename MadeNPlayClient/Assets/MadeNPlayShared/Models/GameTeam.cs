using System;
using UnityEngine;

namespace MadeNPlayShared
{
    [Serializable]
    public struct GameTeam
    {
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private int _maxPlayers;

        public int Id => _id;
        public string Name => _name;
        public int MaxPlayers => _maxPlayers;
    }
}