using System.Collections;
using UnityEngine;

public enum ColorMappingType {
    Normal,
    Red,
    Green,
    Blue,
}

[RequireComponent(typeof(Camera))]
public class ColorMapping : MonoBehaviour {
    public Material materialMapping;

    private double[][,] MappingUt;
    private Material curMaterial;

    private void Awake() {
        MappingUt = new double[3][,] {
            // 红色
            new double[3, 3] { {1, 0, 0}, {0.47, 0.49, 0.04}, {0.59, -0.68, 1.09} },
            // 绿色
            new double[3, 3] { {1.22, -0.31, 0.11}, {0, 1, 0}, {-0.18, 0.17, 1} },
            // 蓝色
            new double[3, 3] { {0.74, -0.39, 0.66}, {0.08, 0.59, 0.33}, {0, 0, 1} }
        };
    }

    public void SetMapping(ColorMappingType type) {
        switch (type) {
            case ColorMappingType.Normal:
                curMaterial = null;
                break;
            case ColorMappingType.Red:
                ApplyColorMatrix(MappingUt[0]);
                curMaterial = materialMapping;
                break;
            case ColorMappingType.Green:
                ApplyColorMatrix(MappingUt[1]);
                curMaterial = materialMapping;
                break;
            case ColorMappingType.Blue:
                ApplyColorMatrix(MappingUt[2]);
                curMaterial = materialMapping;
                break;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Debug.Log("OnRenderImage called");
        if (curMaterial)
            Graphics.Blit(source, destination, curMaterial);
        else
            Graphics.Blit(source, destination);
    }

    private void ApplyColorMatrix(double[,] Ut) {
        materialMapping.SetVector("_UtR", new Vector4((float)Ut[0, 0], (float)Ut[0, 1], (float)Ut[0, 2], 0));
        materialMapping.SetVector("_UtG", new Vector4((float)Ut[1, 0], (float)Ut[1, 1], (float)Ut[1, 2], 0));
        materialMapping.SetVector("_UtB", new Vector4((float)Ut[2, 0], (float)Ut[2, 1], (float)Ut[2, 2], 0));
    }
    public void SetRedMapping() {
        SetMapping(ColorMappingType.Red);
    }

    public void SetGreenMapping() {
        SetMapping(ColorMappingType.Green);
    }

    public void SetBlueMapping() {
        SetMapping(ColorMappingType.Blue);
    }
}