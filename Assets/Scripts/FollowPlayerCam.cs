using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerCam : MonoBehaviour
{
    public Transform follow;

    void LateUpdate()
    {
        // Set cam position to 'follow' position
        this.transform.position = new Vector3(this.follow.position.x, this.follow.position.y, this.transform.position.z);
    }
}
