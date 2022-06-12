using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.NeuralNetwork;
using Network = Assets.Scripts.NeuralNetwork.Network;
using Assets.Scripts;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;

public class NetworkDrawer : MonoBehaviour
{
    [SerializeField] private AiController aiController;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject weights;
    [SerializeField] private GameObject layers;
    [SerializeField] private float baseWidth;
    [SerializeField] private Color positivColor;
    [SerializeField] private Color negativColor;
    [SerializeField] private Color baseColor;
    [SerializeField] private uint maxTileValue;

    private void Update()
    {
        // Destroy old lines
        for (int i = 0; i < weights.transform.childCount; i++)
        {
            Destroy(weights.transform.GetChild(i).gameObject);
        }

        Color color;

        // Update color of input layer nodes
        for (int i = 0; i < layers.transform.GetChild(0).childCount; i++)
        {
            var node = layers.transform.GetChild(0).GetChild(i).GetChild(0);
            var value = aiController.Input[i];

            float h, s, v;

            Color.RGBToHSV(baseColor, out h, out s, out v);
            v *= (float)(value / maxTileValue);

            color = Color.HSVToRGB(h, s, v);

            node.GetComponent<Image>().color = color;
        }

        // Iterate through other layers
        for (int i = 1; i < layers.transform.childCount; i++)
        {
            var curr = layers.transform.GetChild(i);
            var prev = layers.transform.GetChild(i - 1);

            for (int j = 0; j < curr.childCount; j++)
            {
                Layer layer;

                if (i < layers.transform.childCount - 1)
                {
                    layer = aiController.UiNetwork.HiddenLayers[i - 1];
                }
                else
                {
                    layer = aiController.UiNetwork.OuputLayer;
                }

                // Set value color
                var value = aiController.Outputs[i - 1][j];

                float h, s, v;

                Color.RGBToHSV(baseColor, out h, out s, out v);
                v *= (float)(value);
                color = Color.HSVToRGB(h, s, v);

                curr.GetChild(j).GetChild(0).GetComponent<Image>().color = color;

                // Set bias color
                if (layer.Bias[j] > 0)
                {
                    Color.RGBToHSV(positivColor, out h, out s, out v);
                    s *= (float)(layer.Bias[j] / aiController.MaxBias);

                    color = Color.HSVToRGB(h, s, v);
                }
                else
                {
                    Color.RGBToHSV(negativColor, out h, out s, out v);
                    s *= (float)(- layer.Bias[j] / aiController.MaxBias);

                    color = Color.HSVToRGB(h, s, v);
                }

                curr.GetChild(j).GetComponent<Image>().color = color;

                for (int k = 0; k < prev.childCount; k++)
                {
                    // Draw line between noedes
                    var pos1 = prev.GetChild(k).transform.position;
                    var pos2 = curr.GetChild(j).transform.position;

                    pos1.z = -1;
                    pos2.z = -1;

                    var line = Instantiate(linePrefab, weights.transform).GetComponent<LineRenderer>();

                    line.SetPositions(new Vector3[]{pos1, pos2});

                    // Set weight and color of line
                    var weight = layer.Weights[j, k];
                    line.widthMultiplier = (float) Math.Abs(weight) * baseWidth;

                    if (weight > 0)
                    {
                        Color.RGBToHSV(positivColor, out h, out s, out v);
                        s *= (float) weight;

                        color = Color.HSVToRGB(h, s, v);
                    }
                    else
                    {
                        Color.RGBToHSV(negativColor, out h, out s, out v);
                        s *= (float) - weight;

                        color = Color.HSVToRGB(h, s, v);
                    }

                    var gradient = new Gradient
                    {
                        mode = GradientMode.Fixed,
                        colorKeys = new GradientColorKey[] { new GradientColorKey(color, 0) }
                    };

                    line.colorGradient = gradient;
                }
            }
        }
    }
}
