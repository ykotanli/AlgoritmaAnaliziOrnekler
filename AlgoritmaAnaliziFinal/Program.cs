
//Count Sort Radix Sort Örnek

int[] CountingSortWithNegatives(int[] array)
{
    int min = array.Min();
    int max = array.Max();
    int[] count = new int[max-min+1];
    for (int i = 0; i < array.Length; i++)
        count[array[i]-min]++;
    for (int i = 1; i < count.Length; i++)
        count[i] += count[i - 1];
    int[] output = new int[array.Length];
    for (int i = array.Length - 1; i >= 0; i--)
    {
        count[array[i]-min]--;
        output[count[array[i]]-min] = array[i];
    }

    return output;
}


int[] CountingSortByDigits(int[] array, int exp)
{
    int[] count = new int[10];
    for (int i = 0; i < array.Length; i++)
    {
        count[(array[i] / exp) % 10]++;
    }

    for (int i = 1; i < count.Length; i++)
        count[i] += count[i - 1];
    int[] output = new int[array.Length];
    for (int i = array.Length - 1; i >= 0; i--)
    {
        count[(array[i] / exp) % 10]--;
        output[count[(array[i] / exp) % 10]] = array[i];
    }

    return output;
}

int[] RadixSortTest(int[] array)
{
    for (int exp = 1; array.Max() / exp > 0; exp++)
        array = CountingSortByDigits(array, exp);
    return array;
}




//--------------------------------------------------------------------------------
//Greedy



void Swap(int[] weights, int[] prices, int i, int j) // Ne olur ne olmaz sort kullandırmazsa mecbur böyle yazcaz.
{
    int tempWeight = weights[i];
    weights[i] = weights[j];
    weights[j] = tempWeight;

    
    int tempPrice = prices[i];
    prices[i] = prices[j];
    prices[j] = tempPrice;
}
int KnapsackGreedy(int[] weights, int[] prices, int capacity)
{
    int n = weights.Length;
    for (int i = 0; i < n-1; i++)
    {
        for (int j = 0; j < n-i-1; j++)
        {
            double ratio1 = (double)prices[j] / weights[j];
            double ratio2 = (double)prices[j + 1] / weights[j + 1];
            if(ratio2 > ratio1)
                Swap(weights,prices,j,j+1);
        }
    }


    int totalPrice = 0;
    int remainingCapacity = capacity;
    for (int i = 0; i < weights.Length; i++)
    {
        if (weights[i] > remainingCapacity) break;
        totalPrice += prices[i];
        remainingCapacity -= weights[i];
    }

    return totalPrice;
}

int ActivitySelectionGreedy(int[] startTimes, int[] endTimes)
{
    int n = startTimes.Length;
    for (int i = 0; i < n - 1; i++)
    {
        for (int j = 0; j < n - i - 1; j++)
        {
            if (endTimes[j + 1] < endTimes[j])
            {
                Swap(endTimes,startTimes,j,j+1);
            }
        }
    }

    int activityCount = 1;
    int lastEnd = endTimes[0];
    for (int i = 1; i < n; i++)
    {
        if (startTimes[i] > lastEnd)
        {
            activityCount++;
            lastEnd = endTimes[i];
        }
    }

    return activityCount;
}

int JobSchedulingGreedy(int[] profits, int[] deadLines)
{
    int n = profits.Length;
    for (int i = 0; i < n - 1; i++)
    {
        for (int j = 0; j < n - i - 1; j++)
        {
            if(profits[j+1]>profits[j])
                Swap(profits,deadLines,j,j+1);
        }
    }

    bool[] slots = new bool[deadLines.Max()+1];
    int profitSum = 0;

    for (int i = 0; i < n; i++)
    {

        for (int j = deadLines[i]; j > 0; j--)
        {
            if (!slots[j])
            {
                slots[j] = true;
                profitSum += profits[i];
                break;
            }
        }
    }

    return profitSum;

}


//-----------------------------------------------------------------------------------
//Brute Force

int MaxLengthofSubstringBruteForceLength(string s1, string s2)
{
    int n = s1.Length;
    int m = s2.Length;
    int maxLen = 0;

    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < m; j++)
        {
            int k = 0;
            while (i + k <= n && j + k <= m && s1[i + k] == s2[j + k])
                k++;

            if (k > maxLen)
                maxLen = k;

        }
    }

    return maxLen;
}


