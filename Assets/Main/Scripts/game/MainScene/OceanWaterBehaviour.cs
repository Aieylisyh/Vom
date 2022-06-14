using UnityEngine;
using com;

namespace game
{
    public class OceanWaterBehaviour : MeshGenerator
    {
        public float waveHeight = 0.5f;
        public float waveFrequency = 0.5f;
        public float waveLength = 0.75f;
        public Vector2 waveOriginPosition = new Vector2(0.0f, 0.0f);
        public Vector2 waveOriginPosition2 = new Vector2(0.0f, 0.0f);
        public float waveOriginSpeed;
        void Update()
        {
            GenerateWaves();
        }

        /// <summary>
        /// Based on the specified wave height and frequency, generate 
        /// wave motion originating from waveOriginPosition
        /// </summary>
        void GenerateWaves()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 v = vertices[i];

                //Get the distance between wave origin position and the current vertex
                Vector2 origin = Vector2.Lerp(waveOriginPosition, waveOriginPosition2, Mathf.Sin(Time.time * waveOriginSpeed) * 0.5f + 0.5f);
                float distance = Vector3.Distance(v, new Vector3(origin.x, 0, origin.y));
                distance = (distance % waveLength) / waveLength;

                //Oscilate the wave height via sine to create a wave effect
                v.y = waveHeight * Mathf.Sin(Time.time * Mathf.PI * 2.0f * waveFrequency
                + (Mathf.PI * 2.0f * distance));

                //Update the vertex
                vertices[i] = v;
            }

            //Update the mesh properties
            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.MarkDynamic();
            meshFilter.mesh = mesh;
        }
    }


}
