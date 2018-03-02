using System;
using System.IO;
using UnityEngine;

// Based upon https://gist.github.com/mikezila/10557162
// and http://www.paulbourke.net/dataformats/tga/
public class TGALoader {
    public static Texture3D load3DFromTGASlices(String path)
    {
        using (BinaryReader br = new BinaryReader(File.OpenRead(path)))
        {
            br.BaseStream.Seek(12, SeekOrigin.Begin);

            short width = br.ReadInt16();
            short height = br.ReadInt16();
            int height2 = height * height;

            int bitDepth = br.ReadByte();

            br.BaseStream.Seek(1, SeekOrigin.Current);

            Texture3D result;
            Color32[] colors = new Color32[width * height];

            if (bitDepth == 32)
            {
                for (int i = 0; i < colors.Length; i++)
                {
                    byte red = br.ReadByte();
                    byte green = br.ReadByte();
                    byte blue = br.ReadByte();
                    byte alpha = br.ReadByte();

                    int x = i % width % height;
                    int y = i / width;
                    int z = i % width / height;

                    int idx = z * height2 + y * height + x;

                    colors[idx] = new Color32(alpha, blue, green, red);
                }
                result = new Texture3D(height, height, height, TextureFormat.RGBA32, true);
            }
            else if (bitDepth == 24)
            {
                for (int i = 0; i < colors.Length; i++)
                {
                    byte red = br.ReadByte();
                    byte green = br.ReadByte();
                    byte blue = br.ReadByte();

                    int x = i % width % height;
                    int y = i / width;
                    int z = i % width / height;

                    int idx = z * height2 + y * height + x;

                    colors[idx] = new Color32(blue, green, red, 1);
                }
                result = new Texture3D(height, height, height, TextureFormat.RGB24, true);
            }
            else
            {
                throw new Exception("TGA texture had non 32/24 bit depth.");
            }

            result.SetPixels32(colors, 0);
            result.wrapMode = TextureWrapMode.Repeat;
            result.filterMode = FilterMode.Bilinear;
            result.Apply();
            return result;
        }
    }
}
