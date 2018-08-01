using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour {

    public GameObject heart;

    public PlayerManager player;

    public ParticleSystem particleSystem;

    int prevLives = 0;

    List<GameObject> heartList = new List<GameObject>();

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(prevLives != player.lives) {
            if (prevLives > player.lives) {
                for (int i = prevLives - 1; i >= player.lives; i--) {
                    StartCoroutine(LostLife(heartList[i]));
                }
            } else {

                foreach(GameObject h in heartList) {
                    Destroy(h);
                }

                heartList = new List<GameObject>();

                for (int i = 0; i < player.lives; i++) {
                    Vector3 pos = new Vector3(i / 10f, 0, 0);
                    GameObject h = Instantiate(heart, transform);
                    h.transform.eulerAngles = new Vector3(-90, 0, 0);
                    h.transform.localPosition = pos;
                    h.layer = LayerMask.NameToLayer("Lives");
                    h.GetComponent<Spinner>().delay = i / 2;
                    heartList.Add(h);
                }
            }
        }
        prevLives = player.lives;

        /*if(anim != null) {
            // Move the heart
            tran.position = Vector3.Lerp(tran.position, targetPos, Time.deltaTime / 2);

        }*/
    }

    IEnumerator LostLife(GameObject h) {
        bool animationDone = false;
        Spinner spin = h.GetComponent<Spinner>();
        Transform tran = h.transform;

        Vector3 originPos = tran.position;
        Vector3 targetPos = new Vector3(0.8f, -0.431f, -0.028f);

        float maxDist = Vector3.Distance(originPos, targetPos);

        int nFrames = 0;

        Vector3 sum = new Vector3(0, 0, 0.1f);

        while(!animationDone) {
            // Move the heart
            tran.localPosition = Vector3.Lerp(tran.localPosition, targetPos, Time.deltaTime * 5);

            // Enlarge the heart
            float dist = Vector3.Distance(tran.localPosition, targetPos);
            float scale = Mathf.Lerp(1, 2.5f, dist / maxDist);
            tran.localScale = new Vector3(scale, scale, scale);

            //animationDone = true;

            // When is close
            if (dist <= 0.1f) {
                // Spin faster
                //spin.speed += sum;
                spin.speed *= 1.06f;
                nFrames++;
                if (nFrames >= 50) {
                    Destroy(h);
                    //StopAllCoroutines();
                    StartCoroutine("ToggleParticles");
                    animationDone = true;
                    //break;
                }
            }

            yield return null;
        }
    }

    IEnumerator ToggleParticles() {
        var emission = particleSystem.emission;
        emission.enabled = true;
        yield return new WaitForSeconds(0.2f);
        emission.enabled = false;

    }
}
