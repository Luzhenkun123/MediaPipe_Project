using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BodyScript : MonoBehaviour
{
    // Start is called before the first frame update
    public UDPReceive udpReceive;
    public GameObject[] bodyPoints;
    void Start()
    {
        bodyOriginCenter = bodyCenter.position;
    }

    public Transform headTrans, bodyTrans;
    public Transform[] leftHands, leftLegs;
    public Transform[] rightHands, rightLegs;
    public Transform bodyCenter;
    Vector3 bodyOriginCenter;

    Vector3 originKuaPos;
    public Vector3 kuaOffset;
    public float lerpSpeed;
    // Update is called once per frame
    void Update()
    {
        if (udpReceive.isReceive)
        {
            string data = udpReceive.data;

            // data = data.Remove(0, 1);
            // data = data.Remove(data.Length - 1, 1);
            if (data != null && data != "")
            {
                if (originKuaPos == Vector3.zero)
                {
                    originKuaPos = bodyPoints[23].transform.position;
                }
                kuaOffset = bodyPoints[23].transform.position - originKuaPos;

                bodyCenter.position = new Vector3(bodyOriginCenter.x, bodyOriginCenter.y + kuaOffset.y, bodyOriginCenter.z);

                string[] points = data.Split(',');

                //0        1*3      2*3
                //x1,y1,z1,x2,y2,z2,x3,y3,z3

                for (int i = 0; i <= 32; i++)
                {

                    float x = float.Parse(points[0 + (i * 3)]) / 100;
                    float y = float.Parse(points[1 + (i * 3)]) / 100;
                    float z = float.Parse(points[2 + (i * 3)]) / 300;

                    bodyPoints[i].transform.localPosition = new Vector3(x, y, z);

                }
                //取7，8的中间点,计算与0点的方向
                Vector3 headCenter = (bodyPoints[7].transform.position + bodyPoints[8].transform.position) / 2;
                headCenter.y = bodyPoints[0].transform.position.y;

                headTrans.forward =Vector3.Lerp(headTrans.forward,((bodyPoints[0].transform.position-headCenter).normalized),Time.deltaTime* lerpSpeed);

                Vector3 pos1 = bodyPoints[11].transform.position;
                Vector3 pos2 = bodyPoints[12].transform.position;
                pos1.y = 0;
                pos2.y = 0;
                Vector3 bodyDir = Quaternion.AngleAxis(90, Vector3.up) * (pos1 - pos2).normalized;
                bodyTrans.forward =Vector3.Lerp(bodyTrans.forward,bodyDir,Time.deltaTime* lerpSpeed);

                //11,13,15
                //12,14,16
                leftHands[0].right = Vector3.Lerp(leftHands[0].right, (bodyPoints[11].transform.position - bodyPoints[13].transform.position).normalized, Time.deltaTime * lerpSpeed);
                leftHands[1].right = Vector3.Lerp(leftHands[1].right, (bodyPoints[13].transform.position - bodyPoints[15].transform.position).normalized, Time.deltaTime * lerpSpeed);

                rightHands[0].right = Vector3.Lerp(rightHands[0].right,(bodyPoints[14].transform.position - bodyPoints[12].transform.position).normalized,Time.deltaTime * lerpSpeed);
                rightHands[1].right = Vector3.Lerp(rightHands[1].right, -(bodyPoints[14].transform.position - bodyPoints[16].transform.position).normalized,Time.deltaTime* lerpSpeed);

                //23,25,27,31
                //24,26,28,32
                leftLegs[0].up =Vector3.Lerp(leftLegs[0].up,(bodyPoints[23].transform.position - bodyPoints[25].transform.position).normalized,Time.deltaTime* lerpSpeed);
                leftLegs[1].up =Vector3.Lerp(leftLegs[1].up,(bodyPoints[25].transform.position - bodyPoints[27].transform.position).normalized,Time.deltaTime* lerpSpeed);
                leftLegs[2].forward =Vector3.Lerp(leftLegs[2].forward,-(bodyPoints[27].transform.position - bodyPoints[31].transform.position).normalized,Time.deltaTime* lerpSpeed);

                rightLegs[0].up =Vector3.Lerp(rightLegs[0].up,(bodyPoints[24].transform.position - bodyPoints[26].transform.position).normalized,Time.deltaTime* lerpSpeed);
                rightLegs[1].up =Vector3.Lerp(rightLegs[1].up,(bodyPoints[26].transform.position - bodyPoints[28].transform.position).normalized,Time.deltaTime* lerpSpeed);
                rightLegs[2].forward =Vector3.Lerp(rightLegs[2].forward,-(bodyPoints[28].transform.position - bodyPoints[32].transform.position).normalized,Time.deltaTime* lerpSpeed);
            }
        }
    }
}

