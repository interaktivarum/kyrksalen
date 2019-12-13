using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Letters : MonoBehaviour
{

    public GameObject letterPrefab;
    public GameObject freeSpheresContainer;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateLetters(char[] chars) {

        System.Random rnd = new System.Random();
        char[] charsRand = chars.OrderBy(x => rnd.Next()).ToArray();

        int id = 1;
        int sign = 1;
        float delay = 0;
        foreach (char c in charsRand) {
            GameObject go = Instantiate(letterPrefab, transform);
            go.transform.localPosition = new Vector3(sign * (id / 2 * 0.2f), 0, 0);
            Letter letter = go.GetComponent<Letter>();
            letter.letter = c.ToString().ToUpper();

            letter.bounceDelay = delay;
            delay += 0.1f;

            id++;
            sign *= -1;
        }
    }

}