int KnapsackBruteForce(int[] weights, int[] prices,int capacity)
{
    int maxValue = 0;
    int n = weights.Length;

    for (int i = 0; i < (1 << n); i++)
    {
        int totalValue = 0;
        int totalWeight = 0;

        for (int j = 0; j < n; j++)
        {
            if ((i & (1 << j))!=1)
            {
                totalWeight += weights[j];
                totalValue += prices[j];
            }
        }

        if (totalWeight <= capacity && totalValue > maxValue)
            maxValue = totalValue;
    }

    return maxValue;
}


//-----------------------------------------------------------------
//Dynamic Programming




int KnapsackDp(int[] weights, int[] values, int capacity)
{
    int n = weights.Length;
    int[,] dp = new int[n + 1, capacity + 1];

    

    for (int i = 1; i <= n; i++)
    {
        for (int w = 0; w <= capacity; w++)
        {
            if (weights[i - 1] <= w)
            {
                
                dp[i, w] = Math.Max(
                    dp[i - 1, w], 
                    dp[i - 1, w - weights[i - 1]] + values[i - 1] 
                );
            }
            else
            {
                
                dp[i, w] = dp[i - 1, w];
            }
        }
    }

    return dp[n, capacity]; 
    
}


int KnapsackDp(int[] weights, int[] prices, int capacity)
{
    int n = prices.Length;
    int[,] dp = new int[n + 1, capacity + 1];

    for (int i = 1; i <= n; i++)
    {
        for (int w = 0; w <= capacity; w++)
        {
            if (weights[i - 1] <= w)
            {
                dp[i, w] = Math.Max(dp[i - 1, w], dp[i - 1, w - weights[i - 1]] + prices[i - 1]);
            }
            else
            {
                dp[i, w] = dp[i - 1, w];
            }
        }
    }

    return dp[n, capacity];

}




int CoinChangeDp(int[] coins, int target)
{
    int[] dp = new int[target+1];
    Array.Fill(dp,int.MaxValue);
    dp[0] = 0;
    for (int i = 1; i <= target; i++)
    {
        foreach (var coin in coins)
        {
            if (i >= coin)
            {
                dp[i] = Math.Min(dp[i], 1 + dp[i - coin]);
            }
            
        }
    }

    return dp[target] == int.MaxValue ? -1 : dp[target];
}



int MinCostClimbingStairsDpTest(int[] costs)
{
    int n = costs.Length;
    if (n == 0) return 0;
    if (n == 1) return costs[0];
    int[] dp = new int[costs.Length];
    dp[0] = costs[0];
    dp[1] = costs[1];
    for (int i = 2; i < costs.Length; i++)
    {
        dp[i] = costs[i] + Math.Min(dp[i - 1], dp[i - 2]);
    }

    return Math.Min(dp[costs.Length - 1],dp[costs.Length-2]);
}

int CatalanDpTest(int n)
{
    int[] dp = new int[n + 1];
    dp[0] = 1;
    dp[1] = 1;
    for (int i = 3; i <= n; i++)
    {
        dp[i] = 0;
        for (int j = 0; j < i; j++)
        {
            dp[i] += dp[j] * dp[i - j - 1];
        }
    }

    return dp[n];
}




//----------------------------------------------------------------------------------
// Dp-Brute force donusumler belki sorar hoca


int CatalanDpToBruteForce(int n)
{
    if (n <= 1) return 1;
    int result = 0;
    for (int i = 0; i <= n; i++)
    {
        result += CatalanDpToBruteForce(i) * CatalanDpToBruteForce(n - i - 1);
    }

    return result;
}


int TribonacciDpToBruteForce(int n)
{
    if (n == 0) return 0;
    if (n == 1 || n == 2) return 1;
    return TribonacciDpToBruteForce(n - 1) + TribonacciDpToBruteForce(n - 2) + TribonacciDpToBruteForce(n - 3);
}



//----------------------------------------------------------Denemeler----------------------------------------





//------------------------------------------------BENCE ÇIKACAKLAR----------------------------------------------------------------



