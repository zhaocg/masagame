using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class EnergySystem : MonoBehaviour {

    public int energy_current;                          //当前能量值
    public int energy_max;                              //能量的最大值

    public float energyGainDuration = 1f;               //每格能量涨的时间

    public bool energyRecoverOn = true;                 //能量是否可以恢复
    public bool energyFull = false;                     //能量是否充满
    
    public List<EnergyBall> energyBalls;
    public List<EnergyBall> energyBalls_ready;        
    private float timer;                                //计时器
    private Coroutine m_EnergyGainCoroutine;            //能量增长的协同程序

    public float timing = 0;

	// Use this for initialization
	void Start () {
	        
	}
	 
	// Update is called once per frame
	void Update () {

        if(energyRecoverOn)
        {
            //恢复能量
            ResumePowerGain();
        } 

        if (energy_current >= energy_max)
        {
            energyFull = true;
            StopPowerGain();
        }
        else
        {
            energyFull = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            EnergyCost(2, 3);
        }
        
	}

    /// <summary>
    /// 消耗能量
    /// </summary>
    /// <param name="cost"></param>
    public void EnergyCost(int cardIndex, int cost)
    { 
        int current = energyBalls_ready.Count;
        try
        {
            //如果剩余能量值不足，直接返回
            if (cost > current) return;

            //如果当前卡牌下标 > 当前能量值 -- 从右向左依次取能量，包含下标
            if (cardIndex >= current)
            {
                for (int i = current; i > current - cost; i--)
                {
                    energyBalls_ready[i - 1].Cost();
                    energyBalls_ready.RemoveAt(i - 1);
                    ResetEnergyBallQueue();
                }
                //for (int i = 1; i <= cost; i++)
                //{
                //    energyBalls_ready[current - i].Cost();
                //    energyBalls_ready.RemoveAt(current - i);
                //} 
            }
            //如果当前下标 < 当前能量值
            else if (cardIndex < current)
            {

                //右侧总的能量值
                int rightTotal = current - 1 - cardIndex;

                //右侧待消耗
                int rightcurrent = (int)cost / 2;

                //如果右侧待消耗能量充足
                if (rightTotal >= rightcurrent)
                {
                    for (int i = current - rightTotal + rightcurrent; i > current - rightTotal + rightcurrent - cost; i--)
                    {
                        energyBalls_ready[i].Cost();
                        energyBalls_ready.RemoveAt(i);
                        ResetEnergyBallQueue();
                    }

                    //for (int i = cardIndex; i < rightcurrent; i++)
                    //{
                    //    energyBalls_ready[rightcurren].Cost();
                    //    energyBalls_ready[cost - rightcurrent].Cost();
                    //}
                }
                else if (rightTotal < rightcurrent)
                {
                    for (int i = current - 1; i > current - cost - 1; i--)
                    {
                        energyBalls_ready[i].Cost();
                        energyBalls_ready.RemoveAt(i);
                        ResetEnergyBallQueue();
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        

        

        //if (energy_current < cost)
        //{
        //    Debug.Log("low energy!");
        //} 
        //else
        //{
        //    int current = energyBalls_ready.Count;

        //    for (int i=1; i<=cost; i++)
        //    {
        //        energyBalls_ready[current - i].Cost();
        //        energyBalls_ready.RemoveAt(current - i);
        //    }

        //    //清除最前方正在填充的能量球的进度
        //    if(energyFull == false)
        //        energyBalls[current].Clear();

        //    energy_current -= cost;
          
        //    if (energy_current < 0)
        //    {
        //        energy_current = 0;
        //        Debug.Log("energy pool is empty");
        //    }
        //}
    }

    public IEnumerator PowerGain()
    {
        while (!energyFull)
        {
            timer += Time.deltaTime;
            
            energyBalls[energy_current].energyCrimsonfill.fillAmount = timer / energyGainDuration;
            float progress = energyBalls[energy_current].energyCrimsonfill.fillAmount;
            if (progress >= 1)
            {
                energyBalls_ready.Add(energyBalls[energy_current]);
                energy_current++;
                timer = 0;
            }
            yield return null;
        }
    }

    /// <summary>+
    /// 结束能量增长协程
    /// </summary>
    public void StopPowerGain()
    {
        if(m_EnergyGainCoroutine != null)
        {
            StopCoroutine(m_EnergyGainCoroutine);
            m_EnergyGainCoroutine = null;   
        }
    }

    /// <summary>
    /// 继续能量增长协程
    /// </summary>
    public void ResumePowerGain()
    {
        if (m_EnergyGainCoroutine == null)
        {
            m_EnergyGainCoroutine = StartCoroutine(PowerGain());
        }
    }

    public void ResetEnergyBallQueue()
    {
        
        int current = energyBalls_ready.Count;

        energy_current = current;

        for (int i = 0; i < current; i++)
        {
            energyBalls[i].CopyFromOtherBall(energyBalls_ready[i]);
            //energyBalls_ready[i].Clear();
            energyBalls_ready[i] = energyBalls[i];
        }

        if (current < energyBalls.Count - 1)
        {
            energyBalls[energyBalls_ready.Count + 1].Clear();
        }
    }
}