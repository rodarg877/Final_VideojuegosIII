using UnityEngine;

namespace SkyrimProject.DialogueSystem
{
    public class EnableObjectActivable : DialogueActivable
    {
        [SerializeField] private string[] objectNames;

        public override void Activate()
        {
            foreach (var objName in objectNames)
            {
                var obj = GameObject.Find(objName);
                if (obj != null)
                    obj.SetActive(true);
                else
                    Debug.Log($"No se encontró el objeto: {objName}");
            }
        }
    }
}
