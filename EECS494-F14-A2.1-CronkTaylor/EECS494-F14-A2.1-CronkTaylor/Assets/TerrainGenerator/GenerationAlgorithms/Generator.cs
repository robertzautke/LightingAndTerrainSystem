/*
 * This is the base class for terrain generators.
 * */
using UnityEngine;
using System.Collections;

public class Generator {
    // Size of map
    protected int w;
    protected int h;

    // array of map heights
    protected float[,] heights;

    public Generator()
    {
    }

    public virtual void Generate(GameObject inTerrain)
    {

    }
}
