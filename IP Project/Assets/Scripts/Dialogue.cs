using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] sentences;
    public float textSpeed;
    private int index;

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown(0))
        {
            if (textComponent.text == sentences[index])
            {
                NextSentence();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = sentences[index];
            }
        }
    }
    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeSentence(sentences[index]));
    }

    IEnumerator TypeSentence(string sentence)
    {
        textComponent.text = "";
        foreach (char letter in sentences[index].ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextSentence()
    {
        if (index < sentences.Length)
        {
            index++;
            StartCoroutine(TypeSentence(sentences[index]));
        }
        else
        {
            gameObject.SetActive(false); // Hide dialogue when done
        }
    }
}
