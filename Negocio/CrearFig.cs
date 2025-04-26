class CrearFigura
{
    public CrearFigura()
    {

    }
    public Objeto CrearFiguraU(float x, float y, float z)
    {
        //figura U
        //Pilar 1
        //cara delantera
        List<Vertice> cara_delantera_P1 = new List<Vertice>{
                new Vertice(-0.8f, -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.8f,  1.5f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.8f,  1.5f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f,  1.5f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.3f,  1.5f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f, -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.3f,  -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.8f,  -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
            };
        
        //cara trasera
        List<Vertice> cara_trasera_P1 = new List<Vertice>{
                new Vertice(-0.8f, -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.8f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.8f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.3f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f, -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.3f, -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.8f, -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
            };
        //cara de arriba
        List<Vertice> cara_arriba_P1 = new List<Vertice>{
                new Vertice(-0.8f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.8f,  1.5f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f,  1.5f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.8f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.8f,  1.5f,  0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.3f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f,  1.5f,  0.5f, 1.0f, 1.0f, 0.0f),
            };
        //cara de abajo
        List<Vertice> cara_abajo_P1 = new List<Vertice>{
                new Vertice(-0.8f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.8f,  -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f,  -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.8f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.8f,  -1.0f,  0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.3f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f,  -1.0f,  0.5f, 1.0f, 1.0f, 0.0f),
            };
        //cara izquierda
        List<Vertice> cara_izquierda_P1 = new List<Vertice>{
                new Vertice(-0.8f, -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.8f,  1.5f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.8f, -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.8f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.8f,  1.5f,  0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.8f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.8f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.8f,  -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
            };
        //cara derecha
        List<Vertice> cara_derecha_P1 = new List<Vertice>{
                new Vertice(-0.3f, -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f,  1.5f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.3f, -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.3f,  1.5f,  0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.3f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f,  -1.0f, 0.5f, 1.0f, 1.0f, 0.0f)
            };



        //Pilar 2
        
        //cara delantera 
        List<Vertice> cara_delantera_Pilar2 = new List<Vertice>{
                new Vertice(1.3f, -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(1.3f,  1.5f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(0.8f, -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  1.5f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(1.3f,  1.5f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  1.5f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(1.3f,  -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
            };
        //cara trasera
        List<Vertice> cara_trasera_Pilar2 = new List<Vertice>{
                new Vertice(1.3f, -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(1.3f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(0.8f, -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(1.3f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(1.3f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
            };
        //cara arriba
        List<Vertice> cara_arriba_Pilar2 = new List<Vertice>{
                new Vertice(1.3f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(1.3f,  1.5f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  1.5f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(1.3f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(1.3f,  1.5f,  0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(0.8f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  1.5f,  0.5f, 1.0f, 1.0f, 0.0f),
            };
        //cara abajo
        List<Vertice> cara_abajo_Pilar2 = new List<Vertice>{
                new Vertice(1.3f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(1.3f,  -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(1.3f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(1.3f,  -1.0f,  0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(0.8f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  -1.0f,  0.5f, 1.0f, 1.0f, 0.0f),
            };
        //cara derecha
        List<Vertice> cara_derecha_Pilar2 = new List<Vertice>{
                new Vertice(1.3f, -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(1.3f,  1.5f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(1.3f, -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(1.3f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(1.3f,  1.5f,  0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(1.3f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(1.3f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(1.3f,  -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
            };
        //cara izquierda
        List<Vertice> cara_izquierda_Pilar2 = new List<Vertice>{
                new Vertice(0.8f, -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  1.5f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(0.8f, -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(0.8f,  1.5f,  0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  1.5f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(0.8f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  -1.0f, 0.5f, 1.0f, 1.0f, 0.0f)
            };
        
        //Union

        //cara delantera
        List<Vertice> cara_delantera_Union = new List<Vertice>{
                new Vertice(-0.3f, -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.3f,  -0.5f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,   -0.5f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.3f,  -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f,  -0.5f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(0.8f,  -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  -0.5f, 0.5f, 1.0f, 1.0f, 0.0f),
            };
        //cara trasera
        List<Vertice> cara_trasera_Union = new List<Vertice>{
                new Vertice(-0.3f, -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.3f,  -0.5f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,   -0.5f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.3f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f,  -0.5f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(0.8f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  -0.5f, -0.5f, 1.0f, 1.0f, 0.0f),
            };
        //cara de arriba
        List<Vertice> cara_arriba_Union = new List<Vertice>{
                new Vertice(-0.3f, -0.5f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  -0.5f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.3f,  -0.5f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,   -0.5f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.3f,  -0.5f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f,  -0.5f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(0.8f,  -0.5f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  -0.5f, -0.5f, 1.0f, 1.0f, 0.0f),
            };
        //cara de abajo
        List<Vertice> cara_abajo_Union = new List<Vertice>{
                new Vertice(-0.3f, -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.3f,  -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,   -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.3f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f,  -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(0.8f,  -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
            };
        //cara derecha
        List<Vertice> cara_derecha_Union = new List<Vertice>{
                new Vertice(0.8f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  -0.5f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(0.8f,  -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  -0.5f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(0.8f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  -1.0f,  0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(0.8f,  -0.5f,  0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(0.8f,  -0.5f, -0.5f, 1.0f, 1.0f, 0.0f),
            };
        //cara izquierda
        List<Vertice> cara_izquierda_Union = new List<Vertice>{
                new Vertice(-0.3f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f,  -0.5f, -0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.3f,  -1.0f, 0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f,  -0.5f, 0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.3f,  -1.0f, -0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f,  -1.0f,  0.5f, 1.0f, 1.0f, 0.0f),

                new Vertice(-0.3f,  -0.5f,  0.5f, 1.0f, 1.0f, 0.0f),
                new Vertice(-0.3f,  -0.5f, -0.5f, 1.0f, 1.0f, 0.0f),
            };

        Dictionary<string, Cara> parte_Pilar1 = new Dictionary<string, Cara>{
            {"cara_trasera_P1", new Cara("cara_trasera_P1", cara_trasera_P1, 0f, 0f, 0f)},
            {"cara_delantera_P1", new Cara("cara_delantera_P1", cara_delantera_P1, 0f, 0f, 0f)},
            {"cara_arriba_P1", new Cara("cara_arriba_P1", cara_arriba_P1, 0f, 0f, 0f)},
            {"cara_abajo_P1", new Cara("cara_abajo_P1", cara_abajo_P1, 0f, 0f, 0f)},
            {"cara_izquierda_P1", new Cara("cara_izquierda_P1", cara_izquierda_P1, 0f, 0f, 0f)},
            {"cara_derecha_P1", new Cara("cara_derecha_P1", cara_derecha_P1, 0f, 0f, 0f)}
        };

        Dictionary<string, Cara> parte_Pilar2 = new Dictionary<string, Cara>{
            {"cara_delantera_Pilar2", new Cara("cara_delantera_Pilar2", cara_delantera_Pilar2, 0f, 0f, 0f)},
            {"cara_trasera_Pilar2", new Cara("cara_trasera_Pilar2", cara_trasera_Pilar2, 0f, 0f, 0f)},
            {"cara_arriba_Pilar2", new Cara("cara_arriba_Pilar2", cara_arriba_Pilar2, 0f, 0f, 0f)},
            {"cara_abajo_Pilar2", new Cara("cara_abajo_Pilar2", cara_abajo_Pilar2, 0f, 0f, 0f)},
            {"cara_derecha_Pilar2", new Cara("cara_derecha_Pilar2", cara_derecha_Pilar2, 0f, 0f, 0f)},
            {"cara_izquierda_Pilar2", new Cara("cara_izquierda_Pilar2", cara_izquierda_Pilar2, 0f, 0f, 0f)}
        };

        Dictionary<string, Cara> parte_Union = new Dictionary<string, Cara>{
            {"cara_delantera_Union", new Cara("cara_delantera_Union", cara_delantera_Union, 0f, 0f, 0f)},
            {"cara_trasera_Union", new Cara("cara_trasera_Union", cara_trasera_Union, 0f, 0f, 0f)},
            {"cara_arriba_Union", new Cara("cara_arriba_Union", cara_arriba_Union, 0f, 0f, 0f)},
            {"cara_abajo_Union", new Cara("cara_abajo_Union", cara_abajo_Union, 0f, 0f, 0f)},
            {"cara_derecha_Union", new Cara("cara_derecha_Union", cara_derecha_Union, 0f, 0f, 0f)},
            {"cara_izquierda_Union", new Cara("cara_izquierda_Union", cara_izquierda_Union, 0f, 0f, 0f)}
        };

        Dictionary<string, Parte> Upartes = new Dictionary<string, Parte>{
            {"pilar1", new Parte(parte_Pilar1, 0f, 0f, 0f)},
            {"pilar2", new Parte(parte_Pilar2, 0f, 0f, 0f)},
            {"union", new Parte(parte_Union, 0f, 0f, 0f)}
        };

        Objeto U = new Objeto(Upartes, x, y, z);

        return U;
    }

}