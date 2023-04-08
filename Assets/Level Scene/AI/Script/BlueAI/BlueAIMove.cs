using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class BlueAIMove : MonoBehaviour
{

    public static BlueAIMove instance_AI;

    [HideInInspector]
    public NavMeshAgent a�_player;
    private Transform a�_CarrierTransform;

    [HideInInspector]
    public Animator a�_anim;
    public GameObject a�_BringToCoinArea;
    public List<GameObject> a�_targets = new List<GameObject>();
    public List<GameObject> BluefallingsCoins = new List<GameObject>();
    public List<int> usedNumber = new List<int>();

    private int maxNumber = 4;
    private int minNumber = 0;

    private float a�_Coin_YAxis;

    [HideInInspector]
    public int Bluea�_CoinCounter_Limit;
    public int Bluea�CoinCounter;
    public int FallingCoin�ndex;
    public int target�ndex;

    public bool Blue_canSeePlayer;
    public bool a�_�smove;
    public bool a�_isHitCar;

    [Header("Field Of View")]
    public float radius;
    [Range(0, 360)]
    public float angle;
    public GameObject playerRef;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    private void Awake()
    {
        if (instance_AI == null)
        {
            instance_AI = this;
        }
    }
    void Start()
    {
        //playerRef = GameObject.FindGameObjectWithTag("Player");
        a�_player = GetComponent<NavMeshAgent>();
        a�_anim = gameObject.transform.GetChild(0).GetComponent<Animator>();
        StartCoroutine(FOVRoutine());
        target�ndex = GenerateRandomNumber();
        Bluea�_CoinCounter_Limit = Random.Range(5, 7);
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
            if (Bluea�CoinCounter <= Bluea�_CoinCounter_Limit && !Blue_canSeePlayer)
            {

                a�_player.SetDestination(a�_targets[target�ndex].transform.position);


            }
            if (a�_player.remainingDistance < 0.5f && !Blue_canSeePlayer)
            {
                target�ndex = GenerateRandomNumber();
            }

            //Destination decision check for drop off area
            if (Bluea�CoinCounter > Bluea�_CoinCounter_Limit - 1)
            {
                Blue_canSeePlayer = false;
                a�_player.SetDestination(a�_BringToCoinArea.transform.position);

            }
            A�CoinDropOffAreaDistance();
        }

       

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("coin"))
        {
            if (BluefallingsCoins.Contains(other.gameObject))
            {
                BluefallingsCoins.Remove(other.gameObject);
            }
            if (!BlueAIStackCoin.blueAInstance.Blue_A�_coins.Contains(other.gameObject))
            {

                if (Bluea�CoinCounter < Bluea�_CoinCounter_Limit + 1)
                {

                    if (a�_player.speed > 0)
                    {
                        a�_player.speed -= 0.2f;

                    }
                    a�_CarrierTransform = BlueAIStackCoin.blueAInstance.Blue_a�_Carrier.transform;
                    //.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(1).GetChild(0).gameObject.transform;
                    other.gameObject.transform.parent = a�_CarrierTransform;
                    other.GetComponent<BoxCollider>().enabled = false;
                    other.gameObject.tag = "Untagged";
                    other.gameObject.layer = LayerMask.NameToLayer("Default");

                    Bluea�CoinCounter += 1;

                    Vector3 playerPosOffset = new Vector3(transform.position.x, a�_Coin_YAxis, transform.position.z);
                    other.gameObject.transform.DOMove(playerPosOffset, 0.2f).OnComplete(() =>
                    { BlueAIStackCoin.blueAInstance.BlueA�StackCoins(other.gameObject, BlueAIStackCoin.blueAInstance.Blue_A�_coins.Count - 1); });

                    Vector3 coinRotate = new Vector3(0, 0, 90);
                    other.gameObject.transform.DOLocalRotate(coinRotate, 0.3f).OnComplete(() =>
                    { other.gameObject.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.3f); });

                }


            }
        }
    }


     private void OnCollisionEnter(Collision collision)
     {
         if (collision.gameObject.CompareTag("crashingcar"))
         {
             Bluea�CoinCounter = 0;
             a�_anim.Play("a�HitBycar");
             a�_isHitCar = true;
            GetComponent<Rigidbody>().isKinematic = true;
            SoundManager.instance.CrashSound();
            
            StartCoroutine(AIWaithAnimProces(collision.gameObject));
             for (int i = BlueAIStackCoin.blueAInstance.Blue_A�_coins.Count - 1; i > 0; i--)
             {
                 AIRigidbodyState(i, false, true);
                 AICollisionState(i, true, false);

                 Rigidbody rb;
                 rb = BlueAIStackCoin.blueAInstance.Blue_A�_coins[i].GetComponent<Rigidbody>();
                 int directionValue = Random.Range(-1, 2);
                 rb.AddForce(new Vector3(directionValue, 1, directionValue));
                 FallingsCoins(i);
                 BlueAIStackCoin.blueAInstance.Blue_A�_coins[i].transform.parent = null;
                 BlueAIStackCoin.blueAInstance.Blue_A�_coins[i].gameObject.layer = LayerMask.NameToLayer("coin");

                 BlueAIStackCoin.blueAInstance.Blue_A�_coins.RemoveAt(i);
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
                    Blue_canSeePlayer = true;

                }
                else
                {
                    Blue_canSeePlayer = false;
                }


            }
            else
            {
                Blue_canSeePlayer = false;

            }
        }
        else if (Blue_canSeePlayer)
        {
            Blue_canSeePlayer = false;
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
    private void AIRigidbodyState(int index, bool state1, bool state2)
    {
        BlueAIStackCoin.blueAInstance.Blue_A�_coins[index].GetComponent<Rigidbody>().isKinematic = state1;
        BlueAIStackCoin.blueAInstance.Blue_A�_coins[index].GetComponent<Rigidbody>().useGravity = state2;

    }
    private void AICollisionState(int index, bool state1, bool state2)
    {
        BlueAIStackCoin.blueAInstance.Blue_A�_coins[index].GetComponent<BoxCollider>().enabled = state1;
        BlueAIStackCoin.blueAInstance.Blue_A�_coins[index].GetComponent<BoxCollider>().isTrigger = state2;

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
        if (!BluefallingsCoins.Contains(BlueAIStackCoin.blueAInstance.Blue_A�_coins[index]))
        {
            BluefallingsCoins.Add(BlueAIStackCoin.blueAInstance.Blue_A�_coins[index]);
        }
    }
    private void FallingCoinsPickUp()
    {
        if (Blue_canSeePlayer && BluefallingsCoins.Count > 0)
        {
            a�_player.SetDestination(BluefallingsCoins[FallingCoin�ndex].transform.position);
        }

    }
    private void A�CoinDropOffAreaDistance()
    {
        float dist;
        dist = Vector3.Distance(transform.position, a�_BringToCoinArea.transform.position);
        if (dist < 12 && Bluea�CoinCounter > 0 && !Blue_canSeePlayer)
        {
            a�_player.SetDestination(a�_BringToCoinArea.transform.position);
        }
    }
}
