public class AnimacionAuto
{
    private Objeto _auto;
    private float _radio;
    private float _velocidad;
    private float _dist;
    private bool _girando;
    private int fases;

    public AnimacionAuto()
    {
        _dist = 10f;
        _velocidad = 0.02f;
        _radio = 10f;
        _girando = false;
        fases=1;
    }

    public void CargarObj_Dist(Objeto auto)
    {
        _auto = auto;
    }

    public void Animar()
    {
        if (_auto == null) return;
        Console.Write(fases);
        switch (fases)
        {
            case 1:
                _auto.Traslacion(0,0,_velocidad); 
                _dist-=_velocidad;  
                foreach (var rueda in _auto.Partes)
                {
                    if (rueda.Key.ToLower().Contains("rueda"))
                    {
                        rueda.Value.Rotacion(_velocidad * 70f, 0, 0);
                    }
                }
                if(_dist<=4)
                    fases++;
                break;
            
            case 2:
                _auto.Traslacion(0,0,_velocidad); 
                _auto.Rotacion(0,_velocidad*20,0);
                // foreach (var partes in _auto.Partes)
                // {
                //     if (partes.Key.ToLower().Contains("chasis"))
                //     {
                //         partes.Value.Rotacion(0, _velocidad*20, 0);
                //     }
                //     if (partes.Key.ToLower().Contains("rueda_trasera"))
                //     {
                //         partes.Value.Traslacion(-_velocidad, 0, 0);
                //         partes.Value.Rotacion(0, 0, -_velocidad*10);
                //     }
                //     if (partes.Key.ToLower().Contains("rueda_delantera"))
                //     {
                //         partes.Value.Rotacion(0, 0, -_velocidad*10);
                //     }
                // }
                if(_dist==0){
                    fases++; _dist=10f;
                }
                break;
            
            case 3:
                _auto.Traslacion(_velocidad,0,0); 
                _dist-=_velocidad;  
                foreach (var rueda in _auto.Partes)
                {
                    if (rueda.Key.ToLower().Contains("rueda"))
                    {
                        rueda.Value.Rotacion(_velocidad * 70f, 0, 0);
                    }
                }
                if(_dist >= 0f && _dist<=4)
                    fases++;
                break;
            
        }
    }
}
