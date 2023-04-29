using UnityEngine;
using CoolFramework.Core;
using CoolFramework.Utility;

namespace CoolFramework
{
    public class TestBehaviour : CoolBehaviour, IDynamicUpdate
    {
        public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Dynamic;

        void IDynamicUpdate.Update() => UpdateBehaviour(); 

        protected override void OnInit()
        {
            base.OnInit();
            float _f = 12.12584568f;
            Debug.Log(_f); 
            Debug.Log(_f.RoundToDecimals(2));
            Debug.Log(_f.ConvertToIntN(3));
        }

        void UpdateBehaviour()
        {
            Debug.Log("Call Dynamic Update"); 
        }

    }
}
