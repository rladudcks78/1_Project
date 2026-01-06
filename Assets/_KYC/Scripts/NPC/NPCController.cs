using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] private NPCData npcData; // 위에서 만든 SO를 여기에 드래그

    // 플레이어가 근처에서 클릭하거나 상호작용 키를 눌렀을 때 호출
    public void Interact()
    {
        // 1. 이미 대화 중이면 중복 실행 방지
        if (MasterManager.Dialogue.isDialogueActive || MasterManager.Shop.IsShopActive)
        {
            return;
        }

        // 2. 거리 체크: 플레이어가 너무 멀면 무시
        float dist = Vector2.Distance(transform.position, MasterManager.Data.PlayerTransform.position);
        if (dist > 2.0f) return;

        // 3. [핵심] 역할(Role)에 상관없이 무조건 대화부터 시작
        // StartDialogue 내부에서 이미 첫 문장을 출력(DisplayNextSentence)하도록 짜여 있습니다.
        if (npcData != null)
        {
            MasterManager.Dialogue.StartDialogue(npcData);
        }

        // 4. 역할에 따른 추가 동작 (필요할 때만 사용)
        switch (npcData.role)
        {
            case NPCRole.Merchant:
                // 예: 대화가 모두 끝난 후 상점을 열게 하고 싶다면 여기에 플래그를 세울 수 있습니다.
                Debug.Log($"{npcData.npcName}은 상인입니다. 대화 종료 후 상점 로직 연결 가능.");
                break;

            case NPCRole.Blacksmith:
                Debug.Log($"{npcData.npcName}은 대장장이입니다.");
                break;
        }
    }

    private void ShowDialogue()
    {
        // 대화 시스템(DialogueManager)에 데이터를 넘겨주는 로직 (추후 구현)
        if (npcData.defaultDialogues.Length > 0)
        {
            Debug.Log($"{npcData.npcName}: {npcData.defaultDialogues[0]}");
        }
    }

    private void OpenShop()
    {
        // 상점 UI를 열고 npcData.shopItems를 보여주는 로직 (추후 구현)
        Debug.Log($"{npcData.npcName}의 상점을 엽니다.");
    }
}