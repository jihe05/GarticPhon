
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class LineMaterial : MonoBehaviour
{
    public LineRenderer lineRenderer;
 
    List<Vector2> points;
    

    //2.새로운 포인트마다 업데이트
    public void UpdateLine(Vector2 poition)
    {
        //포인트 리스트가 초기화되지 않은경우
        if (points == null)
        { 
            //초기화 
            points = new List<Vector2>();
            SetPoint(poition);
            return;
        }

        //마지막 포인트와 새로운 포인트의 거리가 0.1보다 큰 경우에만 포인트 추가
        if (Vector2.Distance(points.Last(), poition) > 0.1f)
        { 
            SetPoint(poition);
        }

    }

    //1.첫 포인트 설정
    void SetPoint(Vector2 point)
    { 
       
        points.Add(point);//포인트를 리스트의 추가

        //라인랜더러의 포인트 개수를 리스크의 개수로 설정
        lineRenderer.positionCount = points.Count;
        //새로운 포지트의 위치 설정(마지막 포인트)
        lineRenderer.SetPosition(points.Count - 1, point);

    }






}
