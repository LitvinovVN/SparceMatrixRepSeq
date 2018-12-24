using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    /// <summary>
    /// Конвертеры данных
    /// </summary>
    public static class Converters
    {
        /// <summary>
        /// Выполняет преобразование из double в byte[]
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static byte[] ConvertDoubleToByteArray(double d)
        {
            return BitConverter.GetBytes(d);
        }

        /// <summary>
        /// Выполняет преобразование из double[] в byte[]
        /// </summary>
        /// <param name="tempData"></param>
        /// <returns></returns>
        public static byte[] ConvertDoubleArrayToByteArray(double[] tempData)
        {
            byte[] result = new byte[tempData.Length * sizeof(double)];
            for (int i = 0; i < tempData.Length; i++)
            {
                int offset = i * sizeof(double);
                byte[] curElementBytes = BitConverter.GetBytes(tempData[i]);
                for (int j = 0; j < curElementBytes.Length; j++)
                {
                    result[offset + j] = curElementBytes[j];
                }
            }
            return result;
        }

        /// <summary>
        /// Выполняет преобразование из int в byte[]
        /// </summary>
        /// <param name="dataIndex"></param>
        /// <returns></returns>
        public static byte[] ConvertIntToByteArray(int dataIndex)
        {
            return BitConverter.GetBytes(dataIndex);
        }

        /// <summary>
        /// Выполняет преобразование из int[] в byte[]
        /// </summary>
        /// <param name="tempData"></param>
        /// <returns></returns>
        public static byte[] ConvertIntArrayToByteArray(int[] tempData)
        {
            byte[] result = new byte[tempData.Length * sizeof(int)];
            for (int i = 0; i < tempData.Length; i++)
            {
                int offset = i * sizeof(int);
                byte[] curElementBytes = BitConverter.GetBytes(tempData[i]);
                for (int j = 0; j < curElementBytes.Length; j++)
                {
                    result[offset + j] = curElementBytes[j];
                }
            }
            return result;
        }

        /// <summary>
        /// Выполняет преобразование из byte[] в double
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static double ConvertByteArrayToDouble(byte[] bytes)
        {
            return BitConverter.ToDouble(bytes, 0);
        }

        /// <summary>
        /// Выполняет преобразование из byte[] в double[]
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static double[] ConvertByteArrayToDoubleArray(byte[] bytes)
        {
            int numElements = bytes.Length / sizeof(double);
            double[] result = new double[numElements];
            for (int i = 0; i < numElements; i++)
            {
                int offset = i * sizeof(double);
                var curByte = new byte[]
                {
                    bytes[offset+0],
                    bytes[offset+1],
                    bytes[offset+2],
                    bytes[offset+3],
                    bytes[offset+4],
                    bytes[offset+5],
                    bytes[offset+6],
                    bytes[offset+7]
                };
                result[i] = BitConverter.ToDouble(curByte, 0);
            }
            return result;
        }

        /// <summary>
        /// Выполняет преобразование из byte[] в int[]
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int[] ConvertByteArrayToIntArray(byte[] bytes)
        {
            int numElements = bytes.Length / sizeof(int);
            int[] result = new int[numElements];
            for (int i = 0; i < numElements; i++)
            {
                int offset = i * sizeof(int);
                var curByte = new byte[]
                {
                    bytes[offset+0],
                    bytes[offset+1],
                    bytes[offset+2],
                    bytes[offset+3]                    
                };
                result[i] = BitConverter.ToInt32(curByte, 0);
            }
            return result;
        }

        /// <summary>
        /// Выполняет преобразование из byte[] в int
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int ConvertByteArrayToInt(byte[] bytes)
        {
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
