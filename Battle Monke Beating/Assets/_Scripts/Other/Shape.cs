using UnityEngine;
using DG.Tweening;

public class Shape : MonoBehaviour
{
    public GameObject[] movingObjects;
    public GameObject[] rotatingObjects;

    [Header("Side")]
    [SerializeField] private float sideX;
    [SerializeField] private float sideY;
    [SerializeField] private float sideZ;
    [Header("Rotation")]
    [SerializeField] private float rotationAmount = 360f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float cycleLength = 2f;

    void Start()
    {
        foreach(GameObject mO in movingObjects)
        {
            mO.transform.DOLocalMove(new Vector3(sideX, sideY, sideZ), cycleLength).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        foreach (GameObject rO in rotatingObjects)
        {
            rO.transform.DOLocalRotate(new Vector3(0, rotationAmount, 0), cycleLength * rotationSpeed, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        }
    }
}
