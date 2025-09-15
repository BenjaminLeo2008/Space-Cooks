using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PedidosScript : MonoBehaviour
{
    public List<Image> timeBars;
    private float totalTime;
    private float remainingTime;
    
    // Start is called before the first frame update
    public void StartTimer(float maxTime)
    {
        totalTime = maxTime;
        remainingTime = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            float fillAmount = remainingTime / totalTime;

            foreach (Image bar in timeBars)
            {
                bar.fillAmount = fillAmount;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
