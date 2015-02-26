/* Base class for erosion models.
 */

using UnityEngine;
using System.Collections;

public class Erosion {
    protected float[,] heights;
    protected int w;
    protected int h;

    public float smoothness = 12.0f;
    public int iterations = 10;
    public bool doSmooth = true;

    public virtual void Erode(GameObject inTerrain)
    {

    }
}
