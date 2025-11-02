using AlienPAK;
using CATHODE;
using CATHODE.LEGACY;
using CATHODE.Scripting;
using CATHODE.ShaderTypes;
using CathodeLib;
using CommandsEditor.DockPanels;
using OpenCAGE;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CommandsEditor.Popups.UserControls
{
    /// <summary>
    /// Interaction logic for GUI_ModelViewer.xaml
    /// </summary>
    public partial class GUI_ModelViewer : UserControl
    {
        protected LevelContent Content => Singleton.Editor?.CommandsDisplay?.Content;

        public GUI_ModelViewer()
        {
            InitializeComponent();
        }

        public void ShowModel(List<Model> models)
        {
            Model3DGroup group = new Model3DGroup();
            for (int i = 0; i < models.Count; i++)
                group.Children.Add(OffsetModel(models[i].modelIndex, models[i].position, models[i].rotation, models[i].materialIndex));
            modelPreview.Content = group;

            myView.ModelUpDirection = new Vector3D(0, 1, 0);
            myView.Camera.UpDirection = new Vector3D(0, 1, 0);
            myView.Camera.LookDirection = new Vector3D(-0.5, -0.5, 1);

            myView.ZoomExtents();
        }
        
        private Model3DGroup OffsetModel(int modelIndex, Vector3D position, Vector3D rotation, int materialIndex)
        {
            //Get mesh data
            Models.CS2.Component.LOD.Submesh submesh = Content.resource.models.GetAtWriteIndex(modelIndex);
            GeometryModel3D submeshGeo = submesh.ToGeometryModel3D();

            //Get material & texture data
            if (SettingsManager.GetBool(Singleton.Settings.ShowTexOpt))
            {
                Materials.Material material = Content.resource.materials.GetAtWriteIndex(materialIndex == -1 ? submesh.MaterialIndex : materialIndex);
                Shaders.Shader shader = Content.resource.shaders.Entries[material.ShaderIndex];

                int diffuseMap = -1;
                int diffuseUvMult = -1;
                switch (shader.Ubershader)
                {
                    case SHADER_LIST.CA_ENVIRONMENT:
                        diffuseMap = (int)CA_ENVIRONMENT.SAMPLERS.DIFFUSE_MAP;
                        diffuseUvMult = (int)CA_ENVIRONMENT.PARAMETERS.DIFFUSE_UV_MULT;
                        break;
                    case SHADER_LIST.CA_DECAL_ENVIRONMENT:
                        diffuseMap = (int)CA_DECAL_ENVIRONMENT.SAMPLERS.DIFFUSE_MAP;
                        diffuseUvMult = (int)CA_DECAL_ENVIRONMENT.PARAMETERS.DIFFUSE_UV_MULT;
                        break;
                    case SHADER_LIST.CA_CHARACTER:
                        diffuseMap = (int)CA_CHARACTER.SAMPLERS.DIFFUSE_MAP;
                        diffuseUvMult = (int)CA_CHARACTER.PARAMETERS.DIFFUSE_UV_MULT;
                        break;
                    case SHADER_LIST.CA_SKIN:
                        diffuseMap = (int)CA_SKIN.SAMPLERS.DIFFUSE_MAP;
                        diffuseUvMult = (int)CA_SKIN.PARAMETERS.DIFFUSE_UV_MULT;
                        break;
                    case SHADER_LIST.CA_HAIR:
                        diffuseMap = (int)CA_HAIR.SAMPLERS.DIFFUSE_MAP;
                        diffuseUvMult = (int)CA_HAIR.PARAMETERS.DIFFUSE_UV_MULT;
                        break;
                    case SHADER_LIST.CA_EYE:
                        diffuseMap = (int)CA_EYE.SAMPLERS.IRIS_MAP;
                        break;
                    case SHADER_LIST.CA_SKIN_OCCLUSION:
                        diffuseMap = (int)CA_SKIN_OCCLUSION.SAMPLERS.DIFFUSE_MAP;
                        diffuseUvMult = (int)CA_SKIN_OCCLUSION.PARAMETERS.DIFFUSE_UV_MULT;
                        break;
                    case SHADER_LIST.CA_DECAL:
                        diffuseMap = (int)CA_DECAL.SAMPLERS.DIFFUSE_MAP;
                        break;
                    case SHADER_LIST.CA_FOGPLANE:
                        diffuseMap = (int)CA_FOGPLANE.SAMPLERS.DIFFUSE_MAP_0;
                        break;
                    case SHADER_LIST.CA_EFFECT:
                        diffuseMap = (int)CA_EFFECT.SAMPLERS.DIFFUSE_MAP_0;
                        break;
                    case SHADER_LIST.CA_LIQUID_ENVIRONMENT:
                        diffuseMap = (int)CA_LIQUID_ENVIRONMENT.SAMPLERS.LIQUIFLOW_DISTORTION_MAP;
                        break;
                    case SHADER_LIST.CA_LIQUID_CHARACTER:
                        diffuseMap = (int)CA_LIQUID_CHARACTER.SAMPLERS.LIQUIFLOW_DISTORTION_MAP;
                        break;
                    case SHADER_LIST.CA_SKYDOME:
                        diffuseMap = (int)CA_SKYDOME.SAMPLERS.SKYDOME_MAP;
                        break;
                    case SHADER_LIST.CA_SURFACE_EFFECTS:
                        diffuseMap = (int)CA_SURFACE_EFFECTS.SAMPLERS.DIFFUSE_MAP;
                        diffuseUvMult = (int)CA_SURFACE_EFFECTS.PARAMETERS.DIFFUSE_UV_MULT;
                        break;
                    case SHADER_LIST.CA_EFFECT_OVERLAY:
                        diffuseMap = (int)CA_EFFECT_OVERLAY.SAMPLERS.TEXTURE_MAP;
                        break;
                    case SHADER_LIST.CA_TERRAIN:
                        diffuseMap = (int)CA_TERRAIN.SAMPLERS.DIFFUSE_MAP;
                        diffuseUvMult = (int)CA_TERRAIN.PARAMETERS.DIFFUSE_UV_MULT;
                        break;
                    case SHADER_LIST.CA_PLANET:
                        diffuseMap = (int)CA_PLANET.SAMPLERS.TERRAIN_MAP;
                        diffuseUvMult = (int)CA_PLANET.PARAMETERS.TERRAIN_MAP_UV_SCALE;
                        break;
                    case SHADER_LIST.CA_LIGHTMAP_ENVIRONMENT:
                        diffuseMap = (int)CA_LIGHTMAP_ENVIRONMENT.SAMPLERS.DIFFUSE_MAP;
                        diffuseUvMult = (int)CA_LIGHTMAP_ENVIRONMENT.PARAMETERS.DIFFUSE_UV_MULT;
                        break;
                    case SHADER_LIST.CA_STREAMER:
                        diffuseMap = (int)CA_STREAMER.SAMPLERS.DIFFUSE_MAP;
                        diffuseUvMult = (int)CA_STREAMER.PARAMETERS.DIFFUSE_UV_MULT;
                        break;
                    case SHADER_LIST.CA_LOW_LOD_CHARACTER:
                        diffuseMap = (int)CA_LOW_LOD_CHARACTER.SAMPLERS.DIFFUSE_MAP;
                        diffuseUvMult = (int)CA_LOW_LOD_CHARACTER.PARAMETERS.DIFFUSE_UV_MULT;
                        break;
                    case SHADER_LIST.CA_CAMERA_MAP:
                        diffuseMap = (int)CA_CAMERA_MAP.SAMPLERS.DIFFUSE_MAP;
                        break;
                }
                if (diffuseMap != -1)
                {
                    if (shader.SamplerRemaps.Count > diffuseMap)
                    {
                        int diffuseMapIndex = shader.SamplerRemaps[diffuseMap];
                        if (diffuseMapIndex != 255)
                        {
                            TexturePtr ptr = material.TextureReferences[diffuseMapIndex];

                            Textures tex = ptr.Location == TexturePtr.Source.GLOBAL ? Singleton.GlobalTextures : Content.resource.textures;
                            Textures.TEX4 diff = tex.GetAtWriteIndex(ptr.Index);
                            byte[] diffDDS = diff?.ToDDS();
                            ImageBrush brush = new ImageBrush(diffDDS?.ToBitmap()?.ToImageSource());

                            float uvScale = 1.0f;
                            if (diffuseUvMult != -1)
                            {
                                if (shader.PixelShaderParameterRemaps.Count > diffuseUvMult)
                                {
                                    if (shader.PixelShaderParameterRemaps[diffuseUvMult] != 255)
                                    {
                                        uvScale = material.PixelShaderConstants[shader.PixelShaderParameterRemaps[diffuseUvMult]];
                                    }
                                }
                            }
                            brush.Transform = new ScaleTransform(uvScale, uvScale);

                            DiffuseMaterial mat = new DiffuseMaterial(brush);
                            submeshGeo.Material = mat;
                        }
                    }
                }
            }

            //Get transform data
            Transform3DGroup transform = new Transform3DGroup();
            transform.Children.Add(new ScaleTransform3D(submesh.VertexScale, submesh.VertexScale, submesh.VertexScale));
            System.Numerics.Quaternion q = System.Numerics.Quaternion.CreateFromYawPitchRoll((float)(rotation.Y * Math.PI / 180.0f), (float)(rotation.X * Math.PI / 180.0f), (float)(rotation.Z * Math.PI / 180.0f));
            transform.Children.Add(new RotateTransform3D(new QuaternionRotation3D(new System.Windows.Media.Media3D.Quaternion(q.X, q.Y, q.Z, q.W))));
            transform.Children.Add(new TranslateTransform3D(position.X, position.Y, position.Z));

            //Submit
            Model3DGroup model = new Model3DGroup();
            model.Transform = transform;
            model.Children.Add(submeshGeo);
            return model;
        }

        public class Model
        {
            public Model(int modelIndex, int materialIndex = -1)
            {
                Create(modelIndex, new Vector3D(0, 0, 0), new Vector3D(0, 0, 0), materialIndex);
            }
            public Model(int modelIndex, cTransform transform, int materialIndex = -1)
            {
                Create(modelIndex, new Vector3D(transform.position.X, transform.position.Y, transform.position.Z), new Vector3D(transform.rotation.X, transform.rotation.Y, transform.rotation.Z), materialIndex);
            }

            private void Create(int modelIndex, Vector3D position, Vector3D rotation, int materialIndex)
            {
                this.modelIndex = modelIndex;
                this.materialIndex = materialIndex;
                this.position = position;
                this.rotation = rotation;
            }

            public int modelIndex;
            public int materialIndex;
            public Vector3D position;
            public Vector3D rotation;
        }
    }
}
