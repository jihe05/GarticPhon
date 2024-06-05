
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class LineMaterial : MonoBehaviour
{
    public LineRenderer lineRenderer;
 
    List<Vector2> points;
    

    //2.���ο� ����Ʈ���� ������Ʈ
    public void UpdateLine(Vector2 poition)
    {
        //����Ʈ ����Ʈ�� �ʱ�ȭ���� �������
        if (points == null)
        { 
            //�ʱ�ȭ 
            points = new List<Vector2>();
            SetPoint(poition);
            return;
        }

        //������ ����Ʈ�� ���ο� ����Ʈ�� �Ÿ��� 0.1���� ū ��쿡�� ����Ʈ �߰�
        if (Vector2.Distance(points.Last(), poition) > 0.1f)
        { 
            SetPoint(poition);
        }

    }

    //1.ù ����Ʈ ����
    void SetPoint(Vector2 point)
    { 
       
        points.Add(point);//����Ʈ�� ����Ʈ�� �߰�

        //���η������� ����Ʈ ������ ����ũ�� ������ ����
        lineRenderer.positionCount = points.Count;
        //���ο� ����Ʈ�� ��ġ ����(������ ����Ʈ)
        lineRenderer.SetPosition(points.Count - 1, point);

    }






}
