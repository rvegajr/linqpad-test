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

### Spoken Q&A (Glassdoor + common follow-ups)

**One-page two-panel sheet (print-ready):** [spoken-qa.html](https://ricardovega.dev/linq/) · [source](spoken-qa.html)

#### Struct vs class

**Q: Difference between a struct and a class?**  
Struct = value type (copied). Class = reference type (variable holds a reference to the heap).

**Q: What prints here?**
```csharp
struct S { public int X; }
var a = new S { X = 1 };
var b = a;
b.X = 2;
Console.WriteLine(a.X); // 1 — b is a copy
```
Same with a `class` → prints `2` (same object).

**Q: When do you pick a struct?**  
Small, immutable, “is a value” data (`Point`, coords, money amount). Rough rule: keep it tiny. Default to **class** for domain objects, identity, inheritance, or anything mutable/large.

**Q: Can a struct inherit another class?**  
No. Structs don’t support inheritance (they can implement interfaces).

---

#### IEnumerable vs List / IList

**Q: IEnumerable vs List vs IList?**  
- `IEnumerable<T>` — walk with `foreach` only (LINQ builds on this; often lazy).  
- `ICollection<T>` — adds `Count`, Add/Remove.  
- `IList<T>` / `List<T>` — index (`this[i]`), Insert, concrete storage.

**Q: What should a coding problem return?**  
Prefer `List<T>` / `IList<T>` so the caller can index and count without re-running a query.

**Q: What is deferred execution?**  
`var q = xs.Where(...);` does nothing yet. It runs when you enumerate: `foreach`, `ToList()`, `Count()`, `First()`, etc.

**Q: Trap — why does this include the 4?**
```csharp
var numbers = new List<int> { 1, 2, 3 };
var q = numbers.Select(x => x * 2);
numbers.Add(4);
foreach (var x in q) … // 2,4,6,8 — query sees live data
```
Fix: materialize once with `.ToList()` when you want a snapshot.

**Q: IEnumerable vs IQueryable?** (only if they go DB)  
`IEnumerable` = in-memory `Func`, runs in your process. `IQueryable` = expression tree, EF can turn into SQL. Today’s coding round is almost certainly in-memory.

---

#### String / StringBuilder

**Q: Are strings mutable?**  
No. Every change allocates a new `string`.

**Q: Why not `s += x` in a loop?**  
Each `+=` copies the whole string → O(n²) allocations. Use `StringBuilder.Append`, then `ToString()` once.

**Q: String vs StringBuilder?**  
`string` for normal text. `StringBuilder` when you build text in a loop or many pieces.

---

#### Event-driven infinite update loops (Glassdoor)

**Q: In an event-driven architecture, how do you prevent infinite update loops?**  
A handler reacts to an event, writes state, that write publishes the same event again → loop.

**Break it with:**
1. **No-op guard** — if new value equals old, don’t publish.  
2. **Command ≠ event** — commands change state; events announce what happened; handlers don’t re-issue the same command blindly.  
3. **Idempotent handlers** — applying the same event twice is safe / no second emit.  
4. **Version / correlation id** — ignore stale or self-caused updates.  
5. **Separate read vs write models** when a projection shouldn’t write back to the same stream.

**Tiny example:** UI field `TextChanged` → saves → save raises `TextChanged` again. Fix: only save when value actually changed, or suppress events while applying an update.

---

#### Extra they may toss in (Alex already probed some)

| Q | Short answer |
|---|---|
| `async` / `await`? | Marks a method that can yield; `await` continues after the `Task` completes without blocking the thread. |
| `static` method? | Belongs to the type; no instance required. |
| DI Transient vs Singleton? | Transient = new each resolve. Singleton = one for app lifetime. (Scoped = one per request.) |
| `ref` / `out`? | Pass by reference; `out` must be assigned inside the method. |
| `==` vs `.Equals` on strings? | Prefer `Equals` / `string.Equals(..., OrdinalIgnoreCase)` when case rules matter. |
| Array vs List? | Array fixed length; List grows (`Add`). |

## License

MIT — see [LICENSE](LICENSE).
