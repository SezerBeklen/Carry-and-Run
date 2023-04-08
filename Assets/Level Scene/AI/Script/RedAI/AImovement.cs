using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class AImovement : MonoBehaviour
{
    public static AImovement instanceAI;

    [HideInInspector]
    public NavMeshAgent a�_player;
    private Transform a�_CarrierTransform;

    [HideInInspector] 
    public Animator a�_anim;
    public GameObject a�_BringToCoinArea;
    public List<GameObject> a�_targets = new List<GameObject>();
    public List<GameObject> fallingsCoins = new List<GameObject>();
    public List<int> usedNumber = new List<int>();

    private int maxNumber = 4;
    private int minNumber = 0;
    
    private float a�_Coin_YAxis;

    [HideInInspector]
    public int a�_CoinCounter_Limit;
    public int a�CoinCounter;    
    public int FallingCoin�ndex;
    public int target�ndex;

    public bool canSeePlayer;
    public bool a�_�smove;
    public bool a�_isHitCar;

    [Header("Field Of View")]
    public float radius;
    [Range(0,360)]
    public float angle;
    public GameObject playerRef;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    private void Awake()
    {
        if(instanceAI == null)
        {
            instanceAI = this;
        }
    }
    void Start()
    {
        //playerRef = GameObject.FindGameObjectWithTag("Player");
        a�_player = GetComponent<NavMeshAgent>();
        a�_anim = gameObject.transform.GetChild(0).GetComponent<Animator>();
        StartCoroutine(FOVRoutine());
        target�ndex = GenerateRandomNumber();
        a�_CoinCounter_Limit = Random.Range(5, 7);
        a�_�smove = true;

    }

    void Update()
    {
        if(GameManager._instance.gamestate == GameManager.GameState.Ingame)
        {
            //AI animation control
            if (a�_�smove)
            {
                a�_anim.SetBool("a�_move", true);
            }
            else
            {
                a�_anim.SetBool("a�_move", false);
            }

            //Car hit Check
            if (a�_isHitCar)
            {
                a�_player.speed = 0;
            }

            //collect fallen coins
            FallingCoinsPickUp();

            //Table index decision 
            if (a�CoinCounter <= a�_CoinCounter_Limit && !canSeePlayer)
            {

                a�_player.SetDestination(a�_targets[target�ndex].transform.position);


            }
            if (a�_player.remainingDistance < 0.5f && !canSeePlayer)
            {
                target�ndex = GenerateRandomNumber();
            }

            //Destination decision check for drop off area
            if (a�CoinCounter > a�_CoinCounter_Limit - 1)
            {
                canSeePlayer = false;
                a�_player.SetDestination(a�_BringToCoinArea.transform.position);

            }
            A�CoinDropOffAreaDistance();
        }

        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("coin"))
        {
            if (fallingsCoins.Contains(other.gameObject)) 
            {
                fallingsCoins.Remove(other.gameObject);
            }            
            if (!AIStackCoin.instancee.a�_coins.Contains(other.gameObject))
            {
                
                if (a�CoinCounter < a�_CoinCounter_Limit+1)
                {
                   
                    if (a�_player.speed > 0)
                    {
                        a�_player.speed -= 0.2f;
                        
                    }
                    a�_CarrierTransform = AIStackCoin.instancee.a�_Carrier.transform;
                        //.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(1).GetChild(0).gameObject.transform;
                    other.gameObject.transform.parent = a�_CarrierTransform;
                    other.GetComponent<BoxCollider>().enabled = false;
                    other.gameObject.tag = "Untagged";
                    other.gameObject.layer=LayerMask.NameToLayer("Default");

                    a�CoinCounter += 1;

                    Vector3 playerPosOffset = new Vector3(transform.position.x, a�_Coin_YAxis, transform.position.z);
                    other.gameObject.transform.DOMove(playerPosOffset, 0.2f).OnComplete(() =>
                    { AIStackCoin.instancee.AIStackCoins(other.gameObject, AIStackCoin.instancee.a�_coins.Count - 1); });

                    Vector3 coinRotate = new Vector3(0, 0, 90);
                    other.gameObject.transform.DOLocalRotate(coinRotate, 0.3f).OnComplete(() =>
                    { other.gameObject.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.3f); });

                }


            }
        }

        /*if (other.gameObject.CompareTag("crashingcar"))
        {
            SoundManager.instance.CrashSound();
            a�CoinCounter = 0;
            a�_anim.Play("a�HitBycar");
            a�_isHitCar = true;
            GetComponent<Rigidbody>().isKinematic = true;
            StartCoroutine(AIWaithAnimProces());
            
            for (int i = AIStackCoin.instancee.a�_coins.Count - 1; i > 0; i--)
            {
                AIRigidbodyState(i, false, true);
                AICollisionState(i, true, false);

                Rigidbody rb;
                rb = AIStackCoin.instancee.a�_coins[i].GetComponent<Rigidbody>();
                int directionValue = Random.Range(-3, 4);
                rb.AddForce(new Vector3(directionValue, 2, directionValue));
                FallingsCoins(i);
                AIStackCoin.instancee.a�_coins[i].transform.parent = null;
                AIStackCoin.instancee.a�_coins[i].gameObject.layer = LayerMask.NameToLayer("coin");

                AIStackCoin.instancee.a�_coins.RemoveAt(i);
                a�_player.speed = 5;

            }

        }*/


    }
   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("crashingcar"))
        {
            a�CoinCounter = 0;
            a�_anim.Play("a�HitBycar");
            a�_isHitCar = true;
            GetComponent<Rigidbody>().isKinematic = true;
            StartCoroutine(AIWaithAnimProces(collision.gameObject));
            SoundManager.instance.CrashSound();
            for (int i = AIStackCoin.instancee.a�_coins.Count - 1; i > 0; i--)
            {
                AIRigidbodyState(i, false, true);
                AICollisionState(i, true, false);

                Rigidbody rb;
                rb = AIStackCoin.instancee.a�_coins[i].GetComponent<Rigidbody>();
                int directionValue = Random.Range(-1, 2);
                rb.AddForce(new Vector3(directionValue, 2, directionValue));
                FallingsCoins(i);
                AIStackCoin.instancee.a�_coins[i].transform.parent = null;
                AIStackCoin.instancee.a�_coins[i].gameObject.layer = LayerMask.NameToLayer("coin");

                AIStackCoin.instancee.a�_coins.RemoveAt(i);
                a�_player.speed = 5;
                
            }

        }

    } 
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }
    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0) 
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;

                }
                else
                {
                    canSeePlayer = false;
                }


            }
            else
            {
                canSeePlayer = false;

            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }

    }
    private int GenerateRandomNumber()
    {
        target�ndex = Random.Range(minNumber, maxNumber);

        if (!usedNumber.Contains(target�ndex))
        {
            usedNumber.Add(target�ndex);
            return target�ndex;
        }
        else
        {
            if (usedNumber.Count > 3) 
            {
                usedNumber.Clear();
                //target�ndex = Random.Range(minNumber, maxNumber);
            }

            return GenerateRandomNumber();
        }
        
    }
    private void AIRigidbodyState(int index, bool isKinematic, bool Gravity)
    {
        AIStackCoin.instancee.a�_coins[index].GetComponent<Rigidbody>().isKinematic = isKinematic;
        AIStackCoin.instancee.a�_coins[index].GetComponent<Rigidbody>().useGravity = Gravity;

    }
    private void AICollisionState(int index, bool Enabled, bool Trigger)
    {
        AIStackCoin.instancee.a�_coins[index].GetComponent<BoxCollider>().enabled = Enabled;
        AIStackCoin.instancee.a�_coins[index].GetComponent<BoxCollider>().isTrigger = Trigger;

    }
    private IEnumerator AIWaithAnimProces(GameObject obj)
    {
        yield return new WaitForSeconds(0.3f);
        if (obj.name != "obstacle")
        {
            SoundManager.instance.HornSound();
        }
        yield return new WaitForSeconds(3f);
        a�_isHitCar = false;
        a�_player.speed = 5;
        GetComponent<Rigidbody>().isKinematic = false;
    }
    private void FallingsCoins(int index)
    {
        if (!fallingsCoins.Contains(AIStackCoin.instancee.a�_coins[index]))
        {
            fallingsCoins.Add(AIStackCoin.instancee.a�_coins[index]);
        }
    }
    private void FallingCoinsPickUp()
    {
        if (canSeePlayer && fallingsCoins.Count > 0) 
        {
            a�_player.SetDestination(fallingsCoins[FallingCoin�ndex].transform.position);
        }

    }
    private void A�CoinDropOffAreaDistance()
    {
        float dist;
        dist = Vector3.Distance(transform.position, a�_BringToCoinArea.transform.position);
        if (dist < 12 && a�CoinCounter > 0 && !canSeePlayer) 
        {
            a�_player.SetDestination(a�_BringToCoinArea.transform.position);
        }
    }

}
