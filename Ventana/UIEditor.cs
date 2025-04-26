using System.Numerics;
using ImGuiNET;
using System.Collections.Generic;
using System.Linq;

public class UIEditor
{
    private int selectedObjeto = 0;
    private int selectedParte = 0;
    private int tipoTransformacion = 0; // 0: Rotar, 1: Trasladar, 2: Escalar

    private Vector3 valores = Vector3.Zero;
    private float factorEscala = 1.0f;

    private List<string> objetos = new();
    private List<string> partes = new();

    public int TipoTransformacion => tipoTransformacion;
    public bool SeleccionaEscenario => selectedObjeto == 0;
    public string? NombreObjetoSeleccionado => selectedObjeto > 0 && selectedObjeto < objetos.Count ? objetos[selectedObjeto] : null;
    public string? NombreParteSeleccionada => selectedParte >= 0 && selectedParte < partes.Count ? partes[selectedParte] : null;

    public void Dibujar(Escenario escenario)
    {
        if (escenario == null) return;

        ImGui.Begin("Panel de Transformaciones");

        objetos = new List<string> { "(Escenario)" };
        objetos.AddRange(escenario.Objetos.Keys);

        if (selectedObjeto >= objetos.Count) selectedObjeto = 0;
        string nombreObjeto = objetos[selectedObjeto];

        if (ImGui.BeginCombo("Objeto", nombreObjeto))
        {
            for (int i = 0; i < objetos.Count; i++)
            {
                bool isSelected = (selectedObjeto == i);
                if (ImGui.Selectable(objetos[i], isSelected))
                    selectedObjeto = i;
                if (isSelected) ImGui.SetItemDefaultFocus();
            }
            ImGui.EndCombo();
        }

        if (!SeleccionaEscenario)
        {
            var objeto = escenario.Objetos[NombreObjetoSeleccionado!];
            partes = objeto?.Partes.Keys.ToList() ?? new();
            partes.Insert(0, "(Todo el objeto)");

            if (selectedParte >= partes.Count) selectedParte = 0;
            string nombreParte = partes[selectedParte];

            if (ImGui.BeginCombo("Parte", nombreParte))
            {
                for (int i = 0; i < partes.Count; i++)
                {
                    bool isSelected = (selectedParte == i);
                    if (ImGui.Selectable(partes[i], isSelected))
                        selectedParte = i;
                    if (isSelected) ImGui.SetItemDefaultFocus();
                }
                ImGui.EndCombo();
            }
        }

        ImGui.RadioButton("Rotar", ref tipoTransformacion, 0); ImGui.SameLine();
        ImGui.RadioButton("Trasladar", ref tipoTransformacion, 1); ImGui.SameLine();
        ImGui.RadioButton("Escalar", ref tipoTransformacion, 2);

        if (tipoTransformacion == 2)
        {
            ImGui.SliderFloat("Factor de Escala", ref factorEscala, 0.1f, 2.5f);
        }
        else
        {
            ImGui.SliderFloat3("X Y Z", ref valores, -180f, 180f);
        }

        if (ImGui.Button("Aplicar"))
        {
            if (SeleccionaEscenario)
            {
                AplicarTransformacion(escenario);
            }
            else
            {
                var objeto = escenario.Objetos[NombreObjetoSeleccionado!];

                if (selectedParte == 0)
                {
                    if (objeto != null)
                        AplicarTransformacion(objeto);
                }
                else
                {
                    var parte = objeto?.Partes[NombreParteSeleccionada!];
                    if (parte != null)
                        AplicarTransformacion(parte);
                }
            }
        }

        ImGui.End();
    }

    private void AplicarTransformacion(object target)
    {
        switch (target)
        {
            case Escenario escenario:
                if (tipoTransformacion == 0)
                    escenario.Rotacion(valores.X, valores.Y, valores.Z);
                else if (tipoTransformacion == 1)
                    escenario.Traslacion(valores.X, valores.Y, valores.Z);
                else if (tipoTransformacion == 2)
                    escenario.Escalacion(factorEscala);
                break;

            case Objeto objeto:
                if (tipoTransformacion == 0)
                    objeto.Rotacion(valores.X, valores.Y, valores.Z);
                else if (tipoTransformacion == 1)
                    objeto.Traslacion(valores.X, valores.Y, valores.Z);
                else if (tipoTransformacion == 2)
                    objeto.Escalacion(factorEscala);
                break;

            case Parte parte:
                if (tipoTransformacion == 0)
                    parte.Rotacion(valores.X, valores.Y, valores.Z);
                else if (tipoTransformacion == 1)
                    parte.Traslacion(valores.X, valores.Y, valores.Z);
                else if (tipoTransformacion == 2)
                    parte.Escalacion(factorEscala);
                break;
        }
    }
}
