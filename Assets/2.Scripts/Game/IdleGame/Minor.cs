using UnityEngine;
using System.Collections;

public class Miner : MonoBehaviour
{
    public Vector2 startPos;
    public Vector2 targetPos;


    public float speed = 2f;
    public float miningTime = 2f;

    private enum State { Idle, Going, Mining, Returning }
    private State state;

    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(startPos, 0.2f);

        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPos, 0.2f);

        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startPos, targetPos);
    }

    void Start()
    {
        StartCoroutine(MiningLoop());
    }

    IEnumerator MiningLoop()
    {
        while (true)
        {
            state = State.Going;
            yield return MoveTo(targetPos);

            state = State.Mining;
            yield return new WaitForSeconds(miningTime);

            state = State.Returning;
            yield return MoveTo(startPos);

            state = State.Idle;
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator MoveTo(Vector2 target)
    {
        while (Vector2.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }
}
