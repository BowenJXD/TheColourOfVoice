using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextTransparent : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public float fadeDuration = 2.0f; // 淡入或淡出的持续时间
    private float currentFadeTime = 0f;
    private bool isFadingIn = true; // 初始状态设置为淡入

    void Update()
    {
        if (textMesh != null)
        {
            currentFadeTime += Time.deltaTime;

            // 根据是淡入还是淡出计算Alpha值
            float alpha;
            if (isFadingIn)
            {
                alpha = Mathf.Clamp01(currentFadeTime / fadeDuration);
            }
            else
            {
                alpha = Mathf.Clamp01(1.0f - currentFadeTime / fadeDuration);
            }

            // 更新vertex colors
            TMP_TextInfo textInfo = textMesh.textInfo;
            Color32[] newVertexColors;
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible)
                    continue;

                newVertexColors = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;
                int vertexIndex = charInfo.vertexIndex;
                newVertexColors[vertexIndex + 0].a = (byte)(alpha * 255);
                newVertexColors[vertexIndex + 1].a = (byte)(alpha * 255);
                newVertexColors[vertexIndex + 2].a = (byte)(alpha * 255);
                newVertexColors[vertexIndex + 3].a = (byte)(alpha * 255);
            }

            // 更新Mesh
            textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            // 检查时间是否超过，然后切换状态
            if (currentFadeTime >= fadeDuration)
            {
                isFadingIn = !isFadingIn; // 状态切换
                currentFadeTime = 0f; // 时间重置
            }
        }
    }
}
