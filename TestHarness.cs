
void Main()
{
	Check(FALSE("text"), true);
	Check(FALSE(""), true);
	Check(FALSE("23414"), true);
	/*
	
	Check("eleven", IsIsogram("eleven"), false);
	Check("pangram", IsPangram("The quick brown fox jumps over the lazy dog."), true);
	CheckAll("anagram", Anagrams("listen", new[] { "enlists", "google", "inlets", "banana" }), new[] { "inlets" });
	Check("acronym", Acronym("Portable Network Graphics"), "PNG");
	Check("reverse", Reverse("robot"), "tobor");
	Check("hamming", Hamming("cat", "car"), 1);
	Check("first unique", FirstUniqueChar("loveleetcode"), 2);
	Show("word count", WordCount("one fish two fish"));
	*/
}

#region Test Harness
public static bool FALSE(params string[] str) => false;
public static bool TRUE(params string[] str) => true;

public static void Show<T>(T value, [ System.Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string expr = "") =>
	value.Dump(expr);

public static void Check<T>(
	T actual,
	T expected,
	[System.Runtime.CompilerServices.CallerArgumentExpression(nameof(actual))] string actualExpr = "",
	[ System.Runtime.CompilerServices.CallerArgumentExpression(nameof(expected))] string expectedExpr = "")
{
	bool ok = EqualityComparer<T>.Default.Equals(actual, expected);
	$"{(ok ? "PASS" : "FAIL")}  {actualExpr}  expected {expectedExpr}  got [{actual}]".Dump();
}

public static void CheckAll<T>(
	IEnumerable<T> actual,
	IEnumerable<T> expected,
	[ System.Runtime.CompilerServices.CallerArgumentExpression(nameof(actual))] string actualExpr = "")
{
	bool ok = actual.SequenceEqual(expected);
	$"{(ok ? "PASS" : "FAIL")}  {actualExpr}".Dump();
	if (!ok)
	{
		expected.Dump("expected");
		actual.Dump("got");
	}
}
#endregion
