using Core.Audio;
using Core.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTester : MonoBehaviour
{
    private IAudioManager audioManager;

    private void Awake()
    {
        audioManager = ObjectFactory.ResolveService<IAudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
