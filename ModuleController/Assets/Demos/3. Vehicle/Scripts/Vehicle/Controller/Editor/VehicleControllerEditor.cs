using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Demos.Vehicle
{
    [CustomEditor(typeof(VehicleController))]
    public class VehicleControllerEditor : ModuleBehaviourEditor<VehicleController>
    {
        public override void OnEnable()
        {
            base.OnEnable();

            _target.onModulesChanged += UpdateModuleList;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            
            _target.onModulesChanged -= UpdateModuleList;
        }
        
        protected override VisualElement CreateModuleList()
        {
            base.CreateModuleList();

            // Modules property will be null when not in play mode.
            if (_target.Modules != null)
            {
                for (int i = 0; i < _target.Modules.Count; i++)
                {
                    _moduleList.AddElement(new ModuleElement(_target.Modules[i].GetType().Name));
                }
            }
            
            return _moduleList.Root;
        }

        protected override void UpdateModuleList()
        {
            base.UpdateModuleList();

            //Re-create module list if counts dont match.
            if (_moduleList.Elements.Count != _target.Modules.Count)
            {
                CreateModuleList();
                return;
            }
        }
    }
}