using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static Dictionary<int, ItemSpawner> spawners = new Dictionary<int, ItemSpawner>();
    private static int nextSpawnerId = 1;

    public GameObject itemPrefab;

    public int spawnerId;
    public bool hasItem;

    // Start is called before the first frame update
    void Start()
    {
        hasItem = false;
        spawnerId = nextSpawnerId;
        nextSpawnerId++;
        spawners.Add(spawnerId, this);

        StartCoroutine(SpawnItem());
    }

    public void Collided(ItemCollector collector)
    {
        Debug.Log("Collided with itemspawner");

        if (!collector.stats.hasWeapon && itemPrefab.tag == "Weapon" && hasItem)
        {
            Transform righthand = RecursiveFindChild(collector.transform.root, "RightHand");

            GameObject item = Instantiate(itemPrefab, new Vector3(0.156f, 0.34f, 0.036f), Quaternion.Euler(0, 180, -90));

            item.transform.parent = righthand;
            item.transform.localPosition = new Vector3(0.156f, 0.34f, 0.036f);
            item.transform.localRotation = Quaternion.Euler(0, 180, -90);
            item.transform.GetComponent<MeshCollider>().enabled = false;
            item.transform.GetComponent<Weapon>().enabled = true;
            collector.stats.hasWeapon = true;

            Debug.Log("Player has collected a weapon!");

            ItemPickedUp(collector.transform.root.GetComponent<Player>().id);
        }
    }

    Transform RecursiveFindChild(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.tag == tag)
            {
                return child;
            }
            else
            {
                Transform found = RecursiveFindChild(child, tag);
                if (found != null)
                {
                    return found;
                }
            }
        }
        return null;
    }

    public IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(10f);
        ServerSend.ItemSpawned(spawnerId);
        hasItem = true;
    }

    private void ItemPickedUp(int byPlayer)
    {
        hasItem = false;
        ServerSend.ItemPickedUp(spawnerId, byPlayer);
    }

}
