using UnityEngine;
using UnityEngine.UIElements;

namespace Stage
{
    public class StageEntrance : MonoBehaviour {

        void Start()
        {
            var position = transform.position;
            transform.position = new Vector3(position.x, position.y, ZcoordinateConsts.Object);
        }

        void Update()
        {
            
        }

        public Vector3 GetPosition() {
            return transform.position;
        }
    }
}
