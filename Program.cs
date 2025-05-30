﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgGenetyczny
{
    internal class Program
    {
        private static Random rand = new Random();

        public static double Przystosowanie(double[] Osobnik, int LiczbaPmNeurona)
        {
            double[][] wejsciesieci = new double[][]
            {
                new double[] { 0, 0 },
                new double[] { 0, 1 },
                new double[] { 1, 0 },
                new double[] { 1, 1 }
            };
            double przystosowanie = 0.0;
            double[] XOR = {0,1,1,0 };
            for (int i = 0; i < wejsciesieci.Length; i++)
            {

                przystosowanie += Math.Pow(XOR[i] - SiecNeuronowa(wejsciesieci[i],Osobnik,LiczbaPmNeurona), 2);
                //Console.WriteLine("Wyjscie sieci dla " + XOR[i] + " : "+ SiecNeuronowa(wejsciesieci[i], Osobnik, LiczbaPmNeurona));
            }
            return przystosowanie;
        }
        public static double SiecNeuronowa(double[] wejscie, double[] Osobnik, int LiczbaPmNeurona)
        {
            double[] neurony = new double[Osobnik.Length/LiczbaPmNeurona];
            double wyjscie = 0.0;
            int y = 0;
            for (int i=0; i<Osobnik.Length/LiczbaPmNeurona;i++)
            {
                double[] PmTymczasowe = new double[LiczbaPmNeurona];    
                for (int j=0; j<LiczbaPmNeurona;j++)
                {
                    PmTymczasowe[j] = Osobnik[y];
                    y++;
                }
                if (i<=1)
                {
                    neurony[i] = FAktywacji(Neuron(PmTymczasowe, wejscie));
                    
                }
                else
                {
                    double[] neuronki = {
                        neurony[i - 1],
                        neurony[i - 2]
                    };
                    neurony[i] = FAktywacji(Neuron(PmTymczasowe, neuronki));
                }
            }
            wyjscie = neurony[(Osobnik.Length / LiczbaPmNeurona) - 1];
            return wyjscie;
        }
        public static double FAktywacji(double WartNeurona, double B = 1.0)
        {
            return 1.0 / (1.0 + Math.Exp(-B * WartNeurona));
        }

        public static double Neuron(double[] Wagi, double[] wejscie)
        {
            double neuron = 0;
    
                for (int j = 0; j < wejscie.Length; j++)
                {
                    neuron += Wagi[j] * wejscie[j];
                }
            neuron += Wagi[Wagi.Length - 1];
            return neuron;
        }
        public static int[][] StworzPule2(int ilosc, int chromosomy)
        {
            //Random rand = new Random();
            int[][] Pula = new int[ilosc][];
            for (int i = 0; i < ilosc; i++)
            {
                int[] osobnikTymczasowy = new int[chromosomy];
                for (int j = 0; j < chromosomy; j++)
                {
                    int bitlosowy = rand.Next(0, 2);
                    osobnikTymczasowy[j] = bitlosowy;
                }
                Pula[i] = osobnikTymczasowy;
            }
            return Pula;
        }
        public static double[][] StworzPule(int ilosc, int lParametrow, int Min, int Max)
        {
            //Random rand = new Random();
            double[][] Pula = new double[ilosc][];
            for (int i = 0; i < ilosc; i++)
            {
                double[] osobnikTymczasowy = new double[lParametrow];
                for (int j = 0; j < lParametrow; j++)
                {
                    double PARAMETRlosowy = Min + (Max - Min) * rand.NextDouble();
                    osobnikTymczasowy[j] = PARAMETRlosowy;
                }
                Pula[i] = osobnikTymczasowy;
            }
            return Pula;
        }
        public static int[] Kodowanie(double pm, float Max, float Min, int liczbaCh)
        {
            float ZD = Max - Min;
            int[] cb = new int[liczbaCh];
            if (pm < Min)
            {
                pm = Min;
            }
            else if (pm > Max)
            {
                pm = Max;
            }
            double ctmp = Math.Round(((pm - Min) / ZD) * (Math.Pow(2, liczbaCh) - 1));
            for (int b = 0; b <= liczbaCh - 1; b++)
            {
                cb[b] = (int)Math.Floor(ctmp / Math.Pow(2, b)) % 2;
            }
            return cb;
        }
        public static double Dekodowanie(int[] cb, float Min, float Max, int LBnP)
        {
            float ZD = Max - Min;
            double ctmp = 0;
            for (int b = 0; b <= LBnP - 1; b++)
            {
                ctmp += cb[b] * Math.Pow(2, b);
            }
            double pm = Min + (ctmp / (Math.Pow(2, LBnP) - 1)) * ZD;
            return pm;
        }
        public static int[] OperatorSelTurniejowej(int[][] pulaOsobnikow, double[] ocenaOsobnikow)
        {
            int RozmiarTurnieju = 3;
            int[] skladTurnieju = new int[RozmiarTurnieju];
            //Random rand = new Random();
            for (int i = 0; i < RozmiarTurnieju; i++)
            {
                skladTurnieju[i] = rand.Next(pulaOsobnikow.Length);
            }

            int najI = skladTurnieju[0];
            for (int i = 1; i < RozmiarTurnieju; i++)
            {
                if (ocenaOsobnikow[skladTurnieju[i]] < ocenaOsobnikow[najI])
                {
                    najI = skladTurnieju[i];
                }
            }
            return pulaOsobnikow[najI].ToArray();
        }
        public static (int[], int[]) OperatorKrzyżowania(int[] cbr1, int[] cbr2)
        {
            int LBnOs = cbr1.Length;
            // Random rand = new Random();
            int bCiecie = rand.Next(0, LBnOs - 2);
            //Console.WriteLine("Wylosowane miejsce przeciecia: " + bCiecie);
            int[] cbp1 = new int[cbr1.Length];
            int[] cbp2 = new int[cbr2.Length];
            for (int i = 0; i < bCiecie; i++)
            {
                cbp1[i] = cbr1[i];
                cbp2[i] = cbr2[i];
            }
            for (int j = bCiecie; j < LBnOs; j++)
            {
                cbp1[j] = cbr2[j];
                cbp2[j] = cbr1[j];
            }
            return (cbp1, cbp2);
        }
        public static int[] OperatorMutacji(int[] cb)
        {
            int LBnOs = cb.Length;
            //Random rand = new Random();
            int bPunkt = rand.Next(0, LBnOs - 1);
            // Console.WriteLine("Wylosowany punkt mutacji: " + bPunkt);
            int[] cbwy = new int[LBnOs];
            for (int i = 0; i < LBnOs; i++)
            {
                cbwy[i] = cb[i];
            }
            if (cbwy[bPunkt] == 0)
            {
                cbwy[bPunkt] = 1;
            }
            else if (cbwy[bPunkt] == 1)
            {
                cbwy[bPunkt] = 0;
            }
            return cbwy;
        }
        public static int[] OperatorHotDeck(int[][] pulaOsobnikow, double[] ocenaOsobnikow)
        {
            int najlepszy = 0;
            for (int i = 1; i < pulaOsobnikow.Length; i++)
            {
                if (ocenaOsobnikow[i] < ocenaOsobnikow[najlepszy])
                {
                    najlepszy = i;
                }
            }
            return pulaOsobnikow[najlepszy].ToArray();
        }
        static void Main(string[] args)
        {

            int LBnP = 4;
            int lWag = 9;
            int lChromosomow = LBnP * lWag;
            int liczbaParametrowNeurona = 3;
            int lOsobnikow = 30;
            float Min = -10;
            float Max = 10;
            int[][] Pula = StworzPule2(lOsobnikow , lChromosomow);
            double[][] PulaDekodowana = new double[Pula.Length][];
            int[] Bityparametrtymczasowy = new int[Pula[0].Length / lWag];
            double[] przystosowanie = new double[lOsobnikow];
            for (int i = 0; i < Pula.Length; i++)
            {
                double[] ParametryTymczasowe = new double[lWag];
                int y = 0;
                for (int z = 0; z < lWag; z++)
                {
                    for (int j = 0; j < Pula[0].Length / lWag; j++)
                    {
                        Bityparametrtymczasowy[j] = Pula[i][y];
                        y++;
                    }
                    ParametryTymczasowe[z] = Dekodowanie(Bityparametrtymczasowy, Min, Max, LBnP);
                }
                PulaDekodowana[i] = ParametryTymczasowe;
                przystosowanie[i] = Przystosowanie(ParametryTymczasowe, liczbaParametrowNeurona);
            }
            /*for (int i = 0; i < Pula.Length; i++)
            {
                for (int j = 0; j < PulaDekodowana[0].Length;j++)
                {
                    Console.Write(PulaDekodowana[i][j] + " ");
                }
                Console.WriteLine();
            }
            */
            Console.WriteLine("Srednia przystosowania pierwotnej puli: " + przystosowanie.Average());
            Console.WriteLine("Najlepsze przystosowanie w pierwotnej puli: " + przystosowanie.Min());
            
            
            for (int i = 0; i<100; i++)
            {
                int[][] NowaPula = new int[Pula.Length][];
                for (int j = 0; j < Pula.Length - 1; j++)
                {
                    NowaPula[j] = OperatorSelTurniejowej(Pula,przystosowanie);
                }
                (NowaPula[0], NowaPula[1]) = OperatorKrzyżowania(NowaPula[0], NowaPula[1]);
                (NowaPula[2], NowaPula[3]) = OperatorKrzyżowania(NowaPula[2], NowaPula[3]);
                (NowaPula[8], NowaPula[9]) = OperatorKrzyżowania(NowaPula[8], NowaPula[9]);
                (NowaPula[NowaPula.Length - 3], NowaPula[NowaPula.Length - 2]) = OperatorKrzyżowania(NowaPula[NowaPula.Length - 3], NowaPula[NowaPula.Length - 2]);
                for (int j = 4; j < NowaPula.Length - 1; j++)
                {
                    NowaPula[j] = OperatorMutacji(NowaPula[j]);
                }
                NowaPula[NowaPula.Length - 1] = OperatorHotDeck(Pula, przystosowanie);


                double[][] nowaPulaDekodowana = new double[NowaPula.Length][];
                int[] chromosomytmp = new int[Pula[0].Length / lWag];
                double[] noweprzystosowanie = new double[NowaPula.Length];
                for (int k = 0; k < NowaPula.Length; k++)
                {
                    double[] ocenaTymczasowa = new double[lWag];
                    int y = 0;
                    for (int z = 0; z < lWag; z++)
                    {
                        for (int j = 0; j < NowaPula[0].Length / lWag; j++)
                        {
                            chromosomytmp[j] = NowaPula[k][y];
                            y++;
                        }
                        ocenaTymczasowa[z] = Dekodowanie(chromosomytmp, Min, Max, LBnP);
                    }
                    nowaPulaDekodowana[k] = ocenaTymczasowa;
                    noweprzystosowanie[k] = Przystosowanie(ocenaTymczasowa,liczbaParametrowNeurona);
                }
                Console.WriteLine("Srednia przystosowania w" +(i+1)+ "puli: " + noweprzystosowanie.Average());
                Console.WriteLine("Najlepsze przystosowanie w "+(i+1)+" puli: " + noweprzystosowanie.Min());
                Pula = NowaPula.Select(a => a.ToArray()).ToArray();
                przystosowanie = noweprzystosowanie.ToArray();

            }
            int[] Najsiec = OperatorHotDeck(Pula, przystosowanie);
            double[] najlepszeWagi = new double[lWag];
            int[] chromosomyt = new int[Pula[0].Length / lWag];
            int y2 = 0;
            for (int z = 0; z < lWag; z++)
            {
                for (int j = 0; j < Pula[0].Length / lWag; j++)
                {
                    chromosomyt[j] = Najsiec[y2];
                    y2++;
                }
                najlepszeWagi[z] = Dekodowanie(chromosomyt, Min, Max, LBnP);
            }
            Console.WriteLine("Najlesze wagi:");
            Console.WriteLine("pierwszy neuron (warstwa ukryta) = " + najlepszeWagi[0]+" , "+ najlepszeWagi[1] + " , " + najlepszeWagi[2]);
            Console.WriteLine("drugi neuron (warstwa ukryta) = " + najlepszeWagi[3] + " , " + najlepszeWagi[4] + " , " + najlepszeWagi[5]);
            
            Console.WriteLine("ostatni neuron (warstwa wyjciowa) = " + najlepszeWagi[6] + " , " + najlepszeWagi[7] + " , " + najlepszeWagi[8]);
            double[] wejscie = new double[2];
            Console.WriteLine("Testowanie sieci neuronowej : ");
            Console.WriteLine("Podaj wartość 0/1 dla piwerszego wejścia XOR (ZATWIERDZ ENTEREM):");
            wejscie[0]=Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Podaj wartość 0/1 dla drugiego wejścia XOR (ZATWIERDZ ENTEREM):");
            wejscie[1] = Convert.ToDouble(Console.ReadLine());
            double wyjscie = SiecNeuronowa(wejscie, najlepszeWagi, liczbaParametrowNeurona);
            Console.WriteLine("Wyjscie sieci: "+wyjscie);
            Console.ReadKey();
        }
    }
}
