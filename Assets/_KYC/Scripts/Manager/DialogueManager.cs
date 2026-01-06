using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Image portraitImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button nextButton;

    private Queue<string> sentences = new Queue<string>(); // 문장들을 담는 줄서기
    public bool isDialogueActive { get; private set; }

    private bool isTyping = false;  // 현재 글자가 찍히는 중인지 확인
    private string currentSentence; // 현재 전체 문장 저장

    public void Init()
    {
        dialoguePanel.SetActive(false);
        isDialogueActive = false;
        Debug.Log("DialogueManager: 초기화 완료");
    }

    // 대화 시작 함수
    public void StartDialogue(NPCData data)
    {
        isDialogueActive = true;
        dialoguePanel.SetActive(true); // 1. 패널 켜기

        nameText.text = data.npcName;
        portraitImage.sprite = data.portrait;

        sentences.Clear();
        foreach (string sentence in data.defaultDialogues)
        {
            sentences.Enqueue(sentence);
        }

        // [이 코드가 핵심!] 
        // 여기서 직접 호출을 해줘야 버튼을 안 눌러도 즉시 첫 타이핑이 시작됩니다.
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        // 타이핑 중일 때 버튼을 누르면 문장을 즉시 완성합니다.
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentSentence;
            isTyping = false;
            return;
        }

        // 더 이상 남은 문장이 없다면 대화를 종료합니다.
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        // 다음 문장을 가져와 타이핑 코루틴 시작
        currentSentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        isTyping = true;

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f); // 0.05초 간격으로 출력
        }

        isTyping = false;
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = ""; // 텍스트 초기화
        Debug.Log("대화 종료 및 패널 닫기");
    }
}