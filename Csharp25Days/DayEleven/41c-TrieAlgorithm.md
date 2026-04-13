# Trie Algorithm

Overview (what a Trie stores)
- A Trie is a tree where each node represents a character.
- The root node represents the empty string.
- Each node has:
  - a map from characters to child nodes (Children),
  - a flag indicating whether the path to this node forms a complete word (IsWord),
  - optionally a frequency or count for how many times the word was inserted.
- Words are stored implicitly by following characters from the root down to some node; if that node's IsWord is true, the path is a stored word.

Node layout (in English)
- Each Trie node:
  - holds a dictionary from character → child node,
  - has a boolean IsWord,
  - has an integer Frequency (optional): number of times that exact word was inserted.

Algorithm: Insert a word (plain English)
1. If the word is empty or null, do nothing.
2. Start at the root node.
3. For each character in the word (in order):
   a. If the current node has a child for that character, move to that child.
   b. Otherwise, create a new child node for that character and move to it.
4. After processing every character, you are at the node representing the whole word.
5. Mark that node’s IsWord = true.
6. Increment that node’s Frequency by 1 (optional, used to track duplicates/frequency).

Algorithm: Exact search (Search)
1. If the word is empty or null, return false.
2. Start at the root node.
3. For each character in the word:
   a. If a child node for the character exists, move to it.
   b. If not, return false (word not present).
4. After all characters are processed, return the value of IsWord on the final node (true if the exact word is stored).

Algorithm: Prefix existence check (StartsWith)
1. If the prefix is empty or null, you can treat it as true (every word has empty prefix).
2. Start at the root node.
3. For each character in the prefix:
   a. If a child node for the character exists, move to it.
   b. If not, return false (no word starts with this prefix).
4. If you finish the prefix without missing a child, return true.

Algorithm: Get all words with a given prefix (GetWordsWithPrefix)
1. Start at the root node.
2. Traverse the trie following characters of the prefix:
   a. If at any step a required child is missing, return an empty collection (no matches).
3. When you reach the node that corresponds to the last character of the prefix, collect all full words in the subtree rooted at that node.
4. To collect words, perform a depth-first traversal from this node:
   a. Keep a string representing the characters from the root (or the prefix) to the current node.
   b. If the current node’s IsWord is true, yield the current string as a found word.
   c. For each child (character → child node), recurse with the current string appended by that character.
5. Return the collected words (optionally sort them before display).

Algorithm: Delete a word (Delete) — recursive removal with cleanup
Purpose: remove the exact word and optionally remove nodes that become unnecessary.
1. If the word is empty or null, return false (nothing deleted).
2. Use a helper recursive function DeleteInternal(node, word, depth):
   a. If node is null, return false.
   b. If depth equals length of word:
      - If node.IsWord is false, the word doesn’t exist — return false.
      - Otherwise, set node.IsWord = false and node.Frequency = 0.
      - If node has no children, return true to tell the parent it can delete this child node.
      - If node has children, children must be kept — return false (do not delete this node).
   c. Otherwise (depth < word length):
      - Let ch = character at index depth of the word.
      - If node has no child for ch, return false (word not present).
      - Recursively call DeleteInternal(child, word, depth + 1) and get a boolean shouldDeleteChild.
      - If shouldDeleteChild is true, remove the child entry for ch from node.Children.
      - After removing child (if any), decide whether this node should be removed as well:
         * Return true if node.IsWord == false and node.Children.Count == 0 (node is now useless).
         * Otherwise return false (node must remain).
3. The top-level Delete returns true if the recursive process removed nodes; or false if the word wasn’t present or only unmarked.

Small concrete example (in plain English)
- Insert "cat":
  root → 'c' node → 'a' node → 't' node (IsWord=true)
- Insert "car":
  share root → 'c' → 'a' → new 'r' node (IsWord=true)
- Search "cat": traverse c → a → t, return true.
- GetWordsWithPrefix "ca": find node at 'a', then collect words: "cat", "car".
- Delete "cat": unmark t node as IsWord; if 't' node has no children, remove 't' from 'a' children. 'a' still remains because 'r' child exists.

Complexity (brief)
- Let L = length of the word/prefix, N = number of nodes in the trie, A = alphabet size (branching).
- Insert: O(L) time (iterate characters) and O(L) extra space in worst case (if completely new path).
- Search / StartsWith: O(L) time.
- GetWordsWithPrefix: O(L + M) time where L is time to reach prefix node and M is the total number of characters visited during subtree traversal to collect matches; in worst case can be proportional to size of subtree.
- Delete: O(L) time to traverse; additional O(L) time in recursion to potentially prune nodes — still O(L).
- Space: proportional to the total number of characters stored across all words (shared prefixes save space).

Edge cases and teacher notes
- Empty string: decide policy — some implementations treat empty string as a valid word; your implementation treats empty or null input as ignored.
- Case sensitivity: decide whether to normalize input (e.g., to lowercase) before insert/search.
- Non-letter characters: Trie works for any characters as long as the key type is consistent (your implementation uses char so it supports any Unicode char).
- Frequency: your Frequency counter records how many times a particular word was inserted. Deletion here sets Frequency to 0; consider whether you want Delete to decrement Frequency (if you allow multiple insertions to mean duplicates).
- Memory: for dense alphabets, an array of children may be faster but uses more memory. Dictionary-based children are good for sparse sets.
- Iteration order: collecting words by iterating dictionary entries yields an order dependent on dictionary enumeration; sort if you need alphabetical order.

Teaching suggestions / exercises
- Have students run a small example by hand: insert ["a", "to", "tea", "ted", "ten", "i", "in", "inn"] and draw the trie.
- Ask them to explain how Delete works when deleting shared prefixes.
- Implement a suggestion ranking: when collecting words, order by Frequency (most frequent first).
- Compare tries vs. hash sets for search and prefix queries: hash set is O(L) for exact lookup but cannot retrieve prefix matches efficiently.
