using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeteor : MonoBehaviour
{
    bool firstTime = true;
    SphereCollider sc;
    public int dmg = 100;
    public Transform target;
    Vector3 targetPos;
    public float speed;
    public GameObject meteorGraphic;
    public GameObject expl;
    public GameObject indicator;
    GameObject ind;
    bool canGo = false;
    public float detectionRadius = 5f;  // Adjust the radius as needed
    public LayerMask playerLayer;
    // Start is called before the first frame update
    void Start()
    {
        meteorGraphic.SetActive(false);
        StartCoroutine(LoadMeteor());
        targetPos = target.position;
        ind = Instantiate(indicator, targetPos, Quaternion.identity);
        ind.transform.GetChild(0).GetChild(0).position = targetPos;
        sc = GetComponent<SphereCollider>();
    }

    IEnumerator LoadMeteor()
    {
        
        yield return new WaitForSeconds(1);
        canGo = true;
        meteorGraphic.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(canGo)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
        
        if(Mathf.Approximately(transform.position.y, targetPos.y))
        {
            Explode();
        }
    }
    private void Explode()
    {
        if(Vector3.Distance(transform.position, target.position) < detectionRadius)
        {
            target.gameObject.GetComponent<HealthSystem>().TakeDamage(dmg);
        }
        meteorGraphic.SetActive(false);
        expl.GetComponent<ParticleSystem>().Play();
        expl.transform.parent = null;
        CameraShake.Instance.ShakeCamera(3f, 1.5f);
        Destroy(expl, 1f);
        Destroy(ind);
        Destroy(gameObject);
    }



}
