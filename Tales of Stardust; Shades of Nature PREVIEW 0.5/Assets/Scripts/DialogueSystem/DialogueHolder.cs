using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueHolder : MonoBehaviour
    {
        private IEnumerator dialogueSeq;
        private bool dialogueFinished;

        private void OnEnable()
        {
            dialogueSeq = DialogueSequence();
            StartCoroutine(dialogueSeq);
        }
        private IEnumerator DialogueSequence()
        {

            if (!dialogueFinished)
            {
                for (int i = 0; i < transform.childCount - 1; i++)
                {
                    Deactivate();
                    transform.GetChild(i).gameObject.SetActive(true);
                    yield return new WaitUntil(() => transform.GetChild(i).GetComponent<DialogueBaseClass>().Finished);
                }
            }
            else
            {
                int index = transform.childCount - 1;
                Deactivate();
                transform.GetChild(index).gameObject.SetActive(true);
                yield return new WaitUntil(() => transform.GetChild(index).GetComponent<DialogueBaseClass>().Finished);
            }

            dialogueFinished = true;
            gameObject.SetActive(false);
        }

        private void Deactivate ()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
     
    }
}