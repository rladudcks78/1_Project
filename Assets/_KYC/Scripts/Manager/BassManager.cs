using UnityEngine;

/// <summary>
/// 모든 매니저의 부모가 되는 추상 클래스입니다.
/// </summary>
public abstract class BaseManager : MonoBehaviour
{
    // 각 매니저가 초기화될 때 실행될 함수입니다.
    // MasterManager에서 명시적으로 호출합니다.
    public abstract void Init();
}