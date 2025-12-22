using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public GameObject selectedPiece;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);
            if(hit && hit.transform.CompareTag(StaticString.Puzzle))
            {
                selectedPiece = hit.transform.gameObject;
                if(selectedPiece.GetComponent<Pieces>().InPlace)
                {
                    selectedPiece = null;
                    return;
                }
                selectedPiece.GetComponent<Pieces>().SetDragged(true);
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            if (selectedPiece != null)
            {
                selectedPiece.GetComponent<Pieces>().SetDragged(false);
                selectedPiece = null;
            }
        }

        if (selectedPiece != null)
        {
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedPiece.transform.position = new Vector3(mousePoint.x, mousePoint.y, 0.25f);
        }

    }
}
