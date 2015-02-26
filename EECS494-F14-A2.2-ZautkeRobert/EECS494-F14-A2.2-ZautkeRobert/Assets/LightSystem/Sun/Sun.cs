using UnityEngine;
using System.Collections;

public class Sun : MonoBehaviour {

    //---------NOTE----------//
    // The radius of the sun's orbit is dependant on
    // the distance you place the sun from the target(center of game world).

    // The target that the sun will be rotating around
    // Probably best to set this to the terrain on which
    // your game takes place.
    public Transform target;

    // The direction in which the sun rotates. This will be based
    // on initial placement of the sun and its orientation to the
    // game world.
    public enum Direction {left, right, up, down, forward, back};
    public Direction SunDirection;

    private FatherTime ft;

    // A placeHolder prefab to describe the initial position of
    // the sun in the gameworld. The initial position of the sun
    // corresponds to midnight(12:00am) in the gameworld.
    public GameObject PosHolderPrefab;
    private GameObject InitPos;

    void Awake() {
        ft = GameObject.Find("FatherTime").GetComponent<FatherTime>();
    }

	void Start () {
        InitPos = Instantiate(PosHolderPrefab) as GameObject;
        InitPos.transform.position = this.transform.position;
	}
	

    void Update() {

        if(ft != null)
            SetSunPosition();
    }


    // Sets the sun's position based on the GameTime and the
    // direction in which the sun rotates.
    private void SetSunPosition() {

        float angle = (ft.GameTime / ft.secondsInDay) * 360f;

        if (SunDirection != Direction.left &&
            SunDirection != Direction.right &&
            SunDirection != Direction.up &&
            SunDirection != Direction.down &&
            SunDirection != Direction.forward &&
            SunDirection != Direction.back) {

            SunDirection = Direction.left;
        }

        if (SunDirection == Direction.left)
        {
            InitPos.transform.RotateAround(target.position, Vector3.left, angle);
            this.transform.position = InitPos.transform.position;
            InitPos.transform.RotateAround(target.position, Vector3.right, angle);
        }
        else if (SunDirection == Direction.right) {
            InitPos.transform.RotateAround(target.position, Vector3.right, angle);
            this.transform.position = InitPos.transform.position;
            InitPos.transform.RotateAround(target.position, Vector3.left, angle);        
        }
        else if (SunDirection == Direction.up)
        {
            InitPos.transform.RotateAround(target.position, Vector3.up, angle);
            this.transform.position = InitPos.transform.position;
            InitPos.transform.RotateAround(target.position, Vector3.down, angle);
        }
        else if (SunDirection == Direction.down)
        {
            InitPos.transform.RotateAround(target.position, Vector3.down, angle);
            this.transform.position = InitPos.transform.position;
            InitPos.transform.RotateAround(target.position, Vector3.up, angle);
        }
        else if (SunDirection == Direction.forward)
        {
            InitPos.transform.RotateAround(target.position, Vector3.forward, angle);
            this.transform.position = InitPos.transform.position;
            InitPos.transform.RotateAround(target.position, Vector3.back, angle);
        }
        else if (SunDirection == Direction.back)
        {
            InitPos.transform.RotateAround(target.position, Vector3.back, angle);
            this.transform.position = InitPos.transform.position;
            InitPos.transform.RotateAround(target.position, Vector3.forward, angle);
        }
    }
}
