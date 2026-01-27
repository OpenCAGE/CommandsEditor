using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using static CATHODE.Models;
using Vector3D = System.Windows.Media.Media3D.Vector3D;
using Color = System.Windows.Media.Color;
using CathodeLib;
using CATHODE;
using DirectXTex;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows;

namespace AlienPAK
{
    public static class CathodeLibExtensions
    {
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        /* Convert a TEX4 to DDS */
        public static byte[] ToDDS(this Textures.TEX4 texture, bool forceLowRes = false)
        {
            Textures.TEX4.Texture part = texture?.TextureStreamed?.Content != null && !forceLowRes ? texture.TextureStreamed : texture?.TexturePersistent?.Content != null ? texture.TexturePersistent : null;
            if (part == null) return null;
            DirectXTexUtility.DXGIFormat format;
            switch (texture.Format)
            {
                case Textures.TextureFormat.A32R32G32B32F:
                    format = DirectXTexUtility.DXGIFormat.R32G32B32A32FLOAT;
                    break;
                case Textures.TextureFormat.A16R16G16B16:
                    format = DirectXTexUtility.DXGIFormat.R16G16B16A16UNORM;
                    break;
                case Textures.TextureFormat.A8R8G8B8:
                    format = DirectXTexUtility.DXGIFormat.R8G8B8A8UNORM;
                    break;
                case Textures.TextureFormat.X8R8G8B8:
                    format = DirectXTexUtility.DXGIFormat.B8G8R8X8UNORM;
                    break;
                case Textures.TextureFormat.A8:
                    format = DirectXTexUtility.DXGIFormat.A8UNORM;
                    break;
                case Textures.TextureFormat.L8:
                    format = DirectXTexUtility.DXGIFormat.R8UNORM;
                    break;
                case Textures.TextureFormat.DXT1:
                    format = DirectXTexUtility.DXGIFormat.BC1UNORM;
                    break;
                case Textures.TextureFormat.DXT3:
                    format = DirectXTexUtility.DXGIFormat.BC2UNORM;
                    break;
                case Textures.TextureFormat.DXT5:
                    format = DirectXTexUtility.DXGIFormat.BC3UNORM;
                    break;
                case Textures.TextureFormat.DXN:
                    format = DirectXTexUtility.DXGIFormat.BC5UNORM;
                    break;
                case Textures.TextureFormat.A4R4G4B4:
                    format = DirectXTexUtility.DXGIFormat.B4G4R4A4UNORM;
                    break;
                case Textures.TextureFormat.BC6H:
                    format = DirectXTexUtility.DXGIFormat.BC6HUF16;
                    break;
                case Textures.TextureFormat.BC7:
                    format = DirectXTexUtility.DXGIFormat.BC7UNORM;
                    break;
                case Textures.TextureFormat.R16F:
                    format = DirectXTexUtility.DXGIFormat.R16FLOAT;
                    break;
                case Textures.TextureFormat.ASTC4X4:
                    format = DirectXTexUtility.DXGIFormat.ASTC4X4UNORM;
                    break;
                case Textures.TextureFormat.ASTC8X8:
                    format = DirectXTexUtility.DXGIFormat.ASTC8X8UNORM;
                    break;
                case Textures.TextureFormat.ASTC12X12:
                    format = DirectXTexUtility.DXGIFormat.ASTC12X12UNORM;
                    break;
                default:
                    format = DirectXTexUtility.DXGIFormat.UNKNOWN;
                    break;
            }
            DirectXTexUtility.GenerateDDSHeader(
                DirectXTexUtility.GenerateMataData(part.Width, part.Height, part.MipLevels, format, texture.StateFlags.HasFlag(Textures.TextureStateFlag.CUBE)),
                DirectXTexUtility.DDSFlags.FORCEDX10EXT, out DirectXTexUtility.DDSHeader ddsHeader, out DirectXTexUtility.DX10Header dx10Header);
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(DirectXTexUtility.EncodeDDSHeader(ddsHeader, dx10Header));
                bw.Write(part.Content);
            }
            return ms.ToArray();
        }

