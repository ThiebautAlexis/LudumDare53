using UnityEngine;
using CoolFramework.Core;
using TMPro;

public class DialogueBox : CoolSingleton<DialogueBox>, IUpdate
{
    public override UpdateRegistration UpdateRegistration => base.UpdateRegistration | UpdateRegistration.Update;
    [SerializeField] private GameObject dialogueBoxObject; 
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float pauseDuration = .5f;
    private bool isDisplaying = false;
    private bool isInPause = true;
    private float timer = 0f;
    private DialogueData currentData;
    private DialogueData bufferData;

    public void ReadText(DialogueData _data)
    {
        if (currentData != null)
        {
            bufferData = _data;
        }
        else
        {
            dialogueText.SetText(string.Empty);
            currentData = _data;
            TalkieWalkie.Instance.Display(currentData.ChartacterImage, currentData.Duration + pauseDuration * 2);
            Pause();
            dialogueBoxObject.SetActive(true); 
            isDisplaying = true;
        }
    }

    private int tempIndex = 0; 
    private int index = 0; 
    void IUpdate.Update()
    {
        if (!isDisplaying) return;

        timer += Time.deltaTime;
        if (isInPause)
        {  
            if (timer > pauseDuration)
            {
                if(currentData == null) // In this case, this is an after dialogue pause.
                {
                    Hide();
                    if (bufferData != null) // If there is a dialogue in the buffer
                    {
                        ReadText(bufferData);
                        bufferData = null; 
                    }
                    return; 
                }
                UnPause(); // In this case, this is a pre dialogue pause.
            }
            return;
        }
        tempIndex = (int)(timer / currentData.Interval);
        if(tempIndex > index)
        {
            index = tempIndex;
            if(index >= currentData.DisplayedText.Length)
            {
                dialogueText.SetText(currentData.DisplayedText);
                // PAUSE HERE
                currentData = null;
                Pause(); 
                return; 
            }
           dialogueText.SetText(currentData.DisplayedText.Remove(index)); 
        }
    }

    private void UnPause()
    {
        isInPause = false;
        timer = 0f;
        index = 0; 
    }

    private void Pause()
    {
        isInPause = true;
        timer = 0f;
    }

    private void Hide()
    {
        isDisplaying = false;
        dialogueText.SetText(string.Empty);
        dialogueBoxObject.SetActive(false);
    }
}
