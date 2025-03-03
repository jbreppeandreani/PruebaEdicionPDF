namespace PruebaPDF.Modelos
{
    public class Posicion
    {
        public int Pagina { get; }

        public int X { get; }

        public int Y { get; }

        public Posicion(int pagina, int x, int y)
        {
            this.Pagina = pagina;
            this.X = x;
            this.Y = y;
        }
    }
}
