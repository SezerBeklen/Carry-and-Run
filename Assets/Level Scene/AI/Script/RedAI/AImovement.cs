using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class AImovement : MonoBehaviour
{
    public static AImovement instanceAI;

    [HideInInspector]
    public NavMeshAgent aý_player;
    private Transform aý_CarrierTransform;

    [HideInInspector] 
    public Animator aý_anim;
    public GameObject aý_BringToCoinArea;
    public List<GameObject> aý_targets = new List<GameObject>();
    public List<GameObject> fallingsCoins = new List<GameObject>();
    public List<int> usedNumber = new List<int>();

    private int maxNumber = 4;
    private int minNumber = 0;
    
    private float aý_Coin_YAxis;

    [HideInInspector]
    public int aý_CoinCounter_Limit;
    public int aýCoinCounter;    
    public int FallingCoinÝndex;
    public int targetÝndex;

    public bool canSeePlayer;
    public bool aý_Ýsmove;
    public bool aý_isHitCar;

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
        aý_player = GetComponent<NavMeshAgent>();
        aý_anim = gameObject.transform.GetChild(0).GetComponent<Animator>();
        StartCoroutine(FOVRoutine());
        targetÝndex = GenerateRandomNumber();
        aý_CoinCounter_Limit = Random.Range(5, 7);
        aý_Ýsmove = true;

    }

    void Update()
    {
        if(GameManager._instance.gamestate == GameManager.GameState.Ingame)
        {
            //AI animation control
            if (aý_Ýsmove)
            {
                aý_anim.SetBool("aý_move", true);
            }
            else
            {
                aý_anim.SetBool("aý_move", false);
            }

            //Car hit Check
            if (aý_isHitCar)
            {
                aý_player.speed = 0;
            }

            //collect fallen coins
            FallingCoinsPickUp();

            //Table index decision 
            if (aýCoinCounter <= aý_CoinCounter_Limit && !canSeePlayer)
            {

                aý_player.SetDestination(aý_targets[targetÝndex].transform.position);


            }
            if (aý_player.remainingDistance < 0.5f && !canSeePlayer)
            {
                targetÝndex = GenerateRandomNumber();
            }

            //Destination decision check for drop off area
            if (aýCoinCounter > aý_CoinCounter_Limit - 1)
            {
                canSeePlayer = false;
                aý_player.SetDestination(aý_BringToCoinArea.transform.position);

            }
            AýCoinDropOffAreaDistance();
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
            if (!AIStackCoin.instancee.aý_coins.Contains(other.gameObject))
            {
                
                if (aýCoinCounter < aý_CoinCounter_Limit+1)
                {
                   
                    if (aý_player.speed > 0)
                    {
                        aý_player.speed -= 0.2f;
                        
                    }
                    aý_CarrierTransform = AIStackCoin.instancee.aý_Carrier.transform;
                        //.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(1).GetChild(0).gameObject.transform;
                    other.gameObject.transform.parent = aý_CarrierTransform;
                    other.GetComponent<BoxCollider>().enabled = false;
                    other.gameObject.tag = "Untagged";
                    other.gameObject.layer=LayerMask.NameToLayer("Default");

                    aýCoinCounter += 1;

                    Vector3 playerPosOffset = new Vector3(transform.position.x, aý_Coin_YAxis, transform.position.z);
                    other.gameObject.transform.DOMove(playerPosOffset, 0.2f).OnComplete(() =>
                    { AIStackCoin.instancee.AIStackCoins(other.gameObject, AIStackCoin.instancee.aý_coins.Count - 1); });

                    Vector3 coinRotate = new Vector3(0, 0, 90);
                    other.gameObject.transform.DOLocalRotate(coinRotate, 0.3f).OnComplete(() =>
                    { other.gameObject.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.3f); });

                }


            }
        }

        /*if (other.gameObject.CompareTag("crashingcar"))
        {
            SoundManager.instance.CrashSound();
            aýCoinCounter = 0;
            aý_anim.Play("aýHitBycar");
            aý_isHitCar = true;
            GetComponent<Rigidbody>().isKinematic = true;
            StartCoroutine(AIWaithAnimProces());
            
            for (int i = AIStackCoin.instancee.aý_coins.Count - 1; i > 0; i--)
            {
                AIRigidbodyState(i, false, true);
                AICollisionState(i, true, false);

                Rigidbody rb;
                rb = AIStackCoin.instancee.aý_coins[i].GetComponent<Rigidbody>();
                int directionValue = Random.Range(-3, 4);
                rb.AddForce(new Vector3(directionValue, 2, directionValue));
                FallingsCoins(i);
                AIStackCoin.instancee.aý_coins[i].transform.parent = null;
                AIStackCoin.instancee.aý_coins[i].gameObject.layer = LayerMask.NameToLayer("coin");

                AIStackCoin.instancee.aý_coins.RemoveAt(i);
                aý_player.speed = 5;

            }

        }*/


    }
   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("crashingcar"))
        {
            aýCoinCounter = 0;
            aý_anim.Play("aýHitBycar");
            aý_isHitCar = true;
            GetComponent<Rigidbody>().isKinematic = true;
            StartCoroutine(AIWaithAnimProces(collision.gameObject));
            SoundManager.instance.CrashSound();
            for (int i = AIStackCoin.instancee.aý_coins.Count - 1; i > 0; i--)
            {
                AIRigidbodyState(i, false, true);
                AICollisionState(i, true, false);

                Rigidbody rb;
                rb = AIStackCoin.instancee.aý_coins[i].GetComponent<Rigidbody>();
                int directionValue = Random.Range(-1, 2);
                rb.AddForce(new Vector3(directionValue, 2, directionValue));
                FallingsCoins(i);
                AIStackCoin.instancee.aý_coins[i].transform.parent = null;
                AIStackCoin.instancee.aý_coins[i].gameObject.layer = LayerMask.NameToLayer("coin");

                AIStackCoin.instancee.aý_coins.RemoveAt(i);
                aý_player.speed = 5;
                
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
        targetÝndex = Random.Range(minNumber, maxNumber);

        if (!usedNumber.Contains(targetÝndex))
        {
            usedNumber.Add(targetÝndex);
            return targetÝndex;
        }
        else
        {
            if (usedNumber.Count > 3) 
            {
                usedNumber.Clear();
                //targetÝndex = Random.Range(minNumber, maxNumber);
            }

            return GenerateRandomNumber();
        }
        
    }
    private void AIRigidbodyState(int index, bool isKinematic, bool Gravity)
    {
        AIStackCoin.instancee.aý_coins[index].GetComponent<Rigidbody>().isKinematic = isKinematic;
        AIStackCoin.instancee.aý_coins[index].GetComponent<Rigidbody>().useGravity = Gravity;

    }
    private void AICollisionState(int index, bool Enabled, bool Trigger)
    {
        AIStackCoin.instancee.aý_coins[index].GetComponent<BoxCollider>().enabled = Enabled;
        AIStackCoin.instancee.aý_coins[index].GetComponent<BoxCollider>().isTrigger = Trigger;

    }
    private IEnumerator AIWaithAnimProces(GameObject obj)
    {
        yield return new WaitForSeconds(0.3f);
        if (obj.name != "obstacle")
        {
            SoundManager.instance.HornSound();
        }
        yield return new WaitForSeconds(3f);
        aý_isHitCar = false;
        aý_player.speed = 5;
        GetComponent<Rigidbody>().isKinematic = false;
    }
    private void FallingsCoins(int index)
    {
        if (!fallingsCoins.Contains(AIStackCoin.instancee.aý_coins[index]))
        {
            fallingsCoins.Add(AIStackCoin.instancee.aý_coins[index]);
        }
    }
    private void FallingCoinsPickUp()
    {
        if (canSeePlayer && fallingsCoins.Count > 0) 
        {
            aý_player.SetDestination(fallingsCoins[FallingCoinÝndex].transform.position);
        }

    }
    private void AýCoinDropOffAreaDistance()
    {
        float dist;
        dist = Vector3.Distance(transform.position, aý_BringToCoinArea.transform.position);
        if (dist < 12 && aýCoinCounter > 0 && !canSeePlayer) 
        {
            aý_player.SetDestination(aý_BringToCoinArea.transform.position);
        }
    }

}
