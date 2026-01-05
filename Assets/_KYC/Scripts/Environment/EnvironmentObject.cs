using UnityEngine;

/// <summary>
/// 유니티 기본 클래스들의 기능을 확장하는 유틸리티 클래스입니다.
/// 포트폴리오에서 '공통 모듈화' 능력을 보여주기 좋습니다.
/// </summary>
public static class EnvironmentObject
{
    /// <summary>
    /// 컴포넌트가 있으면 가져오고, 없으면 새로 추가하여 반환합니다.
    /// </summary>
    public static T getOrAddComponent<T>(this GameObject obj) where T : Component
    {
        T component = obj.GetComponent<T>();
        return component != null ? component : obj.AddComponent<T>();
    }
}