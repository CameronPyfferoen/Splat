using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public Dropdown down;
    private int optnum = 0;

    private List<Sprite> paints = new List<Sprite>();
    private List<Sprite> paintOptions = new List<Sprite>();

    void Start()
    {
        
        down.ClearOptions();
    }

    public void AddItem(Sprite paintcan)
    {
        paints.Add(paintcan);
        paintOptions.Add(paintcan);
        down.AddOptions(paints);
        down.options[optnum].image = paints[0];
        paints.Clear();
        optnum++;
    }

    public void RemoveItem(int listnum)
    {
        paintOptions.RemoveAt(listnum);
        down.ClearOptions();
        down.AddOptions(paintOptions);
        optnum--;
        down.value--;
    }

    public void Clear()
    {
        optnum = 0;
        paints.Clear();
        paintOptions.Clear();
        down.ClearOptions();
    }
}
