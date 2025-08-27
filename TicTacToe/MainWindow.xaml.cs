using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MarcarCasilla(object sender, MouseButtonEventArgs e)
        {
            Canvas casilla = (Canvas)sender;

            Label textContainer = (Label)casilla.Children[0];

            textContainer.Content = "X";

            InteligenciaArtificial();
        }

        private void InteligenciaArtificial()
        {
            String[][] casillas = MapearTablero();
            int[][] probabilidades = CalcularProbabilidades(casillas);

            int columnaMax = 0;
            int filaMax = 0;

            for (int i = 0; i < casillas.Length; i++)
            {
                for (int j = 0; j < casillas[i].Length; j++)
                {
                    if (casillas[i][j] == "X" || casillas[i][j] == "O")
                    {
                        probabilidades[i][j] = 0;
                    }
                }
            }

            for (int i = 0; i < casillas.Length; i++) // Este bucle lo que busca es la caillas con mas probabilidad de que gane el rival
            {
                for (int j = 0; j < casillas[i].Length; j++)
                {
                    if (probabilidades[i][j] > probabilidades[filaMax][columnaMax])
                    {
                        filaMax = i; 
                        columnaMax = j;
                        
                    }
                }
            }


            int casillasNumero =  filaMax * 3 + columnaMax; // Multiplica por 3, para que me de el resultado del 1 al 9

            if (probabilidades[filaMax][columnaMax] != 0)
            {
                foreach (var el in contenedor.Children)
                {
                    if (el is Canvas)
                    {
                        Canvas actualCanva = (Canvas)el;
                        Label actualLabel = (Label)actualCanva.Children[0];



                        if (actualLabel.Name.ToString()[actualLabel.Name.ToString().Length - 1] == casillasNumero.ToString()[casillasNumero.ToString().Length - 1])
                        {
                            actualLabel.Content = "O";
                            break;
                        }
                    }
                }
            }
        }

        private int[][] CalcularProbabilidades(String[][] casillas) //A travez de funciones, calcula las probabilidades de que la IA gane o bloquee al rival
        {
            int[][] tableroPosibilidades = SumaCasillas(casillas, "X"); 

            return tableroPosibilidades;
        }

        private String[][] MapearTablero() //Mapea el tablero para que la IA pueda analizarlo
        {
            int columna = 0;
            int fila = 0;
            String[][] casillas = new String[3][]; //Mapeo 3x3
            casillas[0] = ["-", "-", "-"];
            casillas[1] = ["-", "-", "-"];
            casillas[2] = ["-", "-", "-"];
            foreach (var el in contenedor.Children)
            {
                if(el is Canvas)
                {
                    Canvas actualCanva = (Canvas)el;
                    Label actualLabel = (Label)actualCanva.Children[0];
                    casillas[fila][columna] = actualLabel.Content.ToString() is String s && s.Length > 0 ? s : "-";
                    fila++;  
                    if(fila == 3)
                    {
                        fila = 0;
                        columna++;
                    }
                    if (columna == 3)
                    {
                        break;
                    }
                }
            }
            return casillas;
        }

        private int[][] SumaCasillas(String[][] casillas, String simbolo) //Usando el mapeo de la funcion anterior, analiza las probabilidades de que el rival gane o saque ventaja y reacciona a ello
        {
            int[] xEnFila = new int[3]; //[0,0,0] fila 1, la 2 y la 3
            xEnFila[0] = 0; //Fila 1
            xEnFila[1] = 0; //Fila 2
            xEnFila[2] = 0; //Fila 3

            int[] xEnColumna = new int[3]; //[0,0,0] columna 1, la 2 y la 3
            xEnColumna[0] = 0; //columna 1
            xEnColumna[1] = 0; //columna 2
            xEnColumna[2] = 0; //columna 3

            int diagonal1 = 0;
            int diagonal2 = 0;



            for (int i = 0; i < casillas.Length; i++) //Analiza combinaciones en el eje X
            {
                int contadorSimbolo = 0;
                for (int j = 0; j < casillas[i].Length; j++)
                {
                    if (casillas[i][j] == simbolo)
                    {
                        contadorSimbolo++;
                    }
                }
                xEnFila[i] = contadorSimbolo;
            }

            for (int i = 0; i < casillas[0].Length; i++) //Analiza combinaciones en el eje Y
            {
                int contadorSimbolo = 0;
                for (int j = 0; j < casillas.Length; j++)
                {
                    if (casillas[j][i] == simbolo)
                    {
                        contadorSimbolo++;
                    }
                }
                xEnColumna[i] = contadorSimbolo;
            }

            for (int i = 0; i < casillas.Length; i++)
            {

                for (int j = 0; j < casillas[i].Length; j++)
                {
                    if (casillas[i][j] == simbolo)
                    {
                        if(i%2 == 0) //Esto verifica si esta en la primera o ultima fila
                        {
                            switch (i)
                            {
                                case 0:
                                    if (j == 0)
                                    {
                                        diagonal1++;
                                    }
                                    else if (j == 2)
                                    {
                                        diagonal2++;
                                    }
                                    break;
                                case 2:
                                    if (j == 0)
                                    {
                                        diagonal2++;
                                    }
                                    else if (j == 2)
                                    {
                                        diagonal1++;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            if (j == 1)
                            {
                                diagonal1++;
                                diagonal2++;
                            }
                        }
                    }
                }
            }



            int[][] tableroPosibilidades = new int[3][];
            tableroPosibilidades[0] = [xEnFila[0], xEnFila[0], xEnFila[0]];
            tableroPosibilidades[1] = [xEnFila[1], xEnFila[1], xEnFila[1]];
            tableroPosibilidades[2] = [xEnFila[2], xEnFila[2], xEnFila[2]];

            for (int i = 0; i < tableroPosibilidades[0].Length; i++)
            {
                if (xEnColumna[i] > 0)
                {
                    tableroPosibilidades[0][i] = xEnColumna[i];
                    tableroPosibilidades[1][i] = xEnColumna[i];
                    tableroPosibilidades[2][i] = xEnColumna[i];
                }

            }


            if (diagonal1 > 0)
            {
                tableroPosibilidades[0][0] = tableroPosibilidades[0][0] < diagonal1 ? diagonal1 : tableroPosibilidades[0][0];
                tableroPosibilidades[1][1] = tableroPosibilidades[1][1] < diagonal1 ? diagonal1 : tableroPosibilidades[1][1];
                tableroPosibilidades[2][2] = tableroPosibilidades[2][2] < diagonal1 ? diagonal1 : tableroPosibilidades[2][2];
            }

            if (diagonal2 > 0)
            {
                tableroPosibilidades[0][2] = tableroPosibilidades[0][2] < diagonal2 ? diagonal2 : tableroPosibilidades[0][2];
                tableroPosibilidades[1][1] = tableroPosibilidades[1][1] < diagonal2 ? diagonal2 : tableroPosibilidades[1][1];
                tableroPosibilidades[2][0] = tableroPosibilidades[2][0] < diagonal2 ? diagonal2 : tableroPosibilidades[2][0];
            }

            return tableroPosibilidades;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach(var el in contenedor.Children)
            {
                if (el is Canvas)
                {
                    Canvas actualCanva = (Canvas)el;
                    Label actualLabel = (Label)actualCanva.Children[0];
                    actualLabel.Content = "";
                }
            }
        }
    }
}