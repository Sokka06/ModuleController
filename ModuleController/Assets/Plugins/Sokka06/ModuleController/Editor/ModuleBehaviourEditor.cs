using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sokka06.ModuleController
{
    public class ModuleList
    {
        public VisualElement Root;
        public Foldout Container;
        public Label TitleLabel;
        
        public List<ModuleElement> Elements;

        public ModuleList(string title = "Modules")
        {
            Elements = new List<ModuleElement>();

            Root = new VisualElement();

            Container = new Foldout();
            Container.text = title;
            //Container.style.paddingBottom = 8;
            //Container.style.paddingLeft = 8;
            //Container.style.paddingRight = 8;
            //Container.style.paddingTop = 8;
            Root.Add(Container);
            
            //TitleLabel = new Label(title);
            //TitleLabel.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);
            //Container.Add(TitleLabel);
        }

        public void AddElement(ModuleElement element)
        {
            Elements.Add(element);
            Container.Add(element.Root);
        }
    }

    public class ModuleElement
    {
        public VisualElement Root;
        public Label Label;
        
        public ModuleElement(string name)
        {
            Root = new VisualElement();

            Label = new Label(name);
            Root.Add(Label);
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModuleBehaviourEditor<T> : Editor where T : MonoBehaviour
    {
        protected T _target;
        protected VisualElement _root;

        protected ModuleList _moduleList;

        public virtual void OnEnable()
        {
            _target = (T)target;
            _root = new VisualElement();
        }

        public virtual void OnDisable()
        {
            
        }

        public override VisualElement CreateInspectorGUI()
        {
            _root.Add(CreateDefaultInspector());
            
            _root.Add(CreateCustomInspector());
            
            return _root;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        /// <summary>
        /// Creates the default inspector Visual Element.
        /// </summary>
        /// <returns></returns>
        protected virtual VisualElement CreateDefaultInspector()
        {
            var container = new IMGUIContainer();
            container.onGUIHandler = () => DrawDefaultInspector();
            
            return container;
        }

        protected virtual VisualElement CreateCustomInspector()
        {
            var container = new VisualElement();
            
            //Add a 8px space between default inspector and custom inspector.
            var space = new VisualElement();
            space.style.height = 8;
            container.Add(space);

            //Add module list
            container.Add(CreateModuleList());
            
            return container;
        }

        /// <summary>
        /// Creates a module list Visual Element.
        /// </summary>
        /// <returns></returns>
        protected virtual VisualElement CreateModuleList()
        {
            _moduleList = new ModuleList();
            return _moduleList.Root;
        }

        /// <summary>
        /// Updates module list. Call this when Modules are added or removed.
        /// </summary>
        protected virtual void UpdateModuleList()
        {
            
        }
    }
}

