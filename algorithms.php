<?php

function LCS($s1, $s2)
{
    $n = strlen($s1);
    $m = strlen($s2);
    $dp = array_fill(0, $n + 1, array_fill(0, $m + 1, 0));

    for ($i = 1; $i <= $n; $i++) {
        for ($j = 1; $j <= $m; $j++) {
            if ($s1[$i - 1] == $s2[$j - 1])
                $dp[$i][$j] = $dp[$i - 1][$j - 1] + 1;
            else
                $dp[$i][$j] = max($dp[$i - 1][$j], $dp[$i][$j - 1]);
        }
    }

    return $dp[$n][$m];
}

function CountCoinCombinations($coins, $amount)
{
    // dp[i] = i tutarını kaç farklı şekilde elde edebileceğimizi tutar
    $dp = array_fill(0, $amount + 1, 0);
    $dp[0] = 1; // 0 miktar için sadece "hiçbir şey almamak" bir yoldur

    foreach ($coins as $coin) {
        for ($i = $coin; $i <= $amount; $i++) {
            $dp[$i] += $dp[$i - $coin];
        }
    }

    return $dp[$amount]; // Toplam "amount" miktarına ulaşmak için kaç kombinasyon var
}

function CoinChange($coins, $amount)
{
    // Başlangıçta tüm değerleri sonsuz yap, çünkü hedefimiz minimumu bulmak
    $dp = array_fill(0, $amount + 1, PHP_INT_MAX);
    $dp[0] = 0; // 0 miktar = 0 coin

    for ($i = 1; $i <= $amount; $i++) {
        foreach ($coins as $coin) {
            if ($i >= $coin && $dp[$i - $coin] != PHP_INT_MAX) {
                $dp[$i] = min($dp[$i], $dp[$i - $coin] + 1);
            }
        }
    }

    return $dp[$amount] == PHP_INT_MAX ? -1 : $dp[$amount];
}

// Vizede bunun analizini yaptirmisti belki yazdırır.
function TribonacciDp($n)
{
    if ($n == 0) return 0;
    if ($n == 1 || $n == 2) return 1;

    $dp = array_fill(0, $n + 1, 0);
    $dp[0] = 0;
    $dp[1] = 1;
    $dp[2] = 1;

    for ($i = 3; $i <= $n; $i++) {
        $dp[$i] = $dp[$i - 1] + $dp[$i - 2] + $dp[$i - 3];
    }

    return $dp[$n];
}

// ==== ALGORITHM ÖRNEKLERİ ====
// Brute Force, Greedy, DP, Dijkstra ve A* örnekleri


// ---------------- Brute Force: Subset Sum ----------------
function SubsetSumBruteForce($nums, $target, $index = 0, $sum = 0)
{
    if ($index == count($nums)) {
        return $sum == $target;
    }
    return SubsetSumBruteForce($nums, $target, $index + 1, $sum + $nums[$index]) ||
        SubsetSumBruteForce($nums, $target, $index + 1, $sum);
}

// Test
$nums = [3, 34, 4, 12, 5, 2];
$target = 9;
echo "Brute Force Subset Sum: " . (SubsetSumBruteForce($nums, $target) ? "Yes" : "No") . "\n";



// ---------------- Greedy: Activity Selection ----------------
function ActivitySelectionGreedy($start, $end)
{
    $n = count($start);
    $activities = [];
    for ($i = 0; $i < $n; $i++) {
        $activities[] = ["start" => $start[$i], "end" => $end[$i]];
    }

    usort($activities, function ($a, $b) {
        return $a["end"] - $b["end"];
    });

    $count = 1;
    $lastEnd = $activities[0]["end"];
    for ($i = 1; $i < $n; $i++) {
        if ($activities[$i]["start"] > $lastEnd) {
            $count++;
            $lastEnd = $activities[$i]["end"];
        }
    }

    return $count;
}

// Test
$start = [1, 3, 0, 5, 8, 5];
$end =   [2, 4, 6, 7, 9, 9];
echo "Greedy Activity Selection: " . ActivitySelectionGreedy($start, $end) . "\n";



// ---------------- Dynamic Programming: Coin Change ----------------
function CoinChange($coins, $amount)
{
    $dp = array_fill(0, $amount + 1, PHP_INT_MAX);
    $dp[0] = 0;

    for ($i = 1; $i <= $amount; $i++) {
        foreach ($coins as $coin) {
            if ($i >= $coin && $dp[$i - $coin] != PHP_INT_MAX) {
                $dp[$i] = min($dp[$i], $dp[$i - $coin] + 1);
            }
        }
    }

    return $dp[$amount] == PHP_INT_MAX ? -1 : $dp[$amount];
}

