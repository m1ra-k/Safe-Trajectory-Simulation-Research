import UnityEngine as ue

objects = ue.Object.FindObjectsOfType(ue.GameObject)
for object in objects:
    ue.Debug.Log(object.name)