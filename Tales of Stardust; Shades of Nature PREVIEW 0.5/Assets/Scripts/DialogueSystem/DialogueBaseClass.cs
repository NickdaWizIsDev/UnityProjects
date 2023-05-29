using System.Collections;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace DialogueSystem
{
    public class DialogueBaseClass : MonoBehaviour
    {
        public bool Finished { get; protected set; }
        protected IEnumerator WriteText(string input, TextMeshProUGUI textHolder, TMPro.VertexGradient textColor, float delay, AudioClip sound)
        {
            textHolder.colorGradient = textColor;

            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                SoundManager.instance.PlaySound(sound);
                yield return new WaitForSeconds(delay);
            }

            yield return new WaitUntil(() => Input.GetMouseButton(0));
            Finished = true;
        }
    }
}
