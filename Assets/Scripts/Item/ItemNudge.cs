using System.Collections;
using UnityEngine;

public class ItemNudge : MonoBehaviour
{
    private WaitForSeconds pause;
    private bool isAnimating = false;



    private void Awake()
    {
        pause = new WaitForSeconds(0.04f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if  (isAnimating == false)
        {
            if (gameObject.transform.position.x < collision.gameObject.transform.position.x)
            {
                StartCoroutine(RotateAntiClock());
            }
            else
            {
                StartCoroutine(RotateClock());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isAnimating == false)
        {
            if (gameObject.transform.position.x > collision.gameObject.transform.position.x)
            {
                StartCoroutine(RotateAntiClock());
            }
            else
            {
                StartCoroutine(RotateClock());
            }
        }
    }

    private IEnumerator RotateAntiClock()
    {
        isAnimating = true;

        for (int i = 0; i<4; i++)
        {
            gameObject.transform.GetChild(0).Rotate(0f, 0f, 4f); //o GetChild(0) é para pegar o sprite do item, que no caso fica no índice 0

            yield return pause;
        }

        for (int i = 0; i<5; i++)
        {
            gameObject.transform.GetChild(0).Rotate(0f, 0f, -4f);

            yield return pause;
        }

        gameObject.transform.GetChild(0).Rotate(0f, 0f, 4f); 

        yield return pause;

        isAnimating = false;
    }

    private IEnumerator RotateClock()
    {
        isAnimating = true;

        for (int i = 0; i<4; i++)
        {
            gameObject.transform.GetChild(0).Rotate(0f, 0f, -4f);

            yield return pause;
        }

        for (int i = 0; i<5; i++)
        {
            gameObject.transform.GetChild(0).Rotate(0f, 0f, 4f);

            yield return pause;
        }

        gameObject.transform.GetChild(0).Rotate(0f, 0f, -4f);

        yield return pause;

        isAnimating = false;
    }

}
