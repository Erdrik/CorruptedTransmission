using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour {

    public ExitDoor _exit;

    public List<MeshRenderer> _quads;

    public Material _activatedMaterial;
    public Material _unactivatedMaterial;

    [Range(0, 2)]
    public int _id;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        Debug.Log("OnTriggerEnter");
        if (other.CompareTag("Professor") && 
            !_exit.IsActivated(_id)) {
            _exit.Activate(_id);
            ChangeMaterial();
            Professor professor = other.GetComponentInParent<Professor>();
            if (professor != null) {
                professor.DropReach();
            }
            else {
                Debug.LogError("Professor collided with button but did not have Professor component in parent!");
            }
        }
    }

    public bool Activated {
        get {
            return _exit.IsActivated(_id);
        }
    }

    private void ChangeMaterial()
    {
        Material material = null;
        if (_exit.IsActivated(_id))
        {
            material = _activatedMaterial;
        }
        else
        {
            material = _unactivatedMaterial;
        }

        foreach (MeshRenderer renderer in _quads)
        {
            renderer.material = material;
        }
    }
}
