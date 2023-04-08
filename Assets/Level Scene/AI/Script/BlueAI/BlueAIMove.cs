using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class BlueAIMove : MonoBehaviour
{

    public static BlueAIMove instance_AI;

    [HideInInspector]
    public NavMeshAgent aý_player;
    private Transform aý_CarrierTransform;

    [HideInInspector]
    public Animator aý_anim;
    public GameObject aý_BringToCoinArea;
    public List<GameObject> aý_targets = new List<GameObject>();
    public List<GameObject> BluefallingsCoins = new List<GameObject>();
    public List<int> usedNumber = new List<int>();

    private int maxNumber = 4;
    private int minNumber = 0;

    private float aý_Coin_YAxis;

    [HideInInspector]
    public int Blueaý_CoinCounter_Limit;
    public int BlueaýCoinCounter;
    public int FallingCoinÝndex;
    public int targetÝndex;

    public bool Blue_canSeePlayer;
    public bool aý_Ýsmove;
    public bool aý_isHitCar;

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
        aý_player = GetComponent<NavMeshAgent>();
        aý_anim = gameObject.transform.GetChild(0).GetComponent<Animator>();
        StartCoroutine(FOVRoutine());
        targetÝndex = GenerateRandomNumber();
        Blueaý_CoinCounter_Limit = Random.Range(5, 7);
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
            if (BlueaýCoinCounter <= Blueaý_CoinCounter_Limit && !Blue_canSeePlayer)
            {

                aý_player.SetDestination(aý_targets[targetÝndex].transform.position);


            }
            if (aý_player.remainingDistance < 0.5f && !Blue_canSeePlayer)
            {
                targetÝndex = GenerateRandomNumber();
            }

            //Destination decision check for drop off area
            if (BlueaýCoinCounter > Blueaý_CoinCounter_Limit - 1)
            {
                Blue_canSeePlayer = false;
                aý_player.SetDestination(aý_BringToCoinArea.transform.position);

            }
            AýCoinDropOffAreaDistance();
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
            if (!BlueAIStackCoin.blueAInstance.Blue_Aý_coins.Contains(other.gameObject))
            {

                if (BlueaýCoinCounter < Blueaý_CoinCounter_Limit + 1)
                {

                    if (aý_player.speed > 0)
                    {
                        aý_player.speed -= 0.2f;

                    }
                    aý_CarrierTransform = BlueAIStackCoin.blueAInstance.Blue_aý_Carrier.transform;
                    //.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(1).GetChild(0).gameObject.transform;
                    other.gameObject.transform.parent = aý_CarrierTransform;
                    other.GetComponent<BoxCollider>().enabled = false;
                    other.gameObject.tag = "Untagged";
                    other.gameObject.layer = LayerMask.NameToLayer("Default");

                    BlueaýCoinCounter += 1;

                    Vector3 playerPosOffset = new Vector3(transform.position.x, aý_Coin_YAxis, transform.position.z);
                    other.gameObject.transform.DOMove(playerPosOffset, 0.2f).OnComplete(() =>
                    { BlueAIStackCoin.blueAInstance.BlueAýStackCoins(other.gameObject, BlueAIStackCoin.blueAInstance.Blue_Aý_coins.Count - 1); });

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
             BlueaýCoinCounter = 0;
             aý_anim.Play("aýHitBycar");
             aý_isHitCar = true;
            GetComponent<Rigidbody>().isKinematic = true;
            SoundManager.instance.CrashSound();
            
            StartCoroutine(AIWaithAnimProces(collision.gameObject));
             for (int i = BlueAIStackCoin.blueAInstance.Blue_Aý_coins.Count - 1; i > 0; i--)
             {
                 AIRigidbodyState(i, false, true);
                 AICollisionState(i, true, false);

                 Rigidbody rb;
                 rb = BlueAIStackCoin.blueAInstance.Blue_Aý_coins[i].GetComponent<Rigidbody>();
                 int directionValue = Random.Range(-1, 2);
                 rb.AddForce(new Vector3(directionValue, 1, directionValue));
                 FallingsCoins(i);
                 BlueAIStackCoin.blueAInstance.Blue_Aý_coins[i].transform.parent = null;
                 BlueAIStackCoin.blueAInstance.Blue_Aý_coins[i].gameObject.layer = LayerMask.NameToLayer("coin");

                 BlueAIStackCoin.blueAInstance.Blue_Aý_coins.RemoveAt(i);
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
    private void AIRigidbodyState(int index, bool state1, bool state2)
    {
        BlueAIStackCoin.blueAInstance.Blue_Aý_coins[index].GetComponent<Rigidbody>().isKinematic = state1;
        BlueAIStackCoin.blueAInstance.Blue_Aý_coins[index].GetComponent<Rigidbody>().useGravity = state2;

    }
    private void AICollisionState(int index, bool state1, bool state2)
    {
        BlueAIStackCoin.blueAInstance.Blue_Aý_coins[index].GetComponent<BoxCollider>().enabled = state1;
        BlueAIStackCoin.blueAInstance.Blue_Aý_coins[index].GetComponent<BoxCollider>().isTrigger = state2;

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
        if (!BluefallingsCoins.Contains(BlueAIStackCoin.blueAInstance.Blue_Aý_coins[index]))
        {
            BluefallingsCoins.Add(BlueAIStackCoin.blueAInstance.Blue_Aý_coins[index]);
        }
    }
    private void FallingCoinsPickUp()
    {
        if (Blue_canSeePlayer && BluefallingsCoins.Count > 0)
        {
            aý_player.SetDestination(BluefallingsCoins[FallingCoinÝndex].transform.position);
        }

    }
    private void AýCoinDropOffAreaDistance()
    {
        float dist;
        dist = Vector3.Distance(transform.position, aý_BringToCoinArea.transform.position);
        if (dist < 12 && BlueaýCoinCounter > 0 && !Blue_canSeePlayer)
        {
            aý_player.SetDestination(aý_BringToCoinArea.transform.position);
        }
    }
}
