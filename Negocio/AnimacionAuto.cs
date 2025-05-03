using OpenTK.Mathematics;

public class AnimacionAuto
{
    private Objeto _auto;
    private float _velocidad;
    private float _dist;
    private int fases;

    public AnimacionAuto()
    {
        _dist = 60f;
        _velocidad = 0.1f;
        fases=1;
    }

    public void CargarObj_Dist(Objeto auto)
    {
        _auto = auto;
    }

    public void Animar()
    {
        if (_auto == null) return;
        switch (fases)
        {
            case 1: // Ir recto
                _auto.Traslacion(0,0,-_velocidad); 
                _dist-=_velocidad;  
                RotarRuedas();
                if(_dist<=0){
                    fases++;
                    _dist=30;
                }
                break;
            
            case 2: // Doblar a a derecha
                _dist-=_velocidad;
                _auto.Traslacion(_velocidad*0.09f,0,-_velocidad*0.7f);
                _auto.Rotacion(0,-_velocidad*1.2f,0);
                RotarRuedas();
                
                if(_dist<=0){
                    fases++; _dist=40f;
                }
                break;
            
            case 3: // Parte del medio 1
                _dist-=_velocidad;
                _auto.Traslacion(_velocidad*0.5f,0,-_velocidad*0.35f);
                _auto.Rotacion(0,-_velocidad*1.0f,0);
                RotarRuedas();
                
                if(_dist<=0){
                    fases++; _dist=30f;
                }
                break;
            case 4: // Parte del medio 2
                _dist-=_velocidad;
                _auto.Traslacion(0.06f,0,_velocidad*0.01f);
                _auto.Rotacion(0,_velocidad*-1.5f,0);
                RotarRuedas();
                
                if(_dist<=0){
                    fases++; _dist=50f;
                }
                break;
            case 5: //Parte del medio 3
                _dist-=_velocidad;
                _auto.Traslacion(_velocidad*0.3f,0,_velocidad*0.2f);
                _auto.Rotacion(0,-_velocidad*0.5f,0);
                RotarRuedas();
                
                if(_dist<=0){
                    fases++; _dist=30f;
                }
                break;
            case 6: //Girar a la Derecha otra vez
                _dist-=_velocidad;
                _auto.Traslacion(_velocidad*0.25f,0,_velocidad);
                _auto.Rotacion(0,-_velocidad*1f,0);
                RotarRuedas();
                
                if(_dist<=0){
                    fases++; _dist=60f;
                }
                break;
            case 7: // Ir Recto
                _dist-=_velocidad;
               _auto.Traslacion(0,0,_velocidad); 
                RotarRuedas();
                
                if(_dist<=0){
                    fases++;
                }
                break;
        }
    }
    private void RotarRuedas()
    {
        foreach (var rueda in _auto.Partes)
        {
            if (rueda.Key.ToLower().Contains("rueda"))
            {
                rueda.Value.Rotacion(_velocidad * 70f, 0, 0);
            }
        }
    }
}