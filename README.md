# BiMap
Simple Generic Bidirectional Map for c#

Usage 
```
BiMap<int, string> letters = new BiMap<int, string>

letters.AddPair(1, "a");
letters.AddPair(2, "b");
letters.AddPair(3, "c");

var checkA = letters.TryGetLeftToRight("a", out var outA);
var checkB = letters.TryGetLeftToRight("b", out var outB);
var checkC = letters.TryGetLeftToRight("c", out var outC);

var check1 = letters.TryGetRightToLeft("a", out var out1);
var check2 = letters.TryGetRightToLeft("b", out var out2);
var check3 = letters.TryGetRightToLeft("c", out var out3);

var a = letters.GetLeftToRight(1);
var b = letters.GetLeftToRight(2);
var c = letters.GetLeftToRight(3);

var num1 = letters.GetRightToLeft("a");
var num2 = letters.GetRightToLeft("b");
var num3 = letters.GetRightToLeft("c");




```
