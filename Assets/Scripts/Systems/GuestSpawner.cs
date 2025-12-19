using UnityEngine;
using UnityEngine.AI;

public class GuestSpawner : MonoBehaviour
{
    [SerializeField] private HotelManager hotel;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform entryPoint;
    [SerializeField] private Transform exitPoint;   // ? добавили
    [SerializeField] private GameObject guestPrefab;

    private float timer;

    private void Awake()
    {
        if (hotel == null) hotel = FindFirstObjectByType<HotelManager>();
    }

    private void Update()
    {
        if (hotel == null || spawnPoint == null || entryPoint == null || exitPoint == null || guestPrefab == null)
            return;

        timer += Time.deltaTime;
        if (timer >= hotel.SpawnIntervalSeconds)
        {
            timer = 0f;
            SpawnGuest();
        }
    }

    private void SpawnGuest()
    {
        // 1) spawn на NavMesh
        Vector3 spawnPos = spawnPoint.position;
        if (NavMesh.SamplePosition(spawnPos, out NavMeshHit sHit, 5f, NavMesh.AllAreas))
            spawnPos = sHit.position;

        // 2) entry на NavMesh
        Vector3 entryPos = entryPoint.position;
        if (NavMesh.SamplePosition(entryPos, out NavMeshHit eHit, 5f, NavMesh.AllAreas))
            entryPos = eHit.position;

        // 3) exit на NavMesh
        Vector3 exitPos = exitPoint.position;
        if (NavMesh.SamplePosition(exitPos, out NavMeshHit xHit, 5f, NavMesh.AllAreas))
            exitPos = xHit.position;

        var go = Instantiate(guestPrefab, spawnPos, Quaternion.identity);

        // фикс "не на навмеш" после Instantiate
        var nav = go.GetComponent<NavMeshAgent>();
        if (nav != null)
            nav.Warp(go.transform.position);

        var agent = go.GetComponent<GuestAgent>();
        if (agent != null)
            agent.Init(hotel, entryPos, exitPos);
    }
}
