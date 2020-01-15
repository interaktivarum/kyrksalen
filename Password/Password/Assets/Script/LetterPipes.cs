using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LetterPipes : MonoBehaviour
{

    public GameObject letterPrefab;
    public GameObject freeSpheresContainer;
    public Anagram anagram;
    int hintId = -1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetAnagram(Anagram a) {
        anagram = a;
        CreatePipes();
        //RearrangeToWord();
    }

    public void ClearPipes() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }

    public void CreatePipes() {

        ClearPipes();
        NextHint();

        char[] chars = DistinctLetters(anagram);

        System.Random rnd = new System.Random();
        char[] charsRand = chars.OrderBy(x => rnd.Next()).ToArray();

        int id = 0;
        int sign = 1;
        float delay = 0;
        foreach (char c in charsRand) {
            GameObject go = Instantiate(letterPrefab, transform);
            //go.transform.localPosition = new Vector3(sign * (id / 2 * 0.25f), 0, 0);
            go.transform.localPosition = GetPipePosition(chars.Length, id);
            LetterPipe letter = go.GetComponent<LetterPipe>();
            letter.letter = c.ToString();

            letter.bounceDelay = delay;
            delay += 0.1f;

            id++;
            sign *= -1;
        }
    }

    public Vector3 GetPipePosition(int n, int id) {
        float firstX = -(n-1) * 0.125f;
        return new Vector3(firstX + id * 0.25f, 0, 0);
    }

    public bool RemoveExtraPipe() {
        bool removed = false;
        foreach (char c in anagram.extra) {
            foreach (LetterPipe pipe in GetComponentsInChildren<LetterPipe>()) {
                if (pipe.letter == c.ToString()) {
                    pipe.Destroy();
                    removed = true;
                    break;
                }
            }
            if (removed) break;
        }
        return removed;
    }

    public void RearrangeToWord() {
        string word = anagram.words[hintId];
        int i = 0;
        foreach (LetterPipe pipe in GetComponentsInChildren<LetterPipe>()) {
            if (word.Contains(pipe.letter)) {
                int id = word.IndexOf(pipe.letter);
                Vector3 pos = GetPipePosition(word.Length, id);
                pipe.AnimateTo(pos);
            }
            else {
                pipe.Destroy();
            }
        }
    }

    public void RearrangeSingle() {
        string word = anagram.words[0];
        int i = 0;
        LetterPipe cCorrectPos = null;
        LetterPipe cCorrectLetter = null;
        Debug.Log(word);
        foreach (LetterPipe pipe in GetComponentsInChildren<LetterPipe>()) {
            if (pipe.letter != word[i].ToString()) {
                cCorrectPos = pipe;
            }
            if (cCorrectPos) {
                cCorrectLetter = pipe;
            }
            i++;
        }
        Debug.Log(cCorrectPos.letter + " " + cCorrectLetter.letter);
    }

    void NextHint() {
        hintId = Random.Range(0, anagram.words.Length);
        //hintId = (hintId + 1) % anagram.words.Length;
    }

    char[] DistinctLetters(Anagram anagram) {
        string appended = "";
        foreach (string word in anagram.words) {
            appended += word;
        }
        appended += anagram.extra;
        return appended.Distinct().ToArray();
    }

}
