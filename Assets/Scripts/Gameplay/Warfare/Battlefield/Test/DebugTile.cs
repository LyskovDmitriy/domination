using UnityEngine;
using TMPro;


namespace Domination.Battle.Test
{
    public class DebugTile : MonoBehaviour
    {
        [SerializeField] private TextMeshPro distanceLabel = default;
        [SerializeField] private TextMeshPro structureObstructionLabel = default;
        [SerializeField] private TextMeshPro warriorObstructionLabel = default;


        public Vector2Int Position { get; private set; }


        private void Awake()
        {
            SetLabelsActive(false);
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
        }

        public void SetLabelsActive(bool areActive)
        {
            distanceLabel.gameObject.SetActive(areActive);
            structureObstructionLabel.gameObject.SetActive(areActive);
            warriorObstructionLabel.gameObject.SetActive(areActive);
        }
    }
}
