using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] ItemData[] itemsToGive;
    [SerializeField] int quantityPerHit = 1;
    [SerializeField] int capacy = 3;

    public void Gather(Vector3 position, Vector3 normal)
    {
        if(capacy == 0) return;

        for (int i = 0; i < quantityPerHit; i++)
        {
            int index = Random.Range(0, itemsToGive.Length);
            Instantiate(itemsToGive[index].dropPrefab, position + Vector3.up, Quaternion.LookRotation(normal, Vector3.up));
        }

        capacy--;
    }
}