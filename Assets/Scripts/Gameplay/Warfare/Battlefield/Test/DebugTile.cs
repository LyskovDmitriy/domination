using UnityEngine;
using TMPro;


namespace Domination.Battle.Test
{
    public class DebugTile : MonoBehaviour
    {
        [SerializeField] private TextMeshPro distanceLabel = default;
        [SerializeField] private TextMeshPro structureObstructionLabel = default;
        [SerializeField] private TextMeshPro warriorObstructionLabel = default;
        [Space]
        [SerializeField] private SpriteRenderer backgroundRenderer = default;
        [SerializeField] private Color defaultColor = default;
        [SerializeField] private Color movementColor = default;
        [Space]
        [SerializeField] private GameObject attackMarker = default;


        public Vector2Int Position { get; private set; }


        private void Awake()
        {
            SetLabelsActive(false);
            SetAttackMarkerActive(false);
            SetMovementTarget(false);
        }

        public void Init(Vector2Int position)
        {
            Position = position;
        }

        public void SetDebugInfo(int distance, bool isObstructedByStructure, bool isObstructedByWarrior)
        {
            SetLabelsActive(true);

            distanceLabel.text = distance.ToString();
            structureObstructionLabel.text = "Obstructed by structure: " + isObstructedByStructure;
            warriorObstructionLabel.text = "Obstructed by warrior: " + isObstructedByWarrior;

            SetAttackMarkerActive(false);
            SetMovementTarget(false);
        }

        public void SetLabelsActive(bool areActive)
        {
            distanceLabel.gameObject.SetActive(areActive);
            structureObstructionLabel.gameObject.SetActive(areActive);
            warriorObstructionLabel.gameObject.SetActive(areActive);
        }

        public void SetAttackMarkerActive(bool isActive) => attackMarker.SetActive(isActive);

        public void SetMovementTarget(bool isMovementTarget) => backgroundRenderer.color = 
            isMovementTarget ? movementColor : defaultColor;
    }
}
