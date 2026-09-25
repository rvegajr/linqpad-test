# linqpad-test

Tiny LINQPad harness (`TestHarness.cs`) plus the LINQ / array / string snippets that match what Filevine has used in coding rounds.

**Sources covered:** Glassdoor (“Solve IList enumerable problems. Use LINQ.”), Alex Becz screen (60 min **array + string**, talk + comment), local practice sets 1–2.

## Quick start

1. Open LINQPad → **C# Program**
2. Paste `TestHarness.cs`
3. Put your methods under `Main`, then call:

```csharp
Check(MyMethod("input"), expected);
CheckAll(MyMethod(new[] { 1, 2 }), new[] { 1, 2 });
Show(someValue); // Dump with the expression as the title
```

`Check` / `Show` use `CallerArgumentExpression`, so the call you wrote shows up in the result — no name string needed.

**During the interview:** say the pipeline out loud, then type it. Comment as you go.

---

## LINQ — most useful

```csharp
int[] xs = { 1, 2, 3, 4, 2 };

xs.Where(x => x > 2);                 // 3, 4
xs.Select(x => x * 2);                // 2, 4, 6, 8, 4
xs.SelectMany(x => new[] { x, x });   // flatten / expand
xs.Distinct();                        // 1, 2, 3, 4  (keeps order)
xs.OrderBy(x => x);                   // ascending
xs.OrderByDescending(x => x);         // descending
xs.ThenBy(x => x);                    // secondary sort (chain after OrderBy)
xs.Skip(1).Take(2);                   // 2, 3
xs.Reverse();                         // LINQ reverse (new sequence)
xs.First();                           // 1  (throws if empty)
xs.FirstOrDefault();                  // 0 / null if empty
xs.Any(x => x > 3);                   // true
xs.All(x => x > 0);                   // true
xs.Count();                           // 5
xs.Sum();                             // 12
xs.Aggregate(1, (a, b) => a * b);     // product
xs.Concat(new[] { 9 });               // append sequence (keeps dupes)

// Group
xs.GroupBy(x => x)
  .Select(g => new { g.Key, n = g.Count() });

// Zip (stops at shorter) — great for Hamming / pair-up
new[] { 1, 2, 3 }.Zip(new[] { "a", "b" }, (n, s) => (n, s));
// (1,a), (2,b)

// Flatten one level
new[] { new[] { 1, 2 }, new[] { 3 } }.SelectMany(x => x); // 1, 2, 3

// Materialize (runs the query)
xs.ToList();
xs.ToArray();
xs.ToHashSet();
xs.ToDictionary(x => x); // key must be unique
xs.SequenceEqual(other);
```

**Remember:** LINQ on `IEnumerable` is lazy until `ToList` / `ToArray` / `Count` / `foreach` / `Any` / `First`.  
Glassdoor shape: return `IList<T>` / `List<T>` so the caller gets a concrete result.

---

## Array / List — most useful

```csharp
int[] a = { 1, 2, 3 };
var list = new List<int> { 1, 2 };

a.Length;                 // 3
list.Count;               // 2
a[0];                     // 1
a[^1];                    // last → 3
a[1..];                   // slice from index 1

list.Add(4);
list.AddRange(a);
list.Contains(2);         // true
Array.IndexOf(a, 2);      // 1  (-1 if missing)

Array.Sort(a);            // in-place
list.Sort();
Array.Reverse(a);         // in-place
Array.Fill(a, 0);         // all zeroes

int[] copy = (int[])a.Clone();
a.Contains(2);            // LINQ extension

var set = new HashSet<int>(a);
set.Add(1);               // false if already present
set.Count == 26;          // pangram check after collecting letters

var d = new Dictionary<string, int>();
d["a"] = 1;
d.TryGetValue("a", out var v);
d.ContainsKey("a");

// Classic nested scan (two-sum style)
for (int i = 0; i < a.Length; i++)
  for (int j = i + 1; j < a.Length; j++)
    if (a[i] + a[j] == target) return (i, j);
```

---

## String — Alex said array + string

