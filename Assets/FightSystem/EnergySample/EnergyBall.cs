using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public class EnergyBall : MonoBehaviour {
    public bool ready = false;
    public float timing = 0;
    public bool canActive = true;
    public bool canRender = false;

    public Transform startPoint;
    public Transform endPoint;

    //是否在移动过程中
    public bool isEnergymove = false;

    //渲染完成后表示魔法球充能完成,完成累计加一个数,这个数是代表可用的能量球
    public int ChargefinishNumber = 0; 
    public Image energyCrimsonfill;

    void Start () {
        startPoint = this.transform;
        energyCrimsonfill = this.GetComponent<Image>();
	}


    public void Cost()
    {
        Clear();
        //Fly();
    }


    public void FillBall(float progress)
    {
        float p = Mathf.Clamp(progress, 0, 0.95f);
        this.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1, 1-p);
    }


    public void Clear(){

        //this.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1, 1);
        this.GetComponent<Image>().fillAmount = 0;
    }


    public void Fly()
    {
        StartCoroutine(PathMove());
    }


    IEnumerator PathMove()
    {
        isEnergymove = true;
        bool reach = false;


        //这里采用创建球体来代替能量球完成能量球的移动，后续需要将该部分改成其他粒子效果  ——by chenguang 2016-5-25
        GameObject energy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        energy.transform.localScale = Vector3.one * 0.1f;
        energy.GetComponent<Renderer>().material.color = Color.blue;
        energy.name = "MagicBall";
        energy.GetComponent<SphereCollider>().isTrigger = true;
        energy.transform.position = startPoint.position;
        

        float duration = 0.9f;
        float timer = 0;
        endPoint = SkillCardManager.instance.currentSelect.objpos;
        while (!reach)
        {
            timer += Time.deltaTime;

            energy.transform.position = Vector3.Slerp(startPoint.position, endPoint.position, timer / duration);
            float remainingDistance = Vector3.Distance(energy.transform.position, endPoint.position);
            if (remainingDistance <= 0.01f)
            {
                reach = true;
                energy.transform.position = endPoint.position;
                  
            }

            energy.transform.DOMove(endPoint.transform.position, duration);

            yield return null;
        }

        if (energy != null)
        {
            Destroy(energy);
        }
        isEnergymove = false;
    }


    Vector3 GetCircleCenter()
    {
        Vector3 s = startPoint.position;
        Vector3 e = endPoint.position;
        Vector3 e0 = new Vector3(e.x, s.y, e.z);

        Vector3 se = e - s;
        Vector3 m = (s + e) / 2;
        Vector3 sm = m - s;
        Vector3 se0 = e0 - s;
        Vector3 ee0 = e0 - e;

        float sc_Length = sm.magnitude * se.magnitude / se0.magnitude;

        Vector3 c = sc_Length * se0.normalized;
        c = s + c;

        return c;
    }



    public void CopyFromOtherBall(EnergyBall other)
    {
        this.energyCrimsonfill.fillAmount = other.energyCrimsonfill.fillAmount;
    }
}
