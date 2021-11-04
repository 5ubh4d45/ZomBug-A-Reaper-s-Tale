using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.DialogueSystem
{
    public class TypeWriterEffect : MonoBehaviour
    {
        #region Variables
        public readonly List<Punctuation> _punctuations = new List<Punctuation>() {
            new Punctuation(new HashSet<char>() {'.', '?', '!'}, 0.6f),
            new Punctuation(new HashSet<char>() {',', ';', ':'}, 0.3f),
        };

        [SerializeField] private float _typingSpeed = 10;

        private Coroutine typingCoroutine;
        #endregion


        #region Getters And Setters
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Is The Passed Character a Punctuation Mark.
        /// </summary>
        /// <param name="waitTime"> The time that should be waited for this character </param>
        public bool IsPunctuation(char character, out float waitTime)
        {
            foreach (var punctuationCategory in _punctuations)
            {
                if (punctuationCategory.Punctuaions.Contains(character))
                {
                    waitTime = punctuationCategory.WaitTime;

                    return true;
                }
            }

            waitTime = default;

            return false;
        }
        #endregion


        #region Unity Calls

        #endregion


        #region Component Functions
        public Coroutine Run(string text, TMP_Text textLabel)
        {
            typingCoroutine = StartCoroutine(TypeText(text, textLabel));
            return typingCoroutine;
        }

        public void Stop()
        {
            if (typingCoroutine == null) return;
            StopCoroutine(typingCoroutine);
            IsRunning = false;
        }

        private IEnumerator TypeText(string text, TMP_Text textLabel)
        {

            IsRunning = true;

            //emptying the text field
            textLabel.text = string.Empty;

            float t = 0;
            int charIndex = 0;

            //loops and increments charcter index then adds to the substring
            while (charIndex < text.Length)
            {
                int lastCharIndex = charIndex;

                t += Time.deltaTime * _typingSpeed;


                charIndex = Mathf.FloorToInt(t);
                charIndex = Mathf.Clamp(charIndex, 0, text.Length);

                for (int i = lastCharIndex; i < charIndex; i++)
                {

                    bool IsLast = i >= text.Length - 1;

                    textLabel.text = text.Substring(0, i + 1);


                    //checks if it has punctuations and not something like "..." if conditions met, wait for waiting time described in the dictionary
                    if (IsPunctuation(text[i], out float waitTime) && !IsLast && !IsPunctuation(text[i + 1], out _))
                    {

                        yield return new WaitForSeconds(waitTime);
                    }
                }


                yield return null;
            }

            IsRunning = false;
        }
        #endregion

        public readonly struct Punctuation
        {

            public readonly HashSet<char> Punctuaions;
            public readonly float WaitTime;

            public Punctuation(HashSet<char> punctuaions, float waitTime)
            {

                Punctuaions = punctuaions;
                WaitTime = waitTime;
            }
        }
    }
}