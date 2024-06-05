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
                //마우스 버튼을 눌렀울때 새 라인 생성
                CreatenewLine();

            }

            if (Input.GetMouseButtonUp(0))
            {
                //마우스 버튼을 땔때 포인트 추가
                NotPoint();
            }



            if (activeLine != null)
            {
                //활성화된 라인 업데이트
                LineUpdete();
            }
        }


    }

    private bool IsMouseOverBackground()
    {
        // 마우스 위치에 레이를 쏘고, 레이가 배경 이미지 오브젝트와 충돌하는지 확인
        RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider == null)
        {
            NotPoint();
        }

        return hit.collider != null && hit.collider.gameObject == DrawingBk;

       
    }

    public void CreatenewLine()
    {
        //라인 생성
        GameObject newLine = Instantiate(linePrefab);
        activeLine = newLine.GetComponent<LineMaterial>();
     
    }

    
    public void NotPoint()
    {

        activeLine = null;
       
    }

    public void LineUpdete()
    {
      
        //마우스 위치를 월드 좌표로 변환
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        activeLine.UpdateLine(mousePos); // 라인 업데이트

    }
 

}
