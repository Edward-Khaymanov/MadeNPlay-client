using Unity.Collections;
using UnityEngine;

public struct AuthData
{
    [field: SerializeField] public ulong UserId { get; set; }
    [field: SerializeField] public FixedString64Bytes UserName { get; set; }
}