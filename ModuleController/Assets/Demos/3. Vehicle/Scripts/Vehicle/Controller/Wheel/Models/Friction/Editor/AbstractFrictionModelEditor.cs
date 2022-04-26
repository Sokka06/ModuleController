/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Demos.Vehicle
{
    public class GraphElement : VisualElement
    {
        public struct GraphPoint
        {
            public float Time;
            public float Value;
        }
        
        public Foldout Container;
        public Box GraphBox;
        public VisualElement GraphContainer;

        private Vertex[] _vertices;
        private ushort[] _indices;

        public GraphPoint[] Points { get; private set; }
        
        public GraphElement(string title, GraphPoint[] points = null)
        {
            _vertices = Array.Empty<Vertex>();
            _indices = Array.Empty<ushort>();
            Points = Array.Empty<GraphPoint>();
            if (points != null)
                SetPoints(points);

            Container = new Foldout
            {
                text = title
            };
            Add(Container);

            GraphBox = new Box
            {
                style =
                {
                    flexGrow = 1,
                    marginTop = 5,
                    marginBottom = 5,
                    marginLeft = 5,
                    marginRight = 5,
                }
            };
            Container.Add(GraphBox);
            
            GraphContainer = new VisualElement { style = { flexGrow = 1, } };
            GraphBox.Add(GraphContainer);
            //GraphContainer.generateVisualContent += GenerateGraph;
        }
        
        void GenerateGraph(MeshGenerationContext mgc)
        {
            var w = mgc.visualElement.worldBound.width;
            var h = mgc.visualElement.worldBound.height;

            var mwd = mgc.Allocate(_vertices.Length, 3);
            var uvRegion = mwd.uvRegion;

            for (int i = 0; i < Points.Length; i++)
            {
                var vertex = new Vertex
                {
                    position = new Vector3(Points[i].Time, Points[i].Value, Vertex.nearZ),
                    tint = Color.red,
                    //uv = new Vector2(1, 0) * uvRegion.size + uvRegion.min
                };
                _vertices[i] = vertex;
                
                _indices[i] = (ushort)i;
            }

            mwd.SetAllVertices(_vertices);
            mwd.SetAllIndices(_indices);
        }

        public void SetPoints(GraphPoint[] points)
        {
            Points = points;
            _vertices = new Vertex[Points.Length];
            _indices = new ushort[Points.Length];
        }
    }
    
    [CustomEditor(typeof(SimplifiedPacejkaFrictionModel))]
    public class AbstractFrictionModelEditor : Editor
    {
        protected AbstractFrictionModel _target;
        protected VisualElement _root;

        protected GraphElement _longitudinalGraph;
        protected GraphElement _lateralGraph;
        
        protected virtual void OnEnable()
        {
            _target = (AbstractFrictionModel)target;
            _root = new VisualElement();
        }

        protected virtual void OnDisable()
        {
            
        }
        
        public override VisualElement CreateInspectorGUI()
        {
            _root.Add(CreateDefaultInspector());
            
            //_root.Add(CreateCustomInspector());
            
            return _root;
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
            var space = new VisualElement
            {
                style =
                {
                    height = 8
                }
            };
            container.Add(space);

            //Add module list
            container.Add(CreateGraphs());
            
            return container;
        }

        protected virtual VisualElement CreateGraphs()
        {
            var container = new VisualElement();
            _longitudinalGraph = new GraphElement("Longitudinal");
            container.Add(_longitudinalGraph);
            _lateralGraph = new GraphElement("Lateral");
            container.Add(_lateralGraph);
            
            return container;
        }
    }
}*/