```csharp
s.Length;
s[i];                              // char
s.ToLowerInvariant();
s.Trim();
s.Contains("ab");
s.StartsWith("A"); s.EndsWith("z");
s.IndexOf('x');                    // -1 if missing
s.Substring(i, len);
s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
s.Replace("a", "b");
string.Join("-", parts);
new string(charArray);
char.IsLetter(c);                  // IsDigit, IsWhiteSpace

// Prefer StringBuilder in a loop — string is immutable
var sb = new StringBuilder();
sb.Append(x);
sb.ToString();
```

---

## Patterns you’ll reuse (maps to practice / Glassdoor)

```csharp
// Letters only
s.ToLowerInvariant().Where(char.IsLetter).ToList();

// Isogram — no repeating letter
letters.Distinct().Count() == letters.Count;

// Pangram — all 26 letters
s.ToLowerInvariant().Where(char.IsLetter).ToHashSet().Count == 26;

// Anagram key (sort letters)
new string(s.Where(char.IsLetter).Select(char.ToLower).OrderBy(c => c).ToArray());

// Anagrams of word among candidates
candidates.Where(c => Key(c) == Key(word)
                   && !c.Equals(word, StringComparison.OrdinalIgnoreCase));

// Group anagrams
words.GroupBy(w => Key(w)).Select(g => (IList<string>)g.ToList()).ToList();

// Word → count
words.GroupBy(w => w).ToDictionary(g => g.Key, g => g.Count());

// First unique char (value)
s.GroupBy(c => c).First(g => g.Count() == 1).Key;

// First unique char (index) — "loveleetcode" → 2
var ch = s.GroupBy(c => c).FirstOrDefault(g => g.Count() == 1)?.Key;
return ch is null ? -1 : s.IndexOf(ch.Value);

// Acronym — first letter of each word; hyphen starts a word
string.Concat(phrase.Split(new[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(w => char.ToUpperInvariant(w[0])));

// Reverse string
new string(s.Reverse().ToArray());
// or: for (int i = s.Length - 1; i >= 0; i--) sb.Append(s[i]);

// Hamming distance (same length) — Zip + count diffs
left.Zip(right, (a, b) => a == b ? 0 : 1).Sum();

// Filter / map
new[] { 1, 2, 3, 4 }.Where(n => n % 2 == 1).ToArray(); // {1,3}
new[] { 1, 2, 3 }.Select(n => n * 2).ToArray();        // {2,4,6}

// Distinct keep order
new[] { 1, 2, 1, 3 }.Distinct().ToArray();             // {1,2,3}

// Flatten
nested.SelectMany(x => x).ToList();

// Pair two lists
left.Zip(right, (a, b) => (a, b)).ToList();

// Move zeroes to end (preserve non-zero order)
nums.Where(n => n != 0).Concat(nums.Where(n => n == 0)).ToList();

// Product
nums.Aggregate(1, (a, b) => a * b); // empty → seed 1

// Top K frequent
words.GroupBy(w => w)
     .OrderByDescending(g => g.Count())
     .Take(k)
     .Select(g => g.Key);

// Majority element
items.GroupBy(x => x).OrderByDescending(g => g.Count()).First().Key;
```

---

## Coverage checklist

| Source | Need | On this sheet |
|---|---|---|
| Glassdoor | IList / enumerable + LINQ | Where/Select/GroupBy/Distinct/Zip/Aggregate/ToList |
| Glassdoor | ~3–8 short challenges | Patterns above (string + collection) |
| Glassdoor spoken | struct vs class | Below |
| Glassdoor spoken | event-driven infinite loops | Below |
| Alex | array + string | Array + String sections |
| Alex | StringBuilder / immutable string | String section |
| Practice set 1 | Isogram → FirstUnique | Patterns |
| Practice set 2 | Distinct → TopK | Patterns |

### Spoken (not code — say it)

- **struct vs class:** struct = value type (copied); class = reference type (heap). Prefer class for domain objects; struct for small immutable data.
- **IEnumerable vs List/IList:** `IEnumerable` is a walkable sequence (often lazy). `List`/`IList` is concrete — indexable, has `Count`, safe to return from a solved problem.
- **Infinite update loops (events):** handler writes the same state → event fires again. Break with no-op guards, command≠event, idempotent handlers, or version/correlation checks.
- **String:** immutable. Loop concat → `StringBuilder`.

## License

MIT — see [LICENSE](LICENSE).
