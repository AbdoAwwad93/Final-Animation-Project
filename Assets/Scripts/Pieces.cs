using UnityEngine;

public class Pieces : MonoBehaviour
{
    public Vector3 RightPosition;
    public Vector3 StartingPosition;
    public Vector2 XPositions = new Vector2 { x = -2.7f, y = 2.64f };
    public Vector2 YPositions = new Vector2 { x = -1.24f, y = -4.88f };

    public bool Dragged = false;
    public bool InPlace = false;
    public bool GoingToStartingPosition = false;

    public float Dist = 0.0f;

    void Start()
    {
        RightPosition = transform.position;

        transform.position = new Vector3(Random.Range(XPositions.x, XPositions.y), Random.Range(YPositions.x, YPositions.y), 0);

        StartingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (InPlace)
            return;
        Dist = Vector3.Distance(transform.position, RightPosition);
        if (Dist < .35f && !Dragged && !InPlace)
        {
            transform.position = RightPosition;
            GameManager.Get().AddPiecePlaced();
            GameManager.Get().TryEndMatch();
            InPlace = true;
        }else
            if(!Dragged && !InPlace)
        {
            GoingToStartingPosition = true;            
        }

        if(GoingToStartingPosition)
        {
            transform.position = Vector3.Slerp(transform.position, StartingPosition, .015f);

            if (Vector3.Distance(transform.position, StartingPosition) < .35f)
            {
                transform.position = StartingPosition;
                GoingToStartingPosition = false;
            }
        }
    }

    public void SetDragged(bool drag)
    {
        Dragged = drag;
    }
}
