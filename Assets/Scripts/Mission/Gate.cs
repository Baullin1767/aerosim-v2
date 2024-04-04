using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Mission
{
    public abstract class SimpleGate : MonoBehaviour
    {
        [SerializeField]
        protected float waitTimeSeconds;
        
        [SerializeField]
        protected string blobDescription;
        
        public abstract void SetColor(Color color);
        public abstract UniTask<Collider> AwaitCollision();
    }

    public class Gate : SimpleGate
    {
        [Header("SquareGate")]
        [SerializeField]
        protected float size = 5f;
        
        [SerializeField]
        private float width = 0.5f;
        
        [SerializeField]
        protected BoxCollider gateCollider;

        [Header("Sides")]
        [SerializeField]
        private MeshRenderer left;

        [SerializeField]
        private MeshRenderer right;

        [SerializeField]
        private MeshRenderer top;

        [SerializeField]
        private MeshRenderer bottom;

        private UniTaskCompletionSource<Collider> _collisionCc;
        private bool _waitingCollision;

        public override void SetColor(Color color)
        {
            left.material.color = color;
            right.material.color = color;
            top.material.color = color;
            bottom.material.color = color;
        }

        public override async UniTask<Collider> AwaitCollision()
        {
            _collisionCc?.TrySetCanceled();
            _collisionCc = new UniTaskCompletionSource<Collider>();
            var result = await _collisionCc.Task;
            _collisionCc = null;
            return result;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (waitTimeSeconds < 0.1f)
            {
                _collisionCc?.TrySetResult(other);
            }
            else
            {
                if (!_waitingCollision) WaitForCollision(other).Forget();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _waitingCollision = false;
        }

        private async UniTask WaitForCollision(Collider other)
        {
            if (_waitingCollision) return;
            _waitingCollision = true;
            var time = 0f;
            while (_collisionCc != null && time < waitTimeSeconds)
            {
                await UniTask.Yield();
                if (!_waitingCollision) return;
                time += Time.deltaTime;
            }

            _waitingCollision = false;
            _collisionCc?.TrySetResult(other);
        }

        private void SetSize(float gateSize, float gatesWidth)
        {
            var halfSize = gateSize / 2f;

            var leftTransform = left.transform;
            leftTransform.localScale = new Vector3(gatesWidth, gateSize, gatesWidth);
            leftTransform.localPosition = new Vector3(-halfSize, 0f, 0f);

            var rightTransform = right.transform;
            rightTransform.localScale = new Vector3(gatesWidth, gateSize, gatesWidth);
            rightTransform.localPosition = new Vector3(halfSize, 0f, 0f);

            var topTransform = top.transform;
            topTransform.localScale = new Vector3(gateSize, gatesWidth, gatesWidth);
            topTransform.localPosition = new Vector3(0f, halfSize, 0f);

            var bottomTransform = bottom.transform;
            bottomTransform.localScale = new Vector3(gateSize, gatesWidth, gatesWidth);
            bottomTransform.localPosition = new Vector3(0f, -halfSize, 0f);

            var colliderSize = new Vector3(gateSize, gateSize, gatesWidth);
            gateCollider.size = colliderSize;
        }

        private void OnValidate()
        {
            //SetColor(color22);
            SetSize(size, width);
        }
    }
    
    
}