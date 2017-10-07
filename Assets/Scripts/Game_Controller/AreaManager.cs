using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour {

    //self
    public static AreaManager instance;

    public List<Sprite> asteroidSprites;
    [SerializeField]
    GameObject asteroidPrefab;

    public float epaisseur = 10.0f;
    public Gradient asteroidColors;

    Dictionary<GameObject, KeyValuePair<Vector3, Vector3>> asteroidBelt; //go & start pos

    GameObject beltRoot;
    GameObject beltRootlvl1, beltRootlvl2, beltRootlvl3;

    public bool shrinking = false;
    //wwise 
    float startTime = 0.0f;
    float endTime = 0.0f;
    float shrinkDuration = 0.0f;
    float circleStart;
    float circleEnd;

    [Header("Lé jolie param de vitesse pour Vinny")]
    public float beltSpeed1; 
    public float beltSpeed2;
    public float beltSpeed3;
    public float parallaxIntensity;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        beltRootlvl1 = new GameObject("AsteroidRing1");
        beltRootlvl2 = new GameObject("AsteroidRing2");
        beltRootlvl3 = new GameObject("AsteroidRing3");

        asteroidBelt = new Dictionary<GameObject, KeyValuePair<Vector3, Vector3>>();
    }

    public void reset()
    {
        shrinking = false;
        asteroidBelt.Clear();
    }

    void OnEnable()
    {
        EventManager.StartListening("generateArea", generate);
        EventManager.StartListening("gameStart", gameStart);
    }

    void OnDisable()
    {
        EventManager.StopListening("generateArea", generate);
        EventManager.StopListening("gameStart", gameStart);
    }

    void generate()
    {
        GameManager instance = GameManager.instance;
        circleStart = instance.asteroidCircleStart;
        circleEnd = instance.asteroidCircleEnd;
        shrinkDuration = instance.shrinkDuration;
        int count = instance.beltAsteroidCount;

        for(int i=0; i<count; ++i)
        {
            GameObject go = GameObject.Instantiate(asteroidPrefab);

            Destroy(go.GetComponent<PolygonCollider2D>());

            go.GetComponent<SpriteRenderer>().color = asteroidColors.Evaluate(Random.value);
            var ps = go.GetComponentInChildren(typeof(ParticleSystem));
            float scaleOffset = Random.value + .5f;
            ps.transform.localScale *= scaleOffset;
            go.transform.Rotate(Vector3.forward,Random.Range(0, 360));
            float offset = (Random.value) * epaisseur;
            go.transform.position += Vector3.Normalize(go.transform.right) * (circleStart + offset);
            go.transform.parent = (Random.value > .5f) ? beltRootlvl1.transform : (Random.value > .85f) ? beltRootlvl2.transform : beltRootlvl3.transform;
            Vector3 endPos = Vector3.Normalize(go.transform.position - Vector3.zero) * (circleEnd + offset*1.5f) + Vector3.forward * Random.value * parallaxIntensity;
            go.transform.localScale *= scaleOffset;
            go.transform.position += Vector3.forward * Random.value * parallaxIntensity;

            asteroidBelt.Add(go, new KeyValuePair<Vector3, Vector3>(go.transform.position, endPos));
        }
    }

    void gameStart()
    {
        startTime = Time.time;
        endTime = startTime + shrinkDuration;
        shrinking = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero, Mathf.Lerp(circleStart, circleEnd, 1 - ((Time.time - startTime) / shrinkDuration)));
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Vector3.zero, circleStart);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Vector3.zero, circleEnd);
    }

    public static float getShrinkRatio() // return current ratio of the shrink occuring
    {
        return 1 - ((Time.time - instance.startTime) / instance.shrinkDuration);
    }

    void Update()
    {
        Quaternion r2 = beltRootlvl1.transform.rotation;
        beltRootlvl1.transform.rotation = Quaternion.identity;
        Quaternion r3 = beltRootlvl2.transform.rotation;
        beltRootlvl2.transform.rotation = Quaternion.identity;
        Quaternion r4 = beltRootlvl3.transform.rotation;
        beltRootlvl3.transform.rotation = Quaternion.identity;
        if (shrinking)
        {
            foreach(KeyValuePair<GameObject, KeyValuePair<Vector3, Vector3>> go in asteroidBelt)
            {
                go.Key.transform.position = Vector3.Lerp(go.Value.Key, go.Value.Value, 1-((Time.time - startTime) / shrinkDuration));
            }

            if(Time.time - startTime > shrinkDuration)
            {
                shrinking = false;
            }
        }
        beltRootlvl1.transform.rotation = r2;
        beltRootlvl1.transform.Rotate(Vector3.forward, Time.deltaTime* beltSpeed1);
        beltRootlvl2.transform.rotation = r3;
        beltRootlvl2.transform.Rotate(Vector3.forward, Time.deltaTime* beltSpeed2);
        beltRootlvl3.transform.rotation = r4;
        beltRootlvl3.transform.Rotate(Vector3.forward, Time.deltaTime* beltSpeed3);
    }
}
