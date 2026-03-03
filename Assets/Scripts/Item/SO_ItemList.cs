using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "so_ItemList", menuName = "Scriptable Objects/Items/Item List")]
public class SO_ItemList : ScriptableObject
{
    [SerializeField]
    // aqui eu só atribui um new() para evitar o warning da unity. Mas ao iniciar o jogo, a unity vai sobrescrever essa lista e ela será coletada pelo GC.
    private List<ItemsData> itemData = new List<ItemsData>();


    public List<ItemsData> ItemData => itemData;
}
