using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DialoguePage
{
    [TextArea(2, 6)] public string text;
    public Sprite portrait;
}

public class DialoguePopup : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Image portraitImage;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private bool isIntro;
    [SerializeField] private bool isEnding;
    [SerializeField] private KeyCode skipKey = KeyCode.Space;
    [SerializeField] private float charactersPerSecond = 30f;
    [SerializeField] private List<DialoguePage> pages = new List<DialoguePage>();

    private int index;
    private int total;
    private float visible;

    private void Start()
    {
        panel.SetActive(false);

        if (!isIntro && !isEnding)
            GameManager.Instance.OnGameEnded += Open;
    }

    private void Update()
    {
        if (!panel.activeSelf) return;

        visible = Mathf.Min(visible + charactersPerSecond * Time.deltaTime, total);
        dialogueText.maxVisibleCharacters = (int)visible;

        if (!Input.GetKeyDown(skipKey)) return;

        if (visible < total)
        {
            visible = total;
        }
        else
        {
            index++;
            ShowPage();
        }
    }

    public void Open()
    {
        index = 0;
        panel.SetActive(true);
        ShowPage();
    }

    private void ShowPage()
    {
        if (index >= pages.Count)
        {
            panel.SetActive(false);

            if (isIntro)
                GameManager.Instance.BeginGame();
            else if (isEnding)
                GameManager.Instance.ShowEnding();
            else
                GameManager.Instance.StartProMode();

            return;
        }

        if (pages[index].portrait != null)
            portraitImage.sprite = pages[index].portrait;

        dialogueText.text = pages[index].text;
        dialogueText.maxVisibleCharacters = 0;
        dialogueText.ForceMeshUpdate();

        total = dialogueText.textInfo.characterCount;
        visible = 0f;
    }
}
