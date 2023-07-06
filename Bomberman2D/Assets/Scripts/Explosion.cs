using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AnimationSpriteRenderer start, middle, end;

    public void SetActiveSpriteRenderer(AnimationSpriteRenderer renderer)
    {
        start.enabled = renderer == start;
        middle.enabled = renderer == middle;
        end.enabled = renderer == end;
    }

    public void SetDirection(Vector2 direction)
    {
        //set the angle around which the sprite is going to rotate
        float angle = Mathf.Atan2(direction.y, direction.x);

        //Rotate the object in angle around an axis - since angle is in radian, we convert degrees into radians 
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    //A little helper function to destroy the gameobject after some seconds 
    public void DestroyAfterSeconds(float seconds)
    {
        Destroy(gameObject, seconds);
    }
}
