using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float typingSpeed = 10f;

    public bool IsRunning{get; private set;}
    

    //different wait time, for punctuaions
    private readonly List<Punctuation> punctuations = new List<Punctuation>(){

        new Punctuation(new HashSet<char>() {'.', '!', '?'}, 0.6f),
        new Punctuation(new HashSet<char>() {',', ';', ':'}, 0.3f)
    
    };


    private Coroutine typingCoruotine;

    //for running the typing effect
    public void Run(string textToType, TMP_Text textLabel)
    {
        typingCoruotine = StartCoroutine(TypeText(textToType, textLabel));
    }

    public void Stop(){
        
        StopCoroutine(typingCoruotine);
        IsRunning = false;
    }


    //text typing method
    private IEnumerator TypeText(string textToType, TMP_Text textLabel){

        IsRunning = true;

        //emptying the text field
        textLabel.text = string.Empty;

        float t = 0;
        int charIndex = 0;

        //loops and increments charcter index then adds to the substring
        while (charIndex < textToType.Length)
        {
            int lastCharIndex = charIndex;

            t += Time.deltaTime * typingSpeed;


            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            for ( int i = lastCharIndex; i < charIndex; i++){
                
                bool IsLast = i >= textToType.Length - 1;

                textLabel.text = textToType.Substring(0, i + 1);


                //checks if it has punctuations and not something like "..." if conditions met, wait for waiting time described in the dictionary
                if (IsPunctuation(textToType[i], out float waitTime) && !IsLast && !IsPunctuation(textToType[i + 1], out _ )){
                    
                    yield return new WaitForSeconds(waitTime);
                }
            }


            yield return null;
        }

        IsRunning = false;
    }

    private bool IsPunctuation(char character, out float waitTime){

        foreach (Punctuation punctuationCatagory in punctuations){

            //checks for punctuation and set wait time
            if (punctuationCatagory.Punctuaions.Contains(character)){

                waitTime = punctuationCatagory.WaitTime;

                return true;
            }
        }

        waitTime = default;
        return false;
    }


    //this struct contains the punctuaaion data
    private readonly struct Punctuation{

        public readonly HashSet<char> Punctuaions;
        public readonly float WaitTime;

        public Punctuation(HashSet<char> punctuaions, float waitTime){

            Punctuaions = punctuaions;
            WaitTime = waitTime;
        }
    }


}
