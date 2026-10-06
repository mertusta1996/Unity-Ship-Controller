using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class WaterBuoyancyPhysics : MonoBehaviour
{
    public Rigidbody rb;
    public float depthBefSub;
    public float displacementAmt;
    public int floaters;
    public float waterDrag;
    public float waterAngularDrag;
    public WaterSurface water;
    public WaterSearchParameters Search;
    public WaterSearchResult SearchResult;

    private void FixedUpdate()
    {
        rb.AddForceAtPosition(Physics.gravity / floaters, transform.position, ForceMode.Acceleration);

        Search.startPositionWS = transform.position;

        water.ProjectPointOnWaterSurface(Search, out SearchResult);

        if (transform.position.y < SearchResult.projectedPositionWS.y)
        {
            float displacementMulti = Mathf.Clamp01((SearchResult.projectedPositionWS.y - transform.position.y) / depthBefSub) * displacementAmt;
            rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMulti, 0f), transform.position, ForceMode.Acceleration);
            rb.AddForce(displacementMulti * Time.fixedDeltaTime * waterDrag * -rb.velocity, ForceMode.VelocityChange);
            rb.AddTorque(displacementMulti * Time.fixedDeltaTime * waterAngularDrag * -rb.angularVelocity, ForceMode.VelocityChange);
        }
    }
}
