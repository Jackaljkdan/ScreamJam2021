using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Horror.Input
{
    public class KeysReadersInstaller : MonoInstaller
    {
        #region Inspector fields

        

        #endregion

        public override void InstallBindings()
        {
            Container.Bind<ActionKeysReader>().ToSelf().AsSingle();
            Container.Bind<MovementKeysReader>().ToSelf().AsSingle();
        }
    }
}