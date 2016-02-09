using UnityEngine;
using System.Collections.Generic;

public class CardSpawnerScript : MonoBehaviour {

    public int NumRows;
    public int NumCols;
    public GameObject CardPrefab;


    private List<Card> CardArray;


	// Use this for initialization
	void Start () {

        CardArray
            = new List<Card>();
        for (int i = 0; i <= NumRows * NumCols / 2; i++)
        {
            Card card1 = new Card();
            card1.ID = i;

            Card card2 = new Card();
            card2.ID = i;

            CardArray.Add(card1);
            CardArray.Add(card2);
        }

        Shuffle(CardArray);


        for (int i = 0; i < NumRows; i++)
        {
            string row = "";
            for (int J = 0; J < NumCols; J++)
            {
                Card c = CardArray[i + J * NumRows];
                row += c.ID + " ";
                GameObject go = (GameObject)Instantiate(CardPrefab, new Vector3(i * 5, J * 5, 0), Quaternion.identity);
                go.GetComponent<Card>().ID = c.ID;
                go.name = c.ID.ToString();
            }

            Debug.Log(row);

        }
	}

    private void Shuffle(List<Card> list)
    {
        for (var i = list.Count - 1; i > 0; i--)
        {
            var r = Random.Range(0, i);
            var tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
