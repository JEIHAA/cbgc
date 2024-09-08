using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightData : MonoBehaviour
{
    [SerializeField]
    private TorchLight torchLight;
    [SerializeField]
    private Bonfire bonfire;

    private static TorchLight Torch;
    private static Bonfire Bonfire;
    public static int BonfireLeftTime => Bonfire.LeftTime;
    public static int TorchLeftTime => Torch.LeftTime;
    public static bool TorchIsBrightest => Torch.LeftTime > Bonfire.LeftTime;

    void Start()
    {
        Bonfire = bonfire;
        Torch = torchLight;
    }
}
