using System;
using System.Collections.Generic;
using System.IO;

namespace DeEncrypter
{
    public static class RSA
    {
        private static int P { get; set; }
        private static int Q { get; set; }
        private static int N { get; set; }
        private static int Z { get; set; }
        private static int E { get; set; }
        private static int D { get; set; }
        private static int ExpMod(int a, int b, int n)
        {
            int res = 1;
            int x = a % n;
            int my_b = b;
            while (my_b > 0)
            {
                if (my_b % 2 != 0) res = (res * x) % n;
                x = (x * x) % n;
                my_b /= 2;
            }
            return res;
        }
        private static int Congruencia(int a, int b, int n)
        {
            int i = 0;
            do
            {
                if ((a * i - b) % n == 0) return i;
                ++i;
            } while (true);
        }
        private static int MCD(int a, int b)
        {
            if (a % b != 0) return MCD(b, a % b);
            else return b;
        }
        private static bool EsPrimo(int a)
        {
            for (int i = 2; i < a; ++i)
            {
                if (a % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
        public static int[] GenerarLLave()
        {
            Random gen = new Random();
            int[] res = new int[3];
            do
            {
                P = gen.Next(23, 49);
            } while (!EsPrimo(P));
            do
            {
                Q = gen.Next(23, 49);
            } while (!EsPrimo(Q) || P == Q);
            N = P * Q;
            res[2] = N;
            Z = (P - 1) * (Q - 1);
            do
            {
                E = gen.Next(51, Z - 1);
            } while (MCD(E, Z) != 1);
            res[0] = E;
            D = Congruencia(E, 1, Z);
            res[1] = D;
            return res;
        }
        public static void EncriptarArchivo(int E, int N, string pathIN, string pathOUT)
        {
            byte[] bytesPathIN = File.ReadAllBytes(pathIN);
            using (FileStream f = File.Create(pathOUT))
            {
                int aux;
                foreach (byte i in bytesPathIN)
                {
                    aux = ExpMod(i, E, N);
                    f.WriteByte((byte)(aux >> 8));
                    f.WriteByte((byte)aux);
                }
            }
        }
        public static void DesencriptarArchivo(int D, int N, string pathIN, string pathOUT)
        {
            byte[] bytesPathIN = File.ReadAllBytes(pathIN);
            using (FileStream f = File.Create(pathOUT))
            {
                int aux;
                for (int i = 0; ; i += 2)
                {
                    try
                    {
                        aux = ExpMod(bytesPathIN[i] * 256 + bytesPathIN[i + 1], D, N);
                        f.WriteByte((byte)aux);
                    }
                    catch
                    {
                        try
                        {
                            aux = ExpMod(bytesPathIN[i] * 256, D, N);
                            f.WriteByte((byte)aux);
                        }
                        catch
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}