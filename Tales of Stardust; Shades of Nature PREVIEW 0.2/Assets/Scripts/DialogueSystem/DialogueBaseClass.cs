using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace DialogueSystem
{
    public class DialogueBaseClass : MonoBehaviour
    {
        public bool finished { get; private set; }
        protected IEnumerator WriteText(string input, TextMeshProUGUI textHolder, TextColorGradient textColor, float delay, float delayBetweenLines)
        {
            if (textHolder == null)
            {
                Debug.LogError("textHolder parameter is null!");
                yield break;
            }
            StringBuilder sb = new StringBuilder(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                sb.Append(input[i]);
                textHolder.text = sb.ToString();
                yield return new WaitForSeconds(delay);
            }

            yield return new WaitUntil(() => Input.GetMouseButton(0));
            finished = true;
        }
    }
}
