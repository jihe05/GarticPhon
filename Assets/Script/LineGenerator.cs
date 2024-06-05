using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Collections.Generic;

public class LineGenerator : NetworkManager
{
    public GameObject linePrefab;
    public GameObject DrawingBk;

    LineMaterial activeLine;
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (IsMouseOverBackground())
        {


            if (Input.GetMouseButtonDown(0))
            {
                //���콺 ��ư�� �����ﶧ �� ���� ����
                CreatenewLine();

            }

            if (Input.GetMouseButtonUp(0))
            {
                //���콺 ��ư�� ���� ����Ʈ �߰�
                NotPoint();
            }



            if (activeLine != null)
            {
                //Ȱ��ȭ�� ���� ������Ʈ
                LineUpdete();
            }
        }


    }

    private bool IsMouseOverBackground()
    {
        // ���콺 ��ġ�� ���̸� ���, ���̰� ��� �̹��� ������Ʈ�� �浹�ϴ��� Ȯ��
        RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider == null)
        {
            NotPoint();
        }

        return hit.collider != null && hit.collider.gameObject == DrawingBk;

       
    }

    public void CreatenewLine()
    {
        //���� ����
        GameObject newLine = Instantiate(linePrefab);
        activeLine = newLine.GetComponent<LineMaterial>();
     
    }

    
    public void NotPoint()
    {

        activeLine = null;
       
    }

    public void LineUpdete()
    {
      
        //���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        activeLine.UpdateLine(mousePos); // ���� ������Ʈ

    }
 

}
