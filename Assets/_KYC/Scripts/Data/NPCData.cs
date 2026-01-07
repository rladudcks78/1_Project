using UnityEngine;
using System.Collections.Generic;

public enum NPCRole
{
    Citizen,
    Merchant,
    Blacksmith,
    Doctor
}

[CreateAssetMenu(fileName = "New NPC Data", menuName = "NPC/NPC Data")]
public class NPCData : ScriptableObject
{
    public string npcName;      //이름
    public Sprite portrait;     // 초상화
    public NPCRole role;        // 역할

    [TextArea(3, 10)]
    public string[] defaultDialogues;

    public List<ItemData> shopItems;
}