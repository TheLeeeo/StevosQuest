using UnityEngine;

public static class GroundCheck
{
    /*public static bool CheckForGrounded(Collider2D collider, float SkinThickness = 0.02f, float WallCollisionSafetyBuffer = 0.05f) //slow as hell boiii
    {
        Vector2 raycastOrigin = new Vector2(collider.bounds.center.x - collider.bounds.extents.x + WallCollisionSafetyBuffer, collider.bounds.center.y - collider.bounds.extents.y - SkinThickness);

        RaycastHit2D raycastHit = Physics2D.Raycast(raycastOrigin, Vector2.right, (collider.bounds.extents.x - WallCollisionSafetyBuffer) * 2, CommonLayerMasks.GroundCheckLayers);

        //Debug.DrawRay(raycastOrigin, Vector2.right * (collider.bounds.extents.x - WallCollisionSafetyBuffer) * 2, raycastHit.collider == null ? Color.blue : Color.red);
        //Debug.DrawRay(raycastOrigin, Vector2.down * (collider.bounds.extents.x - WallCollisionSafetyBuffer) * 2, raycastHit.collider == null ? Color.blue : Color.red);

        return raycastHit.collider != null;
    }*/

    /*public static bool GroundIsStraightDown(Collider2D collider, float Distance = 0.1f)
    {
        Vector2 raycastOrigin = new Vector2(collider.bounds.center.x, collider.bounds.center.y - collider.bounds.extents.y - 0.05f);

        RaycastHit2D raycastHit = Physics2D.Raycast(raycastOrigin, Vector2.down, Distance, CommonLayerMasks.GroundCheckLayers);

        Debug.DrawRay(raycastOrigin, Vector2.down * Distance, raycastHit.collider == null ? Color.blue : Color.red);

        return raycastHit.collider != null;
    }*/

    /// <summary>
    /// Checks if there is ground under the collider
    /// </summary>
    public static bool CheckForGround(Collider2D collider, float thiccness = 0.02f, float wallCollisionSafetyBuffer = 0.02f)
    {
        return Physics2D.OverlapBox(new Vector2(collider.bounds.center.x, collider.bounds.center.y - collider.bounds.extents.y), new Vector2(2 * (collider.bounds.extents.x - wallCollisionSafetyBuffer), thiccness), 0, CommonLayerMasks.GroundCheckLayers);
    }

    /// <summary>
    /// Checks if there is a wall in front of the collider in the spefified direction
    /// </summary>
    public static bool CheckForWall(Collider2D collider, int direction, float thiccness = 0.02f, float groundCollisionSafetyBuffer = 0.05f)
    {
        return Physics2D.OverlapBox(new Vector2(collider.bounds.center.x + (collider.bounds.extents.x + 0.2f) * direction, collider.bounds.center.y), new Vector2(thiccness, 2 * (collider.bounds.extents.y - groundCollisionSafetyBuffer)), 0, CommonLayerMasks.GroundCheckLayers);
    }

    /// <summary>
    /// Checks if there is a hole just in front of the collider in the spefified direction
    /// </summary>
    public static bool CheckForHole(Collider2D collider, int direction, float thiccness = 0.02f, float groundCollisionSafetyBuffer = 0.05f)
    {
        bool hole = !Physics2D.OverlapBox(new Vector2(collider.bounds.center.x + (collider.bounds.extents.x + 0.2f) * direction, collider.bounds.center.y - collider.bounds.extents.y), new Vector2(thiccness, thiccness), 0, CommonLayerMasks.GroundCheckLayers);
        Debugging.DrawCross(new Vector2(collider.bounds.center.x + collider.bounds.extents.x * direction, collider.bounds.center.y - collider.bounds.extents.y), thiccness, hole ? Color.green : Color.red);
        return hole;
    }

    public static bool CheckForGrounded(Collision2D collision)
    {
        ContactPoint2D[] contacts = collision.contacts;

        for (int i = 0; i < contacts.Length; i++)
        {
            if (Vector2.Dot(contacts[i].normal, Vector2.up) > 0.5f)
            {
                return true;
            }
        }
        return false;
    }

    /*public static bool CheckForGrounded(Collider2D collider)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[5];
        collider.GetContacts(contacts);

        for (int i = 0; i < contacts.Length; i++)
        {
            if (Vector2.Dot(contacts[i].normal, Vector2.up) > 0.5f)
            {
                return true;
            }
        }
        return false;
    }*/
}