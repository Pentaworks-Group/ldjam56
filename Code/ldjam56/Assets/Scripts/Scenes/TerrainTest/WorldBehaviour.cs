using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Generation;
using Assets.Scripts.Model;

using UnityEngine;

public class WorldBehaviour : MonoBehaviour
{
    private World world;

    public GameObject templateContainer;
    public GameObject terrainContainer;

    private GameObject terrainTemplate;
    private readonly Dictionary<String, GameObject> entityTemplates = new Dictionary<String, GameObject>();
    
    //private readonly Dictionary<String, ChunkBehaviour> chunkBehaviours = new Dictionary<String, ChunkBehaviour>();

    private void Awake()
    {
        var world = Assets.Scripts.Base.Core.Game.State?.World;

        if (world == default)
        {
            world = new WorldGenerator().Generate();
        }

        this.world = world;
    }

    private void Start()
    {
        LoadTemplates();

        LoadWorld();
    }

    private void LoadTemplates()
    {
        this.terrainTemplate = templateContainer.transform.Find("TerrainTemplate").gameObject;

        var entityTemplateContainerTransform = templateContainer.transform.Find("Entities");

        if (entityTemplateContainerTransform != null)
        {
            foreach (Transform template in entityTemplateContainerTransform)
            {
                this.entityTemplates[template.name] = template.gameObject;
            }
        }
    }

    private void LoadWorld()
    {
        foreach (var chunk in this.world.Chunks)
        {

        }
    }
}
