using UnityEngine;
using System.Collections.Generic;

public abstract class Interactable : MonoBehaviour
{

    protected GameObject SelectableParticles;

    public List<Action.IDs> PossibleActions { get; protected set; }

    public void Start()
    {
        this.tag = "Selectable";

        SelectableParticles = new GameObject("SelectionParticleSystem");
        SelectableParticles.AddComponent(typeof(ParticleSystem));
        SelectableParticles.transform.SetParent(gameObject.transform);
        SelectableParticles.transform.position = SelectableParticles.transform.parent.position;
        SelectableParticles.transform.localScale = new Vector3(1, 1, 1);

        var point = gameObject.transform.Find("SelectablePoint");
        if (point != null)
        {
            SelectableParticles.transform.position = point.transform.position;
        }

        var ps = SelectableParticles.GetComponent<ParticleSystem>();
            
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.2f; 

        var main = ps.main;
        main.startSize       = new ParticleSystem.MinMaxCurve(0.05f, 0.15f);
        main.gravityModifier =  new ParticleSystem.MinMaxCurve(-0.001f, -0.005f);
        main.startSpeed      = 0;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        var emission = ps.emission;
        emission.rateOverTime = 2;

        var sz = ps.sizeOverLifetime;
        sz.enabled = true;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.7f, 1.0f);
        curve.AddKey(1.0f, 0.0f);
        sz.size = new ParticleSystem.MinMaxCurve(1, curve);

        var psRenderer = SelectableParticles.GetComponent<ParticleSystemRenderer>();
        psRenderer.material = Resources.Load("Materials/SelectableParticles/Sparkle", typeof(Material)) as Material;

        ps.Play();
    }

    public virtual bool Interact(Action.IDs id) { return false; }

    public virtual bool CanInteract(Agent interactor, Action.IDs id) { return false; }

    protected bool ActionAvailable(Action.IDs id)
    {
        if(this.PossibleActions.FindIndex(x => x == id) != -1)
        {
            return true;
        }

        return false;
    }
}
