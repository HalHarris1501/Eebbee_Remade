using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resetter : MonoBehaviour, IObserver<Direction>
{
    public void ItemAltered(Direction type, int count)
    {
        this.gameObject.transform.position = new Vector3(42f, 0f, 0f);
    }

    public void ItemRemoved(Direction type)
    {
        throw new System.NotImplementedException();
    }

    public void NewItemAdded(Direction type)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.RegisterObserver(this);
    }
}
