# Sample
Time Complexity: O(M×N) for the matrix scan + O(W×L) where W is the number of words and L is the word length. Precomputing the rolling hashes ensures minimal recomputation.
Space Complexity: O(M×N) for the grid storage and O(W) for storing found words and their frequencies.
