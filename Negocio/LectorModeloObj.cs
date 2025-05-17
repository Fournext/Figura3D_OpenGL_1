using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using OpenTK.Mathematics;

public class LectorModeloObj
{
    public static Objeto ImportarOBJConMaterial(string pathObj, float posX = 0f, float posY = 0f, float posZ = 0f)
    {
        string directorio = Path.GetDirectoryName(pathObj)!;
        string[] lineas = File.ReadAllLines(pathObj);

        List<Vector3> posiciones = new();
        Dictionary<string, Vector3> materiales = new();
        var partesPorGrupo = new Dictionary<string, List<Vertice>>();

        string grupoActual = "default";
        string materialActual = "Material_default";

        foreach (var linea in lineas)
        {
            if (linea.StartsWith("mtllib "))
            {
                string nombreMtl = linea.Substring(7).Trim();
                string pathMtl = Path.Combine(directorio, nombreMtl);
                CargarMateriales(pathMtl, materiales);
            }
            else if (linea.StartsWith("v "))
            {
                var tokens = linea.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                float x = float.Parse(tokens[1], CultureInfo.InvariantCulture);
                float y = float.Parse(tokens[2], CultureInfo.InvariantCulture);
                float z = float.Parse(tokens[3], CultureInfo.InvariantCulture);
                posiciones.Add(new Vector3(x, y, z));
            }
            else if (linea.StartsWith("usemtl "))
            {
                materialActual = linea.Substring(7).Trim();
                if (string.IsNullOrWhiteSpace(materialActual) || materialActual.ToLower() == "none")
                    materialActual = "Material_default";
            }
            else if (linea.StartsWith("o "))
            {
                grupoActual = linea.Substring(2).Trim().ToLower(); 
                if (!partesPorGrupo.ContainsKey(grupoActual))
                    partesPorGrupo[grupoActual] = new List<Vertice>();
            }
            else if (linea.StartsWith("g "))
            {
                grupoActual = linea.Substring(2).Trim().ToLower(); 
                if (!partesPorGrupo.ContainsKey(grupoActual))
                    partesPorGrupo[grupoActual] = new List<Vertice>();
            }
            else if (linea.StartsWith("f "))
            {
                if (!materiales.ContainsKey(materialActual)) continue;

                var indicesStr = linea.Substring(2).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                List<int> indices = new();
                foreach (var index in indicesStr)
                {
                    var split = index.Split('/');
                    if (!int.TryParse(split[0], out int idx)) continue;
                    indices.Add(idx - 1);
                }

                if (indices.Count < 3) continue;

                Vector3 color = materiales[materialActual];
                var lista = partesPorGrupo[grupoActual];

                for (int i = 1; i < indices.Count - 1; i++)
                {
                    var v0 = posiciones[indices[0]];
                    var v1 = posiciones[indices[i]];
                    var v2 = posiciones[indices[i + 1]];

                    lista.Add(new Vertice(v0.X, v0.Y, v0.Z, color.X, color.Y, color.Z));
                    lista.Add(new Vertice(v1.X, v1.Y, v1.Z, color.X, color.Y, color.Z));
                    lista.Add(new Vertice(v2.X, v2.Y, v2.Z, color.X, color.Y, color.Z));
                }
            }
        }

        // Crear las partes sin modificar la posición relativa
        var partes = new Dictionary<string, Parte>();
        foreach (var kv in partesPorGrupo)
        {
            var caraUnica = new Dictionary<string, Cara>();
            caraUnica[kv.Key] = new Cara(kv.Key, kv.Value, 0f, 0f, 0f); 
            partes[kv.Key] = new Parte(caraUnica, 0f, 0f, 0f);
        }

        return new Objeto(partes, posX, posY, posZ);
    }

    private static void CargarMateriales(string pathMtl, Dictionary<string, Vector3> materiales)
    {
        if (!File.Exists(pathMtl)) return;

        string[] lineas = File.ReadAllLines(pathMtl);
        string nombreActual = "";

        foreach (var linea in lineas)
        {
            if (linea.StartsWith("newmtl "))
            {
                nombreActual = linea.Substring(7).Trim();
            }
            else if (linea.StartsWith("Kd ") && !string.IsNullOrEmpty(nombreActual))
            {
                var tokens = linea.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                float r = float.Parse(tokens[1], CultureInfo.InvariantCulture);
                float g = float.Parse(tokens[2], CultureInfo.InvariantCulture);
                float b = float.Parse(tokens[3], CultureInfo.InvariantCulture);
                materiales[nombreActual] = new Vector3(r, g, b);
            }
        }
    }
}
