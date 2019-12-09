using UnityEngine;

public class RayCastBasedTagSelector : MonoBehaviour, ISelector
{
    public string Tag = "Selectable";

    [Range(1f, 10f)]
    public float MaxDistance = 2;

    private GameObject Player;

    public void Start()
    {
        this.Player = GameObject.Find("PlayerCharacter");
    }

    public Transform Check(Ray ray)
    {
        if (Physics.Raycast(ray, out var hit))
        {
            var tr = hit.transform;

            if (tr != null && tr.CompareTag(this.Tag) && 
                Vector3.Distance(tr.position, this.Player.transform.position) <= this.MaxDistance)
            {
                return tr;
            }
        }

        return null;
    }
}
