using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IntergrateToDoller : MonoBehaviour
{
	[SerializeField] float frameDelay = 0;
    float frame = 0;
    
    [SerializeField] public GameObject pointObj;

    [SerializeField] public List<Vector2> positions;
    public Vector3 position;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (frame % 2 == 0)
        {
            convertPosition();

        }

    }

    private void convertPosition()
    {
		Vector3 currPos = pointObj.transform.position;

		Vector2 temp = Vector2.zero;

        temp.x = currPos.x + currPos.z;
        temp.y = currPos.y;

        positions.Add(temp);
    }

    // does the 1$ call and get MATCH
    private void callDoller()
    {
        DollarRecognizer dr = new DollarRecognizer();

		DollarRecognizer.Result res = dr.Recognize(positions);

        //string name = res.Match.Name;
        //float score = res.Score;

    }

}
