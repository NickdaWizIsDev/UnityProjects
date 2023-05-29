using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using Unity.VisualScripting;

namespace DialogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {
        [Header ("Text Options")]
        [SerializeField] private TMPro.VertexGradient textColor;
        [SerializeField] private string input;
        private TextMeshProUGUI textHolder;

        [Header("Sound")]
        [SerializeField] private AudioClip sound;

        [Header ("Delay Options")]
        [SerializeField] private float delay;
        [SerializeField] private float delayBetweenLines;

        [Header ("Character Image")]
        [SerializeField] private Sprite characterSprite;
        [SerializeField] private Image imageHolder;

        private IEnumerator lineAppear;

        private void Awake()
        {
            imageHolder.sprite = characterSprite;
            imageHolder.preserveAspect = true;
        }

        private void OnEnable()
        {
            ResetLine();
            lineAppear = WriteText(input, textHolder, textColor, delay, sound);
            StartCoroutine(lineAppear);
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (textHolder.text != input)
                {
                    StopCoroutine(lineAppear);
                    textHolder.text = input;
                }
                else
                {
                    Finished = true;
                }
                    
            }
        }

        private void ResetLine()
        {
            textHolder = GetComponent<TextMeshProUGUI>();
            textHolder.text = " ";
            Finished = false;
        }
    }
}
