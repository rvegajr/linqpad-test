# linqpad-test

Tiny LINQPad harness (`TestHarness.cs`) plus the LINQ and array snippets you reach for most often.

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
xs.Skip(1).Take(2);                   // 2, 3
xs.First();                           // 1  (throws if empty)
xs.FirstOrDefault();                  // 0 / null if empty
xs.Any(x => x > 3);                   // true
xs.All(x => x > 0);                   // true
xs.Count();                           // 5
xs.Sum();                             // 12
xs.Aggregate(1, (a, b) => a * b);     // product

// Group
xs.GroupBy(x => x)
  .Select(g => new { g.Key, n = g.Count() });

// Zip (stops at shorter)
new[] { 1, 2, 3 }.Zip(new[] { "a", "b" }, (n, s) => (n, s));
// (1,a), (2,b)

// Flatten one level
new[] { new[] { 1, 2 }, new[] { 3 } }.SelectMany(x => x); // 1, 2, 3

// Materialize (runs the query)
xs.ToList();
xs.ToArray();
xs.ToHashSet();
xs.ToDictionary(x => x); // key must be unique
```

**Remember:** LINQ on `IEnumerable` is lazy until `ToList` / `ToArray` / `Count` / `foreach` / `Any`.

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
a.SequenceEqual(copy);

var set = new HashSet<int>(a);
set.Add(1);               // false if already present

var d = new Dictionary<string, int>();
d["a"] = 1;
d.TryGetValue("a", out var v);
d.ContainsKey("a");
```

---

## Patterns you’ll reuse

```csharp
// Letters only
s.ToLowerInvariant().Where(char.IsLetter).ToList();

// Unique letters (isogram check)
letters.Distinct().Count() == letters.Count;

// Anagram key
new string(s.Where(char.IsLetter).Select(char.ToLower).OrderBy(c => c).ToArray());

// Word → count
words.GroupBy(w => w).ToDictionary(g => g.Key, g => g.Count());

// First unique char index / value
s.GroupBy(c => c).First(g => g.Count() == 1).Key;

// Filter odds
new[] { 1, 2, 3, 4 }.Where(n => n % 2 == 1).ToArray(); // {1,3}

// Map
new[] { 1, 2, 3 }.Select(n => n * 2).ToArray();        // {2,4,6}

// Distinct keep order
new[] { 1, 2, 1, 3 }.Distinct().ToArray();             // {1,2,3}

// Move zeroes to end (preserve non-zero order)
nums.Where(n => n != 0).Concat(nums.Where(n => n == 0)).ToList();

// Top K frequent
words.GroupBy(w => w)
     .OrderByDescending(g => g.Count())
     .Take(k)
     .Select(g => g.Key);

// Majority element
items.GroupBy(x => x).OrderByDescending(g => g.Count()).First().Key;
```

---

## String (when arrays of chars show up)

```csharp
s.Length;
s[i];                              // char
s.ToLowerInvariant();
s.Contains("ab");
s.IndexOf('x');                    // -1 if missing
s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
string.Join("-", parts);
new string(charArray);
char.IsLetter(c);                  // IsDigit, IsWhiteSpace

// Prefer StringBuilder in a loop — string is immutable
var sb = new StringBuilder();
sb.Append(x);
sb.ToString();
```

## License

MIT — see [LICENSE](LICENSE).
