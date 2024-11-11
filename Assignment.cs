using System;
using System.Collections.Generic;
using System.Linq;

public class WordFinder
{
    private readonly List<string> matrix;
    private readonly int rows;
    private readonly int cols;

    public WordFinder(IEnumerable<string> matrix)
    {
        this.matrix = matrix.ToList();
        this.rows = this.matrix.Count;
        this.cols = this.matrix[0].Length;
    }

    public IEnumerable<string> Find(IEnumerable<string> wordstream)
    {
        Dictionary<string, int> wordFrequency = new Dictionary<string, int>();

        HashSet<string> uniqueWords = new HashSet<string>(wordstream);

        foreach (var word in uniqueWords)
        {
            int count = 0;
            if (FindWordInMatrix(word))
            {
                wordFrequency[word] = wordFrequency.ContainsKey(word) ? wordFrequency[word] + 1 : 1;
            }
        }

        return wordFrequency.OrderByDescending(wf => wf.Value)
                            .Take(10)
                            .Select(wf => wf.Key);
    }

    private bool FindWordInMatrix(string word)
    {
        int wordLen = word.Length;
        int wordHash = GetHash(word);

        // Horizontal search
        for (int r = 0; r < rows; r++)
        {
            int rowHash = GetHash(matrix[r].Substring(0, wordLen));
            for (int c = 0; c <= cols - wordLen; c++)
            {
                if (rowHash == wordHash && matrix[r].Substring(c, wordLen) == word)
                    return true;

                if (c < cols - wordLen)
                    rowHash = RollHash(matrix[r][c], matrix[r][c + wordLen], rowHash, wordLen);
            }
        }

        // Vertical search
        for (int c = 0; c < cols; c++)
        {
            string columnSubstring = new string(Enumerable.Range(0, wordLen).Select(r => matrix[r][c]).ToArray());
            int colHash = GetHash(columnSubstring);
            for (int r = 0; r <= rows - wordLen; r++)
            {
                if (colHash == wordHash && new string(Enumerable.Range(r, wordLen).Select(i => matrix[i][c]).ToArray()) == word)
                    return true;

                if (r < rows - wordLen)
                    colHash = RollHash(matrix[r][c], matrix[r + wordLen][c], colHash, wordLen);
            }
        }

        return false;
    }

    private int GetHash(string s)
    {
        const int prime = 101; // Prime number for hashing
        int hash = 0;
        foreach (var ch in s)
            hash = hash * prime + ch;
        return hash;
    }

    private int RollHash(char oldChar, char newChar, int oldHash, int wordLen)
    {
        const int prime = 101;
        oldHash -= oldChar * (int)Math.Pow(prime, wordLen - 1);
        oldHash = oldHash * prime + newChar;
        return oldHash;
    }
}