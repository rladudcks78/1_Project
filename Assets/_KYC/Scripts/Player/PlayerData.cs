using UnityEngine;

/// <summary>
/// 플레이어의 고유 능력치를 저장하는 데이터 에셋
/// </summary>
[CreateAssetMenu(fileName = "NewPlayerData", menuName = "ScriptableObjects/Player/StatData")]
public class PlayerData : ScriptableObject
{
    [Header("Health")]
    public int hp = 100;
    public int maxHp = 100;

    [Header("Economy")]
    public int money = 1000;

    [Header("Combat")]
    public int att = 5;
    public int def = 5;

    [Header("Movement")]
    public float moveSpeed = 5.0f;
    public float attackSpeed = 1.0f;
}