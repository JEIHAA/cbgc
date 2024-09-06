using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightData : MonoBehaviour
{
    [SerializeField]
    TorchLight torchLight;
    [SerializeField]
    Bonfire bonfire;
    static TorchLight Torch;
    static Bonfire Bonfire;
    public static int BonfireLeftTime => Bonfire.LeftTime;
    public static int TorchLeftTime => Torch.LeftTime;
    public static bool TorchIsBrightest => Torch.LeftTime > Bonfire.LeftTime;
    // Start is called before the first frame update
    void Start()
    {
        Bonfire = bonfire;
        Torch = torchLight;
    }
}
