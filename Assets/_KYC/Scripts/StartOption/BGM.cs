using UnityEngine;

public class BGM : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MasterManager.Sound.PlayBGM("배경", 0);
    }
}