        /* Convert a TEX4 to Bitmap */
        public static Bitmap ToBitmap(this Textures.TEX4 texture, bool forceLowRes = false)
        {
            byte[] content = texture?.ToDDS(forceLowRes);
            return content?.ToBitmap();
        }
        public static Bitmap ToBitmap(this byte[] content, bool forceLowRes = false)
        {
            Bitmap toReturn = null;
            if (content == null) return null;
            try
            {
                MemoryStream imageStream = new MemoryStream(content);
                using (var image = Pfim.Pfim.FromStream(imageStream))
                {
                    System.Drawing.Imaging.PixelFormat format = System.Drawing.Imaging.PixelFormat.DontCare;
                    switch (image.Format)
                    {
                        case Pfim.ImageFormat.Rgba32:
                            format = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
                            break;
                        case Pfim.ImageFormat.Rgb24:
                            format = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
                            break;
                        case Pfim.ImageFormat.Rgb8:
                            format = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
                            break;
                        default:
                            Console.WriteLine("Unsupported DDS: " + image.Format);
                            break;
                    }
                    if (format != System.Drawing.Imaging.PixelFormat.DontCare)
                    {
                        var handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
                        try
                        {
                            var data = Marshal.UnsafeAddrOfPinnedArrayElement(image.Data, 0);
                            toReturn = new Bitmap(image.Width, image.Height, image.Stride, format, data);
                        }
                        finally
                        {
                            handle.Free();
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
            return toReturn;
        }

        /* Convert a Bitmap to ImageSource */
        public static ImageSource ToImageSource(this Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        /* Convert a CS2 submesh to GeometryModel3D */
        public static GeometryModel3D ToGeometryModel3D(this CS2.Component.LOD.Submesh submesh)
        {
            if (submesh.Data.Length == 0)
                return new GeometryModel3D();

            Int32Collection indices = new Int32Collection();
            Point3DCollection vertices = new Point3DCollection();
            PointCollection[] uvs = new PointCollection[0];

            using (BinaryReader reader = new BinaryReader(new MemoryStream(submesh.Data)))
            {
                for (int i = 0; i < submesh.VertexFormatFull.Attributes.Count; ++i)
                {
                    if (i == submesh.VertexFormatFull.Attributes.Count - 1)
                    {
                        for (int x = 0; x < submesh.IndexCount; x++)
                            indices.Add(reader.ReadUInt16());
                        continue;
                    }

                    for (int x = 0; x < submesh.VertexCount; ++x)
                    {
                        for (int y = 0; y < submesh.VertexFormatFull.Attributes[i].Count; ++y)
                        {
                            VertexFormat.Attribute attr = submesh.VertexFormatFull.Attributes[i][y];
                            Vector4 v = ReadVertexData(reader, attr.Type);

                            switch (attr.Usage)
                            {
                                case VertexFormat.Usage.Position:
                                    vertices.Add(new Point3D(v.X * submesh.VertexScale, v.Y * submesh.VertexScale, -v.Z * submesh.VertexScale));
                                    break;
                                case VertexFormat.Usage.TexCoord:
                                    if (attr.Index >= uvs.Length)
                                        Array.Resize(ref uvs, attr.Index + 1);
                                    if (uvs[attr.Index] == null)
                                        uvs[attr.Index] = new PointCollection();
                                    uvs[attr.Index].Add(new System.Windows.Point(v.X * 16.0f, v.Y * 16.0f));
                                    break;
                            }
                        }
                    }
                    Utilities.Align(reader, 16);
                }
            }

            if (vertices.Count == 0) return new GeometryModel3D();

            Int32Collection reversedIndices = new Int32Collection();
            for (int i = 0; i < indices.Count; i += 3)
            {
                if (i + 2 < indices.Count)
                {
                    reversedIndices.Add(indices[i]);
                    reversedIndices.Add(indices[i + 2]);
                    reversedIndices.Add(indices[i + 1]);
                }
            }

            PointCollection uv = new PointCollection();
            for (int i = 0; i < uvs.Length; i++)
            {
                if (uvs[i] != null)
                {
                    uv = uvs[i];
                    break;
                }
            }

            MeshGeometry3D geometry = new MeshGeometry3D
            {
                Positions = vertices,
                TriangleIndices = reversedIndices,
                TextureCoordinates = uv,
            };
            return new GeometryModel3D
            {
                Geometry = geometry,
                Material = new DiffuseMaterial(new SolidColorBrush(Color.FromRgb(255, 255, 0))),
                BackMaterial = new DiffuseMaterial(new SolidColorBrush(Color.FromRgb(255, 255, 0)))
            };
        }

        private static Vector4 ReadVertexData(BinaryReader reader, VertexFormat.Type type)
        {
            switch (type)
            {
                case VertexFormat.Type.FP32_1:
                    return new Vector4(reader.ReadSingle(), 0, 0, 0);
                case VertexFormat.Type.FP32_2:
                    return new Vector4(reader.ReadSingle(), reader.ReadSingle(), 0, 0);
                case VertexFormat.Type.FP32_3:
                    return new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), 0);
                case VertexFormat.Type.FP32_4:
                    return new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                case VertexFormat.Type.Color:
                    uint data = reader.ReadUInt32();
                    return new Vector4((float)((data & 0xFF000000) >> 24) / 255.0f, (float)((data & 0x00FF0000) >> 16) / 255.0f, (float)((data & 0x0000FF00) >> 8) / 255.0f, (float)((data & 0x000000FF) >> 0) / 255.0f);
                case VertexFormat.Type.U8_4:
                    return new Vector4(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
                case VertexFormat.Type.S16_2:
                    return new Vector4(reader.ReadInt16(), reader.ReadInt16(), 0, 0);
                case VertexFormat.Type.S16_4:
                    return new Vector4(reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16());
                case VertexFormat.Type.U8_4N:
                    return new Vector4((float)reader.ReadByte() / 255.0f, (float)reader.ReadByte() / 255.0f, (float)reader.ReadByte() / 255.0f, (float)reader.ReadByte() / 255.0f);
                case VertexFormat.Type.S16_2N:
                    return new Vector4((float)reader.ReadInt16() / (float)Int16.MaxValue, (float)reader.ReadInt16() / (float)Int16.MaxValue, 0, 0);
                case VertexFormat.Type.S16_4N:
                    return new Vector4((float)reader.ReadInt16() / (float)Int16.MaxValue, (float)reader.ReadInt16() / (float)Int16.MaxValue, (float)reader.ReadInt16() / (float)Int16.MaxValue, (float)reader.ReadInt16() / (float)Int16.MaxValue);
                case VertexFormat.Type.U16_2N:
                    return new Vector4((float)reader.ReadUInt16() / (float)UInt16.MaxValue, (float)reader.ReadUInt16() / (float)UInt16.MaxValue, 0, 0);
                case VertexFormat.Type.U16_4N:
                    return new Vector4((float)reader.ReadUInt16() / (float)UInt16.MaxValue, (float)reader.ReadUInt16() / (float)UInt16.MaxValue, (float)reader.ReadUInt16() / (float)UInt16.MaxValue, (float)reader.ReadUInt16() / (float)UInt16.MaxValue);
                case VertexFormat.Type.Dec3N:
                    uint val = reader.ReadUInt32();
                    short sx = (short)((val >> 20) & 0x3ff);
                    short sy = (short)((val >> 10) & 0x3ff);
                    short sz = (short)((val) & 0x3ff);
                    return new Vector4(((sx < 512) ? sx : (sx - 1024)) / 511.0f, ((sy < 512) ? sy : (sy - 1024)) / 511.0f, ((sz < 512) ? sz : (sz - 1024)) / 511.0f, 0);
            }
            throw new Exception("Unsupported VertexFormatType");
        }
    }
}
