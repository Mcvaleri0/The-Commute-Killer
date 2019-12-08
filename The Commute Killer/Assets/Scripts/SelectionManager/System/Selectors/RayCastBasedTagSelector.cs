using UnityEngine;

public class RayCastBasedTagSelector : MonoBehaviour, ISelector
{
    public string Tag = "Selectable";
    public Transform Selection;

    public Transform LastHit;

    [Range(1f, 10f)]
    public float MaxDistance = 2;

    private GameObject Player;

    public void Start()
    {
        this.Player = GameObject.Find("PlayerCharacter");
    }

    public void Check(Ray ray)
    {
        this.Selection = null;

        if (Physics.Raycast(ray, out var hit))
        {
            this.LastHit = hit.transform;

            if (this.LastHit.CompareTag(this.Tag) && 
                Vector3.Distance(this.LastHit.transform.position, this.Player.transform.position) <= this.MaxDistance)
            {
                this.Selection = this.LastHit;
            }
        }
    }

    public Transform GetSelection()
    {
        return this.Selection;
    }
}
