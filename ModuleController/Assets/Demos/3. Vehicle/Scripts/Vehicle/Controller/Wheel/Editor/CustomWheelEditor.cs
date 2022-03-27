using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Demos.Vehicle
{
    public class InfoElement : VisualElement
    {
        private Foldout _foldout;
        private Label _localVelocityLabel;
        private Label _angularVelocityLabel;
        private Label _slipRatioLabel;
        private Label _slipAngleLabel;
        
        public CustomWheel Wheel { get; private set; }

        public InfoElement(CustomWheel wheel)
        {
            Wheel = wheel;
            
            _foldout = new Foldout
            {
                text = "Info"
            };
            Add(_foldout);

            _localVelocityLabel = new Label("");
            _foldout.Add(_localVelocityLabel);
            
            _angularVelocityLabel = new Label("");
            _foldout.Add(_angularVelocityLabel);
            
            _slipRatioLabel = new Label("");
            _foldout.Add(_slipRatioLabel);
            
            _slipAngleLabel = new Label("");
            _foldout.Add(_slipAngleLabel);
        }

        public void UpdateInfo()
        {
            if (!_foldout.visible || Wheel == null)
                return;
            
            _localVelocityLabel.text = $"Local Velocity: {Wheel.LocalVelocity}";
            _angularVelocityLabel.text = $"Angular Velocity: {Wheel.AngularVelocity}";
            _slipRatioLabel.text = $"Slip Ratio: {Wheel.SlipRatio}";
            _slipAngleLabel.text = $"Slip Angle: {Wheel.SlipAngle}";
        }
    }
    
    [CustomEditor(typeof(CustomWheel))]
    public class CustomWheelEditor : Editor
    {
        private CustomWheel _customWheel;
        
        private VisualElement _root;
        private InfoElement _infoContainer;

        private void OnEnable()
        {
            _customWheel = target as CustomWheel;
            _root = new VisualElement();
            
            EditorApplication.update += UpdateInspector;
        }

        private void OnDisable()
        {
            EditorApplication.update -= UpdateInspector;
        }

        private void UpdateInspector()
        {
            _infoContainer.UpdateInfo();
            //_infoContainer.MarkDirtyRepaint();
        }

        public override VisualElement CreateInspectorGUI()
        {
            _root.Add(CreateDefaultInspector());
            
            _root.Add(CreateCustomInspector());
            
            return _root;
        }
        
        /// <summary>
        /// Creates the default inspector Visual Element.
        /// </summary>
        /// <returns></returns>
        private VisualElement CreateDefaultInspector()
        {
            var container = new IMGUIContainer();
            container.onGUIHandler = () => DrawDefaultInspector();
            
            return container;
        }
        
        private VisualElement CreateCustomInspector()
        {
            var container = new VisualElement();
            
            //Add a 8px space between default inspector and custom inspector.
            var space = new VisualElement();
            space.style.height = 8;
            container.Add(space);

            //Add 
            container.Add(CreateInfo());
            
            return container;
        }
        
        /// <summary>
        /// Creates a module list Visual Element.
        /// </summary>
        /// <returns></returns>
        private VisualElement CreateInfo()
        {
            _infoContainer = new InfoElement(_customWheel);
            return _infoContainer;
        }
    }
}