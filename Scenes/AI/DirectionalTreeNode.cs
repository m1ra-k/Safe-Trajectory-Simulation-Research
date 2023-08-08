using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// attach this to drone so its position can be referenced
public class DirectionalTreeNode : ScriptableObject
{
    public DirectionalTreeNode forward;
    public DirectionalTreeNode left;
    public DirectionalTreeNode right;
    public DirectionalTreeNode behind;

    public double posX;
    public double posZ;

    // increments the number of times a grid space was visited (something about three? forgot ASK)
    public int visitCount = 0;

    // ASK running into the problem of how to implement this so that it works frame by frame despite the fact that its recursive?
    // REMOVE parameter
    public DirectionalTreeNode(double myPosX, double myPosZ, int myVisitCount) {
        
        posX = myPosX;
        posZ = myPosZ;
        visitCount = myVisitCount;

        // depth-first search to traverse as far as possible in one direction
        // backtracking when that direction is not possible anymore
        // dfs(this);
    }

    public void SetVisitCount(int count) {
        visitCount = count;
    }

    public DirectionalTreeNode FindChildPos(string direction) {
        // based on the direction
        double shiftX = 0;
        double shiftZ = 0;

        // using the parameter, determine how to create child
        switch (direction) {
            // for example, difference between parent and its forward child: child shifted 0.1 in + direction
            case "forward":
                shiftX += 0.1;
                break;
            case "left":
                shiftZ += 0.1;
                break;
            case "right":
                shiftZ -= 0.1;
                break;
            case "behind":
                shiftX -= 0.1;
                break;
        }

        // remember, x is vertical (forward/behind) and z is horizontal (left/right)
        // so if out of bounds, there shouldn't be a child there as you can't go that way
        // change 125 and 70 to parameters LATER after working
        if (posX + shiftX > 125 || posZ + shiftZ > 70) {
            Debug.Log("hmm");
            return null;
        }

        DirectionalTreeNode child = new DirectionalTreeNode(posX + shiftX, posZ + shiftZ, 0);
        return child;
    }

    public override bool Equals(object o) {
        if (o == null || this.GetType() != o.GetType()) {
            return false;
        }

        DirectionalTreeNode other = (DirectionalTreeNode) o;
        return Mathf.Approximately((float) posX, (float) other.posX) && Mathf.Approximately((float) posZ, (float) other.posZ);
    }

    public override int GetHashCode() {
        int hashCode = (int)((7 * 11 + posX) * 3 * posZ);
        return hashCode;
    }

    // public void dfs(DirectionalTreeNode node) {

    //     // stores the root (one time only)
    //     if (!gameObject.GetComponent<RaycastManager>().pastRoot) {
    //         gameObject.GetComponent<RaycastManager>().root = this;
    //     }

    //     // stop operation if root is reached again; this means there's no way out
    //     // ASK would this work? as in, exit completely?
    //     if (node.Equals(gameObject.GetComponent<RaycastManager>().root) && gameObject.GetComponent<RaycastManager>().pastRoot) {
    //         return;
    //         // some system quit thing here? maybe?
    //     }

    //     // some way to stop this once the end is reached
    //     // ASK would this work?
    //     if (node.forward == null) {
    //         return;
    //     }

    //     // needs logic to check if way is possible too
    //     if (gameObject.GetComponent<RaycastManager>().pastRoot) {
    //         // this is where the drone should move in certain direction
    //         if (node.Equals(node.behind.forward)) {
    //             print("going forward");
    //             gameObject.transform.position += new Vector3(1f, 0, 0);
    //         }
    //         else if (node.Equals(node.right.left)) {
    //             print("going left");
    //             gameObject.transform.position += new Vector3(0, 0, 1f);
    //         }
    //         else if (node.Equals(node.left.right)) {
    //             print("going right");
    //             gameObject.transform.position += new Vector3(0, 0, -1f);
    //         }
    //         else if (node.Equals(node.forward.behind)) {
    //             print("going behind");
    //             gameObject.transform.position += new Vector3(-1f, 0, 0);
    //         }
    //     }

    //     // makes it so that the previous if will only run on items past the root
    //     gameObject.GetComponent<RaycastManager>().pastRoot = true;

    //     // traverses as far forward as possible, if not backtracks and goes left etc
    //     dfs(node.forward);
    //     dfs(node.left);
    //     dfs(node.right);
    //     dfs(node.behind);
    // }

    public override string ToString()
    {
        return $"DirectionalTreeNode: (x: {posX}, z: {posZ}, visit count: {visitCount})";
    }

}
