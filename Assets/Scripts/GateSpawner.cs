using UnityEngine;

public class GateSpawner : MonoBehaviour
{
    public EnergyGate gatePrefab;
    public float spawnRate = 1f;
    public float minHeight = -1f;
    public float maxHeight = 2f;
    public float verticalGap = 3f;

    private void OnEnable()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
    }

    private void Spawn()
    {
        EnergyGate gate = Instantiate(gatePrefab, transform.position, Quaternion.identity);
        gate.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
        gate.gap = verticalGap;
    }

}
