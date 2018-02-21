using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace Outil_Capture_HYDROS
{
    class Trame
    {
        // Propriétés
        byte[] trame;
        byte[] idTotem = new byte[3];
        byte codeFonction;
        byte[] numPaquet = new byte[2];
        byte[,] données = new byte[4,4];

        public string IDTotem;
        public int CodeFonction;
        public int NumPaquet;
        public float[] Data = new float[4];

        // Constructeur
        public Trame(string data)
        {
            string[] temp = data.Replace("[ ", "").Replace(" ]","").Split(' ');
            trame = temp.Select(s => Convert.ToByte(s, 16)).ToArray();

            //Array.Copy(trame, idTotem, 3);
            //codeFonction = trame[3];
            //Array.Copy(trame, 4, numPaquet, 0, 2);

            for (int i = 0, x = 0; i < trame.Length - 16; i++)
            {
                if (i < 3)
                    idTotem[i] = trame[i];
                else if (i == 3)
                    codeFonction = trame[i];
                else if (i > 3 && i < 6)
                    numPaquet[x++] = trame[i];
            }

            int k = trame.Length - 16;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    données[i,j] = trame[k++];
                }
            }

            Decode();
        }

        // Methodes
        private void Decode()
        {
            IDTotem = Encoding.ASCII.GetString(idTotem);
            CodeFonction = Convert.ToInt32(CodeFonction);
            NumPaquet = 256 * numPaquet[0] + numPaquet[1];
            for (int i = 0, j = 0; i < Data.Length; i++)
            {
                var temp = new byte[4];
                Buffer.BlockCopy(données, j,temp, 0, 4);
                Data[i] = BitConverter.ToSingle(temp, 0);
                j += 4;
            }
        }
    }
}
