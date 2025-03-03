namespace PruebaPDF.Modelos
{

    public class Imagen
    {
        public List<Posicion> Posiciones { get; }

        private int _ancho;

        private int _alto;

        private string _ruta;

        public int Ancho
        {
            get => _ancho;

            private set
            {
                if (value < 1)
                    throw new ArgumentException("El ancho de la imagen debe ser mayor a 0");

                this._ancho = value;
            }
        }

        public int Alto
        {
            get => _alto;

            private set
            {
                if (value < 1)
                    throw new ArgumentException("El alto de la imagen debe ser mayor a 0");

                this._alto = value;
            }
        }

        public string Ruta
        {
            get => _ruta;

            private set
            {
                if (!File.Exists(value))
                    throw new ArgumentException("Ruta de imagen invalida");

                this._ruta = value;
            }
        }

        public Imagen(int ancho, int alto, string ruta)
        {
            this.Ancho = ancho;
            this.Alto = alto;
            this.Ruta = ruta;
            this.Posiciones = new List<Posicion>();
        }

        /// <summary>
        /// Agrega las posiciones en las que se dibujara la imagen.
        /// </summary>
        /// <param name="nuevaPosicion"></param>
        public void AgregarPosicion(Posicion nuevaPosicion) => this.Posiciones.Add(nuevaPosicion);
    }
}
