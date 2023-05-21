using UnityEngine;
using TMPro;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {
        [Header ("Text Options")]
        [SerializeField] private TextColorGradient textColor;
        [SerializeField] private string input;
        private TextMeshProUGUI textHolder;

        [Header ("Delay Options")]
        [SerializeField] private float delay;
        [SerializeField] private float delayBetweenLines;

        [Header ("Character Image")]
        [SerializeField] private Sprite characterSprite;
        [SerializeField] private Image imageHolder;

        private void Awake()
        {
            textHolder = GetComponent<TextMeshProUGUI>();
            textHolder.text = " ";
        }

        private void Start()
        {
            StartCoroutine(WriteText(input, textHolder, textColor, delay, delayBetweenLines));
            imageHolder.sprite = characterSprite;
            imageHolder.preserveAspect = true;
        }
    }
}
