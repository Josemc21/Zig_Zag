using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(DestroyFloor());
        }
    }

    // Update is called once per frame
    IEnumerator DestroyFloor()
    {
        yield return new WaitForSeconds(3.0f);
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        yield return new WaitForSeconds(2.5f);
        Destroy(this.gameObject);
        Debug.Log("Destruido suelo");
    }
}
