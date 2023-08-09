using System;
using UnityEngine;

[Serializable]
public class User
{
    [SerializeField] private ulong _id;
    [SerializeField] private string _name;

    public User(ulong id, string name)
    {
        _id = id;
        _name = name;
    }

    public ulong Id => _id;
    public string Name => _name;

    public void SetName(string name)
    {
        _name = name;
    }
}