//Longest Common String --->   Hoca brute force unu vermişti dp sini sorabilir.
int LCS(string s1, string s2)
{
    int n = s1.Length, m = s2.Length;
    int[,] dp = new int[n + 1, m + 1];

    for (int i = 1; i <= n; i++)
    {
        for (int j = 1; j <= m; j++)
        {
            if (s1[i - 1] == s2[j - 1])
                dp[i, j] = dp[i - 1, j - 1] + 1;
            else
                dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]);
        }
    }

    return dp[n, m];
}

// CoinChange in kaç farklı şekilde olanı çıkar bence
int CountCoinCombinations(int[] coins, int amount)
{
    int[] dp = new int[amount + 1];
    dp[0] = 1;

    foreach (int coin in coins)
    {
        for (int i = coin; i <= amount; i++)
        {
            dp[i] += dp[i - coin];
        }
    }
    return dp[amount];
}




// Varyasyonunu vermiş hoca bunu sorabilir.
int ClimbStairs(int n)
{
    if (n <= 2) return n;

    int[] dp = new int[n + 1];
    dp[1] = 1;
    dp[2] = 2;

    for (int i = 3; i <= n; i++)
        dp[i] = dp[i - 1] + dp[i - 2];

    return dp[n];
}



// MinCostClimbingStairs e çok benziyo sorma ihtimali var.
int HouseRobber(int[] nums)
{
    if (nums.Length == 0) return 0;
    if (nums.Length == 1) return nums[0];

    int[] dp = new int[nums.Length];
    dp[0] = nums[0];
    dp[1] = Math.Max(nums[0], nums[1]);

    for (int i = 2; i < nums.Length; i++)
        dp[i] = Math.Max(dp[i - 1], dp[i - 2] + nums[i]);

    return dp[nums.Length - 1];
}



// Hoca kesirsizini vermiş bunu yazmamızı isteyebilir Greedy ile optimum bulunuyo digerinde bulunmuyo.
int KnapsackGreedyWithFraction(int[] weights, int[] prices, int capacity)
{
    int n = weights.Length;
    for (int i = 0; i < n - 1; i++)
    {

        for (int j = 0; j < n - i - 1; j++)
        {
            double ratio1 = (double)prices[j] / weights[j];
            double ratio2 = (double) prices[j + 1] / weights[j + 1];
            if (ratio2 > ratio1)
            {
                Swap(weights,prices,j,j+1);
            }
        }
    }

    double totalProfit = 0;
    double remainingCapacity = capacity;

    for (int i = 0; i < n; i++)
    {
        if (weights[i] <= remainingCapacity)
        {
            totalProfit += prices[i];
            remainingCapacity -= weights[i];
        }
        else // KESİRLİ ALABİLDİĞİMİZ VERSİYON
        {
            double fraction = remainingCapacity / weights[i]; //kalan kapasiteyi eşyaya oranlıyoruz
            totalProfit = prices[i] * fraction; // ağırlığın oranında değeri toplama ekliyoruz.
            break;
        }
    }

    return (int)totalProfit;
}



// Vizede bunun analizini yaptirmisti belki yazdırır.
int TribonacciDp(int n)
{
    if (n == 0) return 0;
    if (n == 1 || n == 2) return 1;
    int[] dp = new int[n+1];
    dp[0] = 0;
    dp[1] = 1;
    dp[2] = 1;
    for (int i = 3; i <= n; i++)
    {
        dp[i] = dp[i - 1] + dp[i - 2] + dp[i - 3];
    }

    return dp[n];

}


// Çıkma ihtimali olabilen bir algoritma diğerlerine göre daha düşük ihtimal ---> ardışık olmayan elemanlar altkümesi
// targeti verebiliyo mu onu kontrol ediyo.
bool SubsetSum(int[] nums, int target)
{
    bool[,] dp = new bool[nums.Length + 1, target + 1];
    for (int i = 0; i <= nums.Length; i++) dp[i, 0] = true;

    for (int i = 1; i <= nums.Length; i++)
    {
        for (int j = 1; j <= target; j++)
        {
            if (nums[i - 1] > j)
                dp[i, j] = dp[i - 1, j];
            else
                dp[i, j] = dp[i - 1, j] || dp[i - 1, j - nums[i - 1]];
        }
    }

    return dp[nums.Length, target];
}






//--------------------------------------------Donusumler------------------------------------------------------------------------




















































