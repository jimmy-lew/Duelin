using System.Collections;
using UnityEngine;

public class TurnToast : MonoBehaviour
{
    float lifetime = 1.2f;
    private void Start() => StartCoroutine(WaitThenDie());

    IEnumerator WaitThenDie()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(this.gameObject);
    }
}