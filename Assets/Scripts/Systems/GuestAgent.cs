using UnityEngine;
using UnityEngine.AI;

public class GuestAgent : MonoBehaviour
{
    private HotelManager hotel;
    private NavMeshAgent nav;
    private Wallet wallet;

    private Vector3 entryPos;
    private Vector3 exitPos;

    private bool hasTargets;
    private bool leaving;

    public void Init(HotelManager hotelManager, Vector3 entryPosition, Vector3 exitPosition)
    {
        hotel = hotelManager;
        entryPos = entryPosition;
        exitPos = exitPosition;
        hasTargets = true;

        if (nav == null) nav = GetComponent<NavMeshAgent>();
        if (nav != null) nav.SetDestination(entryPos);
    }

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        wallet = FindFirstObjectByType<Wallet>();
    }

    private void Start()
    {
        if (hasTargets && nav != null)
            nav.SetDestination(entryPos);
    }

    private void Update()
    {
        if (nav == null || hotel == null || !hasTargets) return;

        if (nav.pathPending) return;

        if (nav.remainingDistance <= nav.stoppingDistance + 0.1f)
        {
            if (!leaving)
            {
                bool accepted = Random.value <= hotel.AcceptChance;

                if (accepted)
                {
                    if (wallet != null) wallet.Add(hotel.price);
                    hotel.OnGuestArrived(true);
                    Destroy(gameObject);
                }
                else
                {
                    hotel.OnGuestRejected();
                    leaving = true;
                    nav.SetDestination(exitPos);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
