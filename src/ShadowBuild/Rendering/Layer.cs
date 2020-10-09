﻿using ShadowBuild.Config;
using ShadowBuild.Exceptions;
using ShadowBuild.Objects;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace ShadowBuild.Rendering
{
    /// <summary>
    /// Render layer class.
    /// You can add objects to render layers.
    /// You can specify what render layers should render the camera.
    /// You can specify zIndex of the layer.
    /// Each layer should have unique zIndex
    /// </summary>
    public class Layer : ConfigSavable, IComparable<Layer>
    {
        /// <value>Gets default layer</value>
        public static readonly Layer Default = new Layer("default", 0);

        /// <value>Gets all layers</value>
        internal static readonly List<Layer> All = new List<Layer> { Layer.Default };

        /// <value>Gets objects assigned to the layer</value>
        [ScriptIgnore]
        public List<RenderableObject> Objects
        {
            get
            {
                List<RenderableObject> objs = new List<RenderableObject>();
                foreach (RenderableObject obj in World.ActualWorld.Objects)
                {
                    if (obj.RenderLayer == this) objs.Add(obj);
                }
                return objs;
            }
        }

        /// <value>Gets zIndex of the layer</value>
        public readonly int zIndex;

        /// <value>Gets layer name</value>
        public readonly string Name;

        public Layer(string name, int zIndex)
        {
            this.Name = name;
            this.zIndex = zIndex;
        }

        /// <summary>
        /// Finds a layer by name
        /// </summary>
        public static Layer Find(string name)
        {
            Layer l = FindWithoutException(name);
            if (l == null)
                throw new LayerException("Cannot find layer \"" + name + "\"");
            return l;
        }

        /// <summary>
        /// Finds a layer by zIndex
        /// </summary>
        public static Layer Find(int zIndex)
        {
            Layer l = FindWithoutException(zIndex);
            if (l == null)
                throw new LayerException("Cannot find layer with zIndex " + zIndex);
            return l;
        }
        private static Layer FindWithoutException(string name)
        {
            foreach (Layer l in All)
            {
                if (l.Name == name)
                    return l;
            }
            return null;
        }
        private static Layer FindWithoutException(int zIndex)
        {
            foreach (Layer l in All)
            {
                if (l.zIndex == zIndex)
                    return l;
            }
            return null;
        }

        /// <summary>
        /// Sets up a layer
        /// </summary>
        public static void Setup(Layer layer)
        {
            if (Layer.FindWithoutException(layer.Name) != null) throw new LayerException("Layer name \"" + layer.Name + "\" is already in use");
            if (Layer.FindWithoutException(layer.zIndex) != null) throw new LayerException("Layer zIndex [" + layer.zIndex + "] is already in use");
            All.Add(layer);
        }
        public int CompareTo(Layer obj)
        {
            if (this.zIndex > obj.zIndex) return 1;
            return 0;
        }
        public static void SaveConfig(string path)
        {
            var serialized = new { Layers = new List<Layer>(All) };
            serialized.Layers.Remove(Layer.Find("default"));
            WriteConfigFile(path, serialized);
        }
        public static void LoadConfig(string path)
        {
            dynamic val = ReadConfigFile(path);

            try
            {
                var i = val["Layers"];
            }
            catch (Exception e)
            {
                throw new ConfigException(path + " config file is incorrect", e);
            }
            foreach (Dictionary<string, object> dict in val["Layers"])
            {
                string name = "";
                int zi;
                try
                {
                    name = (string)dict["Name"];
                    zi = (int)dict["zIndex"];
                }
                catch (Exception e)
                {
                    throw new ConfigException(path + " config file is incorrect", e);
                }
                All.Add(new Layer(name, zi));
            }

        }
    }
}
