using System;

namespace Discrete.Random.Matrix.Generator
{
    public static class ShufflerHelper
    {
        private static System.Random _random = new System.Random();

        /// <summary>
        /// Shuffle the array.
        /// </summary>
        /// <typeparam name="T">Array element type.</typeparam>
        /// <param name="array">Array to shuffle.</param>
        public static void Shuffle<T>(ref T[] array)
            where T : class
        {
            int n = array.Length;
            for (int i = 0; i < (n - 1); i++)
            {
                // Use Next on random instance with an argument.
                // ... The argument is an exclusive bound.
                //     So we will not go past the end of the array.
                int r = i + _random.Next(n - i);

                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }

        public static void ShuffleWithNeighberhood(ref int[] array, int rowsAmount, int columnsAmount, bool isStrict = false)
        {
            //  when close to the limit or the last iteration just look for it across the matrix
            var retryLimit = 10;

            int n = array.Length;
            for (int i = 0; i < (n - 1); i++)
            {
                if (IsSourceNeighebrhoodOK(ref array, i, array[i], rowsAmount, columnsAmount, isStrict))
                    continue;

                var retryAttempt = 1;
                var success = false;
                do
                {
                    // random
                    int r = i + _random.Next(n - i);
                    //check neighberhood for target
                    var ok1 = IsSourceNeighebrhoodOK(ref array, i, array[r], rowsAmount, columnsAmount, isStrict);
                    //check neighberhood for source
                    var ok2 = IsTargetNeighebrhoodOK(ref array, r, array[i], rowsAmount, columnsAmount, isStrict);

                    if (ok1 && ok2)
                    {
                        int t = array[r];
                        array[r] = array[i];
                        array[i] = t;
                        success = true;
                    }
                    else if (retryAttempt == (retryLimit - 1)) // close to the limit
                    {
                        //look for it without random
                        for(int iii = 0; iii < array.Length; iii++)
                        {
                            if (iii == i) continue;
                            var okk1 = IsSourceNeighebrhoodOK(ref array, i, array[iii], rowsAmount, columnsAmount, isStrict);
                            //check neighberhood for source
                            var okk2 = IsTargetNeighebrhoodOK(ref array, iii, array[i], rowsAmount, columnsAmount, isStrict);

                            if (okk1 && okk2)
                            {
                                int tt = array[iii];
                                array[iii] = array[i];
                                array[i] = tt;
                                success = true;
                                break;
                            }
                        }
                    }
                    else if (retryAttempt == retryLimit) // random for the last time. it ensures success
                    {
                        int t = array[r];
                        array[r] = array[i];
                        array[i] = t;
                        success = true;
                    }
                    else
                    {
                        retryAttempt = retryAttempt + 1;
                    }
                } while (!success);
            }
        }

        private static bool IsSourceNeighebrhoodOK(
            ref int[] array,
            int sourceIndex,
            int sourceValue,
            int nn,
            int mm,
            bool isStrict = false)
        {
            return IsNeighebrhoodOK(ref array, sourceIndex, sourceValue, nn, mm, isStrict);
        }

        private static bool IsTargetNeighebrhoodOK(
            ref int[] array,
            int targetIndex,
            int targetValue,
            int nn,
            int mm,
            bool isStrict = false)
        {
            return IsNeighebrhoodOK(ref array, targetIndex, targetValue, nn, mm, isStrict);
        }

        private static bool IsNeighebrhoodOK(
            ref int[] array,
            int specifiedIndex,
            int specifiedValue,
            int rowsAmount,
            int columnsAmount,
            bool isStrict = false)
        {
            if (rowsAmount <= 2 && columnsAmount <= 2) return true;

            //numberOfRow starts with 0
            var numberOfRow = (int)Math.Floor(((double)specifiedIndex) / (double)columnsAmount);
            // indexInRow starts with 0
            var indexInARow = specifiedIndex - numberOfRow * columnsAmount;

            //columnsAmount is a distance between current and previous/next neighberhood row value(top, bottom)
            // 1 is a distance between current and previous/next neighebrhood column value(left, right)
            //rowsAmount is a vertical localizator of top and bottom
            //columnsAmount is a horizontal localizator of left and right

            if (numberOfRow == rowsAmount - 1)// isLastRow
            {
                //check top index only index - mm
                if (array[specifiedIndex - columnsAmount] == specifiedValue)
                {
                    return false;
                }
            }
            else if (numberOfRow == 0)// isFirstRow
            {
                //check bottom index only index + mm
                if (array[specifiedIndex + columnsAmount] == specifiedValue)
                {
                    return false;
                }
            }
            else
            {
                //check top index -> index - mm
                //check bottom index -> index + mm
                if ((isStrict && (array[specifiedIndex + columnsAmount] == specifiedValue ||
                    array[specifiedIndex - columnsAmount] == specifiedValue)) || 
                    (!isStrict && (array[specifiedIndex + columnsAmount] == specifiedValue &&
                    array[specifiedIndex - columnsAmount] == specifiedValue)))
                {
                    return false;
                }
            }

            if (indexInARow == columnsAmount-1)
            {
                //check left index only
                if (array[specifiedIndex - 1] == specifiedValue)
                {
                    return false;
                }
            }
            else if(indexInARow == 0)
            {
                //check left index only
                if (array[specifiedIndex + 1] == specifiedValue)
                {
                    return false;
                }
            }
            else
            {
                //check left index
                //check right index
                if ((isStrict && (array[specifiedIndex + 1] == specifiedValue ||
                    array[specifiedIndex - 1] == specifiedValue))||
                    !isStrict && (array[specifiedIndex + 1] == specifiedValue &&
                    array[specifiedIndex - 1] == specifiedValue))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