// Test
$coins = [1, 2, 5];
$amount = 11;
echo "DP Coin Change: " . CoinChange($coins, $amount) . "\n";



// ---------------- Dijkstra: En kısa yol (adjacency matrix) ----------------
function Dijkstra($graph, $start)
{
    $n = count($graph);
    $dist = array_fill(0, $n, PHP_INT_MAX);
    $visited = array_fill(0, $n, false);
    $dist[$start] = 0;

    for ($i = 0; $i < $n - 1; $i++) {
        $u = -1;
        $minDist = PHP_INT_MAX;
        for ($j = 0; $j < $n; $j++) {
            if (!$visited[$j] && $dist[$j] < $minDist) {
                $u = $j;
                $minDist = $dist[$j];
            }
        }

        if ($u == -1) break;
        $visited[$u] = true;

        for ($v = 0; $v < $n; $v++) {
            if ($graph[$u][$v] && !$visited[$v] && $dist[$u] + $graph[$u][$v] < $dist[$v]) {
                $dist[$v] = $dist[$u] + $graph[$u][$v];
            }
        }
    }

    return $dist;
}

// Test
$graph = [
    [0, 4, 0, 0, 0, 0, 0, 8, 0],
    [4, 0, 8, 0, 0, 0, 0, 11, 0],
    [0, 8, 0, 7, 0, 4, 0, 0, 2],
    [0, 0, 7, 0, 9, 14, 0, 0, 0],
    [0, 0, 0, 9, 0, 10, 0, 0, 0],
    [0, 0, 4, 14, 10, 0, 2, 0, 0],
    [0, 0, 0, 0, 0, 2, 0, 1, 6],
    [8, 11, 0, 0, 0, 0, 1, 0, 7],
    [0, 0, 2, 0, 0, 0, 6, 7, 0]
];
$distances = Dijkstra($graph, 0);
echo "Dijkstra shortest distances from node 0: " . implode(', ', $distances) . "\n";



// ---------------- A*: Akıllı en kısa yol (grid örneği, Manhattan distance) ----------------
function AStar($start, $goal, $grid)
{
    $rows = count($grid);
    $cols = count($grid[0]);

    $openSet = [$start];
    $cameFrom = [];

    $gScore = [];
    $fScore = [];

    for ($i = 0; $i < $rows; $i++) {
        for ($j = 0; $j < $cols; $j++) {
            $gScore["$i,$j"] = PHP_INT_MAX;
            $fScore["$i,$j"] = PHP_INT_MAX;
        }
    }

    list($sx, $sy) = $start;
    list($gx, $gy) = $goal;

    $gScore["$sx,$sy"] = 0;
    $fScore["$sx,$sy"] = abs($sx - $gx) + abs($sy - $gy); // Manhattan distance

    while (!empty($openSet)) {
        usort($openSet, function ($a, $b) use ($fScore) {
            return $fScore["{$a[0]},{$a[1]}"] - $fScore["{$b[0]},{$b[1]}"];
        });

        $current = array_shift($openSet);
        if ($current == $goal) {
            return $gScore["{$gx},{$gy}"];
        }

        $dirs = [[0, 1], [1, 0], [0, -1], [-1, 0]];
        foreach ($dirs as $dir) {
            $nx = $current[0] + $dir[0];
            $ny = $current[1] + $dir[1];
            if ($nx >= 0 && $ny >= 0 && $nx < $rows && $ny < $cols && $grid[$nx][$ny] == 0) {
                $tentative_g = $gScore["{$current[0]},{$current[1]}"] + 1;
                if ($tentative_g < $gScore["$nx,$ny"]) {
                    $cameFrom["$nx,$ny"] = $current;
                    $gScore["$nx,$ny"] = $tentative_g;
                    $fScore["$nx,$ny"] = $tentative_g + abs($nx - $gx) + abs($ny - $gy);
                    $openSet[] = [$nx, $ny];
                }
            }
        }
    }

    return -1; // No path found
}

// Test 5x5 grid (0 = free, 1 = wall)
$grid = [
    [0, 0, 0, 0, 0],
    [1, 1, 0, 1, 0],
    [0, 0, 0, 1, 0],
    [0, 1, 1, 1, 0],
    [0, 0, 0, 0, 0]
];
$start = [0, 0];
$goal = [4, 4];
echo "A* shortest path length: " . AStar($start, $goal, $grid) . "\n";
