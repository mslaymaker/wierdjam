using UnityEngine;

public class ChangeTexture : MonoBehaviour
{

    public int dmgMeter;

    public Material basemat;
    public Material crack1;
    public Material crack2;
    public Material crack3;
    public Material broke;


    // Update is called once per frame
    void Update()
    {
        matChange();
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    for (int i = 0; i < dmgMeter; i++)
    //    {
    //        if (other.CompareTag("Dmg"))
    //        {
    //            i++;
    //            Debug.Log("hurt");
    //        }
    //        if (other.CompareTag("Clear"))
    //        {
    //            i = 0;
    //            Debug.Log("heal");
    //        }

    //    }
    //}

    void matChange()
    {
        int i = dmgMeter;
        if (i == 0)
        {
            gameObject.GetComponent<Renderer>().material = basemat;
            //Debug.Log("no dmg");
        }
        if (i == 1)
        {
            gameObject.GetComponent<Renderer>().material = crack1; 
            Debug.Log("crack1");
        }
        if (i == 2)
        {
            gameObject.GetComponent<Renderer>().material = crack2;
            Debug.Log("crack2");
        }
        if (i == 3)
        {
            gameObject.GetComponent<Renderer>().material = crack3;
            Debug.Log("crack3");
        }
        if (i == 4)
        {
            gameObject.GetComponent<Renderer>().material = broke;
            Debug.Log("broken");
        }
    }
}
