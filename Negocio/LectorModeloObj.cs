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

        Dictionary<string, Dictionary<string, List<Vertice>>> partesPorGrupo = new()
        {
            { "chasis", new Dictionary<string, List<Vertice>>() },
            { "rueda_trasera", new Dictionary<string, List<Vertice>>() },
            { "rueda_delantera", new Dictionary<string, List<Vertice>>() }
        };

        HashSet<string> grupos_chasis = new()
        {
            "Carroceria_Plano", "Carroceria_2_Plano.003", "Parabrisas_Ad_Plano.005", "Parabrisas_Tr_Plano.015",
            "Marco_Parabrisad_Ad_Plano.006", "Marco_Parabrisad_Tr_Plano.016", "Ventana_1_Plano.013",
            "Ventana_2_Plano.010", "Marco_Ventana_2_Plano.012", "Pueratas_Plano.008", "Rejilla_Plano.002",
            "Plano.002_Plano.019"
        };
        HashSet<string> grupos_rueda_trasera = new()
        {
            "Ruedas_Tr_Círculo.005", "Rin_1_Tr_Plano.018", "Rin_2_Tr_Círculo.006"
        };
        HashSet<string> grupos_rueda_delantera = new()
        {
            "Ruedas_Ad_Círculo.004", "Rin_1_Ad_Plano.017", "Rin_2_Ad_Círculo.003"
        };

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
            else if (linea.StartsWith("g ") || linea.StartsWith("o "))
            {
                grupoActual = linea.Substring(2).Trim();
            }
            else if (linea.StartsWith("f "))
            {
                if (!materiales.ContainsKey(materialActual)) continue;

                string? parte = grupos_chasis.Contains(grupoActual) ? "chasis"
                            : grupos_rueda_trasera.Contains(grupoActual) ? "rueda_trasera"
                            : grupos_rueda_delantera.Contains(grupoActual) ? "rueda_delantera"
                            : null;

                if (parte == null) continue;

                if (!partesPorGrupo[parte].ContainsKey(grupoActual))
                    partesPorGrupo[parte][grupoActual] = new List<Vertice>();

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
                var lista = partesPorGrupo[parte][grupoActual];

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

        Dictionary<string, Parte> partes = new();
        foreach (var kv in partesPorGrupo)
        {
            Dictionary<string, Cara> caras = new();
            Vector3 acumulado = Vector3.Zero;
            int total = 0;

            foreach (var cara in kv.Value)
            {
                if (cara.Value.Count > 0)
                {
                    foreach (var v in cara.Value)
                    {
                        acumulado += v.Posicion;
                        total++;
                    }
                }
            }

            Vector3 centro = total > 0 ? acumulado / total : Vector3.Zero;

            foreach (var cara in kv.Value)
            {
                var lista = new List<Vertice>();
                foreach (var v in cara.Value)
                {
                    var pos = v.Posicion - centro;
                    lista.Add(new Vertice(pos.X, pos.Y, pos.Z, v.Color.X, v.Color.Y, v.Color.Z));
                }
                caras[cara.Key] = new Cara(cara.Key, lista, 0f, 0f, 0f);
            }

            partes[kv.Key] = new Parte(caras, centro.X, centro.Y, centro.Z);
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