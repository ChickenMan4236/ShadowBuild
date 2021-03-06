﻿
using System.Drawing;

namespace ShadowEngine
{
    public class ProjectConfig
    {
        public string Name { get; internal set; }
        public string Author { get; internal set; }

        public string ProjectVersion { get; internal set; }
        public string ShadowEngineVersion { get; internal set; }

        public Size StartResolution { get; internal set; }
        public bool StartFullscreen { get; internal set; }

        public string AxisConfigPath { get; internal set; }
        public bool AxisConfigAutoLoad { get; internal set; }
        public string LayerConfigPath { get; internal set; }
        public bool LayerConfigAutoLoad { get; internal set; }
        public string TextureConfigPath { get; internal set; }
        public bool TextureConfigAutoLoad { get; internal set; }

        internal ProjectConfig() { }
    }
